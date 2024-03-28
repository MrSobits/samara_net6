namespace Bars.Gkh.Regions.Tatarstan.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Regions.Tatarstan.Entities.GasEquipmentOrg;
    using Bars.Gkh.Utils;

    public class GasEquipmentOrgRealityObjViewModel : BaseViewModel<GasEquipmentOrgRealityObj>
    {
        public override IDataResult List(IDomainService<GasEquipmentOrgRealityObj> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var gasEquipmentOrgId = baseParams.Params.GetAs<long>("GasEquipmentOrg");

            return domainService.GetAll()
                .WhereIf(gasEquipmentOrgId > 0, x => x.GasEquipmentOrg.Id == gasEquipmentOrgId)
                .Select(
                    x => new
                    {
                        x.Id,
                        GasEquipmentOrg = x.GasEquipmentOrg.Contragent.Name,
                        GasEquipmentOrgId = x.GasEquipmentOrg.Id,
                        x.RealityObject.Address,
                        Municipality = x.RealityObject.Municipality.Name,
                        x.StartDate,
                        x.EndDate,
                        x.DocumentNum,
                        x.File
                    })
                .ToListDataResult(loadParams, this.Container);
        }
        
        public override IDataResult Get(IDomainService<GasEquipmentOrgRealityObj> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            return new BaseDataResult(
                domainService.GetAll().Where(x => x.Id == id)
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.RealityObject,
                            x.GasEquipmentOrg,
                            Municipality = x.RealityObject.Municipality.Name,
                            x.StartDate,
                            x.EndDate,
                            x.DocumentNum,
                            x.File
                        })
                    .First());
        }
    }
}