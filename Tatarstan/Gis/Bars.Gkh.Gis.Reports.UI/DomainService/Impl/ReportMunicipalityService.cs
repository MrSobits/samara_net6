namespace Bars.Gkh.Gis.Reports.UI.DomainService.Impl
{
    using System.Linq;

    using B4;
    using B4.Utils;

    using Bars.B4.Modules.Dapper.Repository;

    using Castle.Windsor;
    using Entities;

    public class ReportMunicipalityService : IReportMunicipalityService
    {
        public IWindsorContainer Container { get; set; }

        public IDapperRepository<ReportMunicipality> DomainService { get; set; }

        public IDataResult ListWithoutPaging(BaseParams baseParams)
        {
            var AreaId = baseParams.Params["areaId"].ToInt();
            var loadParams = baseParams.GetLoadParam();
            var data = DomainService.GetAll().Where(x => x.Area.Id == AreaId).AsQueryable().Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).ToList(), data.Count());
        }
    }
}
