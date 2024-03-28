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
    using Bars.Gkh.Utils;

    public class HeatSeasonReadinessReport : BasePrintForm
    {
        private readonly List<long> municipalityListId = new List<long>();
        private readonly List<TypeHouse> homeType = new List<TypeHouse>();

        private DateTime reportDate = DateTime.Now;
        private long heatingSeasonId;
        private List<HeatingSystem> heatingSystem = new List<HeatingSystem>();

        public HeatSeasonReadinessReport()
            : base(new ReportTemplateBinary(Properties.Resources.HeatSeasonReadinessReport))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string RequiredPermission
        {
            get { return "Reports.GJI.HeatSeasonReadiness"; }
        }

        public override string Name
        {
            get { return "Паспорт готовности к отопительному сезону"; }
        }

        public override string Desciption
        {
            get { return "Паспорт готовности к отопительному сезону"; }
        }

        public override string GroupName
        {
            get { return "Отопительный сезон"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.HeatSeasonReadiness"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            // текущий период
            var periodId = this.Container.Resolve<IDomainService<HeatSeasonPeriodGji>>().GetAll()
                .Where(x => x.DateStart < DateTime.Now)
                .Select(x => x.Id)
                .ToArray()
                .LastOrDefault();

            // если не выбрали отопит сезон, то текущий
            this.heatingSeasonId = baseParams.Params["heatSeasonPeriod"].ToLong() == 0 ? periodId : baseParams.Params["heatSeasonPeriod"].ToLong();

            // если не выбрали дату, то текущая
            this.reportDate = baseParams.Params["reportDate"].ToDateTime() == DateTime.MinValue ? DateTime.Now.Date : baseParams.Params["reportDate"].ToDateTime();

            var realityObjectType = baseParams.Params["realityObjectType"].ToLong();
            this.homeType.Clear();

            switch (realityObjectType)
            {
                case 30:
                    this.homeType.Add(TypeHouse.ManyApartments);
                    break;
                case 40:
                    this.homeType.Add(TypeHouse.SocialBehavior);
                    break;
                case 50:
                    this.homeType.Add(TypeHouse.SocialBehavior);
                    this.homeType.Add(TypeHouse.ManyApartments);
                    break;
            }

            heatingSystem = baseParams.Params.Get("heatType", new List<HeatingSystem> { HeatingSystem.None });

            this.municipalityListId.AddRange(baseParams.Params["municipalityIds"].ToString().Split(',').Select(x => x.ToLong()).ToArray());
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            reportParams.SimpleReportParams["Дата"] = string.Format("{0:d MMMM yyyy}", this.reportDate);

            var notEmptyMunicipal = this.municipalityListId.Count > 0 && this.municipalityListId.First() != 0;

            var municipalities = this.Container.Resolve<IDomainService<Municipality>>().GetAll()
                .WhereIf(notEmptyMunicipal, x => this.municipalityListId.Contains(x.Id))
                .Select(x => new { x.Id, x.Name })
                .OrderBy(x => x.Name)
                .ToArray();

            var period = this.Container.Resolve<IDomainService<HeatSeasonPeriodGji>>()
                         .GetAll()
                         .FirstOrDefault(x => x.Id == this.heatingSeasonId);

            var periodYear = period != null ? period.DateStart.HasValue ? period.DateStart.Value.ToDateTime().Year : this.reportDate.Year : this.reportDate.Year;

            var serviceHeatSeason = this.Container.Resolve<IDomainService<HeatSeason>>().GetAll()
                         .WhereIf(notEmptyMunicipal, x => this.municipalityListId.Contains(x.RealityObject.Municipality.Id))
                         .Where(x => x.Period.Id == this.heatingSeasonId && this.homeType.Contains(x.RealityObject.TypeHouse)
                             && x.RealityObject.ConditionHouse != ConditionHouse.Razed
                             && !(x.RealityObject.ConditionHouse == ConditionHouse.Emergency
                                  && x.RealityObject.ResidentsEvicted)
                             && (!x.RealityObject.DateCommissioning.HasValue
                                 || x.RealityObject.DateCommissioning < new DateTime(periodYear, 09, 15)));

            var housesInHeatSeasonIdsQuery = serviceHeatSeason.Select(x => x.RealityObject.Id);

            var housesInHeatSeason =
                serviceHeatSeason
                    .WhereIf(this.heatingSystem.Count > 0, x => this.heatingSystem.Contains(x.HeatingSystem))
                    .Select(x => new { x.RealityObject.Id, municipalityId = x.RealityObject.Municipality.Id })
                    .ToList();

            var housesNotInHeatSeason =
                this.Container.Resolve<IDomainService<RealityObject>>()
                         .GetAll()
                         .WhereIf(notEmptyMunicipal, x => this.municipalityListId.Contains(x.Municipality.Id))
                         .Where(x => this.homeType.Contains(x.TypeHouse)
                             && x.ConditionHouse != ConditionHouse.Razed
                             && !(x.ConditionHouse == ConditionHouse.Emergency && x.ResidentsEvicted)
                             && (!x.DateCommissioning.HasValue || x.DateCommissioning < new DateTime(periodYear, 09, 15))
                             && this.heatingSystem.Contains(x.HeatingSystem))
                         .Where(x => !housesInHeatSeasonIdsQuery.Contains(x.Id))
                         .Select(x => new { x.Id, municipalityId = x.Municipality.Id })
                         .ToList();

            var houses = housesInHeatSeason
                .Union(housesNotInHeatSeason)
                .GroupBy(x => x.municipalityId)
                .Select(x => new { x.Key, count = x.Count() })
                .ToDictionary(x => x.Key, x => x.count);

            var heatSeasonIdsQuery = serviceHeatSeason.Select(x => x.Id);

            var passportsData = this.Container.Resolve<IDomainService<HeatSeasonDoc>>().GetAll()
                .Where(x => heatSeasonIdsQuery.Contains(x.HeatingSeason.Id))
                .Where(x => this.heatingSystem.Contains(x.HeatingSeason.HeatingSystem))
                .Where(x => x.TypeDocument == HeatSeasonDocType.Passport)
                .Select(x => new
                {
                    finalState = x.State != null && x.State.FinalState,
                    municipalityId = x.HeatingSeason.RealityObject.Municipality.Id
                })
                .ToArray();

            var submitted = passportsData
                .GroupBy(x => x.municipalityId)
                .ToDictionary(x => x.Key, x => x.Count());

            var accepted = passportsData.Where(x => x.finalState)
                .GroupBy(x => x.municipalityId)
                .ToDictionary(x => x.Key, x => x.Count());

            var totalMkd = 0;
            var totalSubmitted = 0;
            var totalAccepted = 0;

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("Секция");
            foreach (var row in municipalities)
            {
                section.ДобавитьСтроку();
                section["МуниципальноеОбразование"] = row.Name;
                section["МКД"] = houses.ContainsKey(row.Id) ? houses[row.Id] : 0;

                var tempCount = submitted.ContainsKey(row.Id) ? submitted[row.Id].ToDecimal() : 0m;
                section["КолПред"] = tempCount;

                if (houses.ContainsKey(row.Id) && houses[row.Id] != 0) section["ПроцентПред"] = tempCount / houses[row.Id];
                else section["ПроцентПред"] = 0m;

                tempCount = accepted.ContainsKey(row.Id) ? accepted[row.Id] : 0;
                section["КолПринят"] = tempCount;

                if (houses.ContainsKey(row.Id) && houses[row.Id] != 0) section["ПроцентПрин"] = tempCount.ToDecimal() / houses[row.Id];
                else section["ПроцентПрин"] = 0m;

                totalMkd += houses.ContainsKey(row.Id) ? houses[row.Id] : 0;

                totalSubmitted += submitted.ContainsKey(row.Id) ? submitted[row.Id] : 0;
                totalAccepted += accepted.ContainsKey(row.Id) ? accepted[row.Id] : 0;
            }

            reportParams.SimpleReportParams["МКД"] = totalMkd;
            reportParams.SimpleReportParams["КолПред"] = totalSubmitted;
            reportParams.SimpleReportParams["ПроцентПред"] = totalMkd != 0 ? (totalSubmitted.ToDecimal() / totalMkd) : 0;
            reportParams.SimpleReportParams["КолПринят"] = totalAccepted;
            reportParams.SimpleReportParams["ПроцентПрин"] = totalMkd != 0 ? (totalAccepted.ToDecimal() / totalMkd) : 0;
        }
    }
}