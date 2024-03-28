namespace Bars.Gkh.RegOperator.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.RegOperator.Entities;

    using NHibernate.Util;

    public class SpecialCalcAccountInterceptor : EmptyDomainInterceptor<SpecialCalcAccount>
    {
        public override IDataResult AfterUpdateAction(IDomainService<SpecialCalcAccount> service, SpecialCalcAccount entity)
        {
            this.UpdateRealityObjectRelations(entity);

            return this.Success();
        }

        private void UpdateRealityObjectRelations(SpecialCalcAccount entity)
        {
            var accountRoDomain = this.Container.ResolveDomain<CalcAccountRealityObject>();
            using (this.Container.Using(accountRoDomain))
            {
                accountRoDomain.GetAll()
                    .Where(x => x.Account.Id == entity.Id)
                    .Where(x => !x.DateEnd.HasValue || x.DateEnd > entity.DateClose)
                    .ForEach(x =>
                    {
                        x.DateEnd = entity.DateClose;
                        accountRoDomain.Update(x);
                    });
            }
        }
    }
}