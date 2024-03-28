namespace Bars.GkhGji.Regions.Tyumen.Interceptors
{
    using System.Linq;
    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tyumen.Entities;

    public class LicenseNotificationInterceptor : EmptyDomainInterceptor<LicenseNotification>
    {
        public override IDataResult BeforeCreateAction(IDomainService<LicenseNotification> service, LicenseNotification entity)
        {
         //   entity.Contragent = entity.ManagingOrgRealityObject.ManOrgContract.ManagingOrganization.Contragent;

            return Success();
        }
    }
}