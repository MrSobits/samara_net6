namespace Bars.Gkh.RegOperator.Interceptors
{
    using System;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Bars.B4.IoC;
    using DomainService.RealityObjectAccount;
    using Entities;
    using Enums;

    public class CalcAccountCreditInterceptor : EmptyDomainInterceptor<CalcAccountCredit>
    {
        public override IDataResult BeforeCreateAction(IDomainService<CalcAccountCredit> service, CalcAccountCredit entity)
        {
            entity.CreditDebt = entity.CreditSum;

            if (entity.Account.TypeAccount == TypeCalcAccount.Special)
            {
                var roaccpayService = Container.Resolve<IRealityObjectPaymentService>();
                var calcaccroDomain = Container.ResolveDomain<CalcAccountRealityObject>();

                using (Container.Using(roaccpayService, calcaccroDomain))
                {
                    var robject =
                        calcaccroDomain.GetAll()
                            .Where(x => x.Account.Id == entity.Account.Id)
                            .Select(x => x.RealityObject)
                            .FirstOrDefault();

                    roaccpayService.CreatePaymentOperation(robject, entity.CreditSum, PaymentOperationType.IncomeCredit, entity.DateStart);
                }
            }
            else if (entity.Account.TypeAccount == TypeCalcAccount.Regoperator)
            {
                var calcaccDomain = Container.ResolveDomain<CalcAccount>();

                using (Container.Using(calcaccDomain))
                {
                    entity.Account.TotalIn += entity.CreditSum;
                    entity.Account.Balance += entity.Account.TotalIn - entity.Account.TotalOut;

                    calcaccDomain.Update(entity.Account);
                }
            }

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<CalcAccountCredit> service, CalcAccountCredit entity)
        {
            var suspAccCredPaymServ = Container.Resolve<IDomainService<SuspenseAccountCreditPayment>>();

            try
            {
                suspAccCredPaymServ.GetAll()
                    .Where(x => x.Credit.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => suspAccCredPaymServ.Delete(x));

                return Success();
            }
            catch (Exception)
            {
                return Failure("Не удалось удалить связанные записи");
            }
            finally
            {
                Container.Release(suspAccCredPaymServ);
            }
        }
    }
}