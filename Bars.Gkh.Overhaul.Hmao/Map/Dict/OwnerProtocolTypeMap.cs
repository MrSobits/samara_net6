namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;


    /// <summary>
    /// Маппинг для "Контрагент"
    /// </summary>
    public class OwnerProtocolTypeMap : BaseEntityMap<OwnerProtocolType>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public OwnerProtocolTypeMap() :
                base("Виды протоколов решений собственников", "OVRHL_DICT_OWNER_PROTOCOL_TYPE")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Name, "Вид протокола").Column("NAME").Length(500);
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
            this.Property(x => x.Code, "Код").Column("CODE").Length(10);
        }
    }
}
