namespace Bars.Gkh.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class BelayOrgServiceInterceptor : EmptyDomainInterceptor<BelayOrganization>
    {
        public override IDataResult BeforeUpdateAction(IDomainService<BelayOrganization> service, BelayOrganization entity)
        {
            return service.GetAll().Any(x => x.Contragent.Id == entity.Contragent.Id && x.Id != entity.Id)
                ? Failure("Страховая организация с таким контрагентом уже создана")
                : Success();
        }

        public override IDataResult BeforeCreateAction(IDomainService<BelayOrganization> service, BelayOrganization entity)
        {
            return base.BeforeUpdateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<BelayOrganization> service, BelayOrganization entity)
        {
            var belayPolicyServ = Container.Resolve<IDomainService<BelayPolicy>>();

            try
            {
                var belayPolicyList =
                    belayPolicyServ.GetAll().Where(x => x.BelayOrganization.Id == entity.Id).Select(x => x.Id).ToArray();
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
