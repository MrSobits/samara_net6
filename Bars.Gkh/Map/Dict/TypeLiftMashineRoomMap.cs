namespace Bars.Gkh.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Маппинг для сущности "Расположение машинного помещения"
    /// </summary>
    public class TypeLiftMashineRoomMap : BaseImportableEntityMap<TypeLiftMashineRoom>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public TypeLiftMashineRoomMap()
            : base("Расположение машинного помещения", "GKH_DICT_MASH_ROOM")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(300);
        }
    }
}