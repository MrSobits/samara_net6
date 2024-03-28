namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Export
{
    using System.Collections;
    using System.Linq;

    using B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.Entities;

    public class ConfirmContributionExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            return Container.Resolve<IDomainService<ConfirmContribution>>().GetAll()
                .Select(x => new
                {
                    x.Id,
                    MunicipalityName = x.ManagingOrganization.Contragent.Municipality.Name,
                    ManagingOrganizationName = x.ManagingOrganization.Contragent.Name
                })
                .Filter(loadParams, Container)
                .Order(loadParams)
                .ToList();
        }
    }
}