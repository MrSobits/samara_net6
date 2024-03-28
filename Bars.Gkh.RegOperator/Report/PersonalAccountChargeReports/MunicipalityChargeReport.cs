namespace Bars.Gkh.RegOperator.Report
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;

    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Report.PersonalAccountChargeReports;

    using Castle.Windsor;
    using Slepov.Russian.Morpher;

    public class MunicipalityChargeReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private long periodId;
        private long moId;
        private long[] roIds;

        public MunicipalityChargeReport()
            : base(new ReportTemplateBinary(Properties.Resources.MunicipalityChargeReport))
        {
        }

        public override string Name
        {
            get { return "Отчет о начислениях в разрезе МО"; }
        }

        public override string Desciption
        {
            get { return "Отчет о начислениях в разрезе МО"; }
        }

        public override string GroupName
        {
            get { return "Региональный фонд"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.MunicipalityChargeReport"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhRegOp.MunicipalityChargeReport"; }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var склонятель = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ==");

            var service = Container.Resolve<IChargeReportData>();
            var periodDomain = Container.Resolve<IDomainService<ChargePeriod>>();
            var moDomain = Container.Resolve<IRepository<Municipality>>();
            List<RealityObjectChargeRecord> records = null;

            try
            {
                var cultureInfo = new CultureInfo("ru-RU");

                var period = periodDomain.GetAll().FirstOrDefault(x => x.Id == periodId);
                
                if (period == null)
                {
                    throw new ReportProviderException("Не удалось определить период");
                }
                
                var mo = moDomain.GetAll().FirstOrDefault(x => x.Id == moId);

                if (mo == null)
                {
                    throw new ReportProviderException("Не удалось определить муниципальное образование");
                }

                var month = склонятель.Проанализировать(period.StartDate.ToString("MMMM", cultureInfo));
                reportParams.SimpleReportParams["Месяц"] = month.Где;
                reportParams.SimpleReportParams["Год"] = period.StartDate.ToString("yyyy", cultureInfo);

                var moName = склонятель.Проанализировать(mo.Name);

                reportParams.SimpleReportParams["МО"] = moName.Дательный;

                records = service.GetRealityObjectChargeRecords(moId, roIds, periodId);

                var section = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияНачислений");
                var i = 0;
                foreach (var rec in records)
                {
                    i++;

                    section.ДобавитьСтроку();
                    section["Номер"] = i;
                    section["Адрес"] = rec.Address;

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
            moId = baseParams.Params.GetAs("moId", 0L);

            var roIdsStr = baseParams.Params.GetAs("roIds", string.Empty);
            roIds = !string.IsNullOrEmpty(roIdsStr)
                                  ? roIdsStr.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];
        }
    }
}