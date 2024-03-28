using Bars.Gkh.Utils;
using Bars.GkhGji.Regions.Tomsk.Enums;

namespace Bars.GkhGji.Regions.Tomsk.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Report;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    using Bars.GkhGji.Report;

    public class ActCheckGjiStimulReport : GjiBaseStimulReport
    {
        private long DocumentId { get; set; }

        public ActCheckGjiStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_ActSurvey))
        {
        }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        public override string Id
        {
            get { return "ActCheck"; }
        }

        public override string CodeForm
        {
            get { return "ActCheck"; }
        }

        public override string Name
        {
            get { return "Акт проверки"; }
        }

        public override string Description
        {
            get { return "Акт проверки"; }
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
                    Code = "TomskActSurvey",
                    Name = "TomskActSurvey",
                    Description = "Акт проверки Томск",
                    Template = Properties.Resources.BlockGJI_ActSurvey
                }
            };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var tomskDisposalReportData = Container.Resolve<ITomskDisposalReportData>();
            var actCheckDomain = Container.Resolve<IDomainService<ActCheck>>();
            var docChildrenDomain = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var disposalDomain = Container.Resolve<IDomainService<Disposal>>();
            var typeSurveyDomain = Container.Resolve<IDomainService<DisposalTypeSurvey>>();
            var inspectionRoDomain = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var zonalInspectionMuDomain = Container.Resolve<IDomainService<ZonalInspectionMunicipality>>();
            var containerInspectorDomain = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var actRoDomain = Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var actCheckWitnessDomain = Container.Resolve<IDomainService<ActCheckWitness>>();
            var actCheckViolationDomain = Container.Resolve<IDomainService<ActCheckViolation>>();
            var actCheckTimeDomain = Container.Resolve<IDomainService<ActCheckTime>>();
            var actCheckFamiliarizedDomain = Container.ResolveDomain<ActCheckFamiliarized>();
            var actCheckRealityObjectDescriptionDomain = Container.ResolveDomain<ActCheckRealityObjectDescription>();
            var verificationResultDomain = Container.ResolveDomain<ActCheckVerificationResult>();
            var prescriptionGetInfoService = Container.Resolve<IPrescriptionService>();
            var prescriptionDomain = Container.ResolveDomain<Prescription>();
            var prescriptionViolDomain = Container.ResolveDomain<PrescriptionViol>();

            try
            {
                var act = actCheckDomain.FirstOrDefault(x => x.Id == DocumentId);
                if (act == null)
                {
                    throw new ReportProviderException("Не удалось получить акт проверки");
                }

                FillCommonFields(act);

                var disposal = disposalDomain.Load(GetParentDocument(act, TypeDocumentGji.Disposal).Id);

                if (disposal != null)
                {
                    var disposalReportData = tomskDisposalReportData.GetData(disposal);

                    if (disposalReportData != null)
                    {
                        this.ReportParams["ОснованиеОбследования"] = tomskDisposalReportData.GetBaseSurvey(disposalReportData);
                    }


                    if (disposal.Stage != null)
                    {
                        // у родительского приказа получаем дочерние предписания и выводим по ним информацию в источник данных
                        // Дочерними предписаниями считаются те котоыре находятся в тойже группе документов - так хотели в томске
                        var prescriptions = prescriptionDomain.GetAll()
                                              .Where(x => x.Stage.Parent.Id == disposal.Stage.Id)
                                              .Select(x => new { x.Id, x.DocumentNumber, x.DocumentDate, x.Closed })
                                              .OrderBy(x => x.Id)
                                              .ToList();

                        var notClosedPrescriptions =
                            prescriptions.Where(x => x.Closed == YesNoNotSet.No).OrderBy(x => x.DocumentDate);

                        if (notClosedPrescriptions.Any())
                        {
                            var notClsdPrscrptsStr = new StringBuilder();
                            
                            foreach (var prescr in notClosedPrescriptions)
                            {
                                if (notClsdPrscrptsStr.Length > 0)
                                {
                                    notClsdPrscrptsStr.Append(", ");
                                }

                                var prescrDate = prescr.DocumentDate.HasValue
                                                     ? string.Format(" от {0}", prescr.DocumentDate.Value.ToShortDateString())
                                                     : string.Empty;
                                
                                notClsdPrscrptsStr.AppendFormat("№{0}{1}", prescr.DocumentNumber, prescrDate);
                            }
                            
                            this.ReportParams["НевыпПредписания"] = notClsdPrscrptsStr.ToString();
                        }

                        if (prescriptions.Any())
                        {
                            var preIds = prescriptions.Select(x => x.Id).ToList();

                            var dictViols =
                                prescriptionViolDomain.GetAll()
                                                      .Where(x => preIds.Contains(x.Document.Id))
                                                      .Select(
                                                          x =>
                                                          new
                                                          {
                                                              x.Id,
                                                              docId = x.Document.Id,
                                                              violId = x.InspectionViolation.Violation.Id,
                                                              x.InspectionViolation.DatePlanRemoval,
                                                              x.Action
                                                          })
                                                      .AsEnumerable()
                                                      .GroupBy(x => x.docId)
                                                      .ToDictionary(
                                                          x => x.Key,
                                                          y =>
                                                          y.Where(z => !string.IsNullOrEmpty(z.Action))
                                                           .Select(z => z.Action)
                                                           .ToList());

                            var data =
                                prescriptions.Select(
                                    x =>
                                    new
                                    {
                                        НомерПредписания = x.DocumentNumber,
                                        ДатаПредписания =
                                                x.DocumentDate.HasValue ? x.DocumentDate.Value.ToShortDateString() : string.Empty,
                                        ДокументОснование =
                                                (prescriptionGetInfoService.GetInfo(x.Id).Data as PrescriptionGetInfoProxy).baseName,
                                        Мероприятия = dictViols.ContainsKey(x.Id) && dictViols[x.Id].Any() ? dictViols[x.Id].Aggregate((str, result) => !string.IsNullOrEmpty(result) ? result + ", " + str : str) : string.Empty
                                    }).ToList();

                            this.DataSources.Add(new MetaData
                            {
                                SourceName = "ПредписаниеПриказа",
                                MetaType = nameof(Object),
                                Data = data
                            });
                        }
                    }
                }
                
                var verification = verificationResultDomain.GetAll().FirstOrDefault(x => x.ActCheck.Id == act.Id);

                var existVerification = verification != null ? verification.TypeVerificationResult : 0;
                var typeVerificationResult = (TypeVerificationResult[]) Enum.GetValues(typeof(TypeVerificationResult));

                var descriptionsData = typeVerificationResult.Select(x => new
                {
                    Описание = x.GetEnumMeta().Display,
                    Выбран = x == existVerification ? 1 : 0
                })
                .ToList();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "Описания",
                    MetaType = nameof(Object),
                    Data = descriptionsData
                });


                if (verification != null)
                {
                    this.ReportParams["ХодПроведенияПроверки"] = verification.TypeVerificationResult.GetEnumMeta().Display;
                }

                this.ReportParams["Предписание"] = "0";

                // Если у данного акта ест ьхотя бы одно предписание то ставим в параметр  = 1
                if (docChildrenDomain.GetAll()
                                     .Any(
                                         x =>
                                         x.Parent.Id == act.Id
                                         && x.Children.TypeDocumentGji == TypeDocumentGji.Prescription))
                {
                    this.ReportParams["Предписание"] = "1";
                }

                // зональную инспекцию получаем через муниципальное образование первого дома
                var firstRo = inspectionRoDomain.GetAll()
                    .Where(x => x.Inspection.Id == act.Inspection.Id)
                    .Select(x => x.RealityObject)
                    .FirstOrDefault();

                if (firstRo != null)
                {
                    var zonalInspection = zonalInspectionMuDomain.GetAll()
                        .Where(x => x.Municipality.Id == firstRo.Municipality.Id)
                        .Select(x => x.ZonalInspection)
                        .FirstOrDefault();

                    this.ReportParams["ЗональноеНаименование1ГосЯзыкТП"] = zonalInspection != null
                                                                    ? zonalInspection.NameAblative
                                                                    : string.Empty;
                }
                else
                {
                    this.ReportParams["ЗональноеНаименование1ГосЯзыкТП"] = string.Empty;
                }

                this.ReportParams["ДатаАкта"] = act.DocumentDate.HasValue
                    ? act.DocumentDate.Value.ToString("d MMMM yyyy")
                    : string.Empty;
                this.ReportParams["НомерАкта"] = act.DocumentNumber;

                var timeActChek = actCheckTimeDomain.GetAll().FirstOrDefault(x => x.ActCheck.Id == act.Id);

                if (timeActChek != null)
                {
                    var min = timeActChek.Minute;
                    var minStr = min < 10 ? string.Format("0{0}", min) : min.ToString();
                    this.ReportParams["ВремяСоставления"] = string.Format("{0} час. {1} мин.", timeActChek.Hour, minStr);
                }

                if (act.Inspection.Contragent != null)
                {
                    var contragent = act.Inspection.Contragent;

                    this.ReportParams["ИННУправОрг"] = contragent.Inn;

                    this.ReportParams["СокрУправОрг"] = contragent.ShortName;
                }

                // Дома акта проверки
                var actCheckRealtyObjects = actRoDomain.GetAll()
                                       .Where(x => x.ActCheck.Id == DocumentId)
                                       .Select(x => new { x.Id, x.RealityObject, x.HaveViolation, x.Description, x.NotRevealedViolations }).ToArray();

                var shortDescriptions = actCheckRealtyObjects.OrderBy(x => x.Id).Select(x => x.Description).ToList();

                this.ReportParams["ОписаниеСтрокой"] = string.Join(";\n", shortDescriptions);

                if (actCheckRealtyObjects.Length > 0)
                {
                    var realtyObject = actCheckRealtyObjects[0].RealityObject;

                    this.ReportParams["НаселенныйПункт"] = realtyObject.FiasAddress != null
                        ? realtyObject.FiasAddress.PlaceName
                        : string.Empty;

                    if (actCheckRealtyObjects.Length > 0)
                    {
                        var realObjs = new StringBuilder();

                        foreach (var realityObject in actCheckRealtyObjects.Select(x => x.RealityObject))
                        {
                            if (realObjs.Length > 0)
                            {
                                realObjs.Append("; ");
                            }

                            var housing = !string.IsNullOrEmpty(realtyObject.FiasAddress.Housing)
                                              ? ", корп. " + realtyObject.FiasAddress.Housing
                                              : string.Empty;

                            realObjs.AppendFormat(
                                "{0}, д.{1}{2}",
                                realityObject.FiasAddress.StreetName,
                                realityObject.FiasAddress.House,
                                housing);
                        }

                        this.ReportParams["ДомаИАдреса"] = string.Format(
                            "{0}, {1}. ",
                            actCheckRealtyObjects.FirstOrDefault().RealityObject.FiasAddress.PlaceAddressName,
                            realObjs);
                    }
                    else
                    {
                        this.ReportParams["ДомаИАдреса"] = string.Empty;
                    }
                }

                this.ReportParams["ОбщаяПлощадьСумма"] = actCheckRealtyObjects.Select(x => x.RealityObject.AreaMkd).Sum().ToString();


                this.ReportParams["ДатаРаспоряжения"] = disposal.DocumentDate.HasValue
                    ? disposal.DocumentDate.Value.ToString("d MMMM yyyy")
                    : string.Empty;

                this.ReportParams["НомерРаспоряжения"] = disposal.DocumentNumber;

                this.ReportParams["ВидПроверки"] = disposal.KindCheck != null ? disposal.KindCheck.Name : string.Empty;

                if (disposal.IssuedDisposal != null)
                {
                    if (!string.IsNullOrEmpty(disposal.IssuedDisposal.PositionGenitive))
                    {
                        this.ReportParams["КодИнспектораРП)"] = disposal.IssuedDisposal.PositionGenitive.ToLower();
                        this.ReportParams["ДолжностьРаспоряженияРП"] = disposal.IssuedDisposal.PositionGenitive.ToLower();
                    }

                    this.ReportParams["РуководительРП"] = disposal.IssuedDisposal.FioGenitive;
                }
                else
                {
                    this.ReportParams["КодИнспектораРП)"] = string.Empty;
                    this.ReportParams["РуководительРП"] = string.Empty;
                    this.ReportParams["ДолжностьРаспоряженияРП"] = string.Empty;
                }

                if (disposal.DateStart.HasValue && disposal.DateEnd.HasValue)
                {
                    var startDate = disposal.DateStart;

                    int countDays = startDate.Value.DayOfWeek != DayOfWeek.Sunday && startDate.Value.DayOfWeek != DayOfWeek.Saturday ? 1 : 0;

                    while (startDate.Value.Date != disposal.DateEnd.Value.Date)
                    {
                        if (startDate.Value.DayOfWeek != DayOfWeek.Sunday && startDate.Value.DayOfWeek != DayOfWeek.Saturday)
                        {
                            countDays++;
                        }

                        startDate = startDate.Value.AddDays(1);
                    }

                    this.ReportParams["ПродолжительностьПроверки"] = countDays.ToStr();
                }

                this.ReportParams["НачалоПериода"] = disposal.DateStart.HasValue ? disposal.DateStart.Value.ToShortDateString() : string.Empty;
                this.ReportParams["ОкончаниеПериода"] = disposal.DateEnd.HasValue ? disposal.DateEnd.Value.ToShortDateString() : string.Empty;

                var inspectors = containerInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == DocumentId)
                    .Select(x => new
                    {
                        x.Inspector.Id,
                        x.Inspector.Fio,
                        x.Inspector.Position,
                        x.Inspector.ShortFio,
                        x.Inspector.FioAblative
                    })
                    .ToArray();

                this.ReportParams["ИнспекторыИКоды"] = inspectors
                    .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y.Fio + " - " + y.Position : y.Fio + " - " + y.Position));


                var inspectorsData = inspectors.Select(x => new
                {
                    Должность = x.Position,
                    ФамилияИО = x.ShortFio
                })
                .ToList();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "Инспекторы",
                    MetaType = nameof(Object),
                    Data = inspectorsData
                });

                var firstInspector = inspectors.FirstOrDefault();
                if (firstInspector != null && !string.IsNullOrEmpty(firstInspector.ShortFio))
                {
                    this.ReportParams["ИнспекторФамИО"] = firstInspector.ShortFio;
                }
                this.ReportParams["ИнспекторФамИО"] = string.Empty;

                var witness = actCheckWitnessDomain.GetAll()
                       .Where(x => x.ActCheck.Id == DocumentId)
                       .Select(x => new { x.Fio, x.Position, x.IsFamiliar })
                       .ToArray();

                var allWitness = witness
                    .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y.Position + " - " + y.Fio : y.Position + " - " + y.Fio));

                var witnesses =
                    witness.Select(x => "{0} {1}".FormatUsing(x.Fio, x.Position)).AggregateWithSeparator(", ");

                this.ReportParams["ЛицаПрисутствовавшие"] = witnesses;
                this.ReportParams["ДЛприПроверке"] = allWitness;

                var fioFamiliarized = actCheckFamiliarizedDomain.GetAll().Where(x => x.ActCheck.Id == act.Id).Select(x => x.Fio).ToList();

                if (fioFamiliarized.Any())
                {
                    var actCheckFamiliarized = fioFamiliarized.Select(doc => new CheckFamiliarized { ФИО = doc }).ToList();

                    this.DataSources.Add(new MetaData
                    {
                        SourceName = "ЛицаОзнакомленные",
                        MetaType = nameof(CheckFamiliarized),
                        Data = actCheckFamiliarized
                    });
                }

                var fio = string.Empty;

                foreach (var item in fioFamiliarized)
                {
                    if (!string.IsNullOrEmpty(fio))
                    {
                        fio += ", ";
                    }

                    fio += item;
                }

                this.ReportParams["ДЛприОзнакомленныеСКопиейПриказа"] = fio;

                var docs = docChildrenDomain.GetAll()
                    .Where(x => x.Parent.Id == act.Id &&
                        (x.Children.TypeDocumentGji == TypeDocumentGji.Protocol
                        || x.Children.TypeDocumentGji == TypeDocumentGji.Prescription))
                    .Select(x => x.Children)
                    .ToArray();

                var docPrescriptionNames = new StringBuilder();
                var docProtocolNames = new StringBuilder();
                foreach (var doc in docs)
                {
                    var date = doc.DocumentDate.HasValue ? doc.DocumentDate.Value.ToShortDateString() : string.Empty;
                    switch (doc.TypeDocumentGji)
                    {
                        case TypeDocumentGji.Prescription:
                            if (docPrescriptionNames.Length > 0)
                            {
                                docPrescriptionNames.Append(", ");
                            }

                            docPrescriptionNames.AppendFormat("№{0} от {1}г.", doc.DocumentNumber, date);
                            break;
                        case TypeDocumentGji.Protocol:
                            if (docProtocolNames.Length > 0)
                            {
                                docProtocolNames.Append(", ");
                            }

                            docProtocolNames.AppendFormat("№{0} от {1}г.", doc.DocumentNumber, date);
                            break;
                    }
                }

                var serviceActCheckViolation = actCheckViolationDomain.GetAll();
                var queryActViols = serviceActCheckViolation.Where(x => x.Document.Id == act.Id);
                var actViols = queryActViols.Select(x => x.InspectionViolation.Violation)
                                 .ToArray();

                if (actViols.Length > 0)
                {
                    int i = 0;

                    var providedDocuments = actViols.Select(doc => new Violation
                    {
                        НомерПп = ++i,
                        Пункт = doc.CodePin,
                        ТекстНарушения = doc.Name
                    })
                    .ToList();

                    this.DataSources.Add(new MetaData
                    {
                        SourceName = "НарушенияПриПроверке",
                        MetaType = nameof(Violation),
                        Data = providedDocuments
                    });
                }
            }
            finally
            {
                Container.Release(tomskDisposalReportData);
                Container.Release(actCheckDomain);
                Container.Release(disposalDomain);
                Container.Release(typeSurveyDomain);
                Container.Release(typeSurveyDomain);
                Container.Release(inspectionRoDomain);
                Container.Release(zonalInspectionMuDomain);
                Container.Release(containerInspectorDomain);
                Container.Release(actRoDomain);
                Container.Release(actCheckWitnessDomain);
                Container.Release(actCheckViolationDomain);
                Container.Release(actCheckTimeDomain);
                Container.Release(actCheckFamiliarizedDomain);
                Container.Release(actCheckRealityObjectDescriptionDomain);
                Container.Release(verificationResultDomain);
                Container.Release(docChildrenDomain);
                Container.Release(prescriptionGetInfoService);
                Container.Release(prescriptionDomain);
                Container.Release(prescriptionViolDomain);
            }
        }

        public class Violation
        {
            public int НомерПп { get; set; }

            public string Пункт { get; set; }

            public string ТекстНарушения { get; set; }
        }

        public class CheckFamiliarized
        {
            public string ФИО { get; set; }
        }
    }
}