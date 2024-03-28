namespace Bars.Gkh.Modules.Gkh1468.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Modules.Gkh1468.Entities.ContractPart;

    /// <summary>Маппинг для Сторона договора "Юридическое лицо"</summary>
    public class JurPersonOwnerContractMap : JoinedSubClassMap<JurPersonOwnerContract>
    {
        
        public JurPersonOwnerContractMap() : 
                base("Сторона договора \"Юридическое лицо\"", "GKH_RSOCONTRACT_JUR_PERSON")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.TypeContactPerson, "Лицо, являющееся стороной договора").Column("TYPE_PERSON").NotNull();
            this.Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull().Fetch();
        }
    }
}
