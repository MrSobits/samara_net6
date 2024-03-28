namespace Bars.GkhGji.Export
{
    using System.Collections;
    using System.Linq;

    using B4;
    using B4.Modules.DataExport.Domain;
    using Entities;
    using DomainService;

    public class ActivityTsjDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            var service = Container.Resolve<IDomainService<ActivityTsj>>();
            var serviceStatute = Container.Resolve<IDomainService<ActivityTsjStatute>>();

            return Container.Resolve<IActivityTsjService>().GetFilteredByOperator(service)
                .Select(x => new
                    {
                        x.Id,
                        ManOrgName = x.ManagingOrganization.Contragent.Name,
                        MunicipalityName = x.ManagingOrganization.Contragent.Municipality.Name,
                        x.ManagingOrganization.Contragent.Inn,
                        HasStatute = serviceStatute.GetAll().Any(y => x.Id == y.ActivityTsj.Id)
                    })
                .Filter(loadParam, Container)
                .Order(loadParam)
                .ToList();
        }
    }
}