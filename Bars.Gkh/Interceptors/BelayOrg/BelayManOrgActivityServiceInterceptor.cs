namespace Bars.Gkh.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class BelayManOrgActivityServiceInterceptor : EmptyDomainInterceptor<BelayManOrgActivity>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<BelayManOrgActivity> service, BelayManOrgActivity entity)
        {
            var belayPolicyServ = Container.Resolve<IDomainService<BelayPolicy>>();

            try
            {
                var belayPolicyList =
                    belayPolicyServ.GetAll().Where(x => x.BelayManOrgActivity.Id == entity.Id).Select(x => x.Id).ToArray();
                foreach (var id in belayPolicyList)
                {
                    belayPolicyServ.Delete(id);
                }

                return Success();
            }
            catch (Exception)
            {
                return Failure("Не удалось удалить связанные записи");
            }
            finally
            {
                Container.Release(belayPolicyServ);
            }
        }
    }
}