namespace Bars.Gkh.Gasu.Export
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Overhaul.DomainService;

    public class OverhaulToGasuExport : BaseDataExportService
    {
        protected DateTime Period;

        public override ReportStreamResult ExportData(BaseParams baseParams)
        {

            var dateStart = baseParams.Params["periodStart"].ToDateTime();
            var programCrId = baseParams.Params.GetAsId("programCrId"); 
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(GetData(dateStart, programCrId));
            writer.Flush();
            stream.Position = 0;

            return new ReportStreamResult(stream, "export.xml");
        }

        private string GetData(DateTime periodStart, long programCrId)
        {
            Period = periodStart;

            var result = new StringBuilder();

            var res = new OverhaulToGasuResult();
            var services = Container.ResolveAll<IOverhaulToGasuExportService>();

            foreach (var service in services)
            {
                res.Rows.AddRange(service.GetData(periodStart, programCrId));
            }

            result.Append(@"<?xml version=""1.0"" encoding=""windows-1251""?>");

            using (var writer = XmlWriter.Create(result, new XmlWriterSettings()
            {
                OmitXmlDeclaration = true
            }))
            {
                var serializer = new XmlSerializer(typeof(OverhaulToGasuResult));
                serializer.Serialize(writer, res, new XmlSerializerNamespaces(new[] { new XmlQualifiedName(string.Empty) }));
            }

            return result.ToString();
        }
    }

    [XmlType(AnonymousType = true)]
    [XmlRoot(ElementName = "DATA")]
    public class OverhaulToGasuResult
    {
        public OverhaulToGasuResult()
        {
            Rows = new List<OverhaulToGasuProxy>();
        }

        [XmlElement(ElementName = "ROW")]
        public List<OverhaulToGasuProxy> Rows { get; set; }
    }
}