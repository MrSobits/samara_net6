namespace Bars.GkhGji.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Properties;
    using Bars.GkhGji.Report.Form1Controll;

    using Castle.Windsor;

    public class Form1ControllReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private List<long> municipalityIds = new List<long>();

        private DateTime dateStart = DateTime.MinValue;

        private DateTime dateEnd = DateTime.MaxValue;

        private List<int> rowsSection1 = new List<int> { 1, 2, 3, 5, 7, 8, 9, 10, 11, 12, 13, 14 };
        
        private List<int> rowsSection2 = new List<int> { 18, 19, 20, 22, 23, 24, 28, 32, 33, 34, 35, 37, 38, 39, 40, 42, 46, 47, 48, 49, 50, 51 };

        private List<int> rowsSection3 = new List<int> { 54, 55, 56, 57, 58, 60, 61, 62, 63, 64 };

        public Form1ControllReport()
            : base(new ReportTemplateBinary(Resources.Form1ControllReport))
        {
        }

        public override string Name
        {
            get
            {
                return "Форма 1 Контроль";
            }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var inspectionsIds = Container.Resolve<IDomainService<InspectionGjiRealityObject>>().GetAll()
               .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
               .Select(x => x.Inspection.Id)
               .Distinct()
               .ToList();

            BaseReportSection section1 = new Form1ControlSection1(inspectionsIds, dateStart, dateEnd, Container);
            BaseReportSection section2 = new Form1ControlSection2(inspectionsIds, dateStart, dateEnd, Container);
            BaseReportSection section3 = new Form1ControlSection3(inspectionsIds, dateStart, dateEnd, Container, municipalityIds);
            foreach (var row in rowsSection1)
            {
                reportParams.SimpleReportParams[string.Format("cell1_{0}_5", row)] = section1.GetCell(row, 5);
            }

            foreach (var row in rowsSection2)
            {
                reportParams.SimpleReportParams[string.Format("cell2_{0}_6", row)] = section2.GetCell(row, 6);
                reportParams.SimpleReportParams[string.Format("cell2_{0}_7", row)] = section2.GetCell(row, 7);
            }
            reportParams.SimpleReportParams["cell2_17_5"] = section2.GetCell(17, 5);

            foreach (var row in rowsSection3)
            {
                reportParams.SimpleReportParams[string.Format("cell3_{0}_5", row)] = section3.GetCell(row, 5);
            }

        }

        public override string Desciption
        {
            get
            {
                return "Форма 1 Контроль";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Отчеты ГЖИ";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.Form1Controll";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.Form1Control";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            dateStart = baseParams.Params["dateStart"].ToDateTime();
            dateEnd = baseParams.Params["dateEnd"].ToDateTime();            
            this.ParseIds(municipalityIds, baseParams.Params["municipalityIds"].ToString());
        }

        public override string ReportGenerator { get; set; }

        private void ParseIds(List<long> list, string Ids)
        {
            list.Clear();

            if (!string.IsNullOrEmpty(Ids))
            {
                var ids = Ids.Split(',');
                foreach (var id in ids)
                {
                    long Id = 0;
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
    }
}