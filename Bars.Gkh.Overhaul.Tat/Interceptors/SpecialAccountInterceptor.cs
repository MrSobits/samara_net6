namespace Bars.Gkh.Overhaul.Tat.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public class SpecialAccountInterceptor : EmptyDomainInterceptor<SpecialAccount>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<SpecialAccount> service, SpecialAccount entity)
        {
            var specAccOperServ = Container.Resolve<IDomainService<SpecialAccountOperation>>();

            try
            {
                specAccOperServ.GetAll().Where(x => x.Account.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => specAccOperServ.Delete(x));

                return Success();
            }
            catch (Exception)
            {
                return Failure("Не удалось удалить связанные записи");
            }
            finally
            {
                Container.Release(specAccOperServ);
            }
        }
    }
}