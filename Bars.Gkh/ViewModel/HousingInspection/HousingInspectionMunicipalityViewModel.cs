namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using B4;
    using B4.Utils;

    using Bars.Gkh.Entities.HousingInspection;
    using Bars.Gkh.Utils;
    
    /// <summary>
    /// Вьюха для <see cref="HousingInspectionMunicipality"/>
    /// </summary>
    public class HousingInspectionMunicipalityViewModel : BaseViewModel<HousingInspectionMunicipality>
    {
        public override IDataResult List(IDomainService<HousingInspectionMunicipality> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var housingInspectionId = baseParams.Params.GetAs<long>("housingInspectionId");

            return domain.GetAll()
                .Where(x => x.HousingInspection.Id == housingInspectionId)
                .Select(
                    x => new
                    {
                        x.Id,
                        Municipality = x.Municipality.Name,
                        ParentMo = x.Municipality.ParentMo.Name
                    })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .ToListDataResult(loadParams, this.Container);
        }
    }
}