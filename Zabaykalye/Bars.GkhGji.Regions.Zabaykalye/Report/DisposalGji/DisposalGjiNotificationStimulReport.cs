namespace Bars.GkhGji.Regions.Zabaykalye.Report
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Zabaykalye.Entities;
    using GkhGji.Report;
    using Slepov.Russian.Morpher;

    /// <summary> Уведомление о проверке из приказа </summary>
    public class DisposalGjiNotificationStimulReport : GjiBaseStimulReport
    {
        /// <summary>
        /// ИД документа.
        /// </summary>
        private long documentId;

        /// <summary>
        /// Приказ (решение).
        /// </summary>
        private Disposal disposal;
        
        #region .ctor

        /// <summary>
        /// Конструктор.
        /// </summary>
        public DisposalGjiNotificationStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.DisposalNotification))
        {
        }

        #endregion .ctor

        #region Properties

        /// <summary>
        /// Идентификатор отчета.
        /// </summary>
        public override string Id
        {
            get
            {
                return "DisposalNotification";
            }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати.
        /// </summary>
        public override string CodeForm
        {
            get
            {
                return "Disposal";
            }
        }

        /// <summary>
        /// Наименование отчета.
        /// </summary>
        public override string Name
        {
            get
            {
                return "Уведомление о проверке";
            }
        }

        /// <summary>
        /// Описание отчета.
        /// </summary>
        public override string Description
        {
            get
            {
                return "Уведомление о проверке (из приказа)";
            }
        }

        /// <summary>
        /// Формат печатной формы.
        /// </summary>
        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        /// <summary>
        /// Код шаблона (файла).
        /// </summary>
        protected override string CodeTemplate { get; set; }

        /// <summary>
        /// Получить список шаблонов.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Code = "BlockGJI_InstructionNotification",
                            Name = "DisposalNotification",
                            Description = "Уведомление о проверке из приказа",
                            Template = Properties.Resources.DisposalNotification
                        }
                };
        }

        #endregion Properties

        #region Injections
        public IDomainService<Disposal> disposalDomain { get; set; }
        public IDomainService<DocumentGjiChildren> docChildrenDomain { get; set; }
        public IDomainService<ContragentContact> contactDomain { get; set; }
        public IDomainService<DisposalSurveySubject> surveySubjectDomain { get; set; }
        public IDomainService<DisposalTypeSurvey> typeSurveysDomain { get; set; }
        public IDomainService<TypeSurveyGoalInspGji> typeSurveyGoalsDomain { get; set; }
        public IDomainService<InspectionGjiRealityObject> inspRoDomain { get; set; }
        public IDomainService<DisposalProvidedDoc> provDocDomain { get; set; }
        public IDomainService<DocumentGjiInspector> docInspectorDomain { get; set; }
        #endregion

        /// <summary>
        /// Установить пользовательские параметры.
        /// </summary>
        /// <param name="userParamsValues">
        /// Значения пользовательских параметров.
        /// </param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.documentId = userParamsValues.GetValue<object>("DocumentId").ToLong();

            this.disposal = this.disposalDomain.GetAll().FirstOrDefault(x => x.Id == this.documentId);

            if (this.disposal == null)
            {
                throw new ReportProviderException("Не удалось получить приказ");
            }
        }

        /// <summary>
        /// Получить поток шаблона отчета (файла).
        /// </summary>
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
        }

        /// <summary>
        /// Подготовить параметры отчета.
        /// </summary>
        /// <param name="reportParams">
        /// Парметры отчета.
        /// </param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var склонятель = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ==");

            FillCommonFields(disposal);

            this.ReportParams["ВидПроверки"] = this.disposal.KindCheck != null ? this.disposal.KindCheck.Name : string.Empty;
            this.ReportParams["КодПроверки"] = this.disposal.KindCheck != null
                                             ? this.disposal.KindCheck.Code.GetHashCode().ToString()
                                             : string.Empty;
            //// требованию 52365 неправильно описали перменную поэтому ее не вывожу 
            //// Report["Вотношении"] = 

            var cultureInfo = new CultureInfo("ru-RU");
            const string DateFormat = "«dd» MMMM yyyy г.";

            this.ReportParams["НомерПриказа"] = this.disposal.DocumentNumber;
            this.ReportParams["ДатаПриказа"] = this.disposal.DocumentDate.HasValue
                ? this.disposal.DocumentDate.Value.ToString(DateFormat, cultureInfo)
                : string.Empty;

            this.ReportParams["ПериодС"] = this.disposal.DateStart.HasValue
                ? this.disposal.DateStart.Value.ToString(DateFormat, cultureInfo)
                : string.Empty;

            this.ReportParams["ПериодПо"] = this.disposal.DateEnd.HasValue
                ? this.disposal.DateEnd.Value.ToString(DateFormat, cultureInfo)
                : string.Empty;

            var surveySubjects = this.surveySubjectDomain.GetAll()
                                   .Where(x => x.Disposal.Id == this.disposal.Id)
                                   .Select(x => x.SurveySubject.Name)
                                   .ToList();

            this.ReportParams["ПредметПроверки"] = surveySubjects.Any()
                                                 ? surveySubjects.Aggregate((x, y) => !string.IsNullOrEmpty(y) ? y + ", " + x : x)
                                                 : string.Empty;

            var goals = this.typeSurveyGoalsDomain.GetAll()
                    .Where(y => this.typeSurveysDomain.GetAll()
                        .Any(x => x.Disposal.Id == this.disposal.Id && x.TypeSurvey.Id == y.TypeSurvey.Id))
                    .Select(x => x.SurveyPurpose.Name)
                    .ToList();

            this.ReportParams["Цель"] = goals.Any()
                                      ? goals.Aggregate((x, y) => !string.IsNullOrEmpty(y) ? y + ", " + x : x)
                                      : string.Empty;

            var roList = this.inspRoDomain.GetAll()
                            .Where(y => y.Inspection.Id == this.disposal.Inspection.Id)
                            .Select(x => x.RealityObject.Address)
                            .ToList();

            this.ReportParams["Адрес"] = roList.Any()
                                       ? roList.Aggregate((x, y) => !string.IsNullOrEmpty(y) ? y + "; " + x : x)
                                       : string.Empty;

            this.DataSources.Add(new MetaData
            {
                SourceName = "ПредоставляемыеДокументы",
                MetaType = nameof(Object),
                Data = this.provDocDomain.GetAll()
                    .Where(x => x.Disposal.Id == this.disposal.Id)
                    .Select(x => new { ПредоставляемыйДокумент = x.ProvidedDoc.Name })
                    .ToList()
            });
            
            if (this.disposal.IssuedDisposal != null)
            {
                this.ReportParams["РуководительДолжность"] = this.disposal.IssuedDisposal.Position;
                this.ReportParams["РуководительФИОСокр"] = this.disposal.IssuedDisposal.ShortFio;
            }

            if (this.disposal.ResponsibleExecution != null)
            {
                this.ReportParams["ОтветственныйДолжность"] = this.disposal.ResponsibleExecution.Position;
                this.ReportParams["ОтветственныйФИОСокр"] = this.disposal.ResponsibleExecution.ShortFio;
            }

            var firstInspector = this.docInspectorDomain.GetAll()
                                  .Where(x => x.DocumentGji.Id == this.disposal.Id)
                                  .OrderBy(x => x.Id)
                                  .FirstOrDefault();

            if (firstInspector != null)
            {
                this.ReportParams["ОтветсвенныйИнспекторСокр"] = firstInspector.Inspector.Fio;
                this.ReportParams["ДолжностьИнспектра"] = firstInspector.Inspector.Position;
            }

            string typeBase;
            switch (this.disposal.Inspection.TypeBase)
            {
                case TypeBase.CitizenStatement:
                    typeBase = "1";
                    break;
                case TypeBase.PlanJuridicalPerson:
                    typeBase = "2";
                    break;
                case TypeBase.ProsecutorsClaim:
                    typeBase = "3";
                    break;
                case TypeBase.DisposalHead:
                    typeBase = "4";
                    break;
                case TypeBase.PlanAction:
                    typeBase = "5";
                    break;
                default:
                    typeBase = string.Empty;
                    break;
            }
            this.ReportParams["ОснованиеПроверки"] = typeBase;

            var contragent = this.disposal.Inspection.Contragent;

            if (contragent != null)
            {
                this.ReportParams["УправОРГ"] = contragent.Name;
                this.ReportParams["АдресКонтрагента"] = contragent.FactAddress;
                this.ReportParams["ИНН"] = contragent.Inn;
                this.ReportParams["ОГРН"] = contragent.Ogrn;

                var headContragent = this.contactDomain.GetAll()
                            .Where(x => x.Contragent.Id == contragent.Id && 
                                x.DateStartWork.HasValue &&
                                (x.DateStartWork.Value <= DateTime.Now && (!x.DateEndWork.HasValue || x.DateEndWork.Value >= DateTime.Now)))
                            .FirstOrDefault(x => x.Position != null && (x.Position.Code == "1" || x.Position.Code == "4"));

                if (headContragent != null)
                {
                    var fioRuk = склонятель.Проанализировать(string.Format("{0} {1} {2}", headContragent.Surname, headContragent.Name, headContragent.Patronymic).Trim());
                    reportParams.SimpleReportParams["ФИОРукОрг"] = fioRuk.Дательный;
                }
            }

            var appealCits = Container.Resolve<IDomainService<InspectionAppealCits>>().GetAll()
                    .Where(x => x.Inspection.Id == this.disposal.Inspection.Id)
                    .Select(x => new
                                     {
                                         НомерОбращения = x.AppealCits.NumberGji,
                                         ДатаОбращения = x.AppealCits.DateFrom.HasValue ? x.AppealCits.DateFrom.Value.ToShortDateString() : string.Empty,
                                         Корреспондент = x.AppealCits.Correspondent
                                     })
                    .ToList();

            this.DataSources.Add(new MetaData
            {
                SourceName = "Обращения",
                MetaType = nameof(Object),
                Data = appealCits
            });
        }
        
        /// <summary>
        /// Установить значение кода шаблона (файла).
        /// </summary>
        private void GetCodeTemplate()
        {
            this.CodeTemplate = "BlockGJI_InstructionNotification";
        }
    }
}
