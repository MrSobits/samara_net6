namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    public class TatarstanZonalInspectionViewModel : ZonalInspectionViewModel<TatarstanZonalInspection>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<TatarstanZonalInspection> domain, BaseParams baseParams)
        {
            var controlOrgId = baseParams.Params.GetAs<long>("controlOrgId");
            var isKnoWindow = baseParams.Params.GetAs<bool>("isKnoWindow");

            //если вызов не из окна редактирования КНО, то base.List()
            return !isKnoWindow
                ? base.List(domain, baseParams)
                : controlOrgId == default(long)
                    ? domain.GetAll()
                        .Where(x => x.ControlOrganization == null)
                        .ToListDataResult(this.GetLoadParam(baseParams), this.Container)
                    : domain.GetAll()
                        .Where(x => x.ControlOrganization.Id == controlOrgId)
                        .ToListDataResult(this.GetLoadParam(baseParams), this.Container);
        }
    }
}
