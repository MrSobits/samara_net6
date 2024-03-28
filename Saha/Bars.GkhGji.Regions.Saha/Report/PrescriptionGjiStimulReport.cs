using System.Text;
using Bars.B4.DataAccess;
using Bars.GkhGji.Regions.Saha.Entities;
using Slepov.Russian.Morpher;

namespace Bars.GkhGji.Regions.Saha.Report
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.B4.Utils;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using GkhGji.Report;

    public class PrescriptionGjiStimulReport : GjiBaseStimulReport
    {
        private long DocumentId { get; set; }


        public PrescriptionGjiStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.SahaPrescription))
        {
        }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        public override string Id
        {
            get { return "SahaPrescriptionGji"; }
        }

        public override string CodeForm
        {
            get { return "Prescription"; }
        }

        public override string Name
        {
            get { return "Предписание"; }
        }

        public override string Description
        {
            get { return "Предписание"; }
        }

        protected override string CodeTemplate { get; set; }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                       {
                           new TemplateInfo
                               {
                                   Code = "SahaPrescriptionGji",
                                   Name = "Prescription",
                                   Description = "Предписание",
                                   Template = Properties.Resources.SahaPrescription
                               }
                       };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var prescriptionDomain = Container.ResolveDomain<Prescription>();
            var disposalDomain = Container.ResolveDomain<Disposal>();
            var inspectionRoDomain = Container.ResolveDomain<InspectionGjiRealityObject>();
            var prescrViolDomain = Container.ResolveDomain<PrescriptionViol>();
            var docInspectorDomain = Container.ResolveDomain<DocumentGjiInspector>();
            var childrenDomain = Container.ResolveDomain<DocumentGjiChildren>();
            var violPiontDomain = Container.ResolveDomain<DocumentViolGroupPoint>();
            var docViolGroupDomain = Container.ResolveDomain<DocumentViolGroup>();
            var docViolGroupLongDomain = Container.ResolveDomain<DocumentViolGroupLongText>();

            try
            {
                var склонятель = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ==");

                var prescription = prescriptionDomain.GetAll().FirstOrDefault(x => x.Id == DocumentId);

                if (prescription == null)
                    return;

                if (prescription.Stage == null)
                    return;

                if (prescription.Stage.Parent == null)
                    return;

                FillCommonFields(prescription);

                this.ReportParams["ДатаПредписания"] = prescription.DocumentDate.HasValue
                    ? prescription.DocumentDate.Value.ToString("D", new CultureInfo("ru-RU"))
                    : string.Empty;
                this.ReportParams["НомерПредписания"] = prescription.DocumentNumber;
                this.ReportParams["УправОрг"] = prescription.Contragent != null ? prescription.Contragent.Name : string.Empty;
                this.ReportParams["КомуВыдано"] = string.Empty;
                if (prescription.Executant != null)
                {
                    var executant = склонятель.Проанализировать(prescription.Executant.Name ?? "");

                    var shortName = prescription.Contragent != null ? prescription.Contragent.ShortName : string.Empty;
                    var physicalPerson = склонятель.Проанализировать(prescription.PhysicalPerson ?? ""); 

                    switch (prescription.Executant.Code)
                    {
                        case "0":
                        case "9":
                        case "2":
                        case "4":
                        case "8":
                        case "18":
                            this.ReportParams["КомуВыдано"] = "{0} {1}".FormatUsing(executant.Дательный, shortName);
                            break;
                        case "10":
                        case "1":
                        case "3":
                        case "5":
                        case "19":
                            this.ReportParams["КомуВыдано"] = "{0} {1} {2}".FormatUsing(executant.Дательный, shortName, physicalPerson.Дательный);
                            break;
                        case "6":
                        case "7":
                            this.ReportParams["КомуВыдано"] = this.ReportParams["КомуВыдано"] = "{0} {1} {2}".FormatUsing(executant.Дательный, physicalPerson.Дательный,
                                prescription.PhysicalPersonInfo);
                            break;
                    }
                }
                
                var docViolGroupsQuery = docViolGroupDomain.GetAll()
                    .Where(x => x.Document.Id == prescription.Id);

                var violPoints =
                    violPiontDomain.GetAll()
                                   .Where(x => docViolGroupsQuery.Any(y => y.Id == x.ViolGroup.Id))
                                   .Select(
                                       x =>
                                       new
                                       {
                                           x.ViolStage.InspectionViolation.Violation.CodePin,
                                           ViolStageId = x.ViolStage.Id,
                                           violGroupId = x.ViolGroup.Id
                                       })
                                   .AsEnumerable()
                                   .GroupBy(x => x.violGroupId)
                                   .ToDictionary(
                                        x => x.Key,
                                        y => new
                                        {
                                            PointCodes = y.Select(z => z.CodePin)
                                                        .Distinct()
                                                        .Aggregate((str, result) => !string.IsNullOrEmpty(result) ? result + "," + str : str)                                                       
                                        });

                var longTexts = docViolGroupLongDomain.GetAll()
                    .Where(x => docViolGroupsQuery.Any(y => y.Id == x.ViolGroup.Id))
                    .Select(x => new
                    {
                        x.ViolGroup.Id,
                        x.Action,
                        x.Description
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => new
                    {
                        Action = x.Action != null ? Encoding.UTF8.GetString(x.Action) : string.Empty,
                        Description = x.Description != null ? Encoding.UTF8.GetString(x.Description) : string.Empty
                    }).First());

                var violations = docViolGroupsQuery
                         .Select(x => new
                         {
                             x.Id,
                             x.DatePlanRemoval,
                             x.Description,
                             x.Action
                         })
                         .ToList()
                         .Select(x => new
                         {
                             x.Id,
                             x.DatePlanRemoval,
                             Description = longTexts.ContainsKey(x.Id) && longTexts[x.Id].Description.IsNotEmpty() ?   longTexts[x.Id].Description : x.Description,
                             Action = longTexts.ContainsKey(x.Id) && longTexts[x.Id].Action.IsNotEmpty() ? longTexts[x.Id].Action : x.Action,
                             PointCodes = violPoints.ContainsKey(x.Id) ? violPoints[x.Id].PointCodes : null
                         })
                         .ToList();

                var i = 0;
                var dataViol = new List<PrescriptionViolRecord>();
                foreach (var viol in violations)
                {
                    var rec = new PrescriptionViolRecord
                                  {
                                      НомерПП = ++i,
                                      Описание = viol.Description,
                                      Пункты = viol.PointCodes,
                                      Мероприятие = viol.Action,
                                      ДатаВыполнения = viol.DatePlanRemoval.HasValue
                                                    ? viol.DatePlanRemoval.Value.ToShortDateString()
                                                    : string.Empty
                                  };

                    dataViol.Add(rec);
                }
                this.DataSources.Add(new MetaData
                {
                    SourceName = "Нарушения",
                    MetaType = nameof(PrescriptionViolRecord),
                    Data = dataViol
                });


                // Инспекторы
                var inspectors = docInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == DocumentId)
                    .Select(x => new
                    {
                        x.Inspector.Fio,
                        x.Inspector.Position,
                        x.Inspector.ShortFio,
                        x.Inspector.FioAblative,
                        x.Inspector.IsHead
                    })
                    .ToArray();
                this.ReportParams["ИнспекторыИКоды"] = inspectors.AggregateWithSeparator(x => x.Fio + " - " + x.Position, ", ");

                var headInsp = inspectors.FirstOrDefault();
                this.ReportParams["ДолжностьИнспектора"] = headInsp != null ? headInsp.Position : string.Empty;
                this.ReportParams["КодРуководителяФИО"] = headInsp != null ? headInsp.ShortFio : string.Empty;
                

                // Акт проверки
                var actDoc = childrenDomain.GetAll()
                    .Where(x => x.Children.Id == prescription.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck)
                    .Select(x => x.Parent)
                    .FirstOrDefault();
                this.ReportParams["НомерАктаПроверки"] = string.Empty;
                this.ReportParams["ДатаАктаПроверки"] = string.Empty;
                if (actDoc != null)
                {
                    this.ReportParams["НомерАктаПроверки"] = actDoc.DocumentNumber;
                    this.ReportParams["ДатаАктаПроверки"] = actDoc.DocumentDate.HasValue ? actDoc.DocumentDate.Value.ToString("от dd MMMM yyyy") : string.Empty;
                }
                

                // Дома и адреса, распоряжение
                var disposal = disposalDomain.GetAll()
                    .FirstOrDefault(x => x.Stage.Id == prescription.Stage.Parent.Id);
                reportParams.SimpleReportParams["Распоряжение"] = string.Empty;
                reportParams.SimpleReportParams["ДомаИАдреса"] = string.Empty;
                if (disposal != null && disposal.Inspection != null)
                {
                    this.ReportParams["Распоряжение"] = string.Format(
                        "{0} от {1}",
                        disposal.DocumentNumber,
                        disposal.DocumentDate.HasValue ? disposal.DocumentDate.Value.ToString("D", new CultureInfo("ru-RU")) : string.Empty);
                }

                var listRealityObjs = prescrViolDomain.GetAll()
                     .Where(x => x.Document.Id == prescription.Id && x.InspectionViolation.RealityObject != null)
                     .Select(x => x.InspectionViolation.RealityObject.Address)
					 .Distinct()
                     .ToList();

                if (listRealityObjs.Count > 0)
                {
                    this.ReportParams["ДомаИАдреса"] = listRealityObjs.AggregateWithSeparator(", ");
                }
            }

            finally
            {
                Container.Release(prescriptionDomain);
                Container.Release(disposalDomain);
                Container.Release(inspectionRoDomain);
                Container.Release(prescrViolDomain);
                Container.Release(docInspectorDomain);
                Container.Release(childrenDomain);
            }
        }

        protected class PrescriptionViolRecord
        {
            public int НомерПП { get; set; }

            public string Описание { get; set; }

            public string Пункты { get; set; }

            public string Мероприятие { get; set; }

            public string ДатаВыполнения { get; set; }
        }
    }
}