namespace Bars.Gkh.Regions.Voronezh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Regions.Voronezh.Entities.Dicts;

    public class ZonalInspectionPrefixViewModel : BaseViewModel<ZonalInspectionPrefix>
    {
        public override IDataResult Get(IDomainService<ZonalInspectionPrefix> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var zonalInspectionId = baseParams.Params.GetAsId("zonalInspectionId");

            if (id != 0)
            {
                return new BaseDataResult(domainService.GetAll().FirstOrDefault(x => x.Id == id));
            }

            return new BaseDataResult(domainService.GetAll().FirstOrDefault(x => x.ZonalInspection.Id == zonalInspectionId));
        }
    }
}