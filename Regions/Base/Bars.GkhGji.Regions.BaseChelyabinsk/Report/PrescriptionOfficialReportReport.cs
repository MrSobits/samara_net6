namespace Bars.GkhGji.Regions.BaseChelyabinsk.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Entities;
    using Utils;
    using Gkh.Report;
    using System;
    using B4;

    using Bars.B4.Modules.Analytics.Reports.Enums;

    using Gkh.Entities;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;

    /// <summary>
    /// Извещение
    /// </summary>
    public class PrescriptionOfficialReportReport : GkhBaseStimulReport
    {
        #region .ctor

        /// <summary>
        /// .ctor
        /// </summary>
        public PrescriptionOfficialReportReport()
            : base(new ReportTemplateBinary(Properties.Resources.PrescriptionOfficialReport))
        {
        }

        #endregion .ctor

        #region Private fields

        private long _prescriptionOfficialReportId;

        #endregion Private fields

        #region Protected properties

        /// <summary>
        /// Код шаблона (файла)
        /// </summary>
        protected override string CodeTemplate { get; set; }

        #endregion Protected properties

        #region Public properties

        /// <summary>
        /// Наименование отчета
        /// </summary>
        public override string Name
        {
            get { return "Служебная записка"; }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get { return "Служебная записка исполнения предписания"; }
        }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get { return "PrescriptionOfficialReport"; }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get { return "PrescriptionOfficialReport"; }
        }

        /// <summary>Формат печатной формы</summary>
        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
            set { }
        }

        #endregion Public properties

        #region Public methods

        /// <summary>
        /// Подготовить параметры отчета
        /// </summary>
        /// <param name="reportParams"></param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var ChildrenDomain = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var disposalDomain = this.Container.Resolve<IDomainService<Disposal>>();
            var prescriptionOfficialReportDomain = Container.ResolveDomain<PrescriptionOfficialReport>();
            var prescriptionOfficialReporViolDomain = Container.ResolveDomain<PrescriptionOfficialReportViolation>();
            var appealCitsExecutantDomain = Container.ResolveDomain<AppealCitsExecutant>();
            var inspectionAppCitDomain = this.Container.ResolveDomain<InspectionAppealCits>(); 

             var data = prescriptionOfficialReportDomain.GetAll()
                .Where(x => x.Id == _prescriptionOfficialReportId).FirstOrDefault();

            var realityObjects = prescriptionOfficialReporViolDomain.GetAll()
                .Where(x => x.PrescriptionOfficialReport.Id == _prescriptionOfficialReportId)
                .Select(x => x.PrescriptionViol.InspectionViolation.RealityObject).Distinct().ToList();
            string realityAddresses = realityObjects.AggregateWithSeparator(x=> x.Municipality.Name + ", " + x.Address, "; ");
            decimal realityArea = realityObjects.SafeSum(x => x.AreaMkd.HasValue ? x.AreaMkd.Value:0);
            var parentDocument = GetParentDocumentByType(ChildrenDomain, (DocumentGji)data.Prescription, TypeDocumentGji.Disposal);
            var parentDisposal = disposalDomain.Get(parentDocument.Id);
            var appCit = inspectionAppCitDomain.GetAll().Where(x => x.Inspection == parentDisposal.Inspection).Select(x => x.AppealCits).FirstOrDefault();
            var executants = appealCitsExecutantDomain.GetAll().Where(x => x.AppealCits == appCit).FirstOrDefault();

            var executant = appCit.Executant;
            try
            {
                this.ReportParams["СЗ_Номер"] = data.DocumentNumber;
                this.ReportParams["СЗ_Дата"] = data.DocumentDate.HasValue ? data.DocumentDate.Value.ToString("dd.MM.yyyy") : "";
                this.ReportParams["Предп_Номер"] = data.Prescription.DocumentNumber;
                this.ReportParams["Предп_Дата"] = data.Prescription.DocumentDate.HasValue ? data.Prescription.DocumentDate.Value.ToString("dd.MM.yyyy") : "";
                this.ReportParams["ИнспекторРод"] = data.Inspector!= null? data.Inspector.FioAblative:"";
                this.ReportParams["Инспектор"] = data.Inspector != null ? data.Inspector.Fio : "";
                if (executants != null)
                {
                    //Report["Начальник"] = executant.PositionDative + " " + executant.FioDative;
                    //Report["НачальникФИО"] = executant.Fio;
                    this.ReportParams["Начальник"] = executants.Executant.PositionDative + " " + executants.Executant.FioDative;
                    this.ReportParams["НачальникФИО"] = executants.Executant.Fio;
                }
                else if (parentDisposal != null && parentDisposal.IssuedDisposal != null)
                {
                    this.ReportParams["Начальник"] = executant.PositionDative + " " + executant.FioDative;
                    this.ReportParams["НачальникФИО"] = executant.Fio;
                }
                this.ReportParams["ОтИнспектора"] = data.Inspector != null ? data.Inspector.PositionAccusative + " " + data.Inspector.FioAblative : "";
                this.ReportParams["Контрагент_Наименование"] = data.Prescription.Contragent != null? data.Prescription.Contragent.Name:"";
                this.ReportParams["Контрагент_ИНН"] = data.Prescription.Contragent != null ? data.Prescription.Contragent.Inn : "";
                this.ReportParams["Контрагент_ОГРН"] = data.Prescription.Contragent != null ? data.Prescription.Contragent.Ogrn : "";
                this.ReportParams["Выполнение"] = data.YesNo == Gkh.Enums.YesNo.Yes? "Нарушения устранены": "Нарушения не устранены";
                this.ReportParams["Адрес"] = realityAddresses;
                this.ReportParams["Площадь"] = Decimal.Round(realityArea,2).ToString();

            }
            finally
            {

            }

          
        }

        /// <summary>
        /// Получение первого родительского документа с указанным типом
        /// </summary>
        /// <param name="service">DomainService</param>
        /// <param name="document">Дочерний документ</param>
        /// <param name="type">Тип документа, который необходимо получить</param>
        /// <returns></returns>
        public static DocumentGji GetParentDocumentByType(IDomainService<DocumentGjiChildren> service, DocumentGji document, TypeDocumentGji type)
        {
            if (document != null && document.TypeDocumentGji != type)
            {
                var parentDocs = service.GetAll()
                        .Where(x => x.Children.Id == document.Id)
                        .Select(x => x.Parent)
                        .ToList();

                foreach (var doc in parentDocs)
                {
                    document = GetParentDocumentByType(service, doc, type);
                }
            }

            if (document == null || document.TypeDocumentGji != type)
                return null;
            return document;
        }

        /// <summary>
        /// Установить пользовательские параметры
        /// </summary>
        /// <param name="userParamsValues"></param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            _prescriptionOfficialReportId = userParamsValues.GetValue<long>("Id");
        }

        /// <summary>
        /// Получить список шаблонов
        /// </summary>
        /// <returns></returns>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Name = "Служебная записка",
                    Description = "Служебная записка для предписания",
                    Code = "PrescriptionOfficialReport",
                    Template = Properties.Resources.PrescriptionOfficialReport
                }
            };
        }

        #endregion Public methods
    }
}