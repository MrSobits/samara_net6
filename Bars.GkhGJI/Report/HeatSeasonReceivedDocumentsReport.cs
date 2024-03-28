namespace Bars.GkhGji.Report
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

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Utils;

    public class HeatSeasonReceivedDocumentsReport : BasePrintForm
    {
        protected DateTime ReportDate = DateTime.Now;
        protected long HeatingSeasonId;
        protected List<long> MunicipalityListId = new List<long>();
        protected List<TypeHouse> HomeType = new List<TypeHouse>();

        private List<HeatingSystem> heatingSystem = new List<HeatingSystem>();

        public HeatSeasonReceivedDocumentsReport()
            : base(new ReportTemplateBinary(Bars.GkhGji.Properties.Resources.HeatSeasonReceivedDocuments))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string RequiredPermission
        {
            get { return "Reports.GJI.HeatSeasonReceivedDocuments"; }
        }

        public override string Name
        {
            get { return "Принятые документы по подготовке к отопительному сезону"; }
        }

        public override string Desciption
        {
            get { return "Принятые документы по подготовке к отопительному сезону"; }
        }

        public override string GroupName
        {
            get { return "Отопительный сезон"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.HeatSeasonReceivedDocuments"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.HeatingSeasonId = baseParams.Params["heatSeasonPeriod"].ToLong();
            this.ReportDate = baseParams.Params["reportDate"].ToDateTime();

            var realtyObjectType = baseParams.Params["realityObjectType"].ToLong();
            this.HomeType.Clear();

            switch (realtyObjectType)
            {
                case 30:
                    this.HomeType.Add(TypeHouse.ManyApartments);
                    break;
                case 40:
                    this.HomeType.Add(TypeHouse.SocialBehavior);
                    break;
                case 50:
                    this.HomeType.Add(TypeHouse.SocialBehavior);
                    this.HomeType.Add(TypeHouse.ManyApartments);
                    break;
            }

            heatingSystem = baseParams.Params.Get("heatType", new List<HeatingSystem> { HeatingSystem.None });

            var municipalityIdsStr = baseParams.Params.GetAs("municipalityIds", string.Empty);
            this.MunicipalityListId = !string.IsNullOrEmpty(municipalityIdsStr) ? municipalityIdsStr.Split(',').Select(id => id.ToLong()).ToList() : new List<long>();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            reportParams.SimpleReportParams["reportDate"] = string.Format("{0:d MMMM yyyy}", this.ReportDate);
            var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMu");
            var sectionRo = sectionMu.ДобавитьСекцию("sectionRo");
            var sectionTotal = reportParams.ComplexReportParams.ДобавитьСекцию("sectionTotal");

            Func<bool, string> returnStr = b => b ? "1" : string.Empty;

            var realityObjectDomain = this.Container.ResolveDomain<RealityObject>();
            var heatSeasonDomain = this.Container.ResolveDomain<HeatSeason>();
            var heatSeasonDocDomain = this.Container.ResolveDomain<HeatSeasonDoc>();
            var heatSeasonPeriodGjiDomain = this.Container.ResolveDomain<HeatSeasonPeriodGji>();

            using (this.Container.Using(realityObjectDomain, heatSeasonDomain, heatSeasonDocDomain, heatSeasonPeriodGjiDomain))
            {
                var period = heatSeasonPeriodGjiDomain.GetAll().FirstOrDefault(x => x.Id == this.HeatingSeasonId);
                var periodYear = period != null
                    ? period.DateStart.HasValue ? period.DateStart.Value.ToDateTime().Year : this.ReportDate.Year
                    : this.ReportDate.Year;

                var heatSeasonQuery = heatSeasonDomain.GetAll()
                    .WhereIf(this.MunicipalityListId.Count > 0, x => this.MunicipalityListId.Contains(x.RealityObject.Municipality.Id))
                    .Where(x => x.Period.Id == this.HeatingSeasonId)
                    .Where(x => this.HomeType.Contains(x.RealityObject.TypeHouse))
                    .Where(x => x.RealityObject.ConditionHouse != ConditionHouse.Razed)
                    .Where(x => !(x.RealityObject.ConditionHouse == ConditionHouse.Emergency && x.RealityObject.ResidentsEvicted))
                    .Where(x => (!x.RealityObject.DateCommissioning.HasValue || x.RealityObject.DateCommissioning < new DateTime(periodYear, 09, 15)));

                var housesInHeatSeasonIdsQuery = heatSeasonQuery.Select(x => x.RealityObject.Id);

                var housesInHeatSeason = heatSeasonQuery
                    .WhereIf(this.heatingSystem.Count > 0, x => this.heatingSystem.Contains(x.HeatingSystem))
                    .Select(x => new
                    {
                        roId = x.RealityObject.Id,
                        municipalityId = x.RealityObject.Municipality.Id,
                        x.RealityObject.Address,
                        municipalityName = x.RealityObject.Municipality.Name,
                        x.HeatingSystem
                    })
                    .ToList();

                var housesNotInHeatSeason = realityObjectDomain.GetAll()
                    .WhereIf(this.MunicipalityListId.Count > 0, x => this.MunicipalityListId.Contains(x.Municipality.Id))
                    .Where(x => x.ConditionHouse != ConditionHouse.Razed)
                    .Where(x => !(x.ConditionHouse == ConditionHouse.Emergency && x.ResidentsEvicted))
                    .Where(x => (!x.DateCommissioning.HasValue || x.DateCommissioning < new DateTime(periodYear, 09, 15)))
                    .Where(x => this.HomeType.Contains(x.TypeHouse))
                    .Where(x => this.heatingSystem.Contains(x.HeatingSystem))
                    .Where(x => !housesInHeatSeasonIdsQuery.Contains(x.Id))
                    .Select(x => new
                    {
                        roId = x.Id,
                        municipalityId = x.Municipality.Id,
                        x.Address,
                        municipalityName = x.Municipality.Name,
                        x.HeatingSystem
                    })
                    .ToList();

                var realtyObjDict =
                    housesInHeatSeason
                        .Union(housesNotInHeatSeason)
                        .OrderBy(x => x.municipalityName)
                        .GroupBy(x => x.municipalityId)
                        .ToDictionary(
                            x => x.Key,
                            x => x.ToDictionary(y => y.roId, y => new { y.municipalityName, y.Address, y.HeatingSystem }));

                var heatSeasonIdsQuery = heatSeasonQuery.Where(x => this.heatingSystem.Contains(x.HeatingSystem)).Select(x => x.Id);

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

                foreach (var dataByMu in realtyObjDict)
                {
                    sectionMu.ДобавитьСтроку();

                    foreach (var dataByRo in dataByMu.Value.OrderBy(x => x.Value.Address))
                    {
                        sectionRo.ДобавитьСтроку();

                        sectionRo["mu"] = dataByRo.Value.municipalityName;
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
                var countRo = realtyObjDict.Values.SelectMany(x => x.Keys).Count();
                var dataList = documentsData.Values.ToList();
                this.FillTotals(sectionTotal, dataList, countRo, "Total");
            }
        }

        protected void FillTotals(Section section, List<RealityObjHeatSeasonReceivedDocuments> data, int roCount, string totalType)
        {
            section[string.Format("countRo{0}", totalType)] = roCount;
            section[string.Format("haveDoc10{0}", totalType)] = data.Count(x => x.haveDoc10);
            section[string.Format("acceptDoc10{0}", totalType)] = data.Count(x => x.acceptDoc10);
            section[string.Format("haveDoc20{0}", totalType)] = data.Count(x => x.haveDoc20);
            section[string.Format("acceptDoc20{0}", totalType)] = data.Count(x => x.acceptDoc20);
            section[string.Format("haveDoc30{0}", totalType)] = data.Count(x => x.haveDoc30);
            section[string.Format("acceptDoc30{0}", totalType)] = data.Count(x => x.acceptDoc30);
            section[string.Format("haveDoc40{0}", totalType)] = data.Count(x => x.haveDoc40);
            section[string.Format("acceptDoc40{0}", totalType)] = data.Count(x => x.acceptDoc40);
            section[string.Format("haveDoc50{0}", totalType)] = data.Count(x => x.haveDoc50);
            section[string.Format("acceptDoc50{0}", totalType)] = data.Count(x => x.acceptDoc50);
        }
        
        protected sealed class RealityObjHeatSeasonReceivedDocuments
        {
            public bool haveDoc10;

            public bool acceptDoc10;

            public bool haveDoc20;

            public bool acceptDoc20;

            public bool haveDoc30;

            public bool acceptDoc30;

            public bool haveDoc40;

            public bool acceptDoc40;

            public bool haveDoc50;

            public bool acceptDoc50;
        }
    }
}
