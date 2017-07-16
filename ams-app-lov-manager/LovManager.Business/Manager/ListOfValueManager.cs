using LovManager.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSynchro;

namespace LovManager.Business
{
    public class ListOfValueManager
    {
        private ListOfValueRepository listOfValueRepository = ListOfValueRepository.CreateInstance();
        private DomainRepository domainRepository = DomainRepository.CreateInstance();

        public bool ValidateListOfValueFileStream(DomainModel domainModel, Stream listOfValueFileStream)
        {
            bool valid = true;
            var listOfValueRepository = ListOfValueRepository.CreateInstance();
            if (domainModel == null)
            {
                valid = false;
                throw new Exception("Domain Model Null");
            }

            List<ListOfValueEntity> parentDomainListOfValueEntityList = new List<ListOfValueEntity>();

            if (domainModel.ParentDomain != null)
            {
                if (!string.IsNullOrEmpty(domainModel.ParentDomain.Id))
                {
                    parentDomainListOfValueEntityList = listOfValueRepository.SelectByDomainId(domainModel.ParentDomain.Id).ToList();
                }
            }

            #region Map File To List Of Entity
            //Return StreamReader to Beginning
            listOfValueFileStream.Position = 0;
            //sr.DiscardBufferedData();
            var reader = new StreamReader(listOfValueFileStream);
            List<ListOfValueEntity> listOfValueEntityList = new List<ListOfValueEntity>();
            int row = 0;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (row == 0)
                {
                    var values = line.Split(',');
                    if (values.Length != 3)
                    {
                        valid = false;
                        throw new Exception("Invalid CSV Schema");
                    }
                    if (values[0] != "Code")
                    {
                        valid = false;
                        throw new Exception("Invalid CSV Schema, first column must be Code");
                    }
                    if (values[1] != "Description")
                    {
                        valid = false;
                        throw new Exception("Invalid CSV Schema, second column must be Description");
                    }
                    if (values[2] != "ParentCode")
                    {
                        valid = false;
                        throw new Exception("Invalid CSV Schema, third column must be ParentCode");
                    }
                }
                if (row > 0)
                {

                    var values = line.Split(',');
                    if (values.Length > 3)
                    {
                        valid = false;

                        throw new Exception("Invalid CSV at row number" + row);

                    }
                    var listOfValueEntity = new ListOfValueEntity() { Code = values[0], Description = values[1] };

                    listOfValueEntity.DomainId = new Guid(domainModel.Id);
                    // Parent
                    var parentCode = values[2];
                    if (string.IsNullOrEmpty(parentCode) || string.IsNullOrWhiteSpace(parentCode))
                    {
                        listOfValueEntity.ParentId = null;
                    }
                    else
                    {
                        var parentListIfValueEntity = parentDomainListOfValueEntityList.FirstOrDefault(lov => lov.Code.ToInternalName() == parentCode.ToInternalName());
                        if (parentListIfValueEntity == null)
                        {
                            valid = false;
                            throw new Exception("List Of Value Parent Code '"+ parentCode + "' Not Found");
                        }

                        listOfValueEntity.ParentId = parentListIfValueEntity.Id;
                    }
                    if (!string.IsNullOrEmpty(listOfValueEntity.Code))
                    {
                        listOfValueEntityList.Add(listOfValueEntity);
                    }

                }

                row++;
            }
            #endregion
            row = 2;
            foreach (var entity in listOfValueEntityList)
            {
                if (listOfValueEntityList.Count(lov => lov.Code == entity.Code) > 1)
                {
                    valid = false;
                    throw new Exception("Duplicated  Code '"+ entity.Code + "' At Line " + row);
                }
                row++;
            }
            return valid;
        }
        public void Save(DomainModel domainModel, Stream listOfValueFileStream)
        {
            #region get parent domain lov list
            if (domainModel == null)
            {
                throw new Exception("Domain Model Null");

            }

            var parentDomainListOfValueList = new List<ListOfValueEntity>();

            if (domainModel.ParentDomain != null)
            {
                if (!string.IsNullOrEmpty(domainModel.ParentDomain.Id))
                {
                    parentDomainListOfValueList = listOfValueRepository.SelectByDomainId(domainModel.ParentDomain.Id).ToList();
                }
            }
            #endregion


            #region Map File To List Of Entity
            //Return StreamReader to Beginning
            listOfValueFileStream.Position = 0;
            //sr.DiscardBufferedData();
            var reader = new StreamReader(listOfValueFileStream);
            List<ListOfValueEntity> listOfValueEntityList = new List<ListOfValueEntity>();
            int row = 0;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();

                if (row > 0)
                {
                    
                    var values = line.Split(',');
                    if (values.Length > 3)
                    {
                        throw new Exception("Invalid CSV at row number" + row);

                    }
                    var listOfValueEntity = new ListOfValueEntity() { Code = values[0], Description = values[1] };

                    listOfValueEntity.DomainId = new Guid(domainModel.Id);
                    // Parent
                    var parentCode = values[2];
                    if (string.IsNullOrEmpty(parentCode) || string.IsNullOrWhiteSpace(parentCode))
                    {
                        listOfValueEntity.ParentId = null;
                    }
                    else
                    {
                        var parentListIfValueEntity = parentDomainListOfValueList.FirstOrDefault(lov => lov.Code.ToInternalName() == parentCode.ToInternalName());
                        if (parentListIfValueEntity == null)
                        {
                            throw new Exception("List Of Value Parent Not Found");
                        }

                        listOfValueEntity.ParentId = parentListIfValueEntity.Id;
                    }
                    if (!string.IsNullOrEmpty(listOfValueEntity.Code))
                    {
                        listOfValueEntityList.Add(listOfValueEntity);
                    }

                }

                row++;
            }
            #endregion



