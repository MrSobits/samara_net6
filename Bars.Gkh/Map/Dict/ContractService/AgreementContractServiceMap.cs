namespace Bars.Gkh.Map.Dict.ContractService
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts.ContractService;

    /// <summary>
    /// Маппинг для "Услуги/Работы по ДУ"
    /// </summary>
    public class AgreementContractServiceMap : JoinedSubClassMap<AgreementContractService>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public AgreementContractServiceMap()
            : base("Bars.Gkh.Entities.Dicts.AgreementContractService", "GKH_DICT_AGR_CONTRACT_SERVICE")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.WorkAssignment, "Назначение работ").Column("WORK_ASSIGN").NotNull();
            this.Property(x => x.TypeWork, "Тип работ").Column("TYPE_WORK").NotNull();
        }
    }
}