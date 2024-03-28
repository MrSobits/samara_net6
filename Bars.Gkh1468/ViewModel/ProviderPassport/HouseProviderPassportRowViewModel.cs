namespace Bars.Gkh1468.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh1468.Entities;

    public class HouseProviderPassportRowViewModel : BaseProviderPassportRowViewModel<HouseProviderPassportRow>
    {
        public IDomainService<HouseProviderPassport> DomainServiceHouseProviderPassport { get; set; }

        public override IDomainService DomainServiceProviderPassport
        {
            get { return DomainServiceHouseProviderPassport; }
        }

        protected override IQueryable<HouseProviderPassportRow> GetValues(IDomainService<HouseProviderPassportRow> domainService, long providerPassportId, long partId)
        {
            return domainService.GetAll()
                .Where(x => x.ProviderPassport.Id == providerPassportId)
                .Where(x => x.MetaAttribute.ParentPart.Id == partId);
        }
    }
}