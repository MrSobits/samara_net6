namespace Bars.GkhGji.Regions.Tula.Report.ActCheckGji
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;

    using Bars.Gkh.Enums;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tula.Entities;
    using GkhGji.Report;
    using Stimulsoft.Report;

    public class ActCheckGjiReport : GjiBaseStimulReport
    {
        #region .ctor

        public ActCheckGjiReport()
            : base(new ReportTemplateBinary(Properties.Resources.ActCheck_Common))
        {
        }

        #endregion

        #region Properties

        public override StiExportFormat ExportFormat
        {
            get
            {
                return StiExportFormat.Word2007;
            }
        }

        public override string Id
        {
            get
            {
                return "ActCheck";
            }
        }

        public override string CodeForm
        {
            get
            {
                return "ActCheck";
            }
        }

        public override string Name
        {
            get
            {
                return "Акт проверки";
            }
        }

        public override string Description
        {
            get
            {
                return "Акт проверки";
            }
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
                                   Code = "ActCheck_Common",
                                   Name = "Акт проверки",
                                   Description = "Акт проверки",
                                   Template = Properties.Resources.ActCheck_Common
                               }
                       };
        }

        public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
        }

        protected void GetCodeTemplate()
        {
            CodeTemplate = "ActCheck_Common";
        }

        protected override string CodeTemplate { get; set; }

        #endregion

        #region Fields

        private long DocumentId { get; set; }

        #endregion

        #region DomainServices

        public IDomainService<ActCheckRealityObject> ActCheckRealObjDomain { get; set; }
        public IDomainService<ActCheck> ActCheckDomain { get; set; }
        public IDomainService<Disposal> DisposalDomain { get; set; }
        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }
        public IDomainService<DocumentGjiInspector> DocumentGjiInspectorDomain { get; set; }
        public IDomainService<ActCheckWitness> ActCheckWitnessDomain { get; set; }
        public IDomainService<ActCheckPeriod> ActCheckPeriodDomain { get; set; }
        public IDomainService<ActCheckAnnex> ActCheckAnnexDomain { get; set; }
        public IDomainService<ActCheckViolation> ActCheckViolationDomain { get; set; }
        public IDomainService<DisposalProvidedDoc> DisposalProvidedDocDomain { get; set; }
        public IDomainService<InspectionGjiViolWording> InspectionGjiViolWordingDomain { get; set; }
        public IDomainService<DocumentViolGroup> DocumentViolGroupDomain { get; set; }
        public IDomainService<DocumentViolGroupLongText> DocumentViolGroupLongTextDomain { get; set; }
        public IDomainService<DocumentViolGroupPoint> DocumentViolGroupPointDomain { get; set; }
        public IDomainService<ActCheckRoLongDescription> ActCheckRoLongTextDomain { get; set; }

        #endregion

        public override void PrepareReport(ReportParams reportParams)
        {
            var act = ActCheckDomain.Get(DocumentId);
            if (act == null)
            {
                throw new ReportProviderException("Не удалось получить акт проверки");
            }
            FillCommonFields(act);

            Disposal disposal = null;

            var parentDisposal = GkhGji.Utils.Utils.GetParentDocumentByType(DocumentGjiChildrenDomain, act, TypeDocumentGji.Disposal);

            if (parentDisposal != null)
            {
                disposal = DisposalDomain.GetAll().FirstOrDefault(x => x.Id == parentDisposal.Id);
            }

            Report["ВидПроверки"] = disposal != null ? disposal.KindCheck != null ? disposal.KindCheck.Name : string.Empty : string.Empty;
            
            var inspection = act.Return(x => x.Inspection);

            Report["ОснованиеПроверки"] = inspection != null
                                              ? Convert.ChangeType(
                                                  inspection.TypeBase,
                                                  inspection.TypeBase.GetTypeCode())
                                              : "";

            Report["Email"] =
                inspection
                    .Return(x => x.Contragent)
                    .Return(x => x.OfficialWebsite)
                    .Return(x => x, string.Empty);

            Report["Начало"] = string.Empty;
            Report["Окончание"] = string.Empty;
            var actionId = inspection.Return(x => x.Id, 0);
            var action = Container.ResolveDomain<BasePlanAction>().GetAll().FirstOrDefault(x => x.Id == actionId);

            if (action != null)
            {
                var dateStart = action.Return(x => x.DateStart);
                if (dateStart.HasValue)
                {
                    Report["Начало"] = dateStart.Value.ToShortDateString();
                }

                var dateEnd = action.Return(x => x.DateEnd);
                if (dateEnd.HasValue)
                {
                    Report["Окончание"] = dateEnd.Value.ToShortDateString();
                }
            }


            Report["ДатаРаспоряжения"] = disposal != null ? disposal.DocumentDate.HasValue ? disposal.DocumentDate.Value.ToString("d MMMM yyyy") : string.Empty : string.Empty;

            Report["НомерРаспоряжения"] = disposal != null ? disposal.DocumentNumber : string.Empty;

            Report["ДолжностьПриказРП"] = disposal != null
                                          ? disposal.IssuedDisposal != null
                                                ? !string.IsNullOrEmpty(disposal.IssuedDisposal.PositionGenitive)
                                                    ? disposal.IssuedDisposal.PositionGenitive
                                                    : disposal.IssuedDisposal.Position
                                                : string.Empty
                                          : string.Empty;

            Report["РуководительПриказРП"] = disposal != null
                                          ? disposal.IssuedDisposal != null
                                                ? !string.IsNullOrEmpty(disposal.IssuedDisposal.FioGenitive)
                                                    ? disposal.IssuedDisposal.FioGenitive
                                                    : disposal.IssuedDisposal.Fio
                                                : string.Empty
                                          : string.Empty;
            
            Report["ДатаАкта"] = act.DocumentDate.HasValue ? act.DocumentDate.Value.ToString("d MMMM yyyy") : string.Empty;

            Report["НомерАкта"] = act.DocumentNumber;
            
            if (act.Inspection.TypeBase == TypeBase.PlanAction)
            {
                var basePlanAction = Container.Resolve<IDomainService<BasePlanAction>>().GetAll()
                    .FirstOrDefault(x => x.Id == act.Inspection.Id);

                if (basePlanAction != null)
                {
                    this.Report["План"] = basePlanAction.Plan != null ? basePlanAction.Plan.Name : string.Empty;
                }
            }

            Report["НаселенныйПункт"] = string.Empty;
            var actCheckRealityObjects = ActCheckRealObjDomain.GetAll()
                .Where(x => x.ActCheck.Id == DocumentId)
                .Where(x => x.RealityObject != null).ToArray();
            if (actCheckRealityObjects.Length > 0)
            {
                Report["Описание"] = actCheckRealityObjects.First().Description;
                var realityObjects = actCheckRealityObjects.Select(x => x.RealityObject).ToArray();
                if (realityObjects.Length > 0)
                {
                    var realtyObject = realityObjects.First();
                    if (realtyObject.FiasAddress != null)
                    {
                        Report["НаселенныйПункт"] = realtyObject.FiasAddress.PlaceName;
                    }
                }

                Report["ДомАдрес"] = realityObjects
                    .Select(x => x.Address)
                    .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y : y));
            }

            var firstActCheckPeriod = ActCheckPeriodDomain.GetAll().Where(x => x.ActCheck.Id == act.Id).Select(x => x.DateEnd).OrderByDescending(x => x.Value).FirstOrDefault();

            Report["ВремяСоставленияАкта"] = firstActCheckPeriod != null ? firstActCheckPeriod.Value.ToShortTimeString() : string.Empty;

            // Инспекторы
            var inspectors =
                DocumentGjiInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == act.Id)
                    .Select(x => new
                    {
                        x.Inspector.Position,
                        x.Inspector.Fio,
                        x.Inspector.ShortFio
                    })
                    .ToList();

            Report["ДолжностьИнспектор"] = inspectors.AggregateWithSeparator(x => x.Fio + " - " + x.Position, ", ");

            var firstInspector = inspectors.FirstOrDefault();

            Report["Должность"] = firstInspector != null ? firstInspector.Position : string.Empty;
            Report["Инспектор"] = firstInspector != null ? firstInspector.Fio : string.Empty;

            var allInspectorsData =
                inspectors
                    .Select(x => new
                    {
                        ФиоИнспектора = !string.IsNullOrEmpty(x.ShortFio) ? x.ShortFio : x.Fio,
                        Должность = x.Position
                    })
                    .ToList();

            Report.RegData("Инспекторы", allInspectorsData);

            var witnesses = ActCheckWitnessDomain.GetAll().Where(x => x.ActCheck.Id == act.Id).Select(x => new { x.Fio, x.Position }).ToArray();

            Report["Присутствовали"] = witnesses.Any() ? witnesses.Select(x => "{0} {1}".FormatUsing(x.Fio, x.Position)).AggregateWithSeparator(", ") : string.Empty;

            Report["Отказ"] =
                ActCheckWitnessDomain.GetAll()
                    .Where(x => x.ActCheck.Id == act.Id && !x.IsFamiliar)
                    .Select(x => x.Fio)
                    .ToList()
                    .AggregateWithSeparator(", ");

            var contragent = act.Inspection.Contragent;

            Report["УправОрг"] = contragent != null ? contragent.Name : string.Empty;
            Report["ИНН"] = contragent != null ? contragent.Inn : string.Empty;
            Report["ОГРН"] = contragent != null ? contragent.Ogrn : string.Empty;
            Report["АдресЮР"] = contragent != null ? "{0}, {1}".FormatUsing(contragent.FiasJuridicalAddress.PostCode, contragent.FiasJuridicalAddress.AddressName) : string.Empty;

            var actCheckRoQuery = ActCheckRealObjDomain.GetAll().Where(x => x.ActCheck.Id == act.Id);

            var longTextActCheckRo = ActCheckRoLongTextDomain.GetAll()
                                        .Where(x => actCheckRoQuery.Any(y => y.Id == x.ActCheckRo.Id))
                                        .Select(x => new { x.ActCheckRo.Id, x.NotRevealedViolations })
                                        .AsEnumerable()
                                        .GroupBy(x => x.Id)
                                        .ToDictionary(x => x.Key, y => y.Select(z => Encoding.UTF8.GetString(z.NotRevealedViolations)).First());

            // Нарушения
            var actCheckRoInfo = actCheckRoQuery
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    RoId = x.RealityObject != null ? x.RealityObject.Id : 0,
                    TypeHouse = x.RealityObject != null
                        ? x.RealityObject.TypeHouse
                        : 0,
                    Address =  x.RealityObject != null ? x.RealityObject.FiasAddress.AddressName : "",
                    WallMaterial = x.RealityObject != null
                        ? x.RealityObject.WallMaterial != null
                            ? x.RealityObject.WallMaterial.Name
                            : string.Empty
                        : string.Empty,
                    RoofingMaterial = x.RealityObject != null
                        ? x.RealityObject.RoofingMaterial != null
                            ? x.RealityObject.RoofingMaterial.Name
                            : string.Empty
                        : string.Empty,
                    NumberEntrances = x.RealityObject != null
                        ? x.RealityObject.NumberEntrances
                        : null,
                    Floors = x.RealityObject != null
                        ? x.RealityObject.Floors
                        : null,
                    TypeRoof = x.RealityObject != null
                        ? x.RealityObject.TypeRoof.GetEnumMeta().Display
                        : string.Empty,
                    AreaMkd = x.RealityObject != null
                        ? x.RealityObject.AreaMkd.HasValue
                            ? decimal.Round(x.RealityObject.AreaMkd.Value, 2).ToStr()
                            : string.Empty
                        : string.Empty,
                    BuildYear = x.RealityObject != null
                        ? x.RealityObject.BuildYear.HasValue
                            ? x.RealityObject.BuildYear.Value.ToStr()
                            : string.Empty
                        : string.Empty,
                    HeatingSystem = x.RealityObject != null
                        ? x.RealityObject.HeatingSystem.GetEnumMeta().Display
                        : string.Empty,
                    x.NotRevealedViolations,
                    x.HaveViolation
                })
                .ToList();

            var i = 0;
            var dataViol = new List<ActCheckViolationRecord>();
            foreach (var viol in actCheckRoInfo)
            {
                var record = new ActCheckViolationRecord
                {
                    НомерПп = ++i,
                    АдресДома =
                       !viol.Address.IsEmpty()
                           ? viol.Address
                           : string.Empty,
                    ГодПостройки =
                        !viol.BuildYear.IsEmpty()
                           ? "год постройки: " + viol.BuildYear + ", "
                           : string.Empty,
                    ПлощадьМкд =
                        !viol.AreaMkd.IsEmpty()
                           ? "общая площадь: " + viol.AreaMkd + ", "
                           : string.Empty,
                    МатериалКровли =
                        !viol.RoofingMaterial.IsEmpty()
                           ? "материал кровли: " + viol.RoofingMaterial + ", "
                           : string.Empty,
                    МатериалСтен =
                        !viol.WallMaterial.IsEmpty()
                           ? "материал стен: " + viol.WallMaterial + ", "
                           : string.Empty,
                    КоличествоПодъездов =
                        viol.NumberEntrances.HasValue
                           ? "количество подъездов: " + viol.NumberEntrances.Value.ToStr() + ", "
                           : string.Empty,
                    Этажность =
                        viol.Floors.HasValue
                           ? "этажность: " + viol.Floors.Value.ToStr() + ", "
                           : string.Empty,
                    ТипКровли =
                        !viol.TypeRoof.IsEmpty()
                           ? "тип кровли: " + viol.TypeRoof
                           : string.Empty,
                    СистемаОтопления =
                        !viol.HeatingSystem.IsEmpty()
                           ? "система отопления: " + viol.HeatingSystem
                           : string.Empty,
                    НевыявленныеНарушения = longTextActCheckRo.ContainsKey(viol.Id) ? longTextActCheckRo[viol.Id] : viol.NotRevealedViolations
                };

                record.НаличиеНарушения = viol.HaveViolation.ToString();

                if (viol.TypeHouse == TypeHouse.BlockedBuilding
                    || viol.TypeHouse == TypeHouse.Individual
                    || viol.TypeHouse == TypeHouse.ManyApartments)
                {
                    record.ТипДома = viol.TypeHouse.GetEnumMeta().Display + " дом";
                }
                else if (viol.TypeHouse == TypeHouse.SocialBehavior)
                {
                    record.ТипДома = viol.TypeHouse.GetEnumMeta().Display;
                }
                else
                {
                    record.ТипДома = string.Empty;
                }

                var query = DocumentViolGroupDomain.GetAll()
                    .WhereIf(viol.RoId > 0, x => x.RealityObject.Id == viol.RoId)
                    .Where(x => x.Document.Id == act.Id);

                var violPoints =
                    DocumentViolGroupPointDomain.GetAll()
                                   .Where(x => query.Any(y => y.Id == x.ViolGroup.Id))
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

                var longTexts = DocumentViolGroupLongTextDomain.GetAll()
                    .Where(x => query.Any(y => y.Id == x.ViolGroup.Id))
                    .Select(x => new
                    {
                        x.ViolGroup.Id,
                        x.Description
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => new
                    {
                        Description = x.Description != null ? Encoding.UTF8.GetString(x.Description) : string.Empty
                    }).First());

                var data = query
                    .Select(x => new
                    {
                        x.Id,
                        x.Description
                    })
                    .ToList()
                    .Select(x => new
                    {
                        Description = longTexts.ContainsKey(x.Id) && longTexts[x.Id].Description.IsNotEmpty()
                                ? longTexts[x.Id].Description
                                : x.Description,
                        PointCodes = violPoints.ContainsKey(x.Id) ? violPoints[x.Id].PointCodes : null
                    });

                record.Нарушение = data.AggregateWithSeparator(x => x.PointCodes + " - " + x.Description, ", ");

                dataViol.Add(record);
            }

            Report.RegData("Нарушения", dataViol);

            // ПрилагаемыеДокументы
            var annexes = ActCheckAnnexDomain.GetAll()
                .Where(x => x.ActCheck.Id == act.Id)
                .Select(x => new
                {
                    x.Name,
                    x.DocumentDate
                })
                .ToList();
            var j = 0;
            var accompDocs = new List<AccompDoc>();
            foreach (var doc in annexes)
            {
                var record = new AccompDoc
                {
                    НомерПп = ++j,
                    Наименование = doc.Name,
                    ДатаДокумента = doc.DocumentDate.HasValue
                        ? doc.DocumentDate.Value.ToShortDateString()
                        : string.Empty
                };

                accompDocs.Add(record);
            }

            Report.RegData("ПрилагаемыеДокументы", accompDocs);

            Report["ТипКонтрагента"] = act.Inspection.TypeJurPerson.GetEnumMeta().Display;

            FillCheckPeriods(reportParams, act);
        }

        protected class ActCheckViolationRecord
        {
            public int НомерПп { get; set; }
            public string ТипДома { get; set; }
            public string АдресДома { get; set; }
            public string ГодПостройки { get; set; }
            public string ПлощадьМкд { get; set; }
            public string МатериалКровли { get; set; }
            public string МатериалСтен { get; set; }
            public string КоличествоПодъездов { get; set; }
            public string Этажность { get; set; }
            public string ТипКровли { get; set; }
            public string СистемаОтопления { get; set; }
            public string Нарушение { get; set; }
            public string НевыявленныеНарушения { get; set; }
            public string НаличиеНарушения { get; set; }
        }

        protected class AccompDoc
        {
            public int НомерПп { get; set; }
            public string Наименование { get; set; }
            public string ДатаДокумента { get; set; }
        }

        protected void FillCheckPeriods(ReportParams reportParams, DocumentGji doc)
        {
            var periodsCheck = ActCheckPeriodDomain.GetAll().Where(x => x.ActCheck.Id == doc.Id && x.DateStart.HasValue && x.DateEnd.HasValue).ToArray();

            if (periodsCheck.Length == 1)
            {
                var actCheckGjiPeriod = periodsCheck.First();
                var st = actCheckGjiPeriod.DateStart.Value.TimeOfDay;
                var et = actCheckGjiPeriod.DateEnd.Value.TimeOfDay;
                var date1 = actCheckGjiPeriod.DateCheck.HasValue ? actCheckGjiPeriod.DateCheck.Value.ToString("D", new CultureInfo("ru-RU")) : string.Empty;

                Report["ДатаВремяПроверки"] = "{0} с {1} час. {2} мин. до {3} час. {4} мин.".FormatUsing(
                                 date1,
                                 st.Hours,
                                 st.ToString("mm"),
                                 et.Hours,
                                 et.ToString("mm"));

            }
            else if (periodsCheck.Length > 1)
            {
                var startPeriod = periodsCheck.OrderBy(x => x.DateCheck).First();
                var endPeriod = periodsCheck.OrderByDescending(x => x.DateCheck).First();
                var st = startPeriod.DateStart.Value.TimeOfDay;
                var et = endPeriod.DateEnd.Value.TimeOfDay;
                var date1 = startPeriod.DateCheck.HasValue ? startPeriod.DateCheck.Value.ToString("D", new CultureInfo("ru-RU")) : string.Empty;
                var date2 = endPeriod.DateCheck.HasValue ? endPeriod.DateCheck.Value.ToString("D", new CultureInfo("ru-RU")) : string.Empty;

                Report["ДатаВремяПроверки"] = "{0} с {1} час. {2} мин. до {3} {4} час. {5} мин.".FormatUsing(
                                 date1,
                                 st.Hours,
                                 st.ToString("mm"),
                                 date2,
                                 et.Hours,
                                 et.ToString("mm"));
            }

            var dc2 = new StringBuilder();
            var startInspectionDate = new StringBuilder();
            var startInspectionTime = new StringBuilder();
            var endInspectionTime = new StringBuilder();
            var durationStr = new StringBuilder();
            var allDuration = new TimeSpan();
            var durationList = new List<object>();

            foreach (var actCheckGjiPeriod in periodsCheck)
            {

                var st = actCheckGjiPeriod.DateStart.Value.TimeOfDay;
                var et = actCheckGjiPeriod.DateEnd.Value.TimeOfDay;

                var checkLength = et - st;

                var date1 = actCheckGjiPeriod.DateCheck.HasValue ? actCheckGjiPeriod.DateCheck.Value.ToString("D", new CultureInfo("ru-RU")) : string.Empty;

                dc2.AppendFormat(
                    "{0} с {1} час. {2} мин. до {3} час. {4} мин. Продолжительность: {5} час. {6} мин.\n",
                    date1,
                    st.Hours,
                    st.Minutes,
                    et.Hours,
                    et.ToString("mm"),
                    checkLength.Hours,
                    checkLength.Minutes);

                if (actCheckGjiPeriod.DateCheck.HasValue)
                {
                    startInspectionDate.Append(actCheckGjiPeriod.DateCheck.Value.ToString("dd MMMM yyyy"));
                }

                allDuration += checkLength;

                startInspectionTime.Append(actCheckGjiPeriod.DateStart.Value.Hour + " час." + actCheckGjiPeriod.DateStart.Value.Minute + " мин.");
                endInspectionTime.Append(actCheckGjiPeriod.DateEnd.Value.Hour + " час." + actCheckGjiPeriod.DateEnd.Value.Minute + " мин.");
                durationStr.Append(checkLength.Hours + " ч." + checkLength.Minutes + " мин.");

                durationList.Add(
                    new
                        {
                            ДатаПроверки = actCheckGjiPeriod.DateStart.Value.ToString("«dd» MMMM yyyy г.", new CultureInfo("RU-ru")),
                            ВремяНачала = actCheckGjiPeriod.DateStart.Value.ToString("HH:mm"),
                            ВремяОкончания = actCheckGjiPeriod.DateEnd.Value.ToString("HH:mm"),
                            Продолжительность = this.FormatTime(checkLength)
                        });
            }

            Report["ДатаВремяПроверкиПеренос"] = dc2.ToString();
            Report["ДатаПроверки"] = startInspectionDate.ToString();
            Report["ВремяНачала"] = startInspectionTime.ToString();
            Report["ВремяОкончания"] = endInspectionTime.ToString();
            Report["Продолжительность"] = durationStr.ToString();

            Report["ПродолжительностьОбщая"] = this.FormatDuration(allDuration);

            Report.RegData("ПродолжительностьПроверки", durationList);
        }

        private string FormatDuration(TimeSpan timeSpan)
        {
            var timeStr = this.FormatTime(timeSpan);
            return "{0} д.{1}".FormatUsing((int)Math.Ceiling(timeSpan.TotalHours / 8), string.IsNullOrEmpty(timeStr) ? string.Empty : ("/" + timeStr));
        }

        private string FormatTime(TimeSpan timeSpan)
        {
            var timeStr = string.Empty;
            if (timeSpan.Hours > 0 || timeSpan.Minutes > 0)
            {
                if (timeSpan.Hours > 0)
                {
                    timeStr += timeSpan.Hours + " ч. ";
                }

                if (timeSpan.Minutes > 0)
                {
                    timeStr += timeSpan.Minutes + " мин.";
                }
            }

            return timeStr;
        }
    }
}