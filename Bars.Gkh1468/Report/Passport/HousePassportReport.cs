namespace Bars.Gkh1468.Report
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh1468.Entities;
    using Bars.Gkh1468.Entities.Passport;

    public class HousePassportReport : BasePassportReport
    {
        public HousePassport HousePassport { get; set; }

        protected override string GetTitle()
        {
            return string.Format("Форма электронного паспорта многоквартирного дома\nотчетный период: {0} {1} года",
                GetMonthName(HousePassport.ReportMonth),
                HousePassport.ReportYear);
        }
        
        protected override BaseProviderPassport[] GetProviderPassports()
        {
            return Container.Resolve<IDomainService<HouseProviderPassport>>().GetAll()
                .Where(x => x.HousePassport.Id == HousePassport.Id)
                .Cast<BaseProviderPassport>()
                .ToArray();
        }

        protected override BaseProviderPassportRow[] GetProviderPassportsRows(BaseProviderPassport[] passports)
        {
            return Container.Resolve<IDomainService<HouseProviderPassportRow>>().GetAll()
                .Where(x => x.ProviderPassport.HousePassport.Id == HousePassport.Id)
                .Cast<BaseProviderPassportRow>()
                .ToArray();
        }
    }
}