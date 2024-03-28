namespace Bars.Gkh.Overhaul.Tat.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public class RealAccountInterceptor : EmptyDomainInterceptor<RealAccount>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<RealAccount> service, RealAccount entity)
        {
            var realAccOperServ = Container.Resolve<IDomainService<RealAccountOperation>>();

            try
            {
                realAccOperServ.GetAll().Where(x => x.Account.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => realAccOperServ.Delete(x));

                return Success();
            }
            catch (Exception)
            {
                return Failure("Не удалось удалить связанные записи");
            }
            finally
            {
                Container.Release(realAccOperServ);
            }
        }
    }
}