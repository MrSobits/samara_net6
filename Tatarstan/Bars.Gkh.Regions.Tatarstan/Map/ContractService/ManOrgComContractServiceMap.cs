namespace Bars.Gkh.Regions.Tatarstan.Map.ContractService
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities.ContractService;

    public class ManOrgComContractServiceMap : JoinedSubClassMap<ManOrgComContractService>
    {
        public ManOrgComContractServiceMap()
            : base("Работы и услуги управляющей организации", "GKH_MAN_ORG_COM_SERVICE")
        {
        }

        protected override void Map()
        {
        }
    }
}
