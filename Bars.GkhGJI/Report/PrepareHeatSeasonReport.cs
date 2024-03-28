namespace Bars.GkhGji.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Castle.Windsor;

    public class PrepareHeatSeasonReport : BasePrintForm
    {
        private DateTime reportDate = DateTime.Now;
        private long heatingSeasonId;
        private List<long> municipalityListId = new List<long>();
        private HeatSeasonDocType docType = HeatSeasonDocType.ActFlushingHeatingSystem;
        private bool docStateIsFinal = true;
        private List<TypeHouse> homeType = new List<TypeHouse>(); 

        public PrepareHeatSeasonReport()
            : base(new ReportTemplateBinary(Bars.GkhGji.Properties.Resources.PrepareHeatSeasonReport))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string RequiredPermission
        {
            get { return "Reports.GJI.PrepareHeatSeason"; }
        }

        public override string Name
        {
            get { return "Акты подготовки к отопительному сезону"; }
        }

        public override string Desciption
        {
            get { return "Акты подготовки к отопительному сезону"; }
        }

        public override string GroupName
        {
            get { return "Отопительный сезон"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.PrepareHeatSeason"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.heatingSeasonId = baseParams.Params["heatSeasonPeriod"].ToLong();
            this.reportDate = baseParams.Params["reportDate"].ToDateTime();
            this.docStateIsFinal = baseParams.Params["state"].ToBool();

            var realtyObjectType = baseParams.Params["realityObjectType"].ToLong();
            this.homeType.Clear();

            switch (realtyObjectType)
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

            var docTypeInt = baseParams.Params["docType"].ToLong();   // 10 - Акт промывки, 20 - Акт опрессовки

            this.docType = docTypeInt == 10
                          ? HeatSeasonDocType.ActFlushingHeatingSystem
                          : HeatSeasonDocType.ActPressingHeatingSystem;

            this.municipalityListId.AddRange(baseParams.Params["municipalityIds"].ToString().Split(',').Select(x => x.ToLong()).ToArray());
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            reportParams.SimpleReportParams["Дата"] = string.Format("{0:d MMMM yyyy}", this.reportDate);
            reportParams.SimpleReportParams["НазваниеОтчета"] = this.docType == HeatSeasonDocType.ActFlushingHeatingSystem
                                                                    ? "Предъявление актов промывки внутридомовых систем ЦО"
                                                                    : "Предъявление актов опрессовки внутридомовых систем ЦО";

            var notEmptyMunicipal = this.municipalityListId.Count > 0 && this.municipalityListId.First() != 0;

            var municipalities = this.Container.Resolve<IDomainService<Municipality>>().GetAll()
                .WhereIf(notEmptyMunicipal, x => this.municipalityListId.Contains(x.Id))
                .Select(x => new { x.Id, x.Name })
                .ToArray();

            var period = this.Container.Resolve<IDomainService<HeatSeasonPeriodGji>>().GetAll().FirstOrDefault(x => x.Id == this.heatingSeasonId);
            var periodYear = period != null ? period.DateStart.HasValue ? period.DateStart.Value.ToDateTime().Year : this.reportDate.Year : this.reportDate.Year;

            var serviceHeatSeason =
                this.Container.Resolve<IDomainService<HeatSeason>>().GetAll()
                .WhereIf(notEmptyMunicipal, x => this.municipalityListId.Contains(x.Id))
                             .Where(x => 
                                 x.Period.Id == this.heatingSeasonId && this.homeType.Contains(x.RealityObject.TypeHouse)
                                 && x.RealityObject.ConditionHouse != ConditionHouse.Razed
                                 && !(x.RealityObject.ConditionHouse == ConditionHouse.Emergency && x.RealityObject.ResidentsEvicted)
                                 && (!x.RealityObject.DateCommissioning.HasValue || x.RealityObject.DateCommissioning < new DateTime(periodYear, 09, 15)));
            
            var housesInHeatSeasonIdsQuery = serviceHeatSeason.Select(x => x.RealityObject.Id);

            var housesInHeatSeason =
                serviceHeatSeason
                         .Where(x => x.HeatingSystem == HeatingSystem.Centralized)
                         .Select(x => new { x.RealityObject.Id, municipalityId = x.RealityObject.Municipality.Id })
                         .ToList();

            var housesNotInHeatSeason =
                this.Container.Resolve<IDomainService<RealityObject>>()
                         .GetAll()
                         .WhereIf(notEmptyMunicipal, x => this.municipalityListId.Contains(x.Id))
                         .Where(x => !housesInHeatSeasonIdsQuery.Contains(x.Id))
                         .Where(x => this.homeType.Contains(x.TypeHouse)
                             && x.ConditionHouse != ConditionHouse.Razed
                             && !(x.ConditionHouse == ConditionHouse.Emergency && x.ResidentsEvicted)
                             && (!x.DateCommissioning.HasValue || x.DateCommissioning < new DateTime(periodYear, 09, 15))
                             && x.HeatingSystem == HeatingSystem.Centralized)
                         .Select(x => new { x.Id, municipalityId = x.Municipality.Id })
                         .ToList();

            var data = housesInHeatSeason
                .Union(housesNotInHeatSeason)
                .GroupBy(x => x.municipalityId)
                         .Select(x => new { x.Key, count = x.Count() })
                         .ToDictionary(x => x.Key, x => x.count);
            
            var heatSeasonIdsQuery = serviceHeatSeason.Select(x => x.Id);

            var actsData = this.Container.Resolve<IDomainService<HeatSeasonDoc>>().GetAll()
                    .WhereIf(this.docStateIsFinal, x => x.State != null && x.State.FinalState == this.docStateIsFinal) // если конечный
                    .WhereIf(!this.docStateIsFinal, x => x.State == null || x.State.FinalState == this.docStateIsFinal) // если не конечный или статус не указан
                    .Where(x => heatSeasonIdsQuery.Contains(x.HeatingSeason.Id))
                    .Where(x => x.TypeDocument == this.docType)
                    .Where(x => x.HeatingSeason.HeatingSystem == HeatingSystem.Centralized)
                    .Select(x => new { municipalityId = x.HeatingSeason.RealityObject.Municipality.Id, actId = x.Id })
                    .ToArray()
                    .GroupBy(x => x.municipalityId)
                    .ToDictionary(x => x.Key, x => x.Count());

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("Секция");

            var totalMkd = 0;
            var totalAct = 0;

            foreach (var row in municipalities)
            {
                section.ДобавитьСтроку();
                section["МуниципальноеОбразование"] = row.Name;
                section["МКДЦО"] = data.ContainsKey(row.Id) ? data[row.Id] : 0;
                section["КолАктов"] = actsData.ContainsKey(row.Id) ? actsData[row.Id] : 0;
                section["Процент"] = actsData.ContainsKey(row.Id) && data.ContainsKey(row.Id) && data[row.Id] != 0 ? (actsData[row.Id].ToDecimal() / data[row.Id]) : 0;

                totalMkd += data.ContainsKey(row.Id) ? data[row.Id] : 0;
                totalAct += actsData.Keys.Contains(row.Id) ? actsData[row.Id] : 0;
            }

            reportParams.SimpleReportParams["МКДЦО"] = totalMkd;
            reportParams.SimpleReportParams["КолАктов"] = totalAct;
            reportParams.SimpleReportParams["Процент"] = totalMkd != 0 ? (totalAct.ToDecimal() / totalMkd) : 0;
        }
    }
}
