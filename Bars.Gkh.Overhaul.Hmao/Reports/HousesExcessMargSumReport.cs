namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    public class HousesExcessMargSumReport : BasePrintForm
    {
        #region Dependency injection members

        public IDomainService<MissingByMargCostDpkrRec> MissingDpkrRecDomain { get; set; }

        #endregion

        public HousesExcessMargSumReport()
            : base(new ReportTemplateBinary(Properties.Resources.HousesExcessMargSum))
        {

        }

        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get
            {
                return "Дома с  превышением предельной стоимости работ, не включенные в ДПКР";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Дома с  превышением предельной стоимости работ, не включенные в ДПКР";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Долгосрочная программа";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.HousesExcessMargSum";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GkhOverhaul.HousesExcessMargSumReport";
            }
        }

        private long[] municipalityIds;

        public override void SetUserParams(BaseParams baseParams)
        {
            var strMunicpalIds = baseParams.Params.GetAs("municipalityIds", string.Empty);

            municipalityIds = !string.IsNullOrEmpty(strMunicpalIds)
                ? strMunicpalIds.Split(',').Select(x => x.ToLong()).ToArray()
                : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var data = MissingDpkrRecDomain.GetAll()
                .WhereIf(municipalityIds.Any(),
                    x => municipalityIds.Contains(x.RealityObject.Municipality.Id)
                    || municipalityIds.Contains(x.RealityObject.MoSettlement.Id))
                .Select(x => new
                {
                    MuName = x.RealityObject.Municipality.Name,
                    MuId = x.RealityObject.Municipality.Id,
                    RealObjId = x.RealityObject.Id,
                    x.ObjectCreateDate,
                    x.RealityObject.Address,
                    x.Area,
                    x.Sum,
                    x.MargSum,
                    x.RealEstateTypeName,
                    x.Year,
                    x.CommonEstateObjects
                })
                .OrderBy(x => x.MuName)
                .ThenBy(x => x.Address)
                .ThenBy(x => x.Year)
                .AsEnumerable()
                .GroupBy(x => x.MuId)
                .ToDictionary(x => x.Key, y => y.GroupBy(x => x.RealObjId)
                                                .ToDictionary(x => x.Key, z => z.ToList()));

            var sectionMunicipality = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMunicipality");

            var number = 1;
            foreach (var muData in data)
            {
                sectionMunicipality.ДобавитьСтроку();

                
                sectionMunicipality["CalcDate"] = muData.Value.First().Value.First().ObjectCreateDate;

                var sectionRealObj = sectionMunicipality.ДобавитьСекцию("sectionRealObj");
                foreach (var roData in muData.Value)
                {
                    // год который превысил  предельное значение
                    var excludeYear = roData.Value.Min(x => x.Year);

                    // по записи, которая превысила  предельное значение, выводим запись со всеми полями
                    var recs = roData.Value.Where(x => x.Year == excludeYear).ToList();

                    var rec = recs.First();
                    sectionRealObj.ДобавитьСтроку();

                    sectionRealObj["Number"] = number++;
                    sectionRealObj["Municipality"] = rec.MuName;
                    sectionRealObj["Address"] = rec.Address;
                    sectionRealObj["PlanYear"] = rec.Year;
                    sectionRealObj["Ceo"] = recs.Select(x => x.CommonEstateObjects).AggregateWithSeparator(", ");
                    sectionRealObj["AreaMkd"] = rec.Area;

                    var sum = recs.Sum(x => x.Sum);
                    sectionRealObj["Sum"] = sum;
                    sectionRealObj["MargSum"] = rec.Area.ToDecimal() > 0 ? (sum / rec.Area.ToDecimal()).RoundDecimal(2) : 0;
                    sectionRealObj["RealEstateTypeName"] = rec.RealEstateTypeName;
                    sectionRealObj["MargSumForSquareMeter"] = rec.MargSum;


                    var isGrouped = recs.Count > 1;

                    foreach (var missingDpkrRec in roData.Value.Where(x => x.Year != excludeYear || isGrouped))
                    {
                        sectionRealObj.ДобавитьСтроку();

                        sectionRealObj["Number"] = number++;
                        sectionRealObj["Municipality"] = missingDpkrRec.MuName;
                        sectionRealObj["Address"] = missingDpkrRec.Address;
                        sectionRealObj["PlanYear"] = missingDpkrRec.Year;
                        sectionRealObj["Ceo"] = missingDpkrRec.CommonEstateObjects;
                        sectionRealObj["AreaMkd"] = missingDpkrRec.Area;
                        sectionRealObj["Sum"] = missingDpkrRec.Sum;
                    }
                }
            }
        }
    }
}