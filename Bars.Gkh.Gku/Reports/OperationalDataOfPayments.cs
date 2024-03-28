namespace Bars.Gkh.Gku.Reports
{
    using System;
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Hcs;

    using Castle.Windsor;

    public class OperationalDataOfPayments : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private DateTime date;
        private long[] municipalityIds;

        public OperationalDataOfPayments()
            : base(new ReportTemplateBinary(Properties.Resources.OperationalDataOfPayments))
        {
        }

        public override string Name
        {
            get
            {
                return "Отчет по сведениям о ЖКУ";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Отчет по сведениям о ЖКУ";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Сведения о ЖКУ";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.OperationalDataOfPayments";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GKH.OperationalDataOfPayments";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];

            date = baseParams.Params.GetAs("reportDate", string.Empty).ToDateTime();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceHouseOverallBalance = Container.Resolve<IDomainService<HouseOverallBalance>>();
            var serviceMunicipality = Container.Resolve<IDomainService<Municipality>>();

            var municipalities = serviceMunicipality.GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Id))
                .Select(x => new { x.Id, x.Name })
                .OrderBy(x => Name)
                .ToDictionary(x => x.Id, v => v.Name);


            var reportDate = date;

            var balanceData = serviceHouseOverallBalance.GetAll()
                .Where(x => x.DateCharging <= reportDate)
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Select(x => new
                {
                    muId = x.RealityObject.Municipality.Id,
                    x.DateCharging,
                    x.MonthCharge,
                    x.Paid
                })
                .ToArray();

            var groupedData = balanceData.GroupBy(x => x.muId)
                .ToDictionary(
                    x => x.Key,
                    x =>
                    {
                        var res = x.Where(y => y.DateCharging > reportDate.AddYears(-1)).ToArray();
                        var monthAvg = res.Any() ? res.Sum(y => y.MonthCharge) / 12 : 0;

                        var charged = x.Sum(y => y.MonthCharge);
                        var paid = x.Sum(y => y.Paid);
                        var debt = charged - paid;

                        var ratio = monthAvg != 0 ? debt / monthAvg : 0;
                        var collectionRatio = paid != 0 ? charged / paid : 0;

                        return new { debt, monthAvg, ratio, collectionRatio, charged, paid };
                    });

            var totals = groupedData.GroupBy(x => 1)
                .Select(x =>
                {
                    var debt = x.Sum(y => y.Value.debt);
                    var monthAvg = x.Sum(y => y.Value.monthAvg);

                    var ratio = monthAvg != 0 ? debt / monthAvg : 0;

                    var charged = x.Sum(y => y.Value.charged);
                    var paid = x.Sum(y => y.Value.paid);
                    var collectionRatio = paid != 0 ? charged / paid : 0;

                    return new { debt, monthAvg, ratio, collectionRatio };
                })
                    .FirstOrDefault();

            reportParams.SimpleReportParams["date"] = reportDate.ToString("dd.MM.yyyy");
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("Section");

            foreach (var municipality in municipalities.OrderBy(x => x.Value))
            {
                section.ДобавитьСтроку();

                section["col1"] = municipality.Value;

                if (groupedData.ContainsKey(municipality.Key))
                {
                    var data = groupedData[municipality.Key];


                    section["col2"] = data.debt;
                    section["col3"] = data.monthAvg;
                    section["col4"] = data.ratio;
                    section["col5"] = data.collectionRatio;
                }
            }

            reportParams.SimpleReportParams["TotCol2"] = totals != null ? totals.debt : 0;
            reportParams.SimpleReportParams["TotCol3"] = totals != null ? totals.monthAvg : 0;
            reportParams.SimpleReportParams["TotCol4"] = totals != null ? totals.ratio : 0;
            reportParams.SimpleReportParams["TotCol5"] = totals != null ? totals.collectionRatio : 0;
        }
    }
}