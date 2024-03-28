namespace Bars.Gkh.Map.Dict.ContractService
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts.ContractService;

    /// <summary>
    /// Маппинг для "Дополнительная услуга"
    /// </summary>
    public class AdditionalContractServiceMap : JoinedSubClassMap<AdditionalContractService>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public AdditionalContractServiceMap()
            : base("Bars.Gkh.Entities.Dicts.AdditionalContractService", "GKH_DICT_ADD_CONTRACT_SERVICE")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
        }
    }
}
