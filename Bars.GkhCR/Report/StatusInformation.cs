namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using Bars.B4.Modules.States;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    public class StatusInformation: BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private List<long> municipalityIds = new List<long>();
        private long programCrId;

        private DateTime reportDate;

        public StatusInformation()
            : base(new ReportTemplateBinary(Properties.Resources.StatusInformation))
        {
        }

        public override string Name
        {
            get
            {
                return "Информация по статусам";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Информация по статусам";
            }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.StatusInformation"; }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.StatusInformation";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.programCrId = baseParams.Params["programCrId"].ToLong();

            this.reportDate = baseParams.Params["reportDate"].ToDateTime().AddDays(1);

            this.municipalityIds = baseParams.Params.GetAs("municipalityIds", string.Empty).ToStr().Split(",").Select(x => x.Trim().ToLong()).ToList();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceProgramCr = Container.Resolve<IDomainService<ProgramCr>>();
            var serviceObjectCr = Container.Resolve<IDomainService<ObjectCr>>();
            var servicePerformedWorkAct = Container.Resolve<IDomainService<PerformedWorkAct>>();
            var serviceMonitoringSmr = Container.Resolve<IDomainService<MonitoringSmr>>();
            
            reportParams.SimpleReportParams["programName"] = serviceProgramCr.Load(this.programCrId).Name;
            reportParams.SimpleReportParams["reportDate"] = this.reportDate.AddDays(-1).ToShortDateString();

            var objectCrQuery = serviceObjectCr.GetAll()
                .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Where(x => x.ProgramCr.Id == programCrId);

            var objectCrIdsQuery = objectCrQuery.Select(x => x.Id);

            var objectCrList = objectCrQuery
                .Select(x => new
                                 {
                                     objectCrId = x.Id,
                                     address = x.RealityObject.Address,
                                     municipalityName = x.RealityObject.Municipality.Name,
                                     stateName = x.State != null ? x.State.Name : null,
                                     stateCreateDate = x.State != null ? x.ObjectCreateDate : reportDate,
                                 })
                .ToList();

            var objectCrStates = new Dictionary<string, int>();
            objectCrStates["В работе у МО"] = 1;
            objectCrStates["Утверждено МО"] = 2;
            objectCrStates["Согласовано Казань"] = 3;
            objectCrStates["В работе у ГЖИ"] = 4;
            objectCrStates["Согласовано ГЖИ"] = 5;
            objectCrStates["Согласовано МСАЖКХ РТ"] = 6;
            objectCrStates["Капремонт завершен"] = 7;
            objectCrStates["Требует корректировки"] = 8;

            var performedActStates = new Dictionary<string, int>();
            performedActStates["Черновик"] = 1;
            performedActStates["Проверено"] = 2;
            performedActStates["Принято ГЖИ"] = 3;
            performedActStates["Принят ТОДК"] = 4;

            var monitoringSmrStates = new Dictionary<string, int>();
            monitoringSmrStates["Черновик"] = 1;
            monitoringSmrStates["Утверждено МО"] = 2;
            monitoringSmrStates["Утверждено ГЖИ"] = 3;

            // Инициализация статусов на дату отчета
            
            var objectCrStateDict = objectCrList
                .Where(x => x.stateCreateDate < reportDate)
                .GroupBy(x => x.objectCrId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.stateName).First());

            var performedActStateList = servicePerformedWorkAct.GetAll()
                .Where(x => objectCrIdsQuery.Contains(x.ObjectCr.Id))
                .Where(x => x.ObjectCreateDate < reportDate)
                .Select(x => new { actid = x.Id, objectCrId = x.ObjectCr.Id, x.State.Name })
                .ToList();
            
            var performedActStateDict = performedActStateList
                .GroupBy(x => x.objectCrId)
                .ToDictionary(x => x.Key, x => x.ToDictionary(y => y.actid, y => y.Name));

            var monitoringSmrStateList = serviceMonitoringSmr.GetAll()
                .Where(x => objectCrIdsQuery.Contains(x.ObjectCr.Id))
                .Where(x => x.ObjectCreateDate < reportDate)
                .Select(x => new { monitoringId = x.Id, objectCrId = x.ObjectCr.Id, x.State.Name, x.ObjectCreateDate })
                .ToList();

            var monitoringSmrStateDict = monitoringSmrStateList
                .GroupBy(x => x.objectCrId)
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.ObjectCreateDate).Select(y => y.Name).First());

            //  Если дата отчета не сегодня, то ищем статусы в истории
            if (reportDate.Date != DateTime.Now.Date.AddDays(1))
            {
                var objectCrIdsToReportDateQuery = objectCrQuery
                    .Where(x => x.ObjectCreateDate < reportDate)
                    .Select(x => x.Id);

                var performedActIdsQuery = servicePerformedWorkAct.GetAll()
                    .Where(x => objectCrIdsQuery.Contains(x.ObjectCr.Id))
                    .Where(x => x.ObjectCreateDate < reportDate)
                    .Select(x => x.Id);

                var monitoringSmrIdsQuery = serviceMonitoringSmr.GetAll()
                    .Where(x => objectCrIdsQuery.Contains(x.ObjectCr.Id))
                    .Where(x => x.ObjectCreateDate < reportDate)
                    .Select(x => x.Id);

                var objectCrStateHistoryDict = GetStateHistory(objectCrIdsToReportDateQuery, "cr_object"); 

                var performedActsStatesHistoryDict = GetStateHistory(performedActIdsQuery, "cr_obj_performed_work_act");

                var monitoringSmrStatesHistoryDict = GetStateHistory(monitoringSmrIdsQuery, "cr_obj_monitoring_cmp");

                objectCrStateHistoryDict.ForEach(x => objectCrStateDict[x.Key] = x.Value);

                var objectCrByActDict = performedActStateList
                    .GroupBy(x => x.actid)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.objectCrId).First());
                
                performedActsStatesHistoryDict
                    .Where(x => objectCrByActDict.ContainsKey(x.Key))
                    .ForEach(x => performedActStateDict[objectCrByActDict[x.Key]][x.Key] = x.Value);

                var objectCrByMonitoringDict = monitoringSmrStateList
                    .GroupBy(x => x.monitoringId)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.objectCrId).First());

                monitoringSmrStatesHistoryDict
                    .Where(x => objectCrByMonitoringDict.ContainsKey(x.Key))
                    .GroupBy(x => objectCrByMonitoringDict[x.Key])
                    .ToDictionary(x => x.Key, x => x.Select(y => y.Value).First())
                    .ForEach(x => monitoringSmrStateDict[x.Key] = x.Value);
            }

            int counter = 0;
            var sectionMo = reportParams.ComplexReportParams.ДобавитьСекцию("Municipality");
            var sectionObjectCr = sectionMo.ДобавитьСекцию("ObjectCr");
            var totals = this.GetEmptyTotalsDict();

            foreach (var objectCrListByMo in objectCrList.GroupBy(x => x.municipalityName).OrderBy(x => x.Key))
            {
                sectionMo.ДобавитьСтроку();
                sectionMo["municipalityName"] = objectCrListByMo.Key;

                var municiplaityTotals = this.GetEmptyTotalsDict();

                foreach (var objectCr in objectCrListByMo.OrderBy(x => x.address))
                {
                    sectionObjectCr.ДобавитьСтроку();
                    sectionObjectCr["num"] = ++counter;
                    sectionObjectCr["municipalityName"] = objectCr.municipalityName;
                    sectionObjectCr["address"] = objectCr.address;
                    
                    if (objectCrStateDict.ContainsKey(objectCr.objectCrId))
                    {
                        var state = objectCrStateDict[objectCr.objectCrId];
                        if (objectCrStates.ContainsKey(state))
                        {
                            var index = objectCrStates[state];
                            sectionObjectCr["crObj" + index] = 1;
                            municiplaityTotals["crObj"][index]++;
                        }
                    }

                    if (performedActStateDict.ContainsKey(objectCr.objectCrId))
                    {
                        var stateDict = performedActStateDict[objectCr.objectCrId]
                            .GroupBy(x => x.Value)
                            .ToDictionary(y => y.Key, y => y.Count());

                        foreach (var state in stateDict.Where(x => performedActStates.ContainsKey(x.Key)))
                        {
                            var index = performedActStates[state.Key];
                            sectionObjectCr["act" + index] = state.Value;
                            municiplaityTotals["act"][index] += state.Value;
                        }
                    }

                    if (monitoringSmrStateDict.ContainsKey(objectCr.objectCrId))
                    {
                        var state = monitoringSmrStateDict[objectCr.objectCrId];
                        if (state != null && monitoringSmrStates.ContainsKey(state))
                        {
                            var index = monitoringSmrStates[state];
                            sectionObjectCr["mon" + index] = 1;
                            municiplaityTotals["mon"][index]++;
                        }
                    }
                }

                municiplaityTotals["crObj"].ForEach(x => sectionMo["totalcrObj" + x.Key] = x.Value != 0 ? x.Value.ToStr() : string.Empty);
                municiplaityTotals["act"].ForEach(x => sectionMo["totalact" + x.Key] = x.Value != 0 ? x.Value.ToStr() : string.Empty);
                municiplaityTotals["mon"].ForEach(x => sectionMo["totalmon" + x.Key] = x.Value != 0 ? x.Value.ToStr() : string.Empty);

                municiplaityTotals.ForEach(x => x.Value.ForEach(y => totals[x.Key][y.Key] += y.Value));
            }

            totals["crObj"].ForEach(x => reportParams.SimpleReportParams["totalcrObj" + x.Key] = x.Value != 0 ? x.Value.ToStr() : string.Empty);
            totals["act"].ForEach(x => reportParams.SimpleReportParams["totalact" + x.Key] = x.Value != 0 ? x.Value.ToStr() : string.Empty);
            totals["mon"].ForEach(x => reportParams.SimpleReportParams["totalmon" + x.Key] = x.Value != 0 ? x.Value.ToStr() : string.Empty);
        }

        /// <summary>
        /// Получение статуса из истории, соответствующего дате отчета
        /// </summary>
        /// <param name="objectIdsQuery">запрос идентификаторов объектов, которые созданы до дня сборки отчета, для которых необходимо получить статусы</param>
        /// <param name="entityType">"тип объекта"</param>
        private Dictionary<long, string> GetStateHistory(IQueryable<long> objectIdsQuery, string entityType)
        {
            var serviceStateHistory = Container.Resolve<IDomainService<StateHistory>>();

            var result = serviceStateHistory.GetAll()
                .Where(x => objectIdsQuery.Contains(x.EntityId))
                .Where(x => x.TypeId == entityType)
                .Select(x => new { x.EntityId, FinalStateName = x.FinalState.Name, StartStateName = x.StartState.Name, x.ChangeDate })
                .AsEnumerable()
                .GroupBy(x => x.EntityId)
                .ToDictionary(
                    x => x.Key.ToLong(), 
                    x =>
                    {
                        // Если изменение статуса произошло до даты отчета, то берем ближний (по дате изменения) к дате отчета "новый" статус
                        if (x.Any(y => y.ChangeDate <= reportDate))
                        {
                            return x.Where(y => y.ChangeDate <= reportDate)
                                    .OrderByDescending(y => y.ChangeDate)
                                    .Select(y => y.FinalStateName)
                                    .First();
                        }

                        // Если изменение статуса произошло после даты отчета, то берем ближний (по дате изменения) к дате отчета "старый" статус
                        return x.OrderBy(y => y.ChangeDate)
                                .Select(y => y.StartStateName)
                                .First();
                    });

            return result;
        }

        private Dictionary<string, Dictionary<int, int>> GetEmptyTotalsDict()
        {
            var dict = new Dictionary<string, Dictionary<int, int>>();

            dict.Add("crObj", Enumerable.Range(1, 8).ToDictionary(x => x, _ => 0));
            dict.Add("act", Enumerable.Range(1, 4).ToDictionary(x => x, _ => 0));
            dict.Add("mon", Enumerable.Range(1, 3).ToDictionary(x => x, _ => 0));

            return dict;
        }
    }
}