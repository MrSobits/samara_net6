namespace Bars.GkhGji.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.Modules.Reports;
    using B4.Utils;
    using Gkh.Enums;
    using Entities;
    using Enums;

    using Castle.Windsor;

    public class MonthlyProsecutorsOfficeReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }
        public IMonthlyProsecutorsOfficeServiceData _serviceData { get; set; }

        private DateTime reportDate = DateTime.MinValue;
        
        private List<long> municipalityListId = new List<long>();
        
        public MonthlyProsecutorsOfficeReport(IMonthlyProsecutorsOfficeServiceData serviceData)
            : base(new ReportTemplateBinary(serviceData.GetTemplate()))
        {
            _serviceData = serviceData;
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.MonthlyProsecutorsOfficeReport";
            }
        }

        public override string Desciption
        {
            get { return "Отчет для прокуратуры (ежемесячный)"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.MonthlyProsecutorsOffice"; }
        }

        public override string Name
        {
            get { return "Отчет для прокуратуры (ежемесячный)"; }
        }

        private void ParseIds(List<long> list, string Ids)
        {
            list.Clear();

            if (!string.IsNullOrEmpty(Ids))
            {
                var ids = Ids.Split(',');
                foreach (var id in ids)
                {
                    long Id;

                    if (long.TryParse(id, out Id))
                    {
                        if (!list.Contains(Id))
                        {
                            list.Add(Id);
                        }
                    }
                }
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.reportDate = baseParams.Params["reportDate"].ToDateTime();
            this.ParseIds(this.municipalityListId, baseParams.Params["municipalityIds"].ToString());
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            reportParams.SimpleReportParams["month"] = this.reportDate.ToString("MMMM");

            var data = _serviceData.GetData(this.reportDate, municipalityListId);

            var propInfo = data.GetType().GetProperties();

            foreach (var info in propInfo)
            {
                reportParams.SimpleReportParams[info.Name] = info.GetValue(data, new object[0]);
            }
        }
    }
}