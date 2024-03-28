namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;

    /// <summary>
    /// Маппинг для <see cref="AccountOwnershipHistory"/>
    /// </summary>
    public class AccountOwnershipHistoryMap : BaseImportableEntityMap<AccountOwnershipHistory>
    {

        public AccountOwnershipHistoryMap() :
                base("История принадлежности лс абоненту", "REGOP_PERS_ACC_OWNERSHIP_HISTORY")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.PersonalAccount, "Лс").Column("ACCOUNT_ID").NotNull().Fetch();
            this.Reference(x => x.AccountOwner, "Абонент").Column("OWNER_ID").NotNull().Fetch();
            this.Property(x => x.ActualFrom, "Дата, с которой лс принадлежал абоненту").Column("ACTUAL_FROM");
            this.Property(x => x.Date, "Дата установки").Column("DATE").NotNull();
        }
    }
}