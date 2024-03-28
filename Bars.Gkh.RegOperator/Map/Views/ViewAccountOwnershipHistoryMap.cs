namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.Views;

    public class ViewAccountOwnershipHistoryMap : PersistentObjectMap<ViewAccountOwnershipHistory>
    {

        public ViewAccountOwnershipHistoryMap() :
                base("Представление владельца ЛС на определенный период", "VIEW_REGOP_PERS_ACC_OWNER")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.PersonalAccount, "Лицевой счет").Column("ACCOUNT_ID").NotNull();
            this.Reference(x => x.AccountOwner, "Владелец ЛС").Column("OWNER_ID").NotNull();
            this.Reference(x => x.Period, "Период действия").Column("PERIOD_ID").NotNull();
        }
    }
}
