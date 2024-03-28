namespace Bars.GkhGji.Report
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;

    public class PrescriptionGjiCancelReport : GjiBaseReport
    {
        private long CancelId { get; set; }

        protected override string CodeTemplate { get; set; }

        public PrescriptionGjiCancelReport() : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_Solution))
        {
        }

        public override string Id
        {
            get { return "PrescriptionCancel"; }
        }

        public override string CodeForm
        {
            get { return "PrescriptionCancel"; }
        }

        public override string Name
        {
            get { return "Решение"; }
        }

        public override string Description
        {
            get { return "Решение предписания"; }
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            CancelId = userParamsValues.GetValue<object>("CancelId").ToLong();
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Solution",
                            Name = "Solution",
                            Description = "Решение",
                            Template = Properties.Resources.BlockGJI_Solution
                        }
                };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var cancel = Container.Resolve<IDomainService<PrescriptionCancel>>().Load(CancelId);

            if (cancel == null)
            {
                throw new ReportProviderException("Не удалось получить решение об отмене");
            }

            CodeTemplate = "BlockGJI_Solution";

            var prescription = cancel.Prescription;

            // Заполняем общие поля
            FillCommonFields(reportParams, prescription);

            reportParams.SimpleReportParams["Номер"] = cancel.DocumentNum;
            reportParams.SimpleReportParams["ДатаРешения"] = 
                    cancel.DocumentDate.HasValue
                        ? cancel.DocumentDate.Value.ToShortDateString()
                        : string.Empty;

            if (prescription.Contragent != null)
            {
                reportParams.SimpleReportParams["Контрагент"] = prescription.Contragent.Name;

                if (prescription.Contragent.Municipality != null)
                {
                    reportParams.SimpleReportParams["Район"] = prescription.Contragent.Municipality.Name;
                }
            }

            if (cancel.IssuedCancel != null)
            {
                reportParams.SimpleReportParams["КодИнспектора"] = cancel.IssuedCancel.Position;
                reportParams.SimpleReportParams["ФИОИнспектора"] = cancel.IssuedCancel.Fio;
                reportParams.SimpleReportParams["ФИОИнспектора(сИнициалами)"] = cancel.IssuedCancel.ShortFio;
            }

            reportParams.SimpleReportParams["НомерПредписания"] = prescription.DocumentNumber;
            reportParams.SimpleReportParams["ДатаПредписания"] = 
                    prescription.DocumentDate.HasValue
                        ? prescription.DocumentDate.Value.ToShortDateString()
                        : string.Empty;

            var firstInspectorPrescription = 
                    Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                        .Where(x => x.DocumentGji.Id == prescription.Id)
                        .Select(x => x.Inspector)
                        .FirstOrDefault();

            if (firstInspectorPrescription != null)
            {
                reportParams.SimpleReportParams["ФИОИнспектораПредписание"] = firstInspectorPrescription.Fio;
                reportParams.SimpleReportParams["КодИнспектораПредписание"] = firstInspectorPrescription.Position;
            }
        }
    }
}