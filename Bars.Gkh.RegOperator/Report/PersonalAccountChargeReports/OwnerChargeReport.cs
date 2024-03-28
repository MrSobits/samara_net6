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

    public class OwnerChargeReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private long periodId;
        private long roId;
        private long[] accountIds;
        
        public OwnerChargeReport()
            : base(new ReportTemplateBinary(Properties.Resources.OwnerChargeReport))
        {
        }

        public override string Name
        {
            get { return "Отчет о начислениях в разрезе собственников"; }
        }

        public override string Desciption
        {
            get { return "Отчет о начислениях в разрезе собственников"; }
        }

        public override string GroupName
        {
            get { return "Региональный фонд"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.OwnerChargeReport"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhRegOp.OwnerChargeReport"; }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var склонятель = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ==");

            var service = Container.Resolve<IChargeReportData>();
            var periodDomain = Container.Resolve<IDomainService<ChargePeriod>>();
            var roDomain = Container.Resolve<IDomainService<RealityObject>>();
            List<AccountChargeRecord> records = null;

            try
            {
                var cultureInfo = new CultureInfo("ru-RU");

                var period = periodDomain.GetAll().FirstOrDefault(x => x.Id == periodId);
                if (period == null)
                {
                    throw new ReportProviderException("Не удалось определить период");
                }
                
                var ro = roDomain.GetAll().FirstOrDefault(x => x.Id == roId);
                if (ro == null)
                {
                    throw new ReportProviderException("Не удалось определить дом");
                }

                var month = склонятель.Проанализировать(period.StartDate.ToString("MMMM", cultureInfo));
                reportParams.SimpleReportParams["Месяц"] = month.Где;
                reportParams.SimpleReportParams["Год"] = period.StartDate.ToString("yyyy", cultureInfo);
                reportParams.SimpleReportParams["АдресМКД"] = ro.FiasAddress != null ? ro.FiasAddress.AddressName : ro.Address;
                
                records = service.GetOwnerChargeRecords(roId, accountIds, periodId);

                var section = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияНачислений");
                var i = 0;
                foreach (var rec in records)
                {
                    i++;

                    section.ДобавитьСтроку();
                    section["Номер"] = i;
                    section["Помещение"] = rec.Room;
                    section["Собственник"] = rec.Owner;

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
                Container.Release(roDomain);
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            periodId = baseParams.Params.GetAs("periodId", 0L);
            roId = baseParams.Params.GetAs("roId", 0L);

            var accountIdsStr = baseParams.Params.GetAs("accountIds", string.Empty);
            accountIds = !string.IsNullOrEmpty(accountIdsStr)
                                  ? accountIdsStr.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];
        }
    }
}