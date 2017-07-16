using LovManager.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LovManager.Business
{
    public class DomainManager
    {
        private DomainRepository domainRepository = DomainRepository.CreateInstance();
        public List<DomainModel> GetAll()
        {
            var domainEntityList = domainRepository.SelectAll().ToList();
            var domainModelList = Mapper.DomainEntityListToDomainModelList(domainEntityList);
            return domainModelList;
        }

        public DomainModel GetByCode(string code)
        {
            var domainEntity = domainRepository.SelectByCode(code);
            var domainModel = Mapper.DomainEntityToDomainModel(domainEntity);
            return domainModel;
        }

        public DomainModel GetById(string id)
        {
            DomainEntity domainEntity = domainRepository.SelectById(id);
            DomainModel domainModel = Mapper.DomainEntityToDomainModel(domainEntity);

            if (domainEntity.ParentDomainId != null && domainEntity.ParentDomainId != Guid.Empty)
            {
                DomainEntity parentDomainEntity = domainRepository.SelectById(domainEntity.ParentDomainId.ToString());
                domainModel.ParentDomain = Mapper.DomainEntityToDomainModel(parentDomainEntity);
            }
            var lovManager = new ListOfValueManager();
            domainModel.ListOfValueList = lovManager.GetListOfValueByDomainId(domainModel.Id);
            return domainModel;
        }

        public List<DomainModel> GetByName(string name)
        {
            var domainRepository = DomainRepository.CreateInstance();
            var domainEntityList = domainRepository.SelectByName(name).ToList();
            var domainModelList = Mapper.DomainEntityListToDomainModelList(domainEntityList);
            return domainModelList;
        }
        public DomainModel Update(DomainModel domainModel)
        {
            var domainRepository = DomainRepository.CreateInstance();
            var domainEntity = Mapper.DomainModelToDomainEntity(domainModel);
            domainRepository.Update(domainEntity);
            return domainModel;
        }

        public DomainModel Save(DomainModel domainModel)
        {
            var domainEntity = Mapper.DomainModelToDomainEntity(domainModel);
            domainEntity.Id = domainRepository.Insert(domainEntity);
            domainModel = Mapper.DomainEntityToDomainModel(domainEntity);
            return domainModel;
        }
    }
}
