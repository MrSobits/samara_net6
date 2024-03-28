namespace Bars.Gkh.RegOperator.Report
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;

    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Report.PersonalAccountChargeReports;

    using Castle.Windsor;
    using Slepov.Russian.Morpher;

    public class RegionChargeReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private long periodId;
        private long[] mrIds;

        public RegionChargeReport()
            : base(new ReportTemplateBinary(Properties.Resources.RegionChargeReport))
        {
        }

        public override string Name
        {
            get { return "Отчет о начислениях в разрезе края"; }
        }

        public override string Desciption
        {
            get { return "Отчет о начислениях в разрезе края"; }
        }

        public override string GroupName
        {
            get { return "Региональный фонд"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.RegionChargeReport"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhRegOp.RegionChargeReport"; }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var склонятель = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ==");

            var service = Container.Resolve<IChargeReportData>();
            var periodDomain = Container.Resolve<IDomainService<ChargePeriod>>();
            var moDomain = Container.Resolve<IDomainService<Municipality>>();
            List<RaionChargeRecord> records = null;

            try
            {
                var cultureInfo = new CultureInfo("ru-RU");

                var period = periodDomain.GetAll().FirstOrDefault(x => x.Id == periodId);

                if (period == null)
                {
                    throw new ReportProviderException("Не удалось определить период");
                }

                var month = склонятель.Проанализировать(period.StartDate.ToString("MMMM", cultureInfo));
                reportParams.SimpleReportParams["Месяц"] = month.Где;
                reportParams.SimpleReportParams["Год"] = period.StartDate.ToString("yyyy", cultureInfo);

                records = service.GetRaionChargeRecords(mrIds, periodId);

                var section = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияНачислений");
                var i = 0;
                foreach (var rec in records)
                {
                    i++;

                    section.ДобавитьСтроку();
                    section["Номер"] = i;
                    section["Район"] = rec.RaionName;

                    section["МинРазмер"] = rec.MinSize != 0m ? rec.MinSize.ToString() : string.Empty;
                    section["СверхМинРазмер"] = rec.OverMinSize != 0m ? rec.OverMinSize.ToString() : string.Empty;
                    section["Пени"] = rec.Penalty != 0m ? rec.Penalty.ToString() : string.Empty;
                    section["ИтогоНачислено"] = rec.Total != 0m ? rec.Total.ToString() : string.Empty;
                }

                var sumMinSize = records.Sum(x => x.MinSize);
                var sumOverMinSize = records.Sum(x => x.OverMinSize);
                var sumPenalty = records.Sum(x => x.Penalty);
                var sumTotal = records.Sum(x => x.Total);

                reportParams.SimpleReportParams["ИтогоМинРазмер"] = sumMinSize;
                reportParams.SimpleReportParams["ИтогоСверхМинРазмер"] = sumOverMinSize;
                reportParams.SimpleReportParams["ИтогоПени"] = sumPenalty;
                reportParams.SimpleReportParams["Итого"] = sumTotal;
            }
            finally 
            {
                if (records != null)
                {
                    records.Clear();
                }

                Container.Release(service);
                Container.Release(periodDomain);
                Container.Release(moDomain);
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            periodId = baseParams.Params.GetAs("periodId", 0L);
            
            var mrIdsStr = baseParams.Params.GetAs("mrIds", string.Empty);
            mrIds = !string.IsNullOrEmpty(mrIdsStr)
                                  ? mrIdsStr.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];
        }
    }
}