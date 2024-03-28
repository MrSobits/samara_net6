using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Bars.B4;
using Bars.B4.Modules.Reports;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.Report;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using Bars.GkhGji.Report;

namespace Bars.GkhGji.Regions.Stavropol.Report
{
    public class ResolutionNotificationReport : GjiBaseReport
    {
        private long DocumentId { get; set; }

        protected override string CodeTemplate { get; set; }

        public ResolutionNotificationReport()
            : base(new ReportTemplateBinary(Properties.Resources.Notification_V))
        {
        }

        public override string Id
        {
            get { return "ResolutionNotificationReport"; }
        }

        public override string CodeForm
        {
            get { return "Resolution"; }
        }

        public override string Name
        {
            get { return "Уведомление о составлении протокола"; }
        }

        public override string Description
        {
            get { return "Уведомление о составлении протокола (постановление)"; }
        }

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
                            Code = "Resolution_Notification_V",
                            Name = "PrescriptionGJINotification",
                            Description = "Уведомление о составлении протокола (постановление)",
                            Template = Properties.Resources.Notification_V
                        }
                };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var resolution = Container.Resolve<IDomainService<Resolution>>().Get(DocumentId);

            if (resolution == null)
            {
                throw new ReportProviderException("Не удалось получить постановление");
            }

            CodeTemplate = "Resolution_Notification_V";

            this.FillCommonFields(reportParams, resolution);

            // зональную инспекцию получаем через муниципальное образование первого дома
            var firstRo = Container.Resolve<IDomainService<InspectionGjiRealityObject>>().GetAll()
                .Where(x => x.Inspection.Id == resolution.Inspection.Id)
                .Select(x => x.RealityObject)
                .FirstOrDefault();

            var zji = string.Empty;

            // заполнение этих полей вынесено сюда чтобы вывести дополнительные поля
            if (firstRo != null)
            {
                var zonalInspection = Container.Resolve<IDomainService<ZonalInspectionMunicipality>>().GetAll()
                    .Where(x => x.Municipality.Id == firstRo.Municipality.Id)
                    .Select(x => x.ZonalInspection)
                    .FirstOrDefault();

                if (zonalInspection != null)
                {
                    zji = zonalInspection.ZoneName;
                }
            }

            var cultureInfo = new CultureInfo("ru-RU");

            var prescriptionDoc = GetParentDocument(resolution, TypeDocumentGji.Prescription);
            var prescription = Container.Resolve<IDomainService<Prescription>>().Get(prescriptionDoc.Id);

            var contragent = prescription.Contragent;

            if (contragent != null)
            {
                reportParams.SimpleReportParams["УправОрг"] = contragent.Name;

                var headContragent =
                Container.Resolve<IDomainService<ContragentContact>>().GetAll()
                        .Where(x => x.Contragent.Id == contragent.Id && x.DateStartWork.HasValue
                                     && (x.DateStartWork.Value <= DateTime.Now && (!x.DateEndWork.HasValue || x.DateEndWork.Value >= DateTime.Now)))
                        .FirstOrDefault(x => x.Position != null && (x.Position.Code == "1" || x.Position.Code == "4"));

                if (headContragent != null)
                {
                    reportParams.SimpleReportParams["ФИОРукОрг"] = string.Format(
                        "{0} {1}", headContragent.Position.Name, headContragent.FullName);
                }

                if (contragent.FiasFactAddress != null)
                {
                    var subStr = contragent.FiasFactAddress.AddressName.Split(',');

                    var newAddr = new StringBuilder();

                    foreach (var rec in subStr)
                    {
                        if (newAddr.Length > 0)
                        {
                            newAddr.Append(',');
                        }

                        if (rec.Contains("р-н."))
                        {
                            var mu = rec.Replace("р-н.", string.Empty) + " район";
                            newAddr.Append(mu);
                            continue;
                        }

                        newAddr.Append(rec);
                    }

                    reportParams.SimpleReportParams["АдресКонтрагентаФакт"] = newAddr;
                }
                else
                {
                    reportParams.SimpleReportParams["АдресКонтрагентаФакт"] = contragent.FactAddress;
                }
            }

            reportParams.SimpleReportParams["НомерПредписания"] = prescription.DocumentNumber;
            reportParams.SimpleReportParams["ДатаПредписания"] = prescription.DocumentDate;

            var realityObjs = Container.Resolve<IDomainService<PrescriptionViol>>().GetAll()
                           .Where(x => x.Document.Id == prescription.Id)
                           .Select(x => x.InspectionViolation.RealityObject)
                           .ToList()
                           .Distinct(x => x.Id)
                           .ToArray();

            if (realityObjs.Any())
            {
                reportParams.SimpleReportParams["АдресОбъектаПравонарушения"] =
                    realityObjs.Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? "; " + y.Address : y.Address));
            }

            reportParams.SimpleReportParams["ДомАдресПредписание"] = string.Join("; ", realityObjs.Select(x => x.FiasAddress.AddressName));

            var act = GetParentDocument(prescription, TypeDocumentGji.ActCheck);
            var dispDoc = GetParentDocument(prescription, TypeDocumentGji.Disposal);

            if (act != null)
            {
                reportParams.SimpleReportParams["ДатаПроверки"] = act.DocumentDate.HasValue
                                                                      ? act.DocumentDate.Value.ToString(
                                                                          "D", cultureInfo)
                                                                      : string.Empty;
                reportParams.SimpleReportParams["НомерПроверки"] = act.DocumentNumber;
                var firstInspector =
                    Container.Resolve<IDomainService<DocumentGjiInspector>>()
                             .GetAll()
                             .Where(x => x.DocumentGji.Id == DocumentId)
                             .Select(x => new { x.Inspector.ShortFio })
                             .FirstOrDefault();

                if (firstInspector != null)
                {
                    reportParams.SimpleReportParams["ИнспекторФамИО"] = firstInspector.ShortFio;
                }

                if (dispDoc != null)
                {
                    var disposal = Container.Resolve<IDomainService<Disposal>>().Load(dispDoc.Id);
                    if (disposal.KindCheck.Code == TypeCheck.InspectionSurvey)
                    {
                        reportParams.SimpleReportParams["ДатаАктаИн"] = act.DocumentDate.HasValue
                                                                            ? act.DocumentDate.Value.ToShortDateString()
                                                                            : string.Empty;
                        reportParams.SimpleReportParams["НомерАктаИн"] = act.DocumentNumber;
                    }
                    else
                    {
                        reportParams.SimpleReportParams["ДатаАкта"] = act.DocumentDate.HasValue
                                                                          ? act.DocumentDate.Value.ToShortDateString()
                                                                          : string.Empty;
                        reportParams.SimpleReportParams["НомерАкта"] = act.DocumentNumber;
                    }
                }
            }

            if (dispDoc != null)
            {
                var disposal = Container.Resolve<IDomainService<Disposal>>().Load(dispDoc.Id);

                reportParams.SimpleReportParams["Распоряжение"] = string.Format(
                    "{0} от {1}",
                    disposal.DocumentNumber,
                    disposal.DocumentDate.HasValue ? disposal.DocumentDate.Value.ToString("D", cultureInfo) : string.Empty);

                reportParams.SimpleReportParams["ВидОбследования"] = disposal.KindCheck != null ? disposal.KindCheck.Name : string.Empty;

                var inspectors = Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                                      .Where(x => x.DocumentGji.Id == disposal.Id)
                                      .Select(x => x.Inspector)
                                      .ToArray();

                if (inspectors.Any())
                {
                    var firstInspector = inspectors.FirstOrDefault();

                    if (firstInspector != null)
                    {
                        reportParams.SimpleReportParams["Инспектор"] = firstInspector.Fio;
                        reportParams.SimpleReportParams["Должность"] = firstInspector.Position;

                        reportParams.SimpleReportParams["НаименованиеУправленияДолжность"] = string.Format("{0} {1}", zji, firstInspector.Position);
                    }
                }

                reportParams.SimpleReportParams["РуководительДолжность"] = disposal.IssuedDisposal != null ? disposal.IssuedDisposal.Position : string.Empty;
                reportParams.SimpleReportParams["РуководительФИОСокр"] = disposal.IssuedDisposal != null ? disposal.IssuedDisposal.ShortFio : string.Empty;

                var queryInspectorId = Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                                                     .Where(x => x.DocumentGji.Id == disposal.Id)
                                                     .Select(x => x.Inspector.Id);

                var listLocality = Container.Resolve<IDomainService<ZonalInspectionInspector>>().GetAll()
                                    .Where(x => queryInspectorId.Contains(x.Inspector.Id))
                                    .Select(x => x.ZonalInspection.Locality)
                                    .Distinct()
                                    .ToList();

                reportParams.SimpleReportParams["НаселПунктОтдела"] = string.Join("; ", listLocality);

                var strPurposeDetails = string.Empty;
                if (prescription.Inspection.TypeBase == TypeBase.CitizenStatement && disposal.TypeDisposal != TypeDisposalGji.DocumentGji)
                {
                    strPurposeDetails = reportParams.SimpleReportParams["РеквизитыОбращения"].ToStr();
                }
                else
                {
                    strPurposeDetails = reportParams.SimpleReportParams["ДомАдресПредписание"].ToStr();
                }

                reportParams.SimpleReportParams["ЦельРеквизиты"] = strPurposeDetails;
            }            
        }
    }
}
