namespace Bars.Gkh.Modules.Gkh1468.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Modules.Gkh1468.Entities.ContractPart;

    /// <summary>Маппинг для Сторона договора "Бюджетная организация"</summary>
    public class BudgetOrgContractMap : JoinedSubClassMap<BudgetOrgContract>
    {       
        /// <inheritdoc />
        public BudgetOrgContractMap() :
                base("Сторона договора \"Бюджетная организация\"", "GKH_RSOCONTRACT_BUDGET_ORG")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.Organization, "Организация").Column("CONTRAGENT_ID");
            this.Reference(x => x.TypeCustomer, "Вид потребителя").Column("TYPE_CUSTOMER_ID").NotNull();
        }
    }
}
