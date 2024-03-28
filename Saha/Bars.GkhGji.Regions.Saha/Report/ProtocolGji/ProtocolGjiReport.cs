namespace Bars.GkhGji.Regions.Saha.Report.ProtocolGji
{
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Saha.Entities;

    public class ProtocolGjiReport : GkhGji.Report.ProtocolGjiReport
    {
        protected override void FillRegionParams(ReportParams reportParams, DocumentGji doc)
        {
            var protocol = (Protocol)doc;

            var firstPhysPersonInfo = Container.Resolve<IDomainService<DocumentGJIPhysPersonInfo>>().GetAll()
                            .Where(x => x.Document.Id == protocol.Id)
                            .Select(x => new
                                            {
                                                x.PhysPersonAddress,
                                                x.PhysPersonJob,
                                                x.PhysPersonPosition,
                                                x.PhysPersonBirthdayAndPlace,
                                                x.PhysPersonDocument,
                                                x.PhysPersonSalary,
                                                x.PhysPersonMaritalStatus
                                            })
                            .FirstOrDefault();

            if (firstPhysPersonInfo != null)
            {
                reportParams.SimpleReportParams["АдресТелефон"] = firstPhysPersonInfo.PhysPersonAddress;
                reportParams.SimpleReportParams["МестоРаботы"] = firstPhysPersonInfo.PhysPersonJob;
                reportParams.SimpleReportParams["Должность"] = firstPhysPersonInfo.PhysPersonPosition;
                reportParams.SimpleReportParams["ДатаМестоРождения"] = firstPhysPersonInfo.PhysPersonBirthdayAndPlace;
                reportParams.SimpleReportParams["ДокументУдостовЛичность"] = firstPhysPersonInfo.PhysPersonDocument;
                reportParams.SimpleReportParams["Зарплата"] = firstPhysPersonInfo.PhysPersonSalary;
                reportParams.SimpleReportParams["СемейноеПоложение"] = firstPhysPersonInfo.PhysPersonMaritalStatus;
            }
        }
    }
}