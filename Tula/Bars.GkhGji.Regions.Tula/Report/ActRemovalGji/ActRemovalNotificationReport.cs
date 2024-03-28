namespace Bars.GkhGji.Regions.Tula.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using Bars.B4.Modules.Reports;
    using B4.Utils;
    using Gkh.Entities;
    using Gkh.Report;
    using Gkh.Utils;
    using GkhGji.Entities;
    using GkhGji.Enums;
    using GkhGji.Report;
    using Stimulsoft.Report;
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

            Report["ОснованиеПроверки"] = (int) act.Inspection.TypeBase;

            var morpher = Utils.GetMorpher();

            if (!act.Inspection.PhysicalPerson.IsEmpty())
            {
                var fioPhys = morpher.Проанализировать(act.Inspection.PhysicalPerson);

                Report["ФИОФиз"] = act.Inspection.PhysicalPerson;
                Report["ФИОФизРП"] = fioPhys.Родительный;
                Report["ФИОФизДП"] = fioPhys.Дательный;
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

            Report.RegData("Инспектор", inspectors);

            Report["Основание"] = GetDocumentBase(act);

            var parentPrescription = GetParentDocument(act, TypeDocumentGji.Prescription);

            FillPrescription(parentPrescription as Prescription);
        }

        private void FillPrescription(Prescription prescription)
        {
            if (prescription == null) return;

            var contragentContactDomain = Container.ResolveDomain<ContragentContact>();
            var prescViolDomain = Container.ResolveDomain<PrescriptionViol>();

            Report["НомерПредписания"] = prescription.DocumentNumber;
            Report["ДатПредписания"] = prescription.DocumentDate.ToDateString();

            Report["Вотношении"] = prescription.Executant.Return(x => x.Code);

            var morpher = Utils.GetMorpher();

            var contragent = prescription.Contragent;

            if (contragent != null)
            {
                Report["Контрагент"] = contragent.Name;
                Report["КонтрагентСокр"] = contragent.ShortName;
                Report["КонтрагентИНН"] = contragent.Inn;
                Report["КонтрагентОГРН"] = contragent.Ogrn;
                Report["КонтрагентЮрАдрес"] = contragent.JuridicalAddress;

                var contact = contragentContactDomain.GetAll()
                    .FirstOrDefault(x => x.Contragent.Id == contragent.Id);

                if (contact != null)
                {
                    var shortFio = contact.GetShortFio();

                    if (!shortFio.IsEmpty())
                    {
                        var analyzed = morpher.Проанализировать(shortFio);

                        Report["ФИОРукКонтрДП"] = analyzed.Дательный;
                    }

                    Report["КонтрагентКонтактДП"] = contact.GetFioDative();
                    Report["ИО"] = (contact.Name + " " + contact.Surname).Trim();

                    if (contact.Position != null)
                    {
                        Report["КонтрагентКонтактДолжностьДП"] = contact.Position.NameDative;
                        Report["КонтрагентКонтактДолжностьРП"] = contact.Position.NameGenitive;
                    }
                }
            }

            if (!prescription.PhysicalPerson.IsEmpty())
            {
                var analyzed = morpher.Проанализировать(prescription.PhysicalPerson);

                Report["ФизЛицоРП"] = analyzed.Родительный;
                Report["ФизЛицоДП"] = analyzed.Дательный;
            }

            var violAddresses = prescViolDomain.GetAll()
                .Where(x => x.Document.Id == prescription.Id)
                .Select(x => x.InspectionViolation.RealityObject.Address)
                .Distinct()
                .ToArray();

            Report["Адрес"] = violAddresses.AggregateWithSeparator(", ");
        }
    }
}