            #region Save
            foreach (var entity in listOfValueEntityList)
            {
                listOfValueRepository.Insert(entity);
            }
            #endregion
        }


        public void Update(DomainModel domainModel, Stream listOfValueFileStream)
        {
            #region get parent domain lov list
            if (domainModel == null)
            {
                throw new Exception("Domain Model Null");

            }

            var parentDomainListOfValueList = new List<ListOfValueEntity>();

            if (domainModel.ParentDomain != null)
            {
                if (!string.IsNullOrEmpty(domainModel.ParentDomain.Id))
                {
                    parentDomainListOfValueList = listOfValueRepository.SelectByDomainId(domainModel.ParentDomain.Id).ToList();
                }
            }
            #endregion


            #region Map File To List Of Entity
            //Return StreamReader to Beginning
            listOfValueFileStream.Position = 0;
            //sr.DiscardBufferedData();
            var reader = new StreamReader(listOfValueFileStream);
            List<ListOfValueEntity> listOfValueEntityList = new List<ListOfValueEntity>();
            int row = 0;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();

                if (row > 0)
                {

                    var values = line.Split(',');
                    if (values.Length > 3)
                    {
                        throw new Exception("Invalid CSV at row number" + row);

                    }
                    var listOfValueEntity = new ListOfValueEntity() { Code = values[0], Description = values[1] };

                    listOfValueEntity.DomainId = new Guid(domainModel.Id);
                    // Parent
                    var parentCode = values[2];
                    if (string.IsNullOrEmpty(parentCode) || string.IsNullOrWhiteSpace(parentCode))
                    {
                        listOfValueEntity.ParentId = null;
                    }
                    else
                    {
                        var parentListIfValueEntity = parentDomainListOfValueList.FirstOrDefault(lov => lov.Code.ToInternalName() == parentCode.ToInternalName());
                        if (parentListIfValueEntity == null)
                        {
                            throw new Exception("List Of Value Parent Not Found");
                        }

                        listOfValueEntity.ParentId = parentListIfValueEntity.Id;
                    }
                    if (!string.IsNullOrEmpty(listOfValueEntity.Code))
                    {
                        listOfValueEntityList.Add(listOfValueEntity);
                    }

                }

                row++;
            }
            #endregion



            #region Update
            List<ListOfValueEntity> currentListOfValueEntityList = listOfValueRepository.SelectByDomainId(domainModel.Id).ToList();

            foreach (var entity in listOfValueEntityList)
            {
                //if Code exist update else Insert
                var old = currentListOfValueEntityList.FirstOrDefault(lov => lov.Code == entity.Code);
                if (old != null)
                {
                    if (old.Description != entity.Description || old.ParentId.ToString() != entity.ParentId.ToString())
                    {
                        listOfValueRepository.Update(entity);
                    }
                }
                else
                {
                    listOfValueRepository.Insert(entity);
                }
            }
            #endregion
        }

        public List<ListOfValueModel> GetListOfValueByDomainId(string domainId)
        {
            DomainManager domainManager = new DomainManager();
            var domainEntity = domainRepository.SelectById(domainId);
            List<ListOfValueEntity> parentListOfValueEntityList = new List<ListOfValueEntity>();
            if (domainEntity.ParentDomainId != null && domainEntity.ParentDomainId != Guid.Empty)
            {
                 parentListOfValueEntityList = listOfValueRepository.SelectByDomainId(domainEntity.ParentDomainId.ToString()).ToList();
            }
            List<ListOfValueEntity> listOfValueEntityList = listOfValueRepository.SelectByDomainId(domainEntity.Id.ToString()).ToList();

            List<ListOfValueModel> listOfValueList = Mapper.ListOfValueEntityListToListOfValueModelList(listOfValueEntityList, parentListOfValueEntityList);

            return listOfValueList;
        }

    }
}
