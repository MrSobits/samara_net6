namespace Bars.Gkh.Regions.Tatarstan.Map.ContractService
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities.ContractService;

    public class ManOrgAgrContractServiceMap : JoinedSubClassMap<ManOrgAgrContractService>
    {
        public ManOrgAgrContractServiceMap()
            : base("Работы и услуги управляющей организации", "GKH_MAN_ORG_AGR_SERVICE")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.PaymentAmount, "PaymentSum").Column("PAYMENT_SUM");
        }
    }
}
