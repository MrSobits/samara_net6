namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities;
    using Gkh.Map;
    /// <summary>
    /// Маппинг <see cref="DistributionOperation"/>
    /// </summary>
    public class DistributionOperationMap : BaseImportableEntityMap<DistributionOperation>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public DistributionOperationMap()
            : base("Сущность связи Кода банковского распределения и распределения", "REGOP_BANK_STMNT_OP")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.Code, "Код распределения").Column("CODE").NotNull();

            this.Reference(x => x.BankAccountStatement, "Банковская операция").Column("BANK_STMNT_ID").NotNull();
            this.Reference(x => x.Operation, "Операция распределения").Column("OP_ID").NotNull().Fetch();
        }
    }
}