namespace Bars.GkhCr.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.GkhCr.Entities;

    public class MassBuildContractServiceInterceptor : EmptyDomainInterceptor<MassBuildContract>
    {
        public override IDataResult BeforeCreateAction(IDomainService<MassBuildContract> service, MassBuildContract entity)
        {
          
            // Перед сохранением проставляем начальный статус
            var stateProvider = Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<MassBuildContract> service, MassBuildContract entity)
        {
            var massBuildContractWorkService = Container.Resolve<IDomainService<MassBuildContractWork>>();
            var massBuildContractObjectCrService = Container.Resolve<IDomainService<MassBuildContractObjectCr>>();
            var massBuildContractObjectCrWOrkService = Container.Resolve<IDomainService<MassBuildContractObjectCrWork>>();
            if (entity.State != null && !entity.State.StartState)
            {
                return Failure("Удаление при данном статусе запрещено");
            }
            try
            {
                var massBuildContractWorkList = massBuildContractWorkService.GetAll().Where(x => x.MassBuildContract.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in massBuildContractWorkList)
                {
                    massBuildContractWorkService.Delete(value);
                }
                var massBuildContractObjectCrWorkList = massBuildContractObjectCrWOrkService.GetAll().Where(x => x.MassBuildContractObjectCr.MassBuildContract.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in massBuildContractObjectCrWorkList)
                {
                    massBuildContractObjectCrWOrkService.Delete(value);
                }
                var massBuildContractObjectCrkList = massBuildContractObjectCrService.GetAll().Where(x => x.MassBuildContract.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in massBuildContractObjectCrkList)
                {
                    massBuildContractObjectCrService.Delete(value);
                }

                return Success();
            }
            catch (Exception e)
            {
                return Failure("Ошибка удаления работ " + e.Message);
            }
            finally
            {
                Container.Release(massBuildContractWorkService);
                Container.Release(massBuildContractObjectCrService);
                Container.Release(massBuildContractObjectCrWOrkService);
            }
        }
       
    }
}