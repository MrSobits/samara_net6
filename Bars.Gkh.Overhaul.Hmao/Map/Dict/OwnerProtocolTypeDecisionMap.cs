namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Hmao.Entities;


    /// <summary>
    /// Маппинг для "Контрагент"
    /// </summary>
    public class OwnerProtocolTypeDecisionMap : BaseEntityMap<OwnerProtocolTypeDecision>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public OwnerProtocolTypeDecisionMap() :
                base("Виды решений собственников", "OVRHL_DICT_PROTTYPE_DECISION")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование решения").Column("NAME").Length(300).NotNull();
            this.Reference(x => x.OwnerProtocolType, "Вид протокола решений собственников").Column("PROTOCOL_TYPE_ID").Fetch();

        }
    }
}
