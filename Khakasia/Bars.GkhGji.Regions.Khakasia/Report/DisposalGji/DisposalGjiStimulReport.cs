namespace Bars.GkhGji.Regions.Khakasia.Report
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhCalendar.Entities;
    using Bars.GkhCalendar.Enums;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
	using Bars.GkhGji.Entities.Dict;
    using Entities;
    using GkhGji.Report;

	public class DisposalGjiStimulReport : GjiBaseStimulReport
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public DisposalGjiStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.Disposal))
        {
        }

        #region DomainServices

        public IDomainService<Disposal> DisposalDomain { get; set; }

        public IDisposalText DispText { get; set; }

        public IDomainService<DocumentGjiInspector> DispInspectorsDomain { get; set; }

        public IDomainService<InspectionGjiRealityObject> InspRoDomain { get; set; }

        public IDomainService<DisposalProvidedDoc> DisposalProvidedDocDomain { get; set; }

        public IDomainService<DisposalExpert> DisposalExpertDomain { get; set; }

        public IDomainService<InspectionGji> InspectionGjiDomain { get; set; }

        public IDomainService<InspectionAppealCits> InspectionAppealCitsDomain { get; set; }

        public IDomainService<BaseJurPerson> BaseJurPersonDomain { get; set; }

        public IDomainService<AppealCitsSource> AppealCitsSourceDomain { get; set; }

        public IDomainService<DisposalTypeSurvey> DisposalTypeSurveyDomain { get; set; }

        public IDomainService<TypeSurveyGoalInspGji> TypeSurveyGoalInspGjiDomain { get; set; }

        public IDomainService<TypeSurveyLegalReason> TypeSurveyLegalReasonDomain { get; set; }

        public IDomainService<DisposalControlMeasures> DisposalControlMeasuresDomain { get; set; }

        public IDomainService<TypeSurveyTaskInspGji> TypeSurveyTaskInspGjiDomain { get; set; }

        public IDomainService<DisposalSurveySubject> DisposalSurveySubjectDomain { get; set; }

        /// <summary>
        /// Домен-сервис для сущности дня из производственного календаря.
        /// </summary>
        public IDomainService<Day> CalendarDayDomain { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get { return "Disposal"; }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get { return "Disposal"; }
        }

        /// <summary>
        /// Формат печатной формы
        /// </summary>
        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        /// <summary>
        /// Наименование отчета.
        /// </summary>
        public override string Name
        {
            get { return Container.Resolve<IDisposalText>().SubjectiveCase; }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get { return Container.Resolve<IDisposalText>().SubjectiveCase; }
        }

        /// <summary>
        /// Код шаблона (файла)
        /// </summary>
        protected override string CodeTemplate { get; set; }

        /// <summary>
        /// Адрес.
        /// </summary>
        protected string Address { get; set; }

        /// <summary>
        /// ИД документа.
        /// </summary>
        private long DocumentId { get; set; }

        /// <summary>
        /// Распоряжение (приказ) ГЖИ.
        /// </summary>
        private Disposal Document { get; set; }

        #endregion

        /// <summary>
        /// Получить список шаблонов.
        /// </summary>
        /// <returns>
        ///  Список шаблонов.
        /// </returns>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "KhakasiaDisposal_1",
                    Name = "KhakasiaDisposal_1",
                    Description = "Приказы на плановые\\внеплановые проверки,Типы обследования все, кроме 1408,1409, 1410",
                    Template = Properties.Resources.Disposal
                },
                new TemplateInfo
                {
                    Code = "KhakasiaDisposal_3",
                    Name = "KhakasiaDisposal_3",
                    Description = "приказ на осмотр-обследование Виды проверок: 5, 8",
                    Template = Properties.Resources.Disposal
                }
            };
        }

        /// <summary>
        /// Получить поток шаблона отчета (файла).
        /// </summary>
        /// <returns>
        /// Поток.
        /// </returns>
        public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
        }

        /// <summary>
        /// Установить пользовательские параметры.
        /// </summary>
        /// <param name="userParamsValues">
        /// Значения пользовательских параметров.
        /// </param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();

            Document = DisposalDomain.GetAll().FirstOrDefault(x => x.Id == DocumentId);

            if (Document == null)
            {
                throw new ReportProviderException(string.Format("Не удалось получить {0}",
                    DispText.SubjectiveCase.ToLower()));
            }
        }

        /// <summary>
        /// Подготовить параметры отчета.
        /// </summary>
        /// <param name="reportParams">
        /// Параметры отчета.
        /// </param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var cultureInfo = new CultureInfo("ru-RU");
            const string DateFormat = "«dd» MMMM yyyy г.";

            FillCommonFields(Document);

            this.ReportParams["ДатаРаспоряжения"] = Document.DocumentDate.HasValue
                ? Document.DocumentDate.Value.ToString(DateFormat)
                : string.Empty;

            this.ReportParams["НомерРаспоряжения"] = Document.DocumentNumber;


            this.ReportParams["НачалоПериода"] = Document.DateStart.HasValue
                ? Document.DateStart.Value.ToString(DateFormat, cultureInfo)
                : string.Empty;

            this.ReportParams["ОкончаниеПериода"] = Document.DateEnd.HasValue
                ? Document.DateEnd.Value.ToString(DateFormat, cultureInfo)
                : string.Empty;

            if (Document.KindCheck != null)
            {
                var kindCheck = string.Empty;

                switch (Document.KindCheck.Code)
                {
                    case TypeCheck.PlannedExit:
                        kindCheck = "плановой выездной";
                        break;
                    case TypeCheck.NotPlannedExit:
                        kindCheck = "внеплановой выездной";
                        break;
                    case TypeCheck.PlannedDocumentation:
                        kindCheck = "плановой документарной";
                        break;
                    case TypeCheck.NotPlannedDocumentation:
                        kindCheck = "внеплановой документарной";
                        break;
                    case TypeCheck.PlannedDocumentationExit:
                        kindCheck = "плановой документарной и выездной";
                        break;
                    case TypeCheck.VisualSurvey:
                        kindCheck = "о внеплановой проверке технического состояния жилого помещения";
                        break;
                    case TypeCheck.NotPlannedDocumentationExit:
                        kindCheck = "внеплановой документарной и выездной";
                        break;
                    case TypeCheck.InspectionSurvey:
                        kindCheck = "инспекционной";
                        break;
                }

                this.ReportParams["КодВидаПроверки"] = Document.KindCheck.Code.GetHashCode().ToStr();
                this.ReportParams["ВидПроверки"] = kindCheck;
                this.ReportParams["ОснованиеПроверки"] = Document.Inspection != null
                                             ? Convert.ChangeType(
                                                 Document.Inspection.TypeBase,
                                                 Document.Inspection.TypeBase.GetTypeCode()).ToStr()
                                             : string.Empty;

                this.ReportParams["ВидОбследования"] = Document.KindCheck.Name.ToLower();
            }

            this.ReportParams["ТипКонтрагента"] = Document.Inspection.TypeJurPerson.GetEnumMeta().Display;

            if (Document.Inspection.Contragent != null)
            {
                this.ReportParams["УправОрг"] = Document.Inspection.Contragent.Name;
                this.ReportParams["ОГРН"] = Document.Inspection.Contragent.Ogrn;
                this.ReportParams["УправОргРП"] = Document.Inspection.Contragent.NameGenitive;
                this.ReportParams["УправОргСокр"] = Document.Inspection.Contragent.ShortName;
                this.ReportParams["ИНН"] = Document.Inspection.Contragent.Inn;

                if (Document.Inspection.Contragent.FiasJuridicalAddress != null)
                {
                    var subStr = Document.Inspection.Contragent.FiasJuridicalAddress.AddressName.Split(',');

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

                    this.ReportParams["АдресКонтрагента"] = newAddr.ToString();

                    var fiasAddr = Document.Inspection.Contragent.FiasJuridicalAddress;

                    var addrExceptMu = new StringBuilder();

                    var addSeparator = false;

                    if (!string.IsNullOrEmpty(fiasAddr.PlaceName))
                    {
                        addrExceptMu.Append(fiasAddr.PlaceName);
                        addSeparator = true;
                    }

                    if (!string.IsNullOrEmpty(fiasAddr.StreetName))
                    {
                        addrExceptMu.Append(addSeparator ? ", " + fiasAddr.StreetName : fiasAddr.StreetName);
                    }

                    if (!string.IsNullOrEmpty(fiasAddr.House))
                    {
                        addrExceptMu.Append(addSeparator ? ", д. " + fiasAddr.House : "д. " + fiasAddr.House);
                    }

                    if (!string.IsNullOrEmpty(fiasAddr.Housing))
                    {
                        addrExceptMu.Append(addSeparator ? ", корп. " + fiasAddr.Housing : "корп. " + fiasAddr.Housing);
                    }

                    if (!string.IsNullOrEmpty(fiasAddr.Building))
                    {
                        addrExceptMu.Append(addSeparator ? ", секц. " + fiasAddr.Building : "секц. " + fiasAddr.Building);
                    }

                    if (!string.IsNullOrEmpty(fiasAddr.Flat))
                    {
                        addrExceptMu.Append(addSeparator ? ", кв." + fiasAddr.Flat : "кв." + fiasAddr.Flat);
                    }
                }
                else
                {
                    this.ReportParams["АдресКонтрагента"] = Document.Inspection.Contragent.AddressOutsideSubject;
                }

                this.ReportParams["АдресКонтрагентаФакт"] = Document.Inspection.Contragent.FactAddress;
            }

            var inspectors = DispInspectorsDomain.GetAll()
                .Where(x => x.DocumentGji.Id == Document.Id)
                .Select(x => x.Inspector)
                .ToList();

            if (inspectors.Any())
            {
                var inspectorsAndCodes = inspectors.Any()
                    ? inspectors
                        .Aggregate(string.Empty, (x, y) => x + (string.IsNullOrEmpty(x)
                            ? string.Format("{0} - {1}", y.Fio, y.Position)
                            : string.Format(", {0} - {1}", y.Fio, y.Position)))
                    : string.Empty;

                this.ReportParams["ИнспекторыИКоды"] = inspectorsAndCodes;

                var firstInspector = inspectors.FirstOrDefault();
                if (firstInspector != null)
                {
                    this.ReportParams["ТелефонИнспектора"] = firstInspector.Phone;
                    this.ReportParams["Инспектор"] = firstInspector.Fio;
                    this.ReportParams["Должность"] = firstInspector.Position;
                    this.ReportParams["ИнспекторДП"] = !string.IsNullOrEmpty(firstInspector.FioDative) ? firstInspector.FioDative : firstInspector.Fio;
                    this.ReportParams["ДолжностьДП"] = !string.IsNullOrEmpty(firstInspector.PositionDative) ? firstInspector.PositionDative : firstInspector.Position;
                    this.ReportParams["ДолжностьИнспектораТП"] = "{0} - {1}".FormatUsing(firstInspector.PositionAblative, firstInspector.FioAblative);
                }

                this.ReportParams["ДолжностьИнспектораВП"] =
                    inspectors.Select(x => "{0} - {1}".FormatUsing(x.PositionAccusative, x.FioAccusative))
                        .AggregateWithSeparator(", ");
            }

            if (Document.IssuedDisposal != null)
            {
                this.ReportParams["РуководительДолжность"] = Document.IssuedDisposal.Position;
                this.ReportParams["РуководительФИОСокр"] = Document.IssuedDisposal.ShortFio;
            }

            if (Document.ResponsibleExecution != null)
            {
                this.ReportParams["Ответственный"] = Document.ResponsibleExecution.Fio;
                this.ReportParams["ДолжностьОтветственныйРП"] = Document.ResponsibleExecution.PositionGenitive;

                // именно в винительном падеже
                this.ReportParams["ФИООтветственныйСокрРП"] = Document.ResponsibleExecution.FioGenitive;

                this.ReportParams["ДолжностьФИОРП"] = Document.ResponsibleExecution.Position;
            }

            var roData =
                InspRoDomain.GetAll()
                            .Where(x => x.Inspection.Id == Document.Inspection.Id && x.RealityObject != null)
                            .Select(x => new { MoNAme = x.RealityObject.Municipality.Name, x.RealityObject.Address })
                            .AsEnumerable()
                            .Select(x => x.Address.Contains(x.MoNAme) ? x.Address : x.MoNAme + "," + x.Address)
                            .AsEnumerable();

            if (roData.Any())
            {
                this.ReportParams["ДомаИАдреса"] = roData.AggregateWithSeparator(x => x, ", ");
                this.ReportParams["АдресОбр"] = roData.FirstOrDefault();
            }
            
            FillRegionParams(reportParams, Document);
        }

        /// <summary>
        /// Заполнить региональные параметры.
        /// </summary>
        /// <param name="reportParams">
        /// Параметры отчета.
        /// </param>
        /// <param name="doc">
        /// Документ ГЖИ.
        /// </param>
        protected void FillRegionParams(ReportParams reportParams, DocumentGji doc)
        {
            var typeSurveysQuery = DisposalTypeSurveyDomain.GetAll().Where(x => x.Disposal.Id == DocumentId);

            this.ReportParams["Цель"] = TypeSurveyGoalInspGjiDomain.GetAll()
                .Where(x => typeSurveysQuery.Any(y => y.TypeSurvey.Id == x.TypeSurvey.Id))
                .AsEnumerable()
                .GroupBy(x => x.TypeSurvey.Id)
                .SelectMany(x => x)
                .AggregateWithSeparator(x => x.SurveyPurpose.Name, ". ");

            this.ReportParams["Задача"] = TypeSurveyTaskInspGjiDomain.GetAll()
                .Where(x => typeSurveysQuery.Any(y => y.TypeSurvey.Id == x.TypeSurvey.Id))
                .AsEnumerable()
                .GroupBy(x => x.TypeSurvey.Id)
                .SelectMany(x => x)
                .AggregateWithSeparator(x => x.SurveyObjective.Name, ". ");

            var listInspFoundation = TypeSurveyLegalReasonDomain.GetAll()
                .Where(x => typeSurveysQuery.Any(y => y.TypeSurvey.Id == x.TypeSurveyGji.Id))
                .Select(x => x.LegalReason.Name)
                .ToList();

            this.ReportParams["ПравовоеОснование"] = listInspFoundation.AggregateWithSeparator(x => x, ", ");

            var surveySubjects = DisposalSurveySubjectDomain.GetAll()
                .Where(x => x.Disposal.Id == DocumentId)
                .Select(x => x.SurveySubject.Name)
                .ToList();

            this.DataSources.Add(new MetaData
            {
                SourceName = "СекцияПредметПроверки",
                MetaType = nameof(Object),
                Data = surveySubjects.Select(x => new { ПредметПроверки = x })
            });

            this.DataSources.Add(new MetaData
            {
                SourceName = "СекцияПравовыеОснования",
                MetaType = nameof(Object),
                Data = listInspFoundation.Select(x => new { ПравовоеОснование = x })
            });

            var disposal = (Disposal)doc;

            var listDisposalProvidedDoc = DisposalProvidedDocDomain.GetAll()
                .Where(x => x.Disposal.Id == doc.Id).ToList();

            this.ReportParams["ПредоставляемыеДокументы"] = listDisposalProvidedDoc.Select(
                x => !string.IsNullOrEmpty(x.Description) ? x.Description : x.ProvidedDoc.Name)
                .Aggregate(string.Empty,
                    (total, next) => total == string.Empty ? next : total + ", " + next);

            this.DataSources.Add(new MetaData
            {
                SourceName = "СекцияДокументы",
                MetaType = nameof(Object),
                Data = listDisposalProvidedDoc.Select(x => new
                {
                    ПредоставляемыйДокумент = !string.IsNullOrEmpty(x.Description) ? x.Description : x.ProvidedDoc.Name
                })
            });

            this.ReportParams["ПродолжительностьПроверки"] = this.GetDurationInDays();

            var controlMeasures = DisposalControlMeasuresDomain.GetAll()
                .Where(x => x.Disposal.Id == DocumentId)
                .Select(x => x.ControlActivity.Name)
                .ToList();

            this.DataSources.Add(new MetaData
            {
                SourceName = "МероприятияПоКонтролю",
                MetaType = nameof(Object),
                Data = controlMeasures.Select(x => new { МероприятиеПоКонтролю = x })
            });

            var listExperts = DisposalExpertDomain.GetAll()
                .Where(x => x.Disposal.Id == disposal.Id)
                .Select(x => new { Эксперты = x.Expert.Name })
                .ToList();

            if (listExperts.Count == 0)
            {
                listExperts.Add(new { Эксперты = "не привлекаются" });
            }

            this.DataSources.Add(new MetaData
            {
                SourceName = "СекцияЭксперты",
                MetaType = nameof(Object),
                Data = listExperts
            });

            var appealCits = Container.Resolve<IDomainService<InspectionAppealCits>>().GetAll()
                .Where(x => x.Inspection.Id == disposal.Inspection.Id)
                .ToList()
                .Select(x => new
                                {
                                    НомерОбращения = "№ " + x.AppealCits.NumberGji, 
                                    ДатаОбращения = x.AppealCits.DateFrom.HasValue ? "от " + x.AppealCits.DateFrom.Value.ToShortDateString() : string.Empty,
                                    Корреспондент = x.AppealCits.Correspondent
                                })
                .ToList();

            this.DataSources.Add(new MetaData
            {
                SourceName = "Обращения",
                MetaType = nameof(Object),
                Data = appealCits
            });

            this.ReportParams["Эксперты"] = listExperts
                .Aggregate(string.Empty, (x, y) => x + (string.IsNullOrEmpty(x) ? y.Эксперты : ", " + y.Эксперты));

            var inspectionId = disposal.Inspection.Id;

            var inspectionGjiData = InspectionGjiDomain.GetAll()
                .Where(x => x.Id == inspectionId)
                .Select(x => new { x.TypeBase, x.InspectionNumber, x.ObjectCreateDate })
                .FirstOrDefault();

            if (inspectionGjiData != null)
            {
                this.ReportParams["НомерОснованияПроверки"] = inspectionGjiData.InspectionNumber;
                this.ReportParams["ДатаОснованияПроверки"] = inspectionGjiData.ObjectCreateDate.ToString("dd.MM.yyyy г.");
            }

            var appealCitsData = InspectionAppealCitsDomain.GetAll()
                .Where(x => x.Inspection.Id == inspectionId)
                .Select(x => new
                        {
                            x.AppealCits.Id,
                            x.AppealCits.NumberGji,
                            x.AppealCits.DateFrom,
                            x.AppealCits.Correspondent,
                            x.AppealCits.TypeCorrespondent
                        })
                .FirstOrDefault();
            
            DateTime? appCitDate = null;
            if (appealCitsData != null)
            {
                appCitDate = appealCitsData.DateFrom;
                this.ReportParams["НомерОбращения"] = appealCitsData.NumberGji;
                this.ReportParams["ДатаОбращения"] = appealCitsData.DateFrom.HasValue
                    ? appealCitsData.DateFrom.Value.ToString("dd.MM.yyyy г.")
                    : string.Empty;
            }

            this.ReportParams["ТипОснования"] = "{0} {1} {2}".FormatUsing(
                disposal.Inspection.TypeBase.GetEnumMeta().Display,
                disposal.Inspection.InspectionNumber.IsNotEmpty()
                    ? "№" + disposal.Inspection.InspectionNumber
                    : string.Empty,
                appCitDate.HasValue ? "от " + appCitDate.Value.ToShortDateString() : string.Empty);

            var jurPersonPlan = BaseJurPersonDomain.GetAll()
                .Where(x => x.Id == inspectionId)
                .Select(x => x.Plan.DateStart)
                .FirstOrDefault();

            this.ReportParams["ПлановыйГод"] = jurPersonPlan.HasValue ? jurPersonPlan.Value.Year.ToStr() : string.Empty;

            var nameGenitive = appealCitsData != null
                ? AppealCitsSourceDomain.GetAll()
                    .Where(x => x.AppealCits.Id == appealCitsData.Id)
                    .Select(x => new
                            {
                                x.RevenueSource.Name,
                                x.RevenueSource.NameGenitive
                            })
                    .FirstOrDefault()
                : null;

            if (nameGenitive != null)
            {
                this.ReportParams["ИсточникОбращения"] = nameGenitive.ReturnSafe(x => x.Name);
            }

            if ((disposal.Inspection.TypeBase == TypeBase.PlanJuridicalPerson) &&
                (disposal.TypeDisposal != TypeDisposalGji.DocumentGji))
            {
                var countDays = BaseJurPersonDomain.GetAll()
                    .Where(x => x.Id == disposal.Inspection.Id)
                    .Select(x => x.CountDays)
                    .FirstOrDefault();
                reportParams.SimpleReportParams["СрокПроверки"] = countDays != null
                    ? countDays.ToString()
                    : string.Empty;
            }
            else
            {
                reportParams.SimpleReportParams["СрокПроверки"] = "1 рабочий день";
            }

            var inspectionBaseInfo = string.Empty;
            var docNumber = inspectionGjiData != null ? inspectionGjiData.InspectionNumber : string.Empty;
            var docDate = inspectionGjiData != null ? inspectionGjiData.ObjectCreateDate.ToString("dd.MM.yyyy г.") : string.Empty;

            switch (disposal.Inspection.TypeBase)
            {
                case TypeBase.CitizenStatement: // Основание - Обращения граждан
                    inspectionBaseInfo = string.Format("обращение гр. {0}, адрес: {1} (вх.№ {2} от {3}).",
                        appealCitsData != null ? appealCitsData.Correspondent : string.Empty,
                        Address,
                        docNumber, // {НомерТребования}
                        docDate); // {ДатаТребования}
                    break;
                case TypeBase.ProsecutorsClaim: // Основание - проверка по требованию прокурора
                    inspectionBaseInfo =
                        string.Format(
                            "проверка фактов нарушений обязательных требований жилищного законодательства в отношении " +
                            "многоквартирного дома, расположенного по адресу: {0} на основании " +
                            "требования прокурора № {1} от {2}.",
                            Address,
                            docNumber, // {НомерТребования}
                            docDate); // {ДатаТребования}
                    break;
                case TypeBase.DisposalHead: // Основание - поручения руководителя
                    inspectionBaseInfo = string.Format("проверка фактов нарушений обязательных требований жилищного " +
                                                       "законодательства в отношении многоквартирного дома, расположенного по адресу: " +
                                                       "{0} на основании поручения Президента РФ №{1} от {2}",
                                                       Address,
                                                       docNumber, // {Номер}
                                                       docDate); // {Дата}
                    // {Номер} и {Дата} из групбокса "документ" в приказе
                    break;
            }

            this.ReportParams["Основание"] = inspectionBaseInfo;
        }

        /// <summary>
        /// Установить код шаблона (файла).
        /// </summary>
        private void GetCodeTemplate()
        {
            CodeTemplate = "KhakasiaDisposal_1";

            if (Document.KindCheck != null)
            {
                switch (Document.KindCheck.Code)
                {
                    case TypeCheck.Monitoring:
                    case TypeCheck.PlannedDocumentation:
                    case TypeCheck.PlannedExit:
                    case TypeCheck.PlannedDocumentationExit:
                    case TypeCheck.NotPlannedExit:
                    case TypeCheck.NotPlannedDocumentationExit:
                    case TypeCheck.NotPlannedDocumentation:
                        CodeTemplate = "KhakasiaDisposal_1";
                        break;

                    case TypeCheck.InspectionSurvey:
                    case TypeCheck.VisualSurvey:
                        CodeTemplate = "KhakasiaDisposal_3";
                        break;

                }
            }
        }

        /// <summary>
        /// Получить продолжительность проверки в рабочих днях.
        /// </summary>
        /// <remarks>
        /// Для получения количества рабочих дней используется производственный календарь.
        /// </remarks>
        /// <returns>
        /// Количество рабочих дней проверки.
        /// </returns>
        private string GetDurationInDays()
        {
            var countWorkDay = 0;

            if (this.Document.DateEnd.HasValue && this.Document.DateStart.HasValue)
            {
                countWorkDay = this.CalendarDayDomain.GetAll()
                        .Where(x => x.DayDate >= this.Document.DateStart && x.DayDate <= this.Document.DateEnd)
                        .Count(x => x.DayType == DayType.Workday);
            }

            if (countWorkDay <= 20)
            {
                return countWorkDay.ToStr();
            }
            
            // Проверка должна быть не более 20 рабочих дней.
            return "20";
        }
    }
}