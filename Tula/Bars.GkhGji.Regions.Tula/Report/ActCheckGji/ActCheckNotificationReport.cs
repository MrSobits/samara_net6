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

    public class ActCheckNotificationReport : GjiBaseStimulReport
    {
        #region .ctor

        public ActCheckNotificationReport() : base(new ReportTemplateBinary(Properties.Resources.ActCheckNotification))
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
            get { return "Уведомление о составлении протокола (из акта)"; }
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
            get { return "ActCheckNotifocation"; }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get { return "ActCheck,ActView"; }
        }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        #endregion Properties

        private long ActCheckId { get; set; }

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
                    Name = "ActCheckNotification",
                    Code = "ActCheckNotification",
                    Description = "Уведомление о составлении протокола",
                    Template = Properties.Resources.ActCheckNotification
                }
            };
        }

        /// <summary>
        /// Установить пользовательские параметры
        /// </summary>
        /// <param name="userParamsValues"></param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            ActCheckId = userParamsValues.GetValue<long>("DocumentId");
        }

        /// <summary>
        /// Подготовить параметры отчета
        /// </summary>
        /// <param name="reportParams"></param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var actDomain = Container.ResolveDomain<ActCheck>();
            var actPeriodDomain = Container.ResolveDomain<ActCheckPeriod>();
            var docInspectorDomain = Container.ResolveDomain<DocumentGjiInspector>();
            var unproxy = Container.Resolve<IUnProxy>();
            
            var act = actDomain.Get(ActCheckId);

            if (act == null)
            {
                return;
            }

            FillCommonFields(act);

            Report["ОснованиеПроверки"] = (int) act.Inspection.TypeBase;

            FillContragentInfo(act.Inspection.Contragent);

            Report["ТипАкта"] = act.TypeActCheck == TypeActCheckGji.ActView ? "2" : "1";

            var morpher = Utils.GetMorpher();

            if (!act.Inspection.PhysicalPerson.IsEmpty())
            {
                var fioPhys = morpher.Проанализировать(act.Inspection.PhysicalPerson);

                Report["ФИОФиз"] = act.Inspection.PhysicalPerson;
                Report["ФИОФизРП"] = fioPhys.Родительный;
                Report["ФИОФизДП"] = fioPhys.Дательный;
            }

            var checkPeriods = actPeriodDomain.GetAll()
                .Where(x => x.ActCheck.Id == act.Id)
                .ToArray();

            Report["ДатаПроверки"] = checkPeriods
                .AggregateWithSeparator(x =>
                    string.Format("{0} с {1} по {2}",
                        x.DateCheck.ToDateString(),
                        x.DateStart.ToTimeString(),
                        x.DateEnd.ToTimeString()),
                    ",");

            if (act.Inspection.TypeBase == TypeBase.PlanAction)
            {
                var inspection = unproxy.GetUnProxyObject(act.Inspection);

                Report["ДатаНачалоПМ"] = ((BasePlanAction)inspection).DateStart.ToDateString();
                Report["ДатаОкончаниеПМ"] = ((BasePlanAction)inspection).DateEnd.ToDateString();
            }

            var inspectors = docInspectorDomain.GetAll()
                .Where(x => x.DocumentGji.Id == act.Id)
                .Select(x => new
                {
                    Инспектор = x.Inspector.Fio,
                    Должность = x.Inspector.Position,
                    ИнспекторТП = x.Inspector.FioAblative,
                    ДолжностьТП = x.Inspector.PositionAblative,
                    ИнспекторСокр = x.Inspector.ShortFio,
                    ИнспекторТел = x.Inspector.Phone
                })
                .ToArray();

            Report.RegData("Инспектор", inspectors);
        }

        private void FillContragentInfo(Contragent contragent)
        {
            if (contragent == null) return;

            var contragentContactDomain = Container.ResolveDomain<ContragentContact>();

            Report["Контрагент"] = contragent.Name;
            Report["КонтрагентСокр"] = contragent.ShortName;
            Report["КонтрагентИНН"] = contragent.Inn;
            Report["КонтрагентОГРН"] = contragent.Ogrn;
            Report["КонтрагентЮрАдрес"] = contragent.FiasJuridicalAddress != null ? "{0}, {1}".FormatUsing(contragent.FiasJuridicalAddress.PostCode, contragent.FiasJuridicalAddress.AddressName) : string.Empty;

            var contact = contragentContactDomain.GetAll()
                .FirstOrDefault(x => x.Contragent.Id == contragent.Id);

            if (contact != null)
            {
                Report["КонтрагентКонтактРП"] = contact.GetFioGenetive();
                Report["КонтрагентКонтактДолжность"] = contact.Position.Return(x => x.Name);
                Report["КонтрагентКонтактДолжностьРП"] = contact.Position.Return(x => x.NameGenitive);
            }
        }
    }
}