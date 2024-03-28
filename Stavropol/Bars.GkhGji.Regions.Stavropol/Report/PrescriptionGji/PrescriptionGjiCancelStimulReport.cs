namespace Bars.GkhGji.Regions.Stavropol.Report.PrescriptionGji
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using B4.Modules.Reports;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Report;

    using Slepov.Russian.Morpher;

    public class PrescriptionGjiCancelStimulReport : GjiBaseStimulReport
    {
        private const string StavrapolSolution1 = "Stavrapol_Solution_1";

        private const string StavrapolSolution2 = "Stavrapol_Solution_2";

        /// <summary>
        /// Решение об отмене.
        /// </summary>
        private PrescriptionCancel prescriptionCancel;

        /// <summary>
        /// Предписание.
        /// </summary>
        private Prescription prescription;

        public PrescriptionGjiCancelStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_Solution_1))
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

        public override StiExportFormat ExportFormat
        {
            get
            {
                return StiExportFormat.Word2007;
            }
        }

        /// <summary>
        /// Код шаблона (файла).
        /// </summary>
        protected override string CodeTemplate { get; set; }

        /// <summary>
        /// ИД решения об отмене.
        /// </summary>
        private long CancelId { get; set; }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.CancelId = userParamsValues.GetValue<object>("CancelId").ToLong();

            var prescriptionCancelDomain = this.Container.ResolveDomain<PrescriptionCancel>();

            using (this.Container.Using(prescriptionCancelDomain))
            {
                this.prescriptionCancel = prescriptionCancelDomain.GetAll().FirstOrDefault(x => x.Id == this.CancelId);
            }

            if (this.prescriptionCancel == null)
            {
                throw new ReportProviderException("Не удалось получить решение об отмене");
            }

            this.prescription = this.prescriptionCancel.Prescription;
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Solution_1",
                            Name = StavrapolSolution1,
                            Description = "Решение о продлении срока исполнения предписания",
                            Template = Properties.Resources.BlockGJI_Solution_1
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Solution_2",
                            Name = StavrapolSolution2,
                            Description = "Решение об отказе в продлении срока исполнения предписания",
                            Template = Properties.Resources.BlockGJI_Solution_2
                        }
                };
        }

        /// <summary>
        /// Изменяем код шаблона в зависимости от типа решения и вызываем родительский метод.
        /// </summary>
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        public override Stream GetTemplate()
        {
            switch (this.prescriptionCancel.TypeCancel)
            {
                case TypePrescriptionCancel.Prolongation:
                    this.CodeTemplate = "BlockGJI_Solution_1";
                    break;
                case TypePrescriptionCancel.RefusExtend:
                    this.CodeTemplate = "BlockGJI_Solution_2";
                    break;
            }

            return base.GetTemplate();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            this.CodeTemplate = "BlockGJI_Solution_1";
            var склонятель = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ==");

            // Заполняем общие поля
            this.FillCommonFields(this.prescription);

            var prescriptionViols = this.GetPrescriptonViolations(this.prescription);

            this.ReportParams["ДатаРешения"] = this.prescriptionCancel.DocumentDate.HasValue
                                             ? this.prescriptionCancel.DocumentDate.Value.ToShortDateString()
                                             : string.Empty;

            var viol = prescriptionViols.FirstOrDefault();
            if (viol != null)
            {
                try
                {
                    this.ReportParams["НаселенныйПункт"] = viol.InspectionViolation.RealityObject.FiasAddress.PlaceName;
                }
                catch (NullReferenceException ex)
                {
                    this.ReportParams["НаселенныйПункт"] = string.Empty;
                }
            }

            this.ReportParams["КодИнспектора"] = this.prescriptionCancel.IssuedCancel.Position;
            this.ReportParams["ФИОИнспектора"] = this.prescriptionCancel.IssuedCancel.Fio;
            this.ReportParams["Вотношении"] = this.prescription.Executant.Code;

            this.ReportParams["ДатаХодатайства"] = this.prescriptionCancel.PetitionDate.HasValue
                                                 ? this.prescriptionCancel.PetitionDate.Value.ToShortDateString()
                                                 : string.Empty;
            this.ReportParams["НомерХодатайства"] = this.prescriptionCancel.PetitionNumber;
            this.ReportParams["ДатаПредписания"] = this.prescription.DocumentDate.HasValue
                                                 ? this.prescription.DocumentDate.Value.ToShortDateString()
                                                 : string.Empty;
            this.ReportParams["НомерПредписания"] = this.prescription.DocumentNumber;

            if (!string.IsNullOrEmpty(this.prescription.PhysicalPerson))
            {
                var phisPersName = склонятель.Проанализировать(this.prescription.PhysicalPerson);
                this.ReportParams["ФизЛицоРП"] = phisPersName.Родительный;
                this.ReportParams["ФизЛицоИП"] = phisPersName.Именительный;
            }
            else
            {
                this.ReportParams["ФизЛицоРП"] = string.Empty;
                this.ReportParams["ФизЛицоИП"] = string.Empty;
            }

            this.ReportParams["Реквизиты"] = this.prescription.PhysicalPersonInfo ?? string.Empty;
            this.ReportParams["ПродлитьДо"] = this.prescriptionCancel.DateProlongation.HasValue
                                            ? this.prescriptionCancel.DateProlongation.Value.ToShortDateString()
                                            : string.Empty;
            this.ReportParams["Установил"] = this.prescriptionCancel.DescriptionSet;
            this.ReportParams["ФИОИнспектораСокр"] = this.prescriptionCancel.IssuedCancel.ShortFio;

            var firstInspectorPrescription = this.Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                        .Where(x => x.DocumentGji.Id == this.prescription.Id)
                        .Select(x => x.Inspector)
                        .FirstOrDefault();

            if (firstInspectorPrescription != null)
            {
                this.ReportParams["ФИОИнспектораПредписание"] = firstInspectorPrescription.Fio;
            }

            if (this.prescription.Contragent != null)
            {
                this.ReportParams["КонтрагентСокр"] = this.prescription.Contragent.Name;

                var contactsContragent = this.Container.ResolveDomain<ContragentContact>().GetAll()
                    .FirstOrDefault(x => x.Contragent.Id == this.prescription.Contragent.Id);

                if (contactsContragent != null)
                {
                    if (contactsContragent.Position != null)
                    {
                        this.ReportParams["ДолжностьКонтр"] = contactsContragent.Position.NameGenitive ?? string.Empty;
                    }

                    this.ReportParams["АдресКонтрагента"] = string.Format(
                        "{0}, ИНН {1}",
                        this.prescription.Contragent.JuridicalAddress,
                        this.prescription.Contragent.Inn);

                    this.ReportParams["КонтрагентФИО"] = string.Format(
                        "{0} {1} {2}",
                        contactsContragent.SurnameGenitive,
                        contactsContragent.NameGenitive,
                        contactsContragent.PatronymicGenitive);
                }
            }
        }

        /// <summary>
        /// Получает нарушения из предписания.
        /// </summary>
        /// <param name="presc">
        /// Предписание.
        /// </param>
        /// <returns>
        /// Нарушения
        /// </returns>
        private IEnumerable<PrescriptionViol> GetPrescriptonViolations(Prescription presc)
        {
            var prescriptionViolDomain = this.Container.ResolveDomain<PrescriptionViol>();

            using (this.Container.Using(prescriptionViolDomain))
            {
                return prescriptionViolDomain.GetAll().Where(x => x.Document.Id == presc.Id).ToList();
            }
        }
    }
}