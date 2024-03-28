namespace Bars.GkhGji.Report.Form1Control_v2
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using System.Reflection;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Properties;

    using Castle.Windsor;

    public class Form1Control_v2Report : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        protected IForm1ContolServiceData _serviceData { get; set; }

        private DateTime dateStart = DateTime.MinValue;

        private DateTime dateEnd = DateTime.MaxValue;

        private List<long> municipalities = new List<long>();

        public Form1Control_v2Report(IForm1ContolServiceData ServiceData)
            : base(new ReportTemplateBinary(ServiceData.GetTemplate()))
        {
            _serviceData = ServiceData;
        }

        public override string Name
        {
            get
            {
                return "Форма № 1-контроль";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Форма № 1-контроль";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Инспекторская деятельность";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.Form1Control_v2";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.Form1Control_v2";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MaxValue);
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);

            municipalities = !string.IsNullOrEmpty(municipalityIdsList) ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToList() : new List<long>();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            /* Было так 
            reportParams.SimpleReportParams["cell1"] = data.cell1;
            reportParams.SimpleReportParams["cell3"] = data.cell3;
            reportParams.SimpleReportParams["cell4"] = data.cell4;
            reportParams.SimpleReportParams["cell5"] = data.cell5;
            reportParams.SimpleReportParams["cell7"] = data.cell7;
            reportParams.SimpleReportParams["cell9"] = data.cell9;
            reportParams.SimpleReportParams["cell10"] = data.cell10;
            reportParams.SimpleReportParams["cell11"] = data.cell11;
            reportParams.SimpleReportParams["cell12"] = data.cell12;
            reportParams.SimpleReportParams["cell13"] = data.cell13;
            reportParams.SimpleReportParams["cell14"] = data.cell14;
            reportParams.SimpleReportParams["cell15"] = data.cell15;

            reportParams.SimpleReportParams["cell16_1"] = data.cell16_1;
            reportParams.SimpleReportParams["cell16_2"] = data.cell16_2;
            reportParams.SimpleReportParams["cell19_1"] = data.cell19_1;
            reportParams.SimpleReportParams["cell19_2"] = data.cell19_2;
            reportParams.SimpleReportParams["cell21_1"] = data.cell21_1;
            reportParams.SimpleReportParams["cell21_2"] = data.cell21_2;
            reportParams.SimpleReportParams["cell23_1"] = data.cell23_1;
            reportParams.SimpleReportParams["cell23_2"] = data.cell23_2;
            reportParams.SimpleReportParams["cell24_1"] = data.cell24_1;
            reportParams.SimpleReportParams["cell24_2"] = data.cell24_2;
            reportParams.SimpleReportParams["cell25_1"] = data.cell25_1;
            reportParams.SimpleReportParams["cell25_2"] = data.cell25_2;
            reportParams.SimpleReportParams["cell29_1"] = data.cell29_1;
            reportParams.SimpleReportParams["cell29_2"] = data.cell29_2;
            reportParams.SimpleReportParams["cell33_1"] = data.cell33_1;
            reportParams.SimpleReportParams["cell33_2"] = data.cell33_2;
            reportParams.SimpleReportParams["cell35_1"] = data.cell35_1;
            reportParams.SimpleReportParams["cell35_2"] = data.cell35_2;
            reportParams.SimpleReportParams["cell37_1"] = data.cell37_1;
            reportParams.SimpleReportParams["cell37_2"] = data.cell37_2;
            reportParams.SimpleReportParams["cell39_1"] = data.cell39_1;
            reportParams.SimpleReportParams["cell39_2"] = data.cell39_2;
            reportParams.SimpleReportParams["cell41_1"] = data.cell41_1;
            reportParams.SimpleReportParams["cell41_2"] = data.cell41_2;
            reportParams.SimpleReportParams["cell42_1"] = data.cell42_1;
            reportParams.SimpleReportParams["cell42_2"] = data.cell42_2;
            reportParams.SimpleReportParams["cell46_1"] = data.cell46_1;
            reportParams.SimpleReportParams["cell46_2"] = data.cell46_2;
            reportParams.SimpleReportParams["cell47_1"] = data.cell47_1;
            reportParams.SimpleReportParams["cell47_2"] = data.cell47_2;
            reportParams.SimpleReportParams["cell48_1"] = data.cell48_1;
            reportParams.SimpleReportParams["cell48_2"] = data.cell48_2;

            reportParams.SimpleReportParams["cell50"] = data.cell50;
            reportParams.SimpleReportParams["cell51"] = data.cell51;
            reportParams.SimpleReportParams["cell52"] = data.cell52;
            reportParams.SimpleReportParams["cell53"] = data.cell53;
            reportParams.SimpleReportParams["cell54"] = data.cell54;
            reportParams.SimpleReportParams["cell55"] = data.cell55;
            reportParams.SimpleReportParams["cell56"] = data.cell56;
            */

            // стало так
            var data = _serviceData.GetData(dateStart, dateEnd, municipalities);
            var propInfo = data.GetType().GetProperties();

            foreach (var info in propInfo)
            {
                reportParams.SimpleReportParams[info.Name] = info.GetValue(data, new object[0]);
            }
        }
    }
}