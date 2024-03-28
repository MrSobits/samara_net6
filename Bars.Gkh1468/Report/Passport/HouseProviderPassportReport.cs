namespace Bars.Gkh1468.Report
{
    using System.Linq;
    using B4;
    using Entities;

    public class HouseProviderPassportReport : BasePassportReport
    {
        public HouseProviderPassport HouseProviderPassport { get; set; }

        protected override string GetTitle()
        {
            return string.Format("Форма электронного паспорта поставщика");
        }

        protected override BaseProviderPassport[] GetProviderPassports()
        {
            return Container.Resolve<IDomainService<HouseProviderPassport>>().GetAll()
                .Where(x => x.Id == HouseProviderPassport.Id)
                .Cast<BaseProviderPassport>()
                .ToArray();
        }

        protected override BaseProviderPassportRow[] GetProviderPassportsRows(BaseProviderPassport[] passports)
        {
            return Container.Resolve<IDomainService<HouseProviderPassportRow>>().GetAll()
                .Where(x => x.ProviderPassport.Id == HouseProviderPassport.Id)
                .Cast<BaseProviderPassportRow>()
                .ToArray();
        }
    }
}