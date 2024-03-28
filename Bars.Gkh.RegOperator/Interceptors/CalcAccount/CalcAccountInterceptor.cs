namespace Bars.Gkh.RegOperator.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities;

    public class CalcAccountInterceptor<T> : EmptyDomainInterceptor<T>
        where T : CalcAccount
    {
        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var calcAccCreditServ = Container.Resolve<IDomainService<CalcAccountCredit>>();
            var calcAccOverdrafeServ = Container.Resolve<IDomainService<CalcAccountOverdraft>>();
            var calcAccRoServ = Container.Resolve<IDomainService<CalcAccountRealityObject>>();

            try
            {
                calcAccCreditServ.GetAll().Where(x => x.Account.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => calcAccCreditServ.Delete(x));
                calcAccOverdrafeServ.GetAll().Where(x => x.Account.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => calcAccOverdrafeServ.Delete(x));
                calcAccRoServ.GetAll().Where(x => x.Account.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => calcAccRoServ.Delete(x));

                return Success();
            }
            catch (Exception)
            {
                return Failure("Не удалось удалить связанные записи");
            }
            finally
            {
                Container.Release(calcAccCreditServ);
                Container.Release(calcAccOverdrafeServ);
                Container.Release(calcAccRoServ);
            }
        }
    }
}