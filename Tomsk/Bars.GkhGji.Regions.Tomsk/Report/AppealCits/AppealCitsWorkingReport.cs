namespace Bars.GkhGji.Regions.Tomsk.Report
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities.Inspection;

    using Castle.Windsor;
    using Entities.AppealCits;

	/// <summary>
	/// Отчет "Журнал учета переданных в работу обращений"
	/// </summary>
	public class AppealCitsWorkingReport : BasePrintForm
    {
		private const string InWorkingStatusCode = "7001"; //Код для статуса обращения "В работе"

		private DateTime dateStart;
        private DateTime dateEnd;
        private long[] inspectors;

		/// <summary>
		/// Контейнер
		/// </summary>
        public IWindsorContainer Container { get; set; }

		/// <summary>
		/// Домен сервис для Инспектор
		/// </summary>
		public IDomainService<Inspector> InspectorService { get; set; }

		/// <summary>
		/// Домен сервис для Обращение граждан
		/// </summary>
		public IDomainService<AppealCits> AppealCitsService { get; set; }

		/// <summary>
		/// Домен сервис для Исполнитель обращения
		/// </summary>
		public IDomainService<AppealCitsExecutant> AppealCitsExecutantService { get; set; }

		/// <summary>
		/// Домен сервис для Место возникновения проблемы
		/// </summary>
		public IDomainService<AppealCitsRealityObject> AppealCitsRealObjectService { get; set; }

		/// <summary>
		/// Домен сервис для Первичное обращение проверки
		/// </summary>
		public IDomainService<PrimaryBaseStatementAppealCits> PrimaryBaseStatementAppealCitsService { get; set; }

		/// <summary>
		/// Конструктор
		/// </summary>
		public AppealCitsWorkingReport()
            : base(new ReportTemplateBinary(Properties.Resources.AppealCitsWorking))
        {
        }

		/// <summary>
		/// Наименование
		/// </summary>
        public override string Name
        {
            get { return "AppealCitsWorking"; }
        }

		/// <summary>
		/// Описание
		/// </summary>
        public override string Desciption
        {
            get { return "Журнал учета переданных в работу обращений"; }
        }

		/// <summary>
		/// Наименование группы
		/// </summary>
        public override string GroupName
        {
            get { return "ГЖИ";  }
        }

		/// <summary>
		/// Клиентский контроллер
		/// </summary>
        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.AppealCitsWorkingReport";
            }
        }

		/// <summary>
		/// Необходимое разрешение
		/// </summary>
        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.AppealCitsWorkingReport";
            }
        }

		/// <summary>
		/// Установить пользовательские параметры
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
        public override void SetUserParams(BaseParams baseParams)
        {
            var inspectorsIdsList = baseParams.Params.GetAs("inspectors", string.Empty);
            inspectors = !string.IsNullOrEmpty(inspectorsIdsList)
                                  ? inspectorsIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];

            dateStart = baseParams.Params["dateStart"].ToDateTime();
            dateEnd = baseParams.Params["dateEnd"].ToDateTime();
        }

		/// <summary>
		/// Подготовить отчет
		/// </summary>
		/// <param name="reportParams">Параметры отчета</param>
        public override void PrepareReport(ReportParams reportParams)
        {
            reportParams.SimpleReportParams["началоПериода"] = dateStart.ToShortDateString();
            reportParams.SimpleReportParams["окончаниеПериода"] = dateEnd.ToShortDateString();

	        var all = AppealCitsExecutantService.GetAll()
		        .Where(x => x.AppealCits.DateFrom.HasValue && x.AppealCits.DateFrom >= dateStart && x.AppealCits.DateFrom <= dateEnd)
		        .WhereIf(inspectors.Any(), x => inspectors.Contains(x.Executant.Id))
		        .Where(x => x.Executant != null)
				.Where(x => x.AppealCits.State.Code == InWorkingStatusCode);

            var inspectorsIds = all.Select(x => x.Executant.Id).Distinct().ToArray();

            var appealIds = all.Select(x => x.AppealCits.Id).ToArray();

            var allInspectors = InspectorService.GetAll().Where(x => inspectorsIds.Contains(x.Id));

            var appRos =
                AppealCitsRealObjectService.GetAll()
                    .Where(x => appealIds.Contains(x.AppealCits.Id))
                    .GroupBy(x => x.AppealCits.Id)
                    .ToDictionary(x => x.Key, arg => arg.ToList());


            var verticalSection = reportParams.ComplexReportParams.ДобавитьСекцию("vertSection");

            foreach (var inspector in allInspectors)
            {
                verticalSection.ДобавитьСтроку();
                verticalSection["ФИОинспектора"] = inspector.Fio;

                verticalSection["номер"] = string.Format("$номер{0}$", inspector.Id);
                verticalSection["адрес"] = string.Format("$адрес{0}$", inspector.Id);
            }

            var allAppealCits = all.AsEnumerable()
                .OrderBy(x => x.AppealCits.DateFrom.GetValueOrDefault().Month)
                .GroupBy(x => x.AppealCits.DateFrom.GetValueOrDefault().Month)
                .ToDictionary(x => x.Key);

            var inspectionAppealCits = Container.Resolve<IDomainService<InspectionAppealCits>>()
                .GetAll()
                .Where(x => appealIds.Contains(x.AppealCits.Id))
                .ToList();

            var inspectionAppealCitIds = inspectionAppealCits.Select(x => x.Id).ToArray();

            var primAppCits = PrimaryBaseStatementAppealCitsService
                .GetAll()
                .Where(x => inspectionAppealCitIds.Contains(x.BaseStatementAppealCits.Id))
                .Select(x => x.BaseStatementAppealCits.Id).ToArray();

            var additionalAppsDict =
                inspectionAppealCits
                .Where(x => !primAppCits.Contains(x.Id))
                .Select(x => new 
                    {
                        InspectionId = x.Inspection.Id,
                        x.AppealCits.Number
                    })
                .GroupBy(x => x.InspectionId)
                 .ToDictionary(x => x.Key, arg => arg.ToList());

            var inspectionAppDict = inspectionAppealCits.Select(x => new
            {
                AppealCitsId = x.AppealCits.Id,
                InspectionId = x.Inspection.Id

            }).GroupBy(x => x.AppealCitsId).ToDictionary(x => x.Key,arg => arg.ToList());


            var monthSection = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMonth");
            foreach (var month in allAppealCits.Keys)
            {
                var groupByInspector = allAppealCits[month].GroupBy(x => x.Executant).ToDictionary(x => x.Key, arg => arg.ToArray());

                var count = groupByInspector.Select(x => x.Value.Select(y => y.AppealCits).Distinct().Count()).Max(x => x);

                for(var i=1; i<= count; i++)
                {
                    monthSection.ДобавитьСтроку();

                    foreach (var executant in groupByInspector.Keys)
                    {
                        var apps = groupByInspector[executant].Select(x => x.AppealCits).Distinct().ToArray();

                        if (apps.Count() < i) continue;

                        var app = apps[i - 1];

                        var isnpId = executant.Id;

                        monthSection["month"] = app.DateFrom.GetValueOrDefault().ToString("MMMM",CultureInfo.CurrentCulture);

                        var location = "";
                        if (appRos.ContainsKey(app.Id))
                        {
                            location = string.Join(",", appRos[app.Id].Select(x => x.RealityObject.Address).ToList());
                        }

                        var inspections = inspectionAppDict.ContainsKey(app.Id)
                            ? inspectionAppDict[app.Id].Select(x => x.InspectionId).ToArray()
                            : (new List<long>()).ToArray();

                        var nums = new HashSet<string>();

                        foreach (var inspApp in inspections.Select(insp => additionalAppsDict.ContainsKey(insp) ? 
                                                additionalAppsDict[insp] : null)
                            .Where(inspApps => inspApps != null)
                            .SelectMany(inspApps => inspApps))
                            {
                                nums.Add(inspApp.Number);
                            }

                        var additionalNumsResult = nums.Any() ? nums.Aggregate((a1, a2) => a1 + "," + a2) : string.Empty;

                        var resultNumbers = !string.IsNullOrEmpty(additionalNumsResult) && app.Number != additionalNumsResult 
                            ? string.Format("{0},{1}", app.Number, additionalNumsResult )
                            : app.Number;

                        monthSection[string.Format("адрес{0}", isnpId)] = string.Format("{0} ({1})", location, resultNumbers);
                        monthSection[string.Format("номер{0}", isnpId)] = i;
                    }
                }

                monthSection.ДобавитьСтроку();
            }
        }
    }
}
