namespace Bars.Gkh.RegOperator.Interceptors
{
    using B4;
    using B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.RegOperator.Enums;
    using Entities;
    using System;
    using System.Linq;
    using System.Text;

    public class RegopCalcAccountInterceptor : EmptyDomainInterceptor<RegopCalcAccount>
    {
        public override IDataResult BeforeCreateAction(IDomainService<RegopCalcAccount> service, RegopCalcAccount entity)
        {
            if (entity.IsTransit)
            {
                var validation = UniqueTransitAccountValidation(service, entity);

                if (!validation.Success) return validation;
            }

            entity.AccountNumber = entity.ContragentCreditOrg.SettlementAccount;
            entity.CreditOrg = entity.ContragentCreditOrg.CreditOrg;
            entity.TypeAccount = TypeCalcAccount.Regoperator;

            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<RegopCalcAccount> service, RegopCalcAccount entity)
        {
            if (entity.IsTransit)
            {
                var validation = UniqueTransitAccountValidation(service, entity);

                if (!validation.Success) return validation;

                validation = AnyRoOnTransitAccount(entity);

                if (!validation.Success) return validation;
            }

            entity.AccountNumber = entity.ContragentCreditOrg.SettlementAccount;
            entity.CreditOrg = entity.ContragentCreditOrg.CreditOrg;

            return entity.IsTransit ? Success() : CheckOpenCloseDate(entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<RegopCalcAccount> service, RegopCalcAccount entity)
        {
            var regopCalcAccRoServ = Container.Resolve<IDomainService<RegopCalcAccountRealityObject>>();
            var calcAccRoServ = Container.ResolveRepository<CalcAccountRealityObject>();

            try
            {
                var regopCalcAccRoList =
                    regopCalcAccRoServ.GetAll().Where(x => x.RegOpCalcAccount.Id == entity.Id).Select(x => x.Id).ToArray();
                foreach (var id in regopCalcAccRoList)
                {
                    regopCalcAccRoServ.Delete(id);
                }

                var calcAccRoList = calcAccRoServ.GetAll().Where(x => x.Account.Id == entity.Id).Select(x => x.Id).Distinct().ToArray();
                foreach (var id in calcAccRoList)
                {
                    calcAccRoServ.Delete(id);
                }

                return Success();
            }
            catch (Exception)
            {
                return Failure("Не удалось удалить связанные записи");
            }
            finally
            {
                Container.Release(regopCalcAccRoServ);
            }
        }

        private IDataResult CheckOpenCloseDate(RegopCalcAccount entity)
        {
            var regopCalcAccRoDomain = Container.ResolveDomain<CalcAccountRealityObject>();

            using (Container.Using(regopCalcAccRoDomain))
            {
                var entries = regopCalcAccRoDomain.GetAll()
                    .Where(x => x.Account.Id != entity.Id)
                    .Where(x => (x.Account.DateOpen <= entity.DateOpen && (!x.Account.DateClose.HasValue || x.Account.DateClose >= entity.DateOpen))
                                || (!entity.DateClose.HasValue && x.Account.DateOpen >= entity.DateOpen)
                                || (entity.DateClose.HasValue && x.Account.DateOpen <= entity.DateClose && (!x.Account.DateClose.HasValue || x.Account.DateClose >= entity.DateClose))
                    )
                    .Where(x => regopCalcAccRoDomain.GetAll().Where(a => a.Account == entity).Any(y => y.RealityObject.Id == x.RealityObject.Id));
                
                var roList = new StringBuilder();
                foreach (var entry in entries.Where(x => x.Account != entity))
                {
                    if (entry.RealityObject != null)
                    {
                        roList.Append(" " + entry.RealityObject.Address + ";");
                    }
                }

                return 
                    roList.Length > 0 ? 
                    Failure(string.Format("Невозможно сохранить расчетный счет, т.к. следующие дома в данном периоде обслуживаются в других расчетных счетах:{0}", roList).TrimEnd(';')) : 
                    Success();
            }
        }

        private IDataResult UniqueTransitAccountValidation(IDomainService<RegopCalcAccount> service, RegopCalcAccount entity)
        {
            var transitAccontExists =
                service.GetAll()
                    .Where(x => x.AccountOwner.Id == entity.AccountOwner.Id)
                    .Where(x => x.IsTransit)
                    .Any(x => x.Id != entity.Id);

            return transitAccontExists
                ? Failure("Транзитный счет уже выбран.")
                : Success();

        }

        private IDataResult AnyRoOnTransitAccount(RegopCalcAccount entity)
        {
            if (!entity.IsTransit)
                return Success();

            var regopCalcAccRoDomain = Container.ResolveDomain<CalcAccountRealityObject>();

            if (regopCalcAccRoDomain.GetAll().Any(x => x.Account == entity))
            {
                return new BaseDataResult(false, "К транзитному счету нельзя привязывать дома!");
            }

            return Success();
        }
    }
}
