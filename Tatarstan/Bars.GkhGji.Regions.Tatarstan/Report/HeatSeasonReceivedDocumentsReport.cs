namespace Bars.GkhGji.Regions.Tatarstan.Report
{    
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Castle.Windsor;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.IoC;

    /// <summary>
    /// Отчёт "Принятые документы по подготовке к отопительному сезону"
    /// </summary>
    public class HeatSeasonReceivedDocumentsReport : Bars.GkhGji.Report.HeatSeasonReceivedDocumentsReport
    {
        private List<string> heatingSystemCodes = new List<string>();
        private string defaultHeatingSystem = HeatingSystem.None.ToInt().ToString();

        public override void SetUserParams(BaseParams baseParams)
        {
            base.SetUserParams(baseParams);

            var heatingSystemStr = baseParams.Params.Get("heatType").ToString();

            this.heatingSystemCodes = !string.IsNullOrEmpty(heatingSystemStr)
                ? heatingSystemStr.Split(", ").ToList()
                : new List<string> { this.defaultHeatingSystem };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var heatSeasonPeriodGjiDomain = this.Container.Resolve<IDomainService<HeatSeasonPeriodGji>>();
            var heatSeasonDomain = this.Container.Resolve<IDomainService<HeatSeason>>();
            var realityObjectDomain = this.Container.Resolve<IDomainService<RealityObject>>();
            var heatSeasonDocDomain = this.Container.Resolve<IDomainService<HeatSeasonDoc>>();
            var techPassportDomain = this.Container.Resolve<IDomainService<TehPassportValue>>();

            Func<bool, string> returnStr = b => b ? "1" : string.Empty;
            
            reportParams.SimpleReportParams["reportDate"] = string.Format("{0:d MMMM yyyy}", this.ReportDate);
            
            var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMu");
            var sectionRo = sectionMu.ДобавитьСекцию("sectionRo");
            var sectionTotal = reportParams.ComplexReportParams.ДобавитьСекцию("sectionTotal");

            using (this.Container.Using(heatSeasonPeriodGjiDomain, heatSeasonDomain, realityObjectDomain, heatSeasonDocDomain, techPassportDomain))
            {
                var period = heatSeasonPeriodGjiDomain
                    .GetAll()
                    .FirstOrDefault(x => x.Id == this.HeatingSeasonId);

                var periodYear = period != null
                    ? period.DateStart.HasValue ? period.DateStart.Value.ToDateTime().Year : this.ReportDate.Year
                    : this.ReportDate.Year;
                
                // получить значения вида Отопительной системы для объектов 
                var heatingSystemValues = techPassportDomain.GetAll()
                    .Where(x => this.heatingSystemCodes.Contains(x.Value ?? this.defaultHeatingSystem))
                    .Where(x => x.FormCode == "Form_3_1")
                    .Where(x => x.CellCode == "1:3")
                    .WhereIf(this.MunicipalityListId.Count > 0, x => this.MunicipalityListId.Contains(x.TehPassport.RealityObject.Municipality.Id))
                    .Where(x => this.HomeType.Contains(x.TehPassport.RealityObject.TypeHouse))
                    .Where(x => x.TehPassport.RealityObject.ConditionHouse != ConditionHouse.Razed)
                    .Where(x => !(x.TehPassport.RealityObject.ConditionHouse == ConditionHouse.Emergency && x.TehPassport.RealityObject.ResidentsEvicted))
                    .Select(x => new
                    {
                        RealityObjectId = x.TehPassport.RealityObject.Id,
                        HeatingSystem = (HeatingSystem)Enum.Parse(typeof(HeatingSystem), x.Value ?? this.defaultHeatingSystem)
                    });

                var heatingSystemValuesDict = heatingSystemValues
                    .ToDictionary(x => x.RealityObjectId, x => x.HeatingSystem);

                var heatSeasonQuery = heatSeasonDomain
                    .GetAll()
                    .Where(x => heatingSystemValues.Any(hs => hs.RealityObjectId == x.RealityObject.Id))
                    .WhereIf(this.MunicipalityListId.Count > 0, x => this.MunicipalityListId.Contains(x.RealityObject.Municipality.Id))
                    .Where(x => x.Period.Id == this.HeatingSeasonId)
                    .Where(x => this.HomeType.Contains(x.RealityObject.TypeHouse))
                    .Where(x => x.RealityObject.ConditionHouse != ConditionHouse.Razed)
                    .Where(x => !(x.RealityObject.ConditionHouse == ConditionHouse.Emergency && x.RealityObject.ResidentsEvicted))
                    .Where(x => (!x.RealityObject.DateCommissioning.HasValue || x.RealityObject.DateCommissioning < new DateTime(periodYear, 09, 15)))
                    .Select(x => new
                    {
                        x.Id,
                        RealityObjectId = x.RealityObject.Id,
                        x.RealityObject.Address,
                        MunicipalityId = x.RealityObject.Municipality.Id,
                        MunicipalityName = x.RealityObject.Municipality.Name,
                        HeatingSystem = heatingSystemValuesDict[x.RealityObject.Id]
                    });

                var housesInHeatSeasonIdsQuery = heatSeasonQuery.Select(x => x.RealityObjectId);

                var housesNotInHeatSeason = realityObjectDomain
                    .GetAll()
                    .Where(x => heatingSystemValues.Any(hs => hs.RealityObjectId == x.Id))
                    .Where(x => !housesInHeatSeasonIdsQuery.Contains(x.Id))
                    .WhereIf(this.MunicipalityListId.Count > 0, x => this.MunicipalityListId.Contains(x.Municipality.Id))
                    .Where(x => x.ConditionHouse != ConditionHouse.Razed)
                    .Where(x => !(x.ConditionHouse == ConditionHouse.Emergency && x.ResidentsEvicted))
                    .Where(x => (!x.DateCommissioning.HasValue || x.DateCommissioning < new DateTime(periodYear, 09, 15)))
                    .Where(x => this.HomeType.Contains(x.TypeHouse))
                    .Select(x => new
                    {
                        Id = default(long),
                        RealityObjectId = x.Id,
                        x.Address,
                        MunicipalityId = x.Municipality.Id,
                        MunicipalityName = x.Municipality.Name,
                        HeatingSystem = heatingSystemValuesDict[x.Id]
                    });

                var heatSeasonIdsQuery = heatSeasonQuery.Select(x => x.Id);

                var documentsData = heatSeasonDocDomain.GetAll()
                    .Where(x => heatSeasonIdsQuery.Contains(x.HeatingSeason.Id))
                    .Select(x => new
                    {
                        x.DocumentDate,
                        roId = x.HeatingSeason.RealityObject.Id,
                        x.HeatingSeason.RealityObject.Address,
                        x.TypeDocument,
                        State = x.State.Name
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.roId)
                    .ToDictionary(
                        x => x.Key,
                        x =>
                        {
                            var result = new RealityObjHeatSeasonReceivedDocuments();

                            var data = x.Select(z => new { z.TypeDocument, z.DocumentDate, z.State })
                                .GroupBy(z => z.TypeDocument)
                                .ToDictionary(
                                    z => z.Key,
                                    z => z.OrderByDescending(p => p.DocumentDate).Select(p => p.State).FirstOrDefault());

                            result.haveDoc10 = data.ContainsKey(HeatSeasonDocType.ActFlushingHeatingSystem);
                            result.haveDoc20 = data.ContainsKey(HeatSeasonDocType.ActPressingHeatingSystem);
                            result.haveDoc30 = data.ContainsKey(HeatSeasonDocType.ActCheckVentilation);
                            result.haveDoc40 = data.ContainsKey(HeatSeasonDocType.ActCheckChimney);
                            result.haveDoc50 = data.ContainsKey(HeatSeasonDocType.Passport);

                            result.acceptDoc10 = result.haveDoc10 && data[HeatSeasonDocType.ActFlushingHeatingSystem] == "Принято ГЖИ";
                            result.acceptDoc20 = result.haveDoc20 && data[HeatSeasonDocType.ActPressingHeatingSystem] == "Принято ГЖИ";
                            result.acceptDoc30 = result.haveDoc30 && data[HeatSeasonDocType.ActCheckVentilation] == "Принято ГЖИ";
                            result.acceptDoc40 = result.haveDoc40 && data[HeatSeasonDocType.ActCheckChimney] == "Принято ГЖИ";
                            result.acceptDoc50 = result.haveDoc50 && data[HeatSeasonDocType.Passport] == "Принято ГЖИ";

                            return result;
                        });

                var allObjectsDict = 
                    heatSeasonQuery.ToList()
                        .Union(housesNotInHeatSeason.ToList())
                    .OrderBy(x => x.MunicipalityName)
                    .GroupBy(x => x.MunicipalityId)
                    .ToDictionary(
                        x => x.Key,
                        x => x.ToDictionary(y => y.RealityObjectId, y => new { y.MunicipalityName, y.Address, y.HeatingSystem }));

                foreach (var dataByMu in allObjectsDict)
                {
                    sectionMu.ДобавитьСтроку();

                    foreach (var dataByRo in dataByMu.Value.OrderBy(x => x.Value.Address))
                    {
                        sectionRo.ДобавитьСтроку();

                        sectionRo["mu"] = dataByRo.Value.MunicipalityName;
                        sectionRo["address"] = dataByRo.Value.Address;
                        sectionRo["heatingSystem"] = dataByRo.Value.HeatingSystem.GetEnumMeta().Display;

                        if (documentsData.ContainsKey(dataByRo.Key))
                        {
                            var data = documentsData[dataByRo.Key];
                            sectionRo["haveDoc10"] = returnStr(data.haveDoc10);
                            sectionRo["acceptDoc10"] = returnStr(data.acceptDoc10);
                            sectionRo["haveDoc20"] = returnStr(data.haveDoc20);
                            sectionRo["acceptDoc20"] = returnStr(data.acceptDoc20);
                            sectionRo["haveDoc30"] = returnStr(data.haveDoc30);
                            sectionRo["acceptDoc30"] = returnStr(data.acceptDoc30);
                            sectionRo["haveDoc40"] = returnStr(data.haveDoc40);
                            sectionRo["acceptDoc40"] = returnStr(data.acceptDoc40);
                            sectionRo["haveDoc50"] = returnStr(data.haveDoc50);
                            sectionRo["acceptDoc50"] = returnStr(data.acceptDoc50);
                        }
                    }

                    // заполнение итогов по мун. образованию
                    var currentMuRoIdList = dataByMu.Value.Keys.ToList();
                    var dataListByMu = documentsData.Where(x => currentMuRoIdList.Contains(x.Key)).Select(x => x.Value).ToList();
                    this.FillTotals(sectionMu, dataListByMu, currentMuRoIdList.Count, "Mu");
                }

                // заполнение итогов
                sectionTotal.ДобавитьСтроку();
                var countRo = allObjectsDict.Values.SelectMany(x => x.Keys).Count();
                var dataList = documentsData.Values.ToList();
                this.FillTotals(sectionTotal, dataList, countRo, "Total");
            }
        }
    }
}