namespace Bars.Gkh.Regions.Tatarstan.Map.ContractService
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities.ContractService;

    public class ManOrgContractServiceMap : BaseEntityMap<ManOrgContractService>
    {
        public ManOrgContractServiceMap() :
            base("Работа / услуга управляющей организации", "GKH_MAN_ORG_CONTRACT_SERVICE")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.ServiceType, "Тип работы / услуги").Column("SERVICE_TYPE");
            this.Property(x => x.StartDate, "Дата начала предоставления услуги").Column("START_DATE");
            this.Property(x => x.EndDate, "Дата окончания предоставления услуги").Column("END_DATE");
            this.Reference(x => x.Service, "Услуга по договору управления").Column("SERVICE_ID").NotNull().Fetch();
            this.Reference(x => x.Contract, "Договор управления").Column("CONTRACT_ID").NotNull().Fetch();
        }
    }
}
