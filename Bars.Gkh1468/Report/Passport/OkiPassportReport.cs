namespace Bars.Gkh1468.Report
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh1468.Entities;

    public class OkiPassportReport : BasePassportReport
    {
        public OkiProviderPassport OkiProviderPassport { get; set; }

        protected override string GetTitle()
        {
            return string.Format("Форма электронного документа\n" +
                "о состоянии расположенных на териториях муниципальных\n" +
                "образований объектов коммунальной и инженерной инфраструктуры\n" +
                "отчетный период: {0} {1} года",
                GetMonthName(OkiProviderPassport.OkiPassport.ReportMonth),
                OkiProviderPassport.OkiPassport.ReportYear);
        }
        
        protected override BaseProviderPassport[] GetProviderPassports()
        {
            return Container.Resolve<IDomainService<OkiProviderPassport>>().GetAll()
                .Where(x => x.Id == OkiProviderPassport.Id)
                .Cast<BaseProviderPassport>()
                .ToArray();
        }

        protected override BaseProviderPassportRow[] GetProviderPassportsRows(BaseProviderPassport[] passports)
        {
            return Container.Resolve<IDomainService<OkiProviderPassportRow>>().GetAll()
                .Where(x => x.ProviderPassport.Id == OkiProviderPassport.Id)
                .Cast<BaseProviderPassportRow>()
                .ToArray();
        }
    }
}