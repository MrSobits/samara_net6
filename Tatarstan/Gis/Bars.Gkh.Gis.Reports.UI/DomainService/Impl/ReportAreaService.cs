namespace Bars.Gkh.Gis.Reports.UI.DomainService.Impl
{
    using System.Linq;

    using B4;
    using Castle.Windsor;
    using Entities;
    using Bars.B4.Modules.Dapper.Repository;

    public class ReportAreaService : IReportAreaService
    {
        public IWindsorContainer Container { get; set; }

        public IDapperRepository<ReportArea> DomainService { get; set; }

        public IDataResult ListWithoutPaging(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var data = DomainService.GetAll().AsQueryable().Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).ToList(), data.Count());
        }
    }
}
