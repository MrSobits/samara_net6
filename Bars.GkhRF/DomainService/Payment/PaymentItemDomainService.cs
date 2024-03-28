namespace Bars.GkhRf.DomainService
{
    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.GkhRf.Entities;

    public class PaymentItemDomainService : BaseDomainService<PaymentItem>
    {        
        public override PaymentItem Get(object id)
        {
            var obj = Container.Resolve<IDomainService<PaymentItem>>().Load(id);

            if (obj.ManagingOrganization != null && obj.ManagingOrganization.Contragent != null)
            {
                var contragent = this.Container.Resolve<IDomainService<Contragent>>().Get(obj.ManagingOrganization.Contragent.Id);

                obj.ManagingOrganization.ContragentName = contragent.Name;
            }

            return obj;
        }
    }
}