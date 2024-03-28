namespace Bars.Gkh.Map.Dict.ContractService
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts.ContractService;

    /// <summary>
    /// Маппинг для "Коммунальная услуга"
    /// </summary>
    public class CommunalContractServiceMap : JoinedSubClassMap<CommunalContractService>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public CommunalContractServiceMap()
            : base("Bars.Gkh.Entities.Dicts.CommunalContractService", "GKH_DICT_COM_CONTRACT_SERVICE")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.CommunalResource, "Коммунальный ресурс").Column("COM_RESOURCE").NotNull();
            this.Property(x => x.SortOrder, "Порядок сортировки ").Column("SORT_ORDER");
            this.Property(x => x.IsHouseNeeds, "Услуги предоставляется на ОДН").Column("IS_HOUSE_NEEDS").NotNull().DefaultValue(false);
        }
    }
}