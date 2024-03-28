namespace Bars.Gkh.RegOperator.Map.PersonalAccount
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;

    public class PersonalAccountBanRecalcMap : BaseImportableEntityMap<PersonalAccountBanRecalc>
    {
        public PersonalAccountBanRecalcMap() :
                base("Запрет перерасчета", "REGOP_PERS_ACC_BAN_RECALC")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.PersonalAccount, "Лицевой счет").Column("ACCOUNT_ID").NotNull().Fetch();
            this.Reference(x => x.File, "Документ-основание").Column("FILE_ID");
            this.Property(x => x.DateStart, "Период начала действия").Column("DATE_START");
            this.Property(x => x.DateEnd, "Период окончания действия").Column("DATE_END");
            this.Property(x => x.Reason, "Причина").Column("REASON");
            this.Property(x => x.Type, "Тип запрета переасчета").Column("TYPE").NotNull();
        }
    }
}