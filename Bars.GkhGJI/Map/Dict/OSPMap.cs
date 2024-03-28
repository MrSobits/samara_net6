namespace Bars.GkhGji.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;


    /// <summary>
    /// Маппинг для "Контрагент"
    /// </summary>
    public class OSPMap : BaseEntityMap<OSP>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public OSPMap() :
                base("Отдел судебных приставов", "GJI_DICT_OSP")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.BankAccount, "Рассчетный счет").Column("BANK_ACCOUNT").Length(30);
            this.Property(x => x.KBK, "Код бюджетной классификации").Column("KBK").Length(30);
            this.Property(x => x.Name, "Полное наименование").Column("NAME").Length(500);
            this.Property(x => x.ShortName, "Краткое наименование").Column("SHORT_NAME").Length(300);
            this.Property(x => x.Street, "Улица").Column("STREET").Length(100);
            this.Property(x => x.Town, "Город, село").Column("TOWN").Length(100);
            this.Reference(x => x.Municipality, "Муниципальный район").Column("MUNICIPALITY_ID").Fetch();
            this.Reference(x => x.CreditOrg, "Банк").Column("CREDITORG_ID").Fetch();

        }
    }
}