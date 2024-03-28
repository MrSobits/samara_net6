namespace Bars.Gkh1468.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh1468.DomainService.Passport;
    using Bars.Gkh1468.Entities;

    public class OkiProviderPassportRowService : BaseProviderPassportRowService<OkiProviderPassportRow>
    {
        public IDomainService<OkiProviderPassport> DomainServiceOkiProviderPassport { get; set; }

        public override IDomainService DomainServiceProviderPassport
        {
            get
            {
                return DomainServiceOkiProviderPassport;
            }
        }

        public override void UpdateFillPercent(long providerPassportId)
        {
            var percentService =
                Container.Resolve<IPassportFillPercentService<OkiProviderPassport, OkiProviderPassportRow>>();
            try
            {
                var providerPasport = DomainServiceOkiProviderPassport.Get(providerPassportId);
                providerPasport.Percent = percentService.CountFillPercentage(providerPasport).RoundDecimal(20);
                DomainServiceProviderPassport.Update(providerPasport);
            }
            finally
            {
                Container.Release(percentService);
            }
        }

        protected override IQueryable<OkiProviderPassportRow> GetValues(
            IDomainService<OkiProviderPassportRow> domainService,
            long providerPassportId)
        {
            return domainService.GetAll().Where(x => x.ProviderPassport.Id == providerPassportId);
        }
    }
}