namespace Bars.Gkh.Regions.Tatarstan.Map.ContractService
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities.ContractService;

    public class ManOrgAddContractServiceMap : JoinedSubClassMap<ManOrgAddContractService>
    {
        public ManOrgAddContractServiceMap() :
            base("Работы и услуги управляющей организации", "GKH_MAN_ORG_ADD_SERVICE")
        {
        }

        protected override void Map()
        {
        }
    }
}
