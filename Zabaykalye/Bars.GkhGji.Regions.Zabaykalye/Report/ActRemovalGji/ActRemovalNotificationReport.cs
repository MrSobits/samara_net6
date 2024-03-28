namespace Bars.GkhGji.Regions.Zabaykalye.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using Bars.B4.Modules.Reports;
    using B4.Utils;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;

    using Gkh.Entities;
    using Gkh.Report;
    using Gkh.Utils;
    using GkhGji.Entities;
    using GkhGji.Enums;
    using GkhGji.Report;
    using Utils = Utils;

    public class ActRemovalNotificationReport : GjiBaseStimulReport
    {
        #region .ctor

        public ActRemovalNotificationReport() : base(new ReportTemplateBinary(Properties.Resources.ActRemovalNotification))
        {
        }

        #endregion .ctor

        #region Properties

        /// <summary>
        /// Наименование отчета
        /// </summary>
        public override string Name
        {
            get { return "Уведомление о составлении протокола"; }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get { return "Уведомление о составлении протокола(из акта проверки предписания)"; }
        }

        /// <summary>
        /// Код шаблона (файла)
        /// </summary>
        protected override string CodeTemplate { get; set; }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get { return "ActRemovalNotificationReport"; }
        }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get { return "ActRemoval"; }
        }

        #endregion Properties

        private long ActRemovalId { get; set; }

        /// <summary>
        /// Установить пользовательские параметры
        /// </summary>
        /// <param name="userParamsValues"></param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            ActRemovalId = userParamsValues.GetValue<long>("DocumentId");
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
                    Name = "ActRemovalNotification",
                    Code = "ActRemovalNotification",
                    Description = "Уведомление о составлении протокола (из акта проверки предписания)",
                    Template = Properties.Resources.ActRemovalNotification
                }
            };
        }

        /// <summary>
        /// Подготовить параметры отчета
        /// </summary>
        /// <param name="reportParams"></param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var actRemovalDomain = Container.ResolveDomain<ActRemoval>();
            var documentInspectorDomain = Container.ResolveDomain<DocumentGjiInspector>();
            var act = actRemovalDomain.Get(ActRemovalId);

            if (act == null)
            {
                return;
            }

            FillCommonFields(act);

            this.ReportParams["ОснованиеПроверки"] = ((int)act.Inspection.TypeBase).ToString();

            var morpher = Utils.GetMorpher();

            if (!act.Inspection.PhysicalPerson.IsEmpty())
            {
                var fioPhys = morpher.Проанализировать(act.Inspection.PhysicalPerson);

                this.ReportParams["ФИОФиз"] = act.Inspection.PhysicalPerson;
                this.ReportParams["ФИОФизРП"] = fioPhys.Родительный;
                this.ReportParams["ФИОФизДП"] = fioPhys.Дательный;
            }

            var inspectors = documentInspectorDomain.GetAll()
                .Where(x => x.DocumentGji.Id == act.Id)
                .Select(x => new
                {
                    Инспектор = x.Inspector.Fio,
                    Должность = x.Inspector.Position,
                    ИнспекторТП = x.Inspector.FioAblative,
                    ДолжностьТП = x.Inspector.PositionAblative
                })
                .ToArray();

            this.DataSources.Add(new MetaData
            {
                SourceName = "Инспектор",
                MetaType = nameof(Object),
                Data = inspectors
            });

            this.ReportParams["Основание"] = GetDocumentBase(act);

            var parentPrescription = GetParentDocument(act, TypeDocumentGji.Prescription);

            FillPrescription(parentPrescription as Prescription);
        }

        private void FillPrescription(Prescription prescription)
        {
            if (prescription == null) return;

            var contragentContactDomain = Container.ResolveDomain<ContragentContact>();
            var prescViolDomain = Container.ResolveDomain<PrescriptionViol>();

            this.ReportParams["НомерПредписания"] = prescription.DocumentNumber;
            this.ReportParams["ДатПредписания"] = prescription.DocumentDate.ToDateString();

            this.ReportParams["Вотношении"] = prescription.Executant.Return(x => x.Code);

            var morpher = Utils.GetMorpher();

            var contragent = prescription.Contragent;

            if (contragent != null)
            {
                this.ReportParams["Контрагент"] = contragent.Name;
                this.ReportParams["КонтрагентСокр"] = contragent.ShortName;
                this.ReportParams["КонтрагентИНН"] = contragent.Inn;
                this.ReportParams["КонтрагентОГРН"] = contragent.Ogrn;
                this.ReportParams["КонтрагентЮрАдрес"] = contragent.JuridicalAddress;

                var contact = contragentContactDomain.GetAll()
                    .FirstOrDefault(x => x.Contragent.Id == contragent.Id);

                if (contact != null)
                {
                    var shortFio = contact.GetShortFio();

                    if (!shortFio.IsEmpty())
                    {
                        var analyzed = morpher.Проанализировать(shortFio);

                        this.ReportParams["ФИОРукКонтрДП"] = analyzed.Дательный;
                    }

                    this.ReportParams["КонтрагентКонтактДП"] = contact.GetFioDative();
                    this.ReportParams["ИО"] = (contact.Name + " " + contact.Surname).Trim();

                    if (contact.Position != null)
                    {
                        this.ReportParams["КонтрагентКонтактДолжностьДП"] = contact.Position.NameDative;
                        this.ReportParams["КонтрагентКонтактДолжностьРП"] = contact.Position.NameGenitive;
                    }
                }
            }

            if (!prescription.PhysicalPerson.IsEmpty())
            {
                var analyzed = morpher.Проанализировать(prescription.PhysicalPerson);

                this.ReportParams["ФизЛицоРП"] = analyzed.Родительный;
                this.ReportParams["ФизЛицоДП"] = analyzed.Дательный;
            }

            var violAddresses = prescViolDomain.GetAll()
                .Where(x => x.Document.Id == prescription.Id)
                .Select(x => x.InspectionViolation.RealityObject.Address)
                .Distinct()
                .ToArray();

            this.ReportParams["Адрес"] = violAddresses.AggregateWithSeparator(", ");
        }
    }
}