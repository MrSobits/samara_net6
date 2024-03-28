namespace Bars.GkhGji.Report
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

    /// <summary>
    /// Извещение
    /// </summary>
    public class RenewalApplicationReport : GkhBaseStimulReport
    {
        #region .ctor

        /// <summary>
        /// .ctor
        /// </summary>
        public RenewalApplicationReport()
            : base(new ReportTemplateBinary(Properties.Resources.RenewalApplicationReport))
        {
        }

        #endregion .ctor

        #region Private fields

        private long _renewalApplicationReportId;

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
            get { return "Решение о продлении"; }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get { return "Решение о продлении (переносе) срока исполнения предписания"; }
        }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get { return "RenewalApplication"; }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get { return "Prescription"; }
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
            var prescriptionDomain = this.Container.Resolve<IDomainService<Prescription>>();
            var prescriptionAnnexDomain = Container.ResolveDomain<PrescriptionAnnex>();
            var prescriptionViolDomain = Container.ResolveDomain<PrescriptionViol>();
            var disposalDomain = this.Container.Resolve<IDomainService<Disposal>>();
            var zonalDomain = this.Container.Resolve<IDomainService<ZonalInspectionInspector>>();
            var ChildrenDomain = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();

           

            try
            {
                var prescription = prescriptionDomain.GetAll()
               .Where(x => x.Id == _renewalApplicationReportId).FirstOrDefault();

                var renewalAnnex = prescriptionAnnexDomain.GetAll()
                    .Where(x => x.Prescription.Id == _renewalApplicationReportId)
                    .Where(x => x.TypePrescriptionAnnex == TypePrescriptionAnnex.RenewalApplication).FirstOrDefault();

                var violationsList = prescriptionViolDomain.GetAll()
                    .Where(x => x.Document.Id == _renewalApplicationReportId)
                    .Where(x => x.DatePlanExtension.HasValue).ToList();

                var parentDocument = GetParentDocumentByType(ChildrenDomain, (DocumentGji)prescription, TypeDocumentGji.Disposal);
                var parentDisposal = disposalDomain.Get(parentDocument.Id);

                var zonal = zonalDomain.GetAll().Where(x => x.Inspector != null && x.Inspector == parentDisposal.IssuedDisposal).FirstOrDefault();
                var str = parentDisposal.IssuedDisposal.Position + (zonal != null ? " " + zonal.ZonalInspection.NameGenetive : "");
                this.ReportParams["НачальникДолжность"] = parentDisposal.IssuedDisposal.Position + (zonal != null ? " " + zonal.ZonalInspection.NameGenetive : "");
                this.ReportParams["НачальникФИО"] = parentDisposal.IssuedDisposal.Fio;
                this.ReportParams["ДатаРешения"] = prescription.RenewalApplicationDate.HasValue ? prescription.RenewalApplicationDate.Value.ToString("dd:MM:yyyy") : DateTime.Now.ToString("dd:MM:yyyy");
                this.ReportParams["Контрагент"] = prescription.Contragent != null ? prescription.Contragent.Name : "";
                if (renewalAnnex != null)
                {
                    this.ReportParams["Ходатайство"] = renewalAnnex.DocumentDate.HasValue ? renewalAnnex.DocumentDate.Value.ToString("dd:MM:yyyy") + " №" + renewalAnnex.Number : "№ " + renewalAnnex.Number;
                }
                this.ReportParams["НомерПредписания"] = prescription.DocumentNumber;
                this.ReportParams["ДатаПредписания"] = prescription.DocumentDate.HasValue ? prescription.DocumentDate.Value.ToString("dd:MM:yyyy") : "";

                var maxDate = violationsList.Max(x => x.DatePlanExtension);

                this.ReportParams["ДатаПродления"] = maxDate.HasValue ? maxDate.Value.ToString("dd:MM:yyyy") : "";

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
            _renewalApplicationReportId = userParamsValues.GetValue<long>("DocumentId");
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
                    Description = "Решение о продлении",
                    Code = "RenewalApplication",
                    Template = Properties.Resources.RenewalApplicationReport
                }
            };
        }

        #endregion Public methods
    }
}