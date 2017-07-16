using LovManager.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LovManager.Business
{
    class Mapper
    {

        #region Domain Mappers
        public static DomainModel DomainEntityToDomainModel(DomainEntity domainEntity)
        {
            DomainModel domainModel = new DomainModel()
            {
                Id = domainEntity.Id.ToString(),
                Name = domainEntity.Name,
                Code = domainEntity.Code
            };
            if (domainEntity.ParentDomainId != null)
            {
                domainModel.ParentDomain = new DomainModel() { Id = domainEntity.ParentDomainId.ToString() };
            }
            return domainModel;
        }
        public static List<DomainModel> DomainEntityListToDomainModelList(List<DomainEntity> domainEntityList)
        {
            List<DomainModel> domainModelList = new List<DomainModel>();
            foreach (var domainEntity in domainEntityList)
            {
                domainModelList.Add(DomainEntityToDomainModel(domainEntity));
            }

            return domainModelList;
        }

        public static DomainEntity DomainModelToDomainEntity(DomainModel domainModel)
        {
            DomainEntity domainEntity = new DomainEntity();
            if (domainModel == null)
            {
                return domainEntity;
            }
            domainEntity.Name = domainModel.Name;
            domainEntity.Code = domainModel.Code;
            if (domainModel.ParentDomain != null)
            {
                if (!string.IsNullOrEmpty(domainModel.ParentDomain.Id))
                {
                    domainEntity.ParentDomainId = new Guid(domainModel.ParentDomain.Id);
                }
            }
            if (!string.IsNullOrEmpty(domainModel.Id))
            {
                domainEntity.Id = new Guid(domainModel.Id);
            }
            return domainEntity;
        }
        public static List<DomainEntity> DomainModelListToDomainEntityList(List<DomainModel> domainModelList)
        {
            List<DomainEntity> domainEntityList = new List<DomainEntity>();
            foreach (var domainModel in domainModelList)
            {
                domainEntityList.Add(DomainModelToDomainEntity(domainModel));
            }

            return domainEntityList;
        }
        #endregion

        #region List Of Value Mappers
        public static ListOfValueModel ListOfValueEntityToListOfValueModel(ListOfValueEntity listOfValueEntity, ListOfValueEntity parentListOfValueEntity)
        {
            ListOfValueModel ListOfValueModel = new ListOfValueModel()
            {
                Id = listOfValueEntity.Id.ToString(),
                Description = listOfValueEntity.Description,
                Code = listOfValueEntity.Code
            };
            if (parentListOfValueEntity != null)
            {
                ListOfValueModel.ParentCode = parentListOfValueEntity.Code;
                ListOfValueModel.ParentLov = new ListOfValueModel()
                {
                    Id = parentListOfValueEntity.Id.ToString(),
                    Description = parentListOfValueEntity.Description,
                    Code = parentListOfValueEntity.Code
                };
            }
            
            return ListOfValueModel;
        }

        public static List<ListOfValueModel> ListOfValueEntityListToListOfValueModelList(List<ListOfValueEntity> listOfValueEntityList, List<ListOfValueEntity> parentListOfValueEntityList)
        {
            List<ListOfValueModel> ListOfValueModelList = new List<ListOfValueModel>();
            ListOfValueEntity parentListOfValueEntity = new ListOfValueEntity();
            foreach (var listOfValueEntity in listOfValueEntityList)
            {
                //getParent LOV
                parentListOfValueEntity = parentListOfValueEntityList.FirstOrDefault(lov => lov.Id == listOfValueEntity.ParentId);
                
                //Map LOV
                ListOfValueModelList.Add(ListOfValueEntityToListOfValueModel(listOfValueEntity, parentListOfValueEntity));
            }

            return ListOfValueModelList;
        }

        public static ListOfValueEntity ListOfValueModelToListOfValueEntity(ListOfValueModel ListOfValueModel)
        {
            ListOfValueEntity ListOfValueEntity = new ListOfValueEntity()
            {
                Description = ListOfValueModel.Description,
                Code = ListOfValueModel.Code
            };

            return ListOfValueEntity;
        }
        public static List<ListOfValueEntity> ListOfValueModelListToListOfValueEntityList(List<ListOfValueModel> ListOfValueModelList)
        {
            List<ListOfValueEntity> ListOfValueEntityList = new List<ListOfValueEntity>();
            foreach (var ListOfValueModel in ListOfValueModelList)
            {
                ListOfValueEntityList.Add(ListOfValueModelToListOfValueEntity(ListOfValueModel));
            }

            return ListOfValueEntityList;
        }

        public static List<ListOfValueEntity> ListOfValueFileToListOfValueEntityList(Stream listOfValueFileStream)
        {
            var reader = new StreamReader(listOfValueFileStream);
            List<ListOfValueEntity> listOfValueEntityList = new List<ListOfValueEntity>();
            int row = 0;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();

                if (row > 0)
                {
                    var values = line.Split(',');
                    listOfValueEntityList.Add(new ListOfValueEntity() { Code = values[0], Description = values[1] });
                    //values[2] Parent
                    
                }

                row++;
            }
            return listOfValueEntityList;
        }

        #endregion

    }
}
