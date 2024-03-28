namespace Bars.GkhGji.Report
{
    using System;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    public class WorkWinterInfoReport : BasePrintForm
    {
        private readonly IDomainService<Municipality> _municipalityDomain;
        private readonly IDomainService<WorkWinterCondition> _workWinterInfoDomain;
        private readonly IDomainService<WorkInWinterMark> _workInWinterMarkDomain;

        private DateTime reportDate = DateTime.Now;
        private long[] municipalityIds;

        public WorkWinterInfoReport(
            IDomainService<Municipality> municipalityDomain,
            IDomainService<WorkWinterCondition> workWinterInfoDomain,
            IDomainService<WorkInWinterMark> workInWinterMarkDomain)
            : base(new ReportTemplateBinary(Properties.Resources.WorkWinterInfoReport))
        {
            _municipalityDomain = municipalityDomain;
            _workWinterInfoDomain = workWinterInfoDomain;
            _workInWinterMarkDomain = workInWinterMarkDomain;
        }

        public override string Name
        {
            get
            {
                return "Сведения о подготовке жилищно-коммунального хозяйства к работе в зимних условиях";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Отопительный сезон";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Сведения о подготовке жилищно-коммунального хозяйства к работе в зимних условиях";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.WorkWinterInfo";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.WorkWinterInfo";
            }
        }

        public override string ReportGenerator { get; set; }

        public override void SetUserParams(BaseParams baseParams)
        {
            reportDate = baseParams.Params["reportDate"].ToDateTime();
            municipalityIds = baseParams.Params.GetAs("municipalityIds", string.Empty).ToLongArray();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            reportParams.SimpleReportParams["Период"] = string.Format("{0:MMMM yyyy}", reportDate);

            var information = _workWinterInfoDomain.GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.HeatInputPeriod.Municipality.Id))
                .Where(x => x.HeatInputPeriod.Month == reportDate.Month)
                .Where(x => x.HeatInputPeriod.Year == reportDate.Year)
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.HeatInputPeriod.Municipality.Id,
                    x.WorkInWinterMark.RowNumber,
                    x.Total,
                    x.PreparationTask,
                    x.PreparedForWork,
                    x.FinishedWorks,
                    x.Percent
                })
                .AsEnumerable()
                .GroupBy(x => x.RowNumber)
                .Select(x => new
                {
                    x.Key,
                    TotalSum = x.Sum(y => y.Total),
                    PreparationTaskSum = x.Sum(y => y.PreparationTask),
                    PreparedForWorkSum = x.Sum(y => y.PreparedForWork),
                    FinishedWorksSum = x.Sum(y => y.FinishedWorks),
                    PercentTotal = x.Sum(y => y.PreparationTask) != 0
                        ? x.Sum(y => y.PreparedForWork) / x.Sum(y => y.PreparationTask)
                        : 0
                })
                .ToList();

            var workInWinterMarks = _workInWinterMarkDomain.GetAll().OrderBy(x => x.RowNumber).ToList();

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("Секция");

            foreach (var row in workInWinterMarks)
            {
                var info = information.FirstOrDefault(x => x.Key == row.RowNumber);
                section.ДобавитьСтроку();
                section["Показатели"] = row.Name;
                section["НомерПп"] = row.RowNumber;
                section["ЕдИзмер"] = row.Measure;
                section["Океи"] = row.Okei;
                if (info != null)
                {
                    section["Всего"] = info.TotalSum;
                    section["Задание"] = info.PreparationTaskSum;
                    section["Подготовлено"] = info.PreparedForWorkSum;
                    section["Выполнено"] = info.FinishedWorksSum;
                    section["Процент"] = info.PercentTotal;
                }
            }
        }
    }
}