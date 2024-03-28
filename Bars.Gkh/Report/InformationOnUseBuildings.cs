using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bars.Gkh.Report
{
    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    class InformationOnUseBuildings : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private long[] municipalityIds;

        private DateTime reportDate = DateTime.Now;

        private Dictionary<int, string> monthDict = new Dictionary<int, string>()
                                                        {
                                                            { 1, "Января" }, { 2, "Февраля" }, { 3, "Марта" }, { 4, "Апреля" }, { 5, "Мая" }, { 6, "Июня" },
                                                            { 7, "Июля" }, { 8, "Августа" }, { 9, "Сентября" }, { 10, "Октября" }, { 11, "Ноября" }, { 12, "Декабря" }
                                                        };

        public InformationOnUseBuildings()
            : base(new ReportTemplateBinary(Properties.Resources.InformationOnUseBuildings))
        {
        }

       public override string Name
        {
            get
            {
                return "Справка по использованию домов (Приложение 3)"; 
            }
        }

        public override string Desciption
        {
            get
            {
                return "Справка по использованию домов (Приложение 3)";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Аварийность";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.InformationOnUseBuildings";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GKH.InformationOnUseBuildings";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];
            reportDate = baseParams.Params["dateReport"].ToDateTime();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceEmergencyObject = Container.Resolve<IDomainService<EmergencyObject>>();
            var serviceEmerObjResettlementProgram = Container.Resolve<IDomainService<EmerObjResettlementProgram>>();

            #region Запросы
            var municipalityList = Container.Resolve<IDomainService<Municipality>>().GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Id))
                .Select(x => x.Name)
                .ToList();

            var infoEmergRealObjByCountFloors = serviceEmergencyObject.GetAll()
                         .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                         .Where(x => x.FurtherUse != null)
                         .GroupBy(x => x.FurtherUse.Code)
                         .Select(x => new
                         {
                             x.Key,
                             countRealObj = x.Count(),
                             sumArea = x.Sum(y => y.RealityObject.AreaMkd),
                             sumAreaLiv = x.Sum(y => y.RealityObject.AreaLiving),
                             countPerson = x.Sum(y => y.RealityObject.NumberLiving),
                             countResettlementFlatAmount = x.Sum(y => y.ResettlementFlatAmount),
                             resettlementFlatArea = x.Sum(y => y.ResettlementFlatArea)
                         })
                         .ToDictionary(y => y.Key);

            var infoEmergRealObjByFinPrCountFloors = serviceEmerObjResettlementProgram.GetAll()
                        .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.EmergencyObject.RealityObject.Municipality.Id))
                        .Where(x => x.EmergencyObject.FurtherUse != null)
                        .GroupBy(x => new { FurtherUseCode = x.EmergencyObject.FurtherUse.Code, x.ResettlementProgramSource.Code })
                        .Select(x => new
                        {
                            x.Key,
                            residents = x.Sum(y => y.CountResidents),
                            area = x.Sum(y => y.Area),
                            cost = x.Sum(y => y.Cost)
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.Key.Code)
                        .ToDictionary(x => x.Key, x => x
                               .GroupBy(y => y.Key.FurtherUseCode)
                                   .ToDictionary(y => y.Key, y => new
                                   {
                                       countResidents = y.Sum(z => z.residents),
                                       sumArea = y.Sum(z => z.area),
                                       sumCost = y.Sum(z => z.cost)
                                   }));

            #endregion

            #region Вывод данных
            reportParams.SimpleReportParams["municipalities"] = municipalityList.Aggregate((a, b) => a + ", " + b);
            reportParams.SimpleReportParams["Day"] = reportDate.Date.Day;
            reportParams.SimpleReportParams["Month"] = monthDict.ContainsKey(reportDate.Date.Month) ? monthDict[reportDate.Date.Month] : string.Empty;
            reportParams.SimpleReportParams["Year"] = reportDate.Date.Year;

            var columns = Enumerable.Range(1, 3).Select(x => x.ToStr()).ToList();

            var roByFlDict = infoEmergRealObjByCountFloors;
            foreach (var item in roByFlDict.Where(item => columns.Contains(item.Key)))
            {
                reportParams.SimpleReportParams["totCountHouse" + item.Key] = item.Value.countRealObj;
                reportParams.SimpleReportParams["totArea" + item.Key] = item.Value.sumArea;
                reportParams.SimpleReportParams["totAreaLiv" + item.Key] = item.Value.sumAreaLiv;
                reportParams.SimpleReportParams["totPerson" + item.Key] = item.Value.countPerson;
                reportParams.SimpleReportParams["totCountRes" + item.Key] = item.Value.countResettlementFlatAmount;
                reportParams.SimpleReportParams["totAreaRes" + item.Key] = item.Value.resettlementFlatArea;
            }

            var itemsList = roByFlDict.Where(item => columns.Contains(item.Key)).ToList();

            reportParams.SimpleReportParams["totCountHouse0"] = itemsList.Sum(x => x.Value.countRealObj);
            reportParams.SimpleReportParams["totArea0"] = itemsList.Sum(x => x.Value.sumArea);
            reportParams.SimpleReportParams["totAreaLiv0"] = itemsList.Sum(x => x.Value.sumAreaLiv);
            reportParams.SimpleReportParams["totPerson0"] = itemsList.Sum(x => x.Value.countPerson);
            reportParams.SimpleReportParams["totCountRes0"] = itemsList.Sum(x => x.Value.countResettlementFlatAmount);
            reportParams.SimpleReportParams["totAreaRes0"] = itemsList.Sum(x => x.Value.resettlementFlatArea);

            var roByFinPr = infoEmergRealObjByFinPrCountFloors;
            foreach (var finPr in roByFinPr)
            {
                var itemsList2 = finPr.Value.Where(x => columns.Contains(x.Key)).ToList();

                foreach (var group in itemsList2)
                {
                    reportParams.SimpleReportParams["countPerson" + finPr.Key + group.Key] = group.Value.countResidents;
                    reportParams.SimpleReportParams["sumAreaRes" + finPr.Key + group.Key] = group.Value.sumArea;
                    reportParams.SimpleReportParams["sumCost" + finPr.Key + group.Key] = group.Value.sumCost;
                }

                reportParams.SimpleReportParams["countPerson" + finPr.Key + "0"] = itemsList2.Sum(x => x.Value.countResidents);
                reportParams.SimpleReportParams["sumAreaRes" + finPr.Key + "0"] = itemsList2.Sum(x => x.Value.sumArea);
                reportParams.SimpleReportParams["sumCost" + finPr.Key + "0"] = itemsList2.Sum(x => x.Value.sumCost);
            }
            #endregion

        }
    }
}
