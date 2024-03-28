namespace Bars.GkhGji.Regions.Saha.Report
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.B4.Utils;

    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Saha.Entities;
	using Bars.GkhGji.Entities.Dict;
	using GkhGji.Report;

	/// <summary>
	/// Отчет для приказа
	/// </summary>
    public class DisposalGjiStimulReport : GjiBaseStimulReport
    {
		/// <summary>
		/// Конструктор
		/// </summary>
        public DisposalGjiStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.Disposal))
        {
        }

        private long DocumentId { get; set; }
        private Disposal Document { get; set; }

		/// <summary>
		/// Адрес
		/// </summary>
        protected string Address { get; set; }

        #region Properties

		/// <summary>
		/// Идентификатор документа
		/// </summary>
        public override string Id
        {
            get { return "Disposal"; }
        }

		/// <summary>
		/// Код формы
		/// </summary>
        public override string CodeForm
        {
            get { return "Disposal"; }
        }

		/// <summary>
		/// Формат экспорта
		/// </summary>
        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

		/// <summary>
		/// Наименование
		/// </summary>
        public override string Name
        {
            get { return this.Container.Resolve<IDisposalText>().SubjectiveCase; }
        }

		/// <summary>
		/// Описание
		/// </summary>
        public override string Description
        {
            get { return this.Container.Resolve<IDisposalText>().SubjectiveCase; }
        }

		/// <summary>
		/// Код шаблона
		/// </summary>
        protected override string CodeTemplate { get; set; }

        #endregion

        #region Templates
		/// <summary>
		/// Получить информацию о шаблонах
		/// </summary>
		/// <returns></returns>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "SahaDisposal_1",
                    Name = "SahaDisposal_1",
                    Description = "Приказы на плановые\\внеплановые проверки,Типы обследования все, кроме 1408,1409, 1410",
                    Template = Properties.Resources.Disposal
                },
                new TemplateInfo
                {
                    Code = "SahaDisposal_3",
                    Name = "SahaDisposal_3",
                    Description = "приказ на осмотр-обследование Виды проверок: 5, 8",
                    Template = Properties.Resources.Disposal
                }
            };
        }

		/// <summary>
		/// Получить шаблон
		/// </summary>
		/// <returns></returns>
        public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
        }

		private void GetCodeTemplate()
        {
            this.CodeTemplate = "SahaDisposal_1";

            if (this.Document.KindCheck != null)
            {
                switch (this.Document.KindCheck.Code)
                {
                    case TypeCheck.Monitoring:
                    case TypeCheck.PlannedDocumentation:
                    case TypeCheck.PlannedExit:
                    case TypeCheck.PlannedDocumentationExit:
                    case TypeCheck.NotPlannedExit:
                    case TypeCheck.NotPlannedDocumentationExit:
                    case TypeCheck.NotPlannedDocumentation:
                        this.CodeTemplate = "SahaDisposal_1";
                        break;

                    case TypeCheck.InspectionSurvey:
                    case TypeCheck.VisualSurvey:
                        this.CodeTemplate = "SahaDisposal_3";
                        break;
                    
                }
            }
        }
        #endregion

        #region DomainServices

		/// <summary>
		/// Домен сервис для Приказ
		/// </summary>
        public IDomainService<Disposal> DisposalDomain { get; set; }

		/// <summary>
		/// Домен сервис для интерфейса описания тектовых значений Распоряжения
		/// </summary>
		public IDisposalText DispText { get; set; }

		/// <summary>
		/// Домен сервис для Инспектор приказа
		/// </summary>
        public IDomainService<DocumentGjiInspector> DispInspectorsDomain { get; set; }

		/// <summary>
		/// Домен сервис для Жилой дом инспекции
		/// </summary>
        public IDomainService<InspectionGjiRealityObject> InspRoDomain { get; set; }

		/// <summary>
		/// Домен сервис для Предоставляемые документы приказа
		/// </summary>
        public IDomainService<DisposalProvidedDoc> DisposalProvidedDocDomain { get; set; }

		/// <summary>
		/// Домен сервис для Эксперт приказа
		/// </summary>
        public IDomainService<DisposalExpert> DisposalExpertDomain { get; set; }

		/// <summary>
		/// Домен сервис для Проверка ГЖИ
		/// </summary>
        public IDomainService<InspectionGji> InspectionGjiDomain { get; set; }

		/// <summary>
		/// Домен сервис для Обращение граждан проверки по обращениям граждан
		/// </summary>
		public IDomainService<InspectionAppealCits> InspectionAppealCitsDomain { get; set; }

		/// <summary>
		/// Домен сервис для Основание плановой проверки юр. лиц ГЖИ
		/// </summary>
		public IDomainService<BaseJurPerson> BaseJurPersonDomain { get; set; }

		/// <summary>
		/// Домен сервис для Источник поступления обращения
		/// </summary>
		public IDomainService<AppealCitsSource> AppealCitsSourceDomain { get; set; }

		/// <summary>
		/// Домен сервис для Тип обследования рапоряжения ГЖИ
		/// </summary>
		public IDomainService<DisposalTypeSurvey> DisposalTypeSurveyDomain { get; set; }

		/// <summary>
		/// Домен сервис для Цель проверки
		/// </summary>
		public IDomainService<TypeSurveyGoalInspGji> TypeSurveyGoalInspGjiDomain { get; set; }

		/// <summary>
		/// Домен сервис для Правовые основания
		/// </summary>
		public IDomainService<TypeSurveyLegalReason> TypeSurveyLegalReasonDomain { get; set; }

		/// <summary>
		/// Домен сервис для Мероприятия по контролю распоряжения ГЖИ
		/// </summary>
		public IDomainService<DisposalControlMeasures> DisposalControlMeasuresDomain { get; set; }

		/// <summary>
		/// Домен сервис для Задача проверки
		/// </summary>
		public IDomainService<TypeSurveyTaskInspGji> TypeSurveyTaskInspGjiDomain { get; set; }

		/// <summary>
		/// Домен сервис для Предмет проверки для приказа 
		/// </summary>
		public IDomainService<DisposalSurveySubject> DisposalSurveySubjectDomain { get; set; }

        #endregion

		/// <summary>
		/// Установить пользовательские параметры
		/// </summary>
		/// <param name="userParamsValues">Пользовательские параметры</param>
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
		/// 
		/// </summary>
		/// <param name="reportParams"></param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var cultureInfo = new CultureInfo("ru-RU");
            var dateFormat = "«dd» MMMM yyyy г.";

            FillCommonFields(Document);

            this.ReportParams["ДатаРаспоряжения"] = Document.DocumentDate.HasValue
                ? Document.DocumentDate.Value.ToString(dateFormat)
                : string.Empty;

            this.ReportParams["НомерРаспоряжения"] = Document.DocumentNumber;


            this.ReportParams["НачалоПериода"] = Document.DateStart.HasValue
                ? Document.DateStart.Value.ToString(dateFormat, cultureInfo)
                : string.Empty;

            this.ReportParams["ОкончаниеПериода"] = Document.DateEnd.HasValue
                ? Document.DateEnd.Value.ToString(dateFormat, cultureInfo)
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

        protected void FillRegionParams(ReportParams reportParams, DocumentGji doc)
        {
            var typeSurveysQuery = DisposalTypeSurveyDomain.GetAll().Where(x => x.Disposal.Id == DocumentId);

            this.ReportParams["Цель"] =
                TypeSurveyGoalInspGjiDomain.GetAll()
                    .Where(x => typeSurveysQuery.Any(y => y.TypeSurvey.Id == x.TypeSurvey.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.TypeSurvey.Id)
                    .SelectMany(x => x)
                    .AggregateWithSeparator(x => x.SurveyPurpose?.Name, ". ");


            this.ReportParams["Задача"] =
                TypeSurveyTaskInspGjiDomain.GetAll()
                    .Where(x => typeSurveysQuery.Any(y => y.TypeSurvey.Id == x.TypeSurvey.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.TypeSurvey.Id)
                    .SelectMany(x => x)
                    .AggregateWithSeparator(x => x.SurveyObjective?.Name, ". ");

            var listInspFoundation = TypeSurveyLegalReasonDomain.GetAll()
                .Where(x => typeSurveysQuery.Any(y => y.TypeSurvey.Id == x.TypeSurveyGji.Id))
                .Select(x => x.LegalReason.Name)
                .ToList();

            this.ReportParams["ПравовоеОснование"] = listInspFoundation
                    .AggregateWithSeparator(x => x, ", ");

            var surveySubjects = DisposalSurveySubjectDomain.GetAll()
                .Where(x => x.Disposal.Id == DocumentId)
                .Select(x => x.SurveySubject.Name)
                .ToList();

            DataSources.Add(new MetaData
            {
                SourceName = "СекцияПредметПроверки",
                Data = surveySubjects.Select(x => new { ПредметПроверки = x }),
                MetaType = nameof(Object),
            });
            
            DataSources.Add(new MetaData
            {
                SourceName = "СекцияПравовыеОснования",
                Data = listInspFoundation.Select(x => new { ПравовоеОснование = x }),
                MetaType = nameof(Object),
            });

            var disposal = (Disposal)doc;

            var listDisposalProvidedDoc =
                DisposalProvidedDocDomain.GetAll().Where(x => x.Disposal.Id == doc.Id).ToList();

            this.ReportParams["ПредоставляемыеДокументы"] = listDisposalProvidedDoc.Select(
                x => !string.IsNullOrEmpty(x.Description) ? x.Description : x.ProvidedDoc.Name)
                .Aggregate(string.Empty,
                    (total, next) => total == string.Empty ? next : total + ", " + next);

            DataSources.Add(new MetaData
            {
                SourceName = "СекцияДокументы",
                Data = listDisposalProvidedDoc.Select(x => new
                    { ПредоставляемыйДокумент = !string.IsNullOrEmpty(x.Description) ? x.Description : x.ProvidedDoc.Name }),
                MetaType = nameof(Object),
            });

            this.ReportParams["ПродолжительностьПроверки"] = Document.DateEnd.HasValue && Document.DateStart.HasValue ? (Document.DateEnd.Value - Document.DateStart.Value).Days.ToStr() : "20";


            var controlMeasures = DisposalControlMeasuresDomain.GetAll()
                .Where(x => x.Disposal.Id == DocumentId)
                .Select(x => x.ControlActivity.Name)
                .ToList();

            DataSources.Add(new MetaData
            {
                SourceName = "МероприятияПоКонтролю",
                Data = controlMeasures.Select(x => new { МероприятиеПоКонтролю = x }),
                MetaType = nameof(Object),
            });

            var listExperts = DisposalExpertDomain.GetAll()
                .Where(x => x.Disposal.Id == disposal.Id)
                .Select(x => new
                {
                    Эксперты = x.Expert.Name
                })
                .ToList();

            if (listExperts.Count == 0)
            {
                listExperts.Add(new
                {
                    Эксперты = "не привлекаются"
                });
            }

            DataSources.Add(new MetaData
            {
                SourceName = "СекцияЭксперты",
                Data = listExperts,
                MetaType = nameof(Object),
            });

            this.ReportParams["Эксперты"] = listExperts
                .Aggregate(string.Empty, (x, y) => x + (string.IsNullOrEmpty(x) ? y.Эксперты : ", " + y.Эксперты));

            var inspectionId = disposal.Inspection.Id;

            var inspectionGjiData =
                InspectionGjiDomain.GetAll()
                    .Where(x => x.Id == inspectionId)
                    .Select(x => new { x.TypeBase, x.InspectionNumber, x.ObjectCreateDate })
                    .FirstOrDefault();

            if (inspectionGjiData != null)
            {
                this.ReportParams["НомерОснованияПроверки"] = inspectionGjiData.InspectionNumber;
                this.ReportParams["ДатаОснованияПроверки"] = inspectionGjiData.ObjectCreateDate.ToString("dd.MM.yyyy г.");
            }

            var appealCitsData =
                InspectionAppealCitsDomain.GetAll()
                    .Where(x => x.Inspection.Id == inspectionId)
                    .Select(
                        x =>
                            new
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

            this.ReportParams["ОснованиеПроверки"] = "{0} {1} {2}".FormatUsing(
                disposal.Inspection.TypeBase.GetEnumMeta().Display,
                disposal.Inspection.InspectionNumber.IsNotEmpty()
                    ? "№" + disposal.Inspection.InspectionNumber
                    : string.Empty,
                appCitDate.HasValue ? "от " + appCitDate.Value.ToShortDateString() : string.Empty);

            var jurPersonPlan =
                BaseJurPersonDomain.GetAll()
                    .Where(x => x.Id == inspectionId)
                    .Select(x => x.Plan.DateStart)
                    .FirstOrDefault();

            this.ReportParams["ПлановыйГод"] = jurPersonPlan.HasValue ? jurPersonPlan.Value.Year.ToStr() : string.Empty;

            var nameGenitive = appealCitsData != null
                ? AppealCitsSourceDomain.GetAll()
                    .Where(x => x.AppealCits.Id == appealCitsData.Id)
                    .Select(
                        x =>
                            new
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
    }
}