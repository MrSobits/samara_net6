namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities.Owner;

    /// <summary>Маппинг для "Абонент"</summary>
    public class LawsuitLegalOwnerInfoMap : JoinedSubClassMap<LawsuitLegalOwnerInfo>
    {
        public LawsuitLegalOwnerInfoMap() :  base("Собственник юр. лицо в исковом заявлении", "REGOP_LAWSUIT_LEGAL_OWNER_INFO") {}

        protected override void Map()
        {
            this.Property(x => x.ContragentName, "Наименование контрагента").Column("CONTRAGENT_NAME").NotNull();
            this.Property(x => x.Inn, "ИНН").Column("INN").NotNull();
            this.Property(x => x.Kpp, "КПП").Column("KPP").NotNull();
        }
    }
}
