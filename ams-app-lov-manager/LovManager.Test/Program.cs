using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LovManager.Business;
using LovManager.DataAccess;

using Challenges.DataAccess;



namespace LovManager.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var listOfValueDomainEntity = new ListOfValueDomainEntity();
            listOfValueDomainEntity.Code = "2";
            listOfValueDomainEntity.Name = "Countries";
            listOfValueDomainEntity.ParentDomainId = new Guid();
            var listOfValueDomainRepository = ListOfValueDomainRepository.CreateInstance();
            var list = listOfValueDomainRepository.SelectAll();
            listOfValueDomainRepository.Insert(listOfValueDomainEntity);

            //var rep = ChallengeRepository.CreateInstance();
            //var list = rep.SelectAll();
        }
    }
}
