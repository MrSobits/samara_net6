namespace Bars.GkhGji.Regions.Sahalin.Report.Form1Control
{
    using System;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    
    using B4.Modules.Reports;
    using B4.Utils;

    using Bars.Gkh.Entities;

    using Castle.Windsor;
    using Entities;
    using Gkh.Domain;

    public class SahalinForm1ControlReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private long[] _municipalityIds = new long[0];

        private DateTime _dateStart = DateTime.MinValue;

        private DateTime _dateEnd = DateTime.MaxValue;

        private long _headId = 0;
        private long _personId = 0;

        private readonly int[] _rowsSection1 = {1, 2, 3, 5, 7, 8, 9, 10, 11, 12, 13, 14};
        
        private readonly int[] _rowsSection2 = {18, 19, 20, 22, 23, 24, 28, 32, 33, 34, 35, 37, 38, 39, 40, 42, 46, 47, 48, 49, 50, 51};

        private readonly int[] _rowsSection3 = {54, 55, 56, 57, 58, 60, 61, 62, 63, 64};

        public SahalinForm1ControlReport() : base(new ReportTemplateBinary(Properties.Resources.SahalinForm1ControlReport))
        {
        }

        public override string Name { get { return "Форма 1 Контроль"; } }

        public override void PrepareReport(ReportParams reportParams)
        {
            reportParams.SimpleReportParams["monthStart"] = _dateStart.ToString("MMMM");
            reportParams.SimpleReportParams["monthEnd"] = _dateEnd.ToString("MMMM");
            reportParams.SimpleReportParams["year"] = _dateStart.Year == _dateEnd.Year
                ? _dateEnd.Year.ToString()
                : _dateStart.Year + " - " + _dateEnd.Year;
            reportParams.SimpleReportParams["dateNow"] = DateTime.Now.ToString("dd MMMM yyyy");

            var inspectionsIds = Container.ResolveDomain<InspectionGjiRealityObject>().GetAll()
               .WhereIf(_municipalityIds.IsNotEmpty(), x => _municipalityIds.Contains(x.RealityObject.Municipality.Id))
               .Select(x => x.Inspection.Id)
               .Distinct()
               .ToArray();

            BaseReportSection section1 = new Form1ControlSection1(inspectionsIds, _dateStart, _dateEnd, Container);
            BaseReportSection section2 = new Form1ControlSection2(inspectionsIds, _dateStart, _dateEnd, Container);
            BaseReportSection section3 = new Form1ControlSection3(inspectionsIds, _dateStart, _dateEnd, Container, _municipalityIds);

            foreach (var row in _rowsSection1)
            {
                reportParams.SimpleReportParams[string.Format("cell1_{0}_5", row)] = section1.GetCell(row, 5);
            }

            foreach (var row in _rowsSection2)
            {
                reportParams.SimpleReportParams[string.Format("cell2_{0}_6", row)] = section2.GetCell(row, 6);
                reportParams.SimpleReportParams[string.Format("cell2_{0}_7", row)] = section2.GetCell(row, 7);
            }

            reportParams.SimpleReportParams["cell2_17_5"] = section2.GetCell(17, 5);

            foreach (var row in _rowsSection3)
            {
                reportParams.SimpleReportParams[string.Format("cell3_{0}_5", row)] = section3.GetCell(row, 5);
            }

            var inspectorRepo = Container.ResolveDomain<Inspector>();

            var person = inspectorRepo.GetAll().Select(x => new
            {
                x.Id,
                x.Position,
                x.ShortFio,
                x.Phone
            }).First(x => x.Id == _personId);
            reportParams.SimpleReportParams["dl_position"] = person.Position;
            reportParams.SimpleReportParams["dl_name"] = person.ShortFio;
            reportParams.SimpleReportParams["dl_phone"] = person.Phone;

            var head = inspectorRepo.GetAll().Select(x => new
            {
                x.Id,
                x.ShortFio
            }).First(x => x.Id == _headId);
            reportParams.SimpleReportParams["chief_name"] = head.ShortFio;
        }

        public override string Desciption
        {
            get { return "Форма 1 Контроль (Сахалин)"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.Form1Controll"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GJI.Form1Control"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            _dateStart = baseParams.Params["dateStart"].ToDateTime();
            _dateEnd = baseParams.Params["dateEnd"].ToDateTime();
            _municipalityIds = baseParams.Params.GetAs<string>("municipalityIds").ToLongArray();

            _headId = baseParams.Params["headId"].ToLong();
            _personId = baseParams.Params["personId"].ToLong();
        }

        public override string ReportGenerator { get; set; }
    }
}