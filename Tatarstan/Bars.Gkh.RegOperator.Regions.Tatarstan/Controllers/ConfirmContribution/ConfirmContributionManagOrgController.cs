namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.DomainService;

    public class ConfirmContributionManagOrgController : B4.Alt.DataController<ManagingOrganization>
    {
        public ActionResult ListManagOrg(BaseParams baseParams)
        {
            var service = Container.Resolve<IConfirmContributionService>();
            try
            {
                var result = (ListDataResult)service.ManagOrgsList(baseParams);
                return new JsonListResult((IList)result.Data, result.TotalCount);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}