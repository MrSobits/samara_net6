namespace Bars.GkhGji.Regions.Tatarstan.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    public class ControlOrganizationControlTypeRelationViewModel : BaseViewModel<ControlOrganizationControlTypeRelation>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<ControlOrganizationControlTypeRelation> domainService, BaseParams baseParams)
        {
            var controlOrgId = baseParams.Params.GetAs<long>("controlOrgId");
            return domainService.GetAll()
                .Where(x => x.ControlOrganization.Id == controlOrgId)
                .ToListDataResult(this.GetLoadParam(baseParams), this.Container);
        }
    }
}
