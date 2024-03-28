namespace Bars.GkhDi.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhDi.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Заполнение данных для рейтинга УК в разделе Деятельность УК
    /// </summary>

    class FillGenInformationHousesRankingYK : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private List<long> municipalityIdsList = new List<long>();

        private long periodDiId;

        public FillGenInformationHousesRankingYK()
            : base(new ReportTemplateBinary(Properties.Resources.FillGenInformationHousesRankingYK))
        {
        }
        
        public override string Name
        {
            get
            {
                return "Заполнение общей информации по домам для Рейтинга УК";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Заполнение общей информации по домам для Рейтинга УК";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Раскрытие  информации о деятельности УК";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.FillGenInformationHousesRankingYK";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.Di.FillGenInformationHousesRankingYK";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            periodDiId = baseParams.Params.GetAs<long>("periodDi");

            var strMunicpalIds = baseParams.Params.GetAs("municipalityIds", string.Empty);

            municipalityIdsList = !string.IsNullOrEmpty(strMunicpalIds)
                ? strMunicpalIds.Split(',').Select(x => x.ToLong()).ToList()
                : new List<long>();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceMunicipality = this.Container.Resolve<IDomainService<Municipality>>();
            var servicePeriodDi = this.Container.Resolve<IDomainService<PeriodDi>>();
            var serviceRealityObject = this.Container.Resolve<IDomainService<RealityObject>>();
            var serviceTehPassportValue = this.Container.Resolve<IDomainService<TehPassportValue>>();
            var serviceManOrgContractRealityObject = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var period = servicePeriodDi.Load(this.periodDiId);

            var startDate = period.DateStart;
            var endDate = period.DateEnd;

            var municipalityDict = serviceMunicipality.GetAll()
                .Where(x => this.municipalityIdsList.Contains(x.Id))
                .OrderBy(x => x.Name)
                .ToDictionary(x => x.Id, x => x.Name);

            var manOrgRobjectQuery = serviceManOrgContractRealityObject.GetAll()
                .WhereIf(this.municipalityIdsList.Count > 0, x => this.municipalityIdsList.Contains(x.RealityObject.Municipality.Id))
                .Where(x => x.ManOrgContract.ManagingOrganization.ActivityDateEnd == null || x.ManOrgContract.ManagingOrganization.ActivityDateEnd > period.DateStart)
                .Where(x => x.ManOrgContract.StartDate == null || x.ManOrgContract.StartDate <= endDate)
                .Where(x => x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= startDate)
                .Where(x => x.RealityObject.TypeHouse != TypeHouse.BlockedBuilding)
                .Where(x => x.ManOrgContract.TypeContractManOrgRealObj != TypeContractManOrg.DirectManag)
                .Where(x => x.ManOrgContract.ManagingOrganization != null);

            var manOrgRobjectsDictByMuDict = manOrgRobjectQuery
                .Select(x => new
                {
                    municipality = x.RealityObject.Municipality.Id,
                    manOrgName = x.ManOrgContract.ManagingOrganization.Contragent != null ? x.ManOrgContract.ManagingOrganization.Contragent.Name : string.Empty,
                    manOrgId = x.ManOrgContract.ManagingOrganization.Id,
                    robjectId = x.RealityObject.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.municipality)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => new { y.manOrgId, y.manOrgName })
                          .ToDictionary(
                                y => y.Key,
                                y => y.Select(z => z.robjectId).Distinct().ToList()));


            var manOrgsIdsQuery = manOrgRobjectQuery.Select(x => x.ManOrgContract.ManagingOrganization.Id);

            var conditionHouseList = Enum.GetValues(typeof(ConditionHouse)).Cast<ConditionHouse>().ToList();
            var realtyObjectLandQuery = this.Container.Resolve<IDomainService<RealityObjectLand>>().GetAll().Where(x => x.CadastrNumber != null);
            var commonAreas = serviceTehPassportValue.GetAll()
                .Where(x => x.FormCode == "Form_1_3")
                .Where(x => x.CellCode == "3:1")
                .Where(x => x.Value != null)
                .Where(x => x.Value != string.Empty);

            var roInfo = serviceRealityObject.GetAll()
                 .Where(x => this.municipalityIdsList.Contains(x.Municipality.Id))
                 .Select(x => new
                 {
                     x.Id,
                     x.Address,
                     col5 = x.DateCommissioning.HasValue,
                     col6 = x.AreaMkd.HasValue,
                     col7 = x.AreaLivingNotLivingMkd.HasValue,
                     col8 = x.AreaLiving.HasValue,
                     col9 = commonAreas.Any(y => y.TehPassport.RealityObject.Id == x.Id),
                     col10 = x.NumberLiving.HasValue,
                     col11 = realtyObjectLandQuery.Any(y => y.RealityObject.Id == x.Id),
                     col12 = conditionHouseList.Contains(x.ConditionHouse)
                 })
                 .ToDictionary(x => x.Id);

            var roDiInfo = Container.Resolve<IDomainService<FinActivityManagRealityObj>>().GetAll()
                  .Where(x => manOrgsIdsQuery.Contains(x.DisclosureInfo.ManagingOrganization.Id))
                  .Where(x => x.DisclosureInfo.PeriodDi.Id == period.Id)
                  .Where(x => this.municipalityIdsList.Contains(x.RealityObject.Municipality.Id))
                  .Select(x => new
                  {
                      manOrgId = x.DisclosureInfo.ManagingOrganization.Id,
                      roId = x.RealityObject.Id,
                      hasSumIncomeManage = x.SumIncomeManage.HasValue,
                      hasSumFactExpense = x.SumFactExpense.HasValue
                  })
                  .AsEnumerable()
                  .GroupBy(x => x.manOrgId)
                  .ToDictionary(
                      x => x.Key,
                      x => x.GroupBy(y => y.roId)
                             .ToDictionary(
                                 y => y.Key,
                                 y => new
                                 {
                                     col13 = y.Any(z => z.hasSumIncomeManage),
                                     col14 = y.Any(z => z.hasSumFactExpense),
                                 })
                  );
            var sectionMO = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMO");
            var sectionOrgName = sectionMO.ДобавитьСекцию("sectionOrgName");
            var section = sectionOrgName.ДобавитьСекцию("section");
            reportParams.SimpleReportParams["Year"] = period.Name.Substring(0, 4);

            var n = 1;
            foreach (var municipality in municipalityDict)
            {
                sectionMO.ДобавитьСтроку();
                sectionMO["MO"] = municipality.Value;
                
                if(!manOrgRobjectsDictByMuDict.ContainsKey(municipality.Key))
                {
                    continue;
                }

                var moTotals = Enumerable.Range(5, 10).ToDictionary(x => x.ToStr(), _ => false);

                var manOrgPercentSum = 0d;
                var manOrgCount = 0;

                foreach (var manOrg in manOrgRobjectsDictByMuDict[municipality.Key])
                {
                    var manorgTotals = Enumerable.Range(5, 10).ToDictionary(x => x.ToStr(), _ => false);
                    
                    sectionOrgName.ДобавитьСтроку();
                    sectionOrgName["OrgName"] = manOrg.Key.manOrgName;

                    var roPercentSum = 0d;
                    var roCount = 0;
                    
                    foreach (var ro in manOrg.Value.Where(roInfo.ContainsKey))
                    {
                        section.ДобавитьСтроку();

                        var robjectInfo = roInfo[ro];
                       
                        var col5 = robjectInfo.col5;
                        var col6 = robjectInfo.col6;
                        var col7 = robjectInfo.col7;
                        var col8 = robjectInfo.col8;
                        var col9 = robjectInfo.col9;
                        var col10 = robjectInfo.col10;
                        var col11 = robjectInfo.col11;
                        var col12 = robjectInfo.col12;
                        var col13 = false;
                        var col14 = false;

                        if (roDiInfo.ContainsKey(manOrg.Key.manOrgId))
                        {
                            var manOrgInfo = roDiInfo[manOrg.Key.manOrgId];
                            if (manOrgInfo.ContainsKey(ro))
                            {
                                col13 = manOrgInfo[ro].col13;
                                col14 = manOrgInfo[ro].col14;
                            }
                        }

                        section["Num"] = n++;
                        section["MOName"] = municipality.Value;
                        section["ManagingOrgName"] = manOrg.Key.manOrgName;
                        section["Address"] = robjectInfo.Address;

                        section["YearOperation"] = col5 ? 1 : 0;
                        section["GrossArea"] = col6 ? 1 : 0;
                        section["AreaResNonRes"] = col7 ? 1 : 0;
                        section["AreaRes"] = col8 ? 1 : 0;
                        section["AreaCommon"] = col9 ? 1 : 0;
                        section["ResidentCount"] = col10 ? 1 : 0;
                        section["CadNumber"] = col11 ? 1 : 0;
                        section["State"] = col12 ? 1 : 0;
                        section["Income"] = col13 ? 1 : 0;
                        section["Consumption"] = col14 ? 1 : 0;

                        manorgTotals["5"] |= col5;
                        manorgTotals["6"] |= col6;
                        manorgTotals["7"] |= col7;
                        manorgTotals["8"] |= col8;
                        manorgTotals["9"] |= col9;
                        manorgTotals["10"] |= col10;
                        manorgTotals["11"] |= col11;
                        manorgTotals["12"] |= col12;
                        manorgTotals["13"] |= col13;
                        manorgTotals["14"] |= col14;

                        var PersentInfo = (col5.ToLong() + col6.ToLong() + col7.ToLong() + col8.ToLong() + col9.ToLong() + col10.ToLong() + col11.ToLong() + col12.ToLong() +
                           col13.ToLong() + col14.ToLong()) * 100 / 10.0;

                        roPercentSum += PersentInfo;
                        ++roCount;
                        section["PersentInfo"] = PersentInfo;
                    }

                    Enumerable.Range(5, 10).Select(x => x.ToStr()).ForEach(x => moTotals[x] |= manorgTotals[x]);

                    var ItogoYearOperation = manorgTotals["5"] ? 1 : 0;
                    var ItogoGrossArea = manorgTotals["6"] ? 1 : 0;
                    var ItogoAreaResNonRes = manorgTotals["7"] ? 1 : 0;
                    var ItogoAreaRes = manorgTotals["8"] ? 1 : 0;
                    var ItogoAreaCommon = manorgTotals["9"] ? 1 : 0;
                    var ItogoResidentCount = manorgTotals["10"] ? 1 : 0;
                    var ItogoCadNumber = manorgTotals["11"] ? 1 : 0;
                    var ItogoState = manorgTotals["12"] ? 1 : 0;
                    var ItogoIncome = manorgTotals["13"] ? 1 : 0;
                    var ItogoConsumption = manorgTotals["14"] ? 1 : 0;
                    
                    sectionOrgName["ItogoMOName"] = municipality.Value;
                    sectionOrgName["ItogoManagingOrgName"] = manOrg.Key.manOrgName;

                    sectionOrgName["ItogoYearOperation"] = ItogoYearOperation;
                    sectionOrgName["ItogoGrossArea"] = ItogoGrossArea;
                    sectionOrgName["ItogoAreaResNonRes"] = ItogoAreaResNonRes;
                    sectionOrgName["ItogoAreaRes"] = ItogoAreaRes;
                    sectionOrgName["ItogoAreaCommon"] = ItogoAreaCommon;
                    sectionOrgName["ItogoResidentCount"] = ItogoResidentCount;
                    sectionOrgName["ItogoCadNumber"] = ItogoCadNumber;
                    sectionOrgName["ItogoState"] = ItogoState;
                    sectionOrgName["ItogoIncome"] = ItogoIncome;
                    sectionOrgName["ItogoConsumption"] = ItogoConsumption;
                    sectionOrgName["ItogoPersentInfo"] = Math.Round(roCount > 0 ? (roPercentSum / roCount): 0, 2);

                    manOrgPercentSum += roCount > 0 ? (roPercentSum / roCount) : 0;
                    ++manOrgCount;

                }
                
                sectionMO["TotalMOName"] = municipality.Value;

                sectionMO["TotalYearOperation"] = moTotals["5"] ? 1 : 0;
                sectionMO["TotalGrossArea"] = moTotals["6"] ? 1 : 0;
                sectionMO["TotalAreaResNonRes"] = moTotals["7"] ? 1 : 0;
                sectionMO["TotalAreaRes"] = moTotals["8"] ? 1 : 0;
                sectionMO["TotalAreaCommon"] = moTotals["9"] ? 1 : 0;
                sectionMO["TotalResidentCount"] = moTotals["10"] ? 1 : 0;
                sectionMO["TotalCadNumber"] = moTotals["11"] ? 1 : 0;
                sectionMO["TotalState"] = moTotals["12"] ? 1 : 0;
                sectionMO["TotalIncome"] = moTotals["13"] ? 1 : 0;
                sectionMO["TotalConsumption"] = moTotals["14"] ? 1 : 0;
                sectionMO["TotalPersentInfo"] = Math.Round(manOrgCount > 0 ? (manOrgPercentSum / manOrgCount): 0, 2);

            }

        }
    }
}
