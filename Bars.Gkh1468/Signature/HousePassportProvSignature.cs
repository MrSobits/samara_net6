namespace Bars.Gkh1468.DomainService.Impl
{
    using System.IO;
    using Bars.B4;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.Gkh1468.Domain.ProviderPassport.Serialize;
    using Bars.Gkh1468.Entities;
    using Bars.Gkh1468.Report;
    using Castle.Windsor;

    public class HousePassportProvSignature : ISignature<HouseProviderPassport>
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<HouseProviderPassport> DomainService { get; set; }

        public MemoryStream GetXmlStream(long id)
        {
            var prov = DomainService.Get(id);
            return new ProviderPassportSerializeProvider(Container).GetStreamXml(prov) as MemoryStream;
        }

        public MemoryStream GetPdfStream(long id)
        {
            var prov = DomainService.Get(id);

            var p = new HouseProviderPassportReport { Container = Container, HouseProviderPassport = prov, ExportFormat = StiExportFormat.Pdf };

            return p.GetGeneratedReport();
        }
    }
}