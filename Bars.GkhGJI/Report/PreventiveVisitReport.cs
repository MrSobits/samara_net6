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

    /// <summary>
    /// Извещение
    /// </summary>
    public class PreventiveVisitReport : GkhBaseStimulReport
    {
        #region .ctor

        /// <summary>
        /// .ctor
        /// </summary>
        public PreventiveVisitReport()
            : base(new ReportTemplateBinary(Properties.Resources.ActPreventiveVisit))
        {
        }

        #endregion .ctor

        #region Private fields

        private long _preventiveVisitId;

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
            get { return "Акт профилактического визита"; }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get { return "Акт профилактического визита"; }
        }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get { return "PreventiveVisit"; }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get { return "PreventiveVisit"; }
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
            var PreventiveVisitDomain = Container.ResolveDomain<PreventiveVisit>();
            var PreventiveVisitPeriodDomain = Container.ResolveDomain<PreventiveVisitPeriod>();

            var data = PreventiveVisitDomain.GetAll()
                .Where(x => x.Id == _preventiveVisitId).FirstOrDefault();

            var period = PreventiveVisitPeriodDomain.GetAll()
                .Where(x => x.PreventiveVisit.Id == _preventiveVisitId).FirstOrDefault();
            var fromHours = period.DateStart.HasValue ? period.DateStart.Value.TimeOfDay.Hours.ToString() : "___";
            var fromMinutes = period.DateStart.HasValue ? period.DateStart.Value.TimeOfDay.Minutes.ToString() : "___";
            var toHours = period.DateEnd.HasValue ? period.DateEnd.Value.TimeOfDay.Hours.ToString() : "___";
            var toMinutes = period.DateEnd.HasValue ? period.DateEnd.Value.TimeOfDay.Minutes.ToString() : "___";

            try
            {
                this.ReportParams["НомерАктаПроверки"] = data.DocumentNumber;
                this.ReportParams["ДатаСоставленияАкта"] = data.DocumentDate.HasValue ? data.DocumentDate.Value.ToString("dd.MM.yyyy") : "";
                this.ReportParams["МестоСоставленияАкта"] = data.ActAddress;
                this.ReportParams["Id"] = data.Id;
                this.ReportParams["ВОтношении"] = GetSubject(data);
                this.ReportParams["ИННОбъекта"] = data.Contragent != null ? data.Contragent.Inn : "";
                this.ReportParams["АдресФЛ"] = data.PhysicalPersonAddress;
                this.ReportParams["АдресЮЛ"] = data.Contragent != null ? data.Contragent.JuridicalAddress : "";
                this.ReportParams["ВидКонтроля"] = data.KindKND == Enums.KindKNDGJI.HousingSupervision ? "государственного жилищного надзора" : "лицензионного контроля";
                this.ReportParams["Присутствовал"] = GetWitness(data.Id);
                this.ReportParams["СЧас"] = fromHours;
                this.ReportParams["СМин"] = fromMinutes;
                this.ReportParams["ПОЧас"] = toHours;
                this.ReportParams["ПоМин"] = toMinutes;
                this.ReportParams["ВидДеятельности"] = data.KindKND == Enums.KindKNDGJI.HousingSupervision ? "Государственный жилищный надзор" : "Лицензионный контроль";
                this.ReportParams["ОбъектаПроверки"] = Getobjects(data.Inspection.Id);
                this.ReportParams["ФИОСотрудникаПолностью"] = GetInspectors(data.Id);
                this.ReportParams["Результаты"] = GetResult(data.Id);
                this.ReportParams["ДатаСоставленияАкта"] = GetNotNullDate(period, data.DocumentDate);
                this.ReportParams["Ознакомился"] = GetWitnessIsFamiliar(data.Id);
            }
            finally
            {
                Container.Release(PreventiveVisitDomain);
                Container.Release(PreventiveVisitPeriodDomain);
            }
        }

        /// <summary>
        /// Установить пользовательские параметры
        /// </summary>
        /// <param name="userParamsValues"></param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            _preventiveVisitId = userParamsValues.GetValue<long>("Id");
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
                    Name = "Акт профилактического визита",
                    Description = "Акт профилактического визита",
                    Code = "PreventiveVisitReport",
                    Template = Properties.Resources.ActPreventiveVisit
                }
            };
        }

        #endregion Public methods

        #region Private methods

        private string GetNotNullDate(PreventiveVisitPeriod period, DateTime? actDate)
        {
            if (period != null)
            {
                if (period.DateCheck.HasValue)
                {
                    return period.DateCheck.Value.ToShortDateString();
                }
            }
            if (actDate.HasValue)
            {
                return actDate.Value.ToShortDateString();
            }
            return DateTime.Now.ToShortDateString();
        }
        private string GetSubject(PreventiveVisit data)
        {
            if (data.PersonInspection == Enums.PersonInspection.PhysPerson)
            {
                return $"{data.PhysicalPerson} {data.PhysicalPersonInfo}";
            }
            else if (data.PersonInspection == Enums.PersonInspection.Organization && data.Contragent != null)
            {
                return data.Contragent.Name;
            }
            else if (data.PersonInspection == Enums.PersonInspection.Official && data.Contragent != null)
            {
                return $"Должностного лица {data.Contragent.Name} {data.PhysicalPerson} {data.PhysicalPersonInfo}";
            }
            return "";
        }
        private string GetWitness(long ActId)
        {
            var PreventiveVisitWitnessDomain = Container.ResolveDomain<PreventiveVisitWitness>();
            var witness = PreventiveVisitWitnessDomain.GetAll()
                   .Where(x => x.PreventiveVisit.Id == ActId)
                   .Select(x => new { x.Fio, x.Position, x.IsFamiliar })
                   .ToArray();

            var allWitness = witness
                .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y.Position + " - " + y.Fio : y.Position + " - " + y.Fio));

            return allWitness;
        }
        private string GetWitnessIsFamiliar(long ActId)
        {
            var PreventiveVisitWitnessDomain = Container.ResolveDomain<PreventiveVisitWitness>();
            var witness = PreventiveVisitWitnessDomain.GetAll()
                   .Where(x => x.PreventiveVisit.Id == ActId && x.IsFamiliar)
                   .Select(x => new { x.Fio, x.Position, x.IsFamiliar })
                   .ToArray();

            var allWitness = witness
                .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y.Position + " - " + y.Fio : y.Position + " - " + y.Fio));

            return allWitness;
        }
        private string Getobjects(long InspectionId)
        {
            var InspectionGjiRealityObjectomain = Container.ResolveDomain<InspectionGjiRealityObject>();
            var objects = InspectionGjiRealityObjectomain.GetAll()
                   .Where(x => x.Inspection.Id == InspectionId)
                   .Select(x => new { x.RealityObject.Municipality.Name, x.RealityObject.Address })
                   .ToList();
            if (objects.Count == 1)
            {
                return $"Многоквартирный дом, расположенный по адресу {objects[0].Name}, {objects[0].Address}";
            }
            else if (objects.Count > 1)
            {
                string obj = "Многоквартирные дома, расположенные по адресам: ";
                foreach (var mkd in objects)
                {
                    if (obj == "Многоквартирные дома, расположенные по адресам: ")
                    {
                        obj += $"{objects[0].Name}, {objects[0].Address}";
                    }
                    else
                        obj += $"; {objects[0].Name}, {objects[0].Address}";
                }
                return obj;
            }
            return "";

        }
        private string GetInspectors(long ActId)
        {
            var DocumentGjiInspectorDomain = Container.ResolveDomain<DocumentGjiInspector>();
            var inspectors = DocumentGjiInspectorDomain.GetAll()
                   .Where(x => x.DocumentGji.Id == ActId)
                   .Select(x => new { x.Inspector.Fio, x.Inspector.Position })
                   .ToArray();

            var allinspectors = inspectors
                .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y.Position + " - " + y.Fio : y.Position + " - " + y.Fio));

            return allinspectors;
        }
        private string GetResult(long ActId)
        {
            var PreventiveVisitResultDomain = Container.ResolveDomain<PreventiveVisitResult>();

            var objects = PreventiveVisitResultDomain.GetAll()
                   .Where(x => x.PreventiveVisit.Id == ActId)
                   .ToList();
            string result = "";
            foreach (var actRes in objects)
            {
                if (!string.IsNullOrEmpty(result))
                {
                    result += "; ";
                }
                if (actRes.ProfVisitResult == Enums.ProfVisitResult.HasNoViolations)
                {
                    if (actRes.RealityObject != null)
                    {
                        result += $"{actRes.RealityObject.Address} - нарушений не выявлено";
                    }
                    else
                    {
                        result += $"нарушений не выявлено";
                    }
                }
                if (actRes.ProfVisitResult == Enums.ProfVisitResult.Inform)
                {
                    if (actRes.RealityObject != null)
                    {
                        result += $"{actRes.RealityObject.Address} - проведено информирование проверяемого лица. {actRes.InformText}";
                    }
                    else
                    {
                        result += $"проведено информирование проверяемого лица. {actRes.InformText}";
                    }
                }
                if (actRes.ProfVisitResult == Enums.ProfVisitResult.HasViolations)
                {
                    string violations = GetViolations(actRes.Id);
                    if (actRes.RealityObject != null)
                    {
                        result += $"{actRes.RealityObject.Address} - обнаружены следующие нарушения: {violations}";
                    }
                    else
                    {
                        result += $"обнаружены следующие нарушения: {violations}";
                    }
                }
            }
            if (string.IsNullOrEmpty(result))
            {
                return "_________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________";
            }
            return result;

        }
        private string GetViolations(long resId)
        {
            var PreventiveVisitResultViolationDomain = Container.ResolveDomain<PreventiveVisitResultViolation>();
            var witness = PreventiveVisitResultViolationDomain.GetAll()
                   .Where(x => x.PreventiveVisitResult.Id == resId)
                   .Select(x => new { x.ViolationGji.NormativeDocNames, x.ViolationGji.Name })
                   .ToArray();

            var allWitness = witness
                .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y.NormativeDocNames + " - " + y.Name : y.NormativeDocNames + " - " + y.Name));

            return allWitness;
        }
        #endregion
    }
}