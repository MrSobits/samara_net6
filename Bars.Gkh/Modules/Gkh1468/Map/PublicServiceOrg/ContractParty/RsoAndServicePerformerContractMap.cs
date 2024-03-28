namespace Bars.Gkh.Modules.Gkh1468.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Modules.Gkh1468.Entities.ContractPart;

    /// <summary>Маппинг для Стороны договора: РСО и исполнитель коммунальных услуг</summary>
    public class RsoAndServicePerformerContractMap : JoinedSubClassMap<RsoAndServicePerformerContract>
    {
        
        public RsoAndServicePerformerContractMap() : 
                base("Стороны договора: РСО и исполнитель коммунальных услуг", "GKH_RSOCONTRACT_SERVICE_PERFORMER")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.CommercialMeteringResourceType, "Коммерческий учет ресурса осуществляет").Column("RESOURCE_TYPE").NotNull();
            this.Reference(x => x.ManagingOrganization, "Управляющая организация").Column("MANORG_ID").NotNull().Fetch();
        }
    }
}
