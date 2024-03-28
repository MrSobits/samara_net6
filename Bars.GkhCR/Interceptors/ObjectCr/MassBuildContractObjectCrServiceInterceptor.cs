namespace Bars.GkhCr.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.GkhCr.Entities;

    public class MassBuildContractObjectCrServiceInterceptor : EmptyDomainInterceptor<MassBuildContractObjectCr>
    {
        public override IDataResult BeforeUpdateAction(IDomainService<MassBuildContractObjectCr> service, MassBuildContractObjectCr entity)
        {
            var massBuildContractObjectCrWOrkService = Container.Resolve<IDomainService<MassBuildContractObjectCrWork>>();
            try
            {

                var worksSum = massBuildContractObjectCrWOrkService.GetAll().Where(x => x.MassBuildContractObjectCr.Id == entity.Id)
                    .Where(x=> x.Sum.HasValue)
                    .Sum(x=> x.Sum);
                entity.Sum = worksSum;
            }
            catch (Exception e)
            {
                return Failure("Ошибка сохранения" + e.Message);
            }
            finally
            {
                Container.Release(massBuildContractObjectCrWOrkService);
            }

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<MassBuildContractObjectCr> service, MassBuildContractObjectCr entity)
        {
            
            var massBuildContractObjectCrWOrkService = Container.Resolve<IDomainService<MassBuildContractObjectCrWork>>();          
            try
            {
              
                var massBuildContractObjectCrWorkList = massBuildContractObjectCrWOrkService.GetAll().Where(x => x.MassBuildContractObjectCr.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in massBuildContractObjectCrWorkList)
                {
                    massBuildContractObjectCrWOrkService.Delete(value);
                }
             

                return Success();
            }
            catch (Exception e)
            {
                return Failure("Ошибка удаления работ " + e.Message);
            }
            finally
            {
                Container.Release(massBuildContractObjectCrWOrkService);
            }
        }
       
    }
}