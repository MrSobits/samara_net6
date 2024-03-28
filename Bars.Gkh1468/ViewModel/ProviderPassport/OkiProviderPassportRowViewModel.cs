using System.Linq;
using Bars.B4;
using Bars.Gkh1468.Entities;

namespace Bars.Gkh1468.ViewModel
{
    public class OkiProviderPassportRowViewModel : BaseProviderPassportRowViewModel<OkiProviderPassportRow>
    {
        public IDomainService<OkiProviderPassport> DomainServiceOkiProviderPassport { get; set; }

        public override IDomainService DomainServiceProviderPassport
        {
            get { return DomainServiceOkiProviderPassport; }
        }

        protected override IQueryable<OkiProviderPassportRow> GetValues(IDomainService<OkiProviderPassportRow> domainService, long providerPassportId, long partId)
        {
            return domainService.GetAll()
                .Where(x => x.ProviderPassport.Id == providerPassportId)
                .Where(x => x.MetaAttribute.ParentPart.Id == partId);
        }
    }
}