namespace Bars.GkhGji.Regions.Saha.Report
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using B4.Modules.Reports;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.B4.Utils;

    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Saha.Entities;
    using GkhGji.Report;
    using Slepov.Russian.Morpher;

    /// <summary> Уведомление о проверке из приказа </summary>
    public class DisposalGjiNotificationStimulReport : GjiBaseStimulReport
    {
        #region .ctor

        public DisposalGjiNotificationStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.DisposalNotification))
        {
        }

        #endregion .ctor

        #region Properties

        public override string Id
        {
            get { return "DisposalNotification"; }
        }

        public override string CodeForm
        {
            get { return "Disposal"; }
        }

        public override string Name
        {
            get { return "Уведомление о проверке"; }
        }

        public override string Description
        {
            get { return "Уведомление о проверке (из приказа)"; }
        }

        protected override string CodeTemplate { get; set; }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

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

        #region Fields
        protected long DocumentId;

        protected Disposal disposal;
        #endregion Fields

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

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();

            disposal = disposalDomain.GetAll().FirstOrDefault(x => x.Id == DocumentId);

            if (disposal == null)
            {
                throw new ReportProviderException("Не удалось получить приказ");
            }
        }

        private void GetCodeTemplate()
        {
            CodeTemplate = "BlockGJI_InstructionNotification";
        }

        public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var склонятель = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ==");

            FillCommonFields(disposal);
            this.ReportParams["ВидПроверки"] = disposal.KindCheck != null ? disposal.KindCheck.Name : string.Empty;
            this.ReportParams["КодПроверки"] = disposal.KindCheck != null ? disposal.KindCheck.Code.GetHashCode().ToString() : string.Empty;
            // требованию 52365 неправильно описали перменную поэтому ее невывожу 
            //Report["Вотношении"] = 

            var cultureInfo = new CultureInfo("ru-RU");
            var dateFormat = "«dd» MMMM yyyy г.";

            this.ReportParams["НомерПриказа"] = disposal.DocumentNumber;

            this.ReportParams["ДатаПриказа"] = disposal.DocumentDate.HasValue
                ? disposal.DocumentDate.Value.ToString(dateFormat, cultureInfo)
                : string.Empty;

            this.ReportParams["ПериодС"] = disposal.DateStart.HasValue
                ? disposal.DateStart.Value.ToString(dateFormat, cultureInfo)
                : string.Empty;

            this.ReportParams["ПериодПо"] = disposal.DateEnd.HasValue
                ? disposal.DateEnd.Value.ToString(dateFormat, cultureInfo)
                : string.Empty;

            var surveySubjects = surveySubjectDomain.GetAll()
                                   .Where(x => x.Disposal.Id == disposal.Id)
                                   .Select(x => x.SurveySubject.Name)
                                   .ToList();

            this.ReportParams["ПредметПроверки"] = surveySubjects.Any() ? surveySubjects.Aggregate((x, y) => !string.IsNullOrEmpty(y) ? y + ", " + x : x) : string.Empty;

            var goals = typeSurveyGoalsDomain.GetAll()
                                     .Where(
                                         y =>
                                         typeSurveysDomain.GetAll()
                                                          .Any(
                                                              x =>
                                                              x.Disposal.Id == disposal.Id
                                                              && x.TypeSurvey.Id == y.TypeSurvey.Id))
                                     .Select(x => x.SurveyPurpose.Name)
                                     .ToList();

            this.ReportParams["Цель"] = goals.Any() ? goals.Aggregate((x, y) => !string.IsNullOrEmpty(y) ? y + ", " + x : x) : string.Empty;

            var roList = inspRoDomain.GetAll()
                            .Where(y => y.Inspection.Id == disposal.Inspection.Id)
                            .Select(x => x.RealityObject.Address)
                            .ToList();

            this.ReportParams["Адрес"] = roList.Any() ? roList.Aggregate((x, y) => !string.IsNullOrEmpty(y) ? y + "; " + x : x) : string.Empty;

            this.DataSources.Add(new MetaData
            {
                SourceName = "ПредоставляемыеДокументы",
                MetaType = nameof(Object),
                Data = provDocDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
                    .Select(x => new { ПредоставляемыйДокумент = x.ProvidedDoc.Name })
                    .ToList()
            });
            
            if (disposal.IssuedDisposal != null)
            {
                this.ReportParams["РуководительДолжность"] = disposal.IssuedDisposal.Position;
                this.ReportParams["РуководительФИОСокр"] = disposal.IssuedDisposal.ShortFio;
            }

            if (disposal.ResponsibleExecution != null)
            {
                this.ReportParams["ОтветственныйДолжность"] = disposal.ResponsibleExecution.Position;
                this.ReportParams["ОтветственныйФИОСокр"] = disposal.ResponsibleExecution.ShortFio;
            }

            var firstInspector =
                docInspectorDomain.GetAll()
                                  .Where(x => x.DocumentGji.Id == disposal.Id)
                                  .OrderBy(x => x.Id)
                                  .FirstOrDefault();

            if (firstInspector != null)
            {
                this.ReportParams["ОтветсвенныйИнспекторСокр"] = firstInspector.Inspector.Fio;
                this.ReportParams["ДолжностьИнспектра"] = firstInspector.Inspector.Position;
            }


            var contragent = disposal.Inspection.Contragent;

            if (contragent != null)
            {
                this.ReportParams["УправОРГ"] = contragent.Name;
                this.ReportParams["АдресКонтрагента"] = contragent.FactAddress;
               
                var headContragent = contactDomain.GetAll()
                            .Where(x => x.Contragent.Id == contragent.Id && x.DateStartWork.HasValue
                                         && (x.DateStartWork.Value <= DateTime.Now && (!x.DateEndWork.HasValue || x.DateEndWork.Value >= DateTime.Now)))
                            .FirstOrDefault(x => x.Position != null && (x.Position.Code == "1" || x.Position.Code == "4"));

                if (headContragent != null)
                {
                    var fioRuk = склонятель.Проанализировать(string.Format("{0} {1} {2}", headContragent.Surname, headContragent.Name, headContragent.Patronymic).Trim());
                    reportParams.SimpleReportParams["ФИОРукОрг"] = fioRuk.Дательный;
                }
            }
        }
    }
}
