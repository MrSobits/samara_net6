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

            this.ReportParams["ОснованиеПроверки"] = ((int)act.Inspection.TypeBase).ToString();

            FillContragentInfo(act.Inspection.Contragent);

            this.ReportParams["ТипАкта"] = act.TypeActCheck == TypeActCheckGji.ActView ? "2" : "1";

            var morpher = Utils.GetMorpher();

            if (!act.Inspection.PhysicalPerson.IsEmpty())
            {
                var fioPhys = morpher.Проанализировать(act.Inspection.PhysicalPerson);

                this.ReportParams["ФИОФиз"] = act.Inspection.PhysicalPerson;
                this.ReportParams["ФИОФизРП"] = fioPhys.Родительный;
                this.ReportParams["ФИОФизДП"] = fioPhys.Дательный;
            }

            var checkPeriods = actPeriodDomain.GetAll()
                .Where(x => x.ActCheck.Id == act.Id)
                .ToArray();

            this.ReportParams["ДатаПроверки"] = checkPeriods
                .AggregateWithSeparator(x =>
                    string.Format("{0} с {1} по {2}",
                        x.DateCheck.ToDateString(),
                        x.DateStart.ToTimeString(),
                        x.DateEnd.ToTimeString()),
                    ",");

            if (act.Inspection.TypeBase == TypeBase.PlanAction)
            {
                var inspection = unproxy.GetUnProxyObject(act.Inspection);

                this.ReportParams["ДатаНачалоПМ"] = ((BasePlanAction)inspection).DateStart.ToDateString();
                this.ReportParams["ДатаОкончаниеПМ"] = ((BasePlanAction)inspection).DateEnd.ToDateString();
            }

            var inspectors = docInspectorDomain.GetAll()
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
        }

        private void FillContragentInfo(Contragent contragent)
        {
            if (contragent == null) return;

            var contragentContactDomain = Container.ResolveDomain<ContragentContact>();

            this.ReportParams["Контрагент"] = contragent.Name;
            this.ReportParams["КонтрагентСокр"] = contragent.ShortName;
            this.ReportParams["КонтрагентИНН"] = contragent.Inn;
            this.ReportParams["КонтрагентОГРН"] = contragent.Ogrn;
            this.ReportParams["КонтрагентЮрАдрес"] = contragent.JuridicalAddress;

            var contact = contragentContactDomain.GetAll()
                .FirstOrDefault(x => x.Contragent.Id == contragent.Id);

            if (contact != null)
            {
                this.ReportParams["КонтрагентКонтактРП"] = contact.GetFioGenetive();
                this.ReportParams["КонтрагентКонтактДолжность"] = contact.Position.Return(x => x.Name);
                this.ReportParams["КонтрагентКонтактДолжностьРП"] = contact.Position.Return(x => x.NameGenitive);
            }
        }
    }
}