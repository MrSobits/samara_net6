namespace Bars.Gkh1468.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh1468.DomainService.Passport;
    using Bars.Gkh1468.Entities;

    public class HouseProviderPassportRowService : BaseProviderPassportRowService<HouseProviderPassportRow>
    {
        public IDomainService<HouseProviderPassport> DomainServiceHouseProviderPassport { get; set; }

        public override IDomainService DomainServiceProviderPassport
        {
            get
            {
                return DomainServiceHouseProviderPassport;
            }
        }

        public override void UpdateFillPercent(long providerPassportId)
        {
            var percentService =
                Container.Resolve<IPassportFillPercentService<HouseProviderPassport, HouseProviderPassportRow>>();
            try
            {
                var providerPasport = DomainServiceHouseProviderPassport.Get(providerPassportId);
                providerPasport.Percent = percentService.CountFillPercentage(providerPasport).RoundDecimal(20);
                DomainServiceProviderPassport.Update(providerPasport);
            }
            finally
            {
                Container.Release(percentService);
            }
        }

        protected override IQueryable<HouseProviderPassportRow> GetValues(
            IDomainService<HouseProviderPassportRow> domainService,
            long providerPassportId)
        {
            return domainService.GetAll().Where(x => x.ProviderPassport.Id == providerPassportId);
        }
    }
}