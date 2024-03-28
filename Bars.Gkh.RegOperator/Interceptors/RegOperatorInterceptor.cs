namespace Bars.Gkh.RegOperator.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.Gkh.RegOperator.Entities;

    public class RegOperatorInterceptor : EmptyDomainInterceptor<RegOperator>
    {
        public override IDataResult BeforeCreateAction(IDomainService<RegOperator> service, RegOperator entity)
        {
            return CheckContragent(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<RegOperator> service, RegOperator entity)
        {
            return CheckContragent(service, entity);
        }

        private IDataResult CheckContragent(IDomainService<RegOperator> service, RegOperator entity)
        {
            return service.GetAll().Any(x => x.Contragent.Id == entity.Contragent.Id && x.Id != entity.Id)
                       ? Failure("Региональный оператор с таким контрагентом уже существует")
                       : Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<RegOperator> service, RegOperator entity)
        {
            var regOpCalcAccountDomain = Container.Resolve<IDomainService<RegOpCalcAccount>>();
            var regOperatorMunicipalityDomain = Container.Resolve<IDomainService<RegOperatorMunicipality>>();
            var regOpPersAccMunicipalityDomain = Container.Resolve<IDomainService<RegOpPersAccMunicipality>>();
            var fundFormationContractDomain = Container.Resolve<IDomainService<FundFormationContract>>();

            try
            {
                var regOpCalcAccountList =
                    regOpCalcAccountDomain.GetAll().Where(x => x.RegOperator.Id == entity.Id).Select(x => x.Id).ToArray();
                var regOperatorMunicipalityList =
                    regOperatorMunicipalityDomain.GetAll().Where(x => x.RegOperator.Id == entity.Id).Select(x => x.Id).ToArray();
                var regOpPersAccMunicipalityList =
                    regOpPersAccMunicipalityDomain.GetAll().Where(x => x.RegOperator.Id == entity.Id).Select(x => x.Id).ToArray();
                var fundFormationContractList =
                    fundFormationContractDomain.GetAll().Where(x => x.RegOperator.Id == entity.Id).Select(x => x.Id).ToArray();
                foreach (var id in regOpCalcAccountList)
                {
                    regOpCalcAccountDomain.Delete(id);
                }
                foreach (var id in regOperatorMunicipalityList)
                {
                    regOperatorMunicipalityDomain.Delete(id);
                }
                foreach (var id in regOpPersAccMunicipalityList)
                {
                    regOpPersAccMunicipalityDomain.Delete(id);
                }
                foreach (var id in fundFormationContractList)
                {
                    fundFormationContractDomain.Delete(id);
                }

                return Success();
            }
            catch (Exception)
            {
                return Failure("Не удалось удалить связанные записи");
            }
            finally
            {
                Container.Release(regOpCalcAccountDomain);
                Container.Release(regOperatorMunicipalityDomain);
                Container.Release(regOpPersAccMunicipalityDomain);
                Container.Release(fundFormationContractDomain);
            }
        }
    }
}