namespace Bars.Gkh.Gasu.Export
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.Gkh.Gasu.DomainService;
    using Bars.Gkh.Gasu.Entities;
    using Bars.Gkh.Gasu.Enums;

    public class GasuIndicatorExport : BaseDataExportService
    {
        public override ReportStreamResult ExportData(BaseParams baseParams)
        {
            var year = baseParams.Params.GetAs("year", 0);
            var month = baseParams.Params.GetAs("month", 0);

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(GetData(year, month));
            writer.Flush();
            stream.Position = 0;

            return new ReportStreamResult(stream, "export.xml");
        }

        private string GetData(int year, int month)
        {
            var gasuIndicatorValueDomain = Container.ResolveDomain<GasuIndicatorValue>();
            var gasuIndicatorService = Container.Resolve<IGasuIndicatorService>();

            var avaliableModulesEbir = gasuIndicatorService.GetAvailableModulesEbir();

            var values = gasuIndicatorValueDomain.GetAll()
                .Where(x => avaliableModulesEbir.Contains(x.GasuIndicator.EbirModule))
                .Where(x => x.Month == month && x.Year == year)
                .Select(x => new
                {
                    x.GasuIndicator.Periodicity,
                    x.PeriodStart,
                    x.Municipality.Okato,
                    x.Value,
                    x.GasuIndicator.Code,
                    x.GasuIndicator.UnitMeasure.ShortName
                })
                .ToList()
                .Select(x => new GasuIndicatorResponseProxy
                {
                    Periodicity = x.Periodicity,
                    DateStart = x.PeriodStart.ToShortDateString(),
                    Okato = x.Okato,
                    Value = x.Value.ToString(new NumberFormatInfo
                    {
                        NumberDecimalSeparator = "."
                    }),
                    IndicatorCode = x.Code,
                    UnitMeasure = x.ShortName
                }).ToList();

            var result = new StringBuilder();

            var res = new GasuIndicatorResponseResult();

            result.Append(@"<?xml version=""1.0"" encoding=""windows-1251""?>");

            res.Rows = values;
            using (var writer = XmlWriter.Create(result, new XmlWriterSettings()
            {
                OmitXmlDeclaration = true
            }))
            {
                var serializer = new XmlSerializer(typeof(GasuIndicatorResponseResult));
                serializer.Serialize(writer, res, new XmlSerializerNamespaces(new[] { new XmlQualifiedName(string.Empty) }));
            }

            return result.ToString();
        }
    }


    [XmlType(AnonymousType = true)]
    [XmlRoot(ElementName = "DATA")]
    public class GasuIndicatorResponseResult
    {
        public GasuIndicatorResponseResult()
        {
            Rows = new List<GasuIndicatorResponseProxy>();
        }

        [XmlElement(ElementName = "ROW")]
        public List<GasuIndicatorResponseProxy> Rows { get; set; }
    }

    [XmlType(AnonymousType = true)]
    public class GasuIndicatorResponseProxy
    {
        public GasuIndicatorResponseProxy()
        {
            FactOrPlan = 1;
            Info = "4";
        }

        [XmlIgnore]
        public Periodicity Periodicity { get; set; }

        [XmlElement(ElementName = "N_CALLVL")]
        public int PeriodicityValue
        {
            get { return (int) Periodicity; }
            set { Periodicity = (Periodicity) value; }
        }

        [XmlElement(ElementName = "D_CALEN")]
        public string DateStart { get; set; }

        [XmlElement(ElementName = "ID_INFO")]
        public int FactOrPlan { get; set; }

        [XmlElement(ElementName = "ID_SINFO")]
        public string Info { get; set; }

        [XmlElement(ElementName = "ID_TER")]
        public string Okato { get; set; }

        [XmlElement(ElementName = "N_VAL")]
        public string Value { get; set; }

        [XmlElement(ElementName = "ID_POK")]
        public string IndicatorCode { get; set; }

        [XmlElement(ElementName = "ID_UNITS")]
        public string UnitMeasure { get; set; }
    }
}