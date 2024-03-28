namespace Sobits.GisGkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using NHibernate.Mapping.ByCode.Conformist;

    using Sobits.GisGkh.Entities;

    /// <summary>Маппинг для "Sobits.GisGkh.GisGkhRequests"</summary>
    public class NsiItemMap : BaseEntityMap<NsiItem>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public NsiItemMap()
            : base("Sobits.GisGkh.Entities", NsiItemMap.TableName)
        {
        }

        public static string TableName => "GIS_GKH_NSI_ITEM";

        public static string SchemaName => "public";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.NsiList, "Справочник").Column("nsilist").NotNull().Fetch();
            this.Property(x => x.GisGkhItemCode, "Код пункта справочника в ГИС ЖКХ").Column("gisgkhitemcode").NotNull();
            this.Property(x => x.GisGkhGUID, "GUID пункта справочника в ГИС ЖКХ").Column("gisgkhguid").NotNull();
            this.Property(x => x.EntityItemId, "ИД записи в системном справочнике").Column("entityitemid");
            this.Property(x => x.IsActual, "Акстуальность").Column("isactual");
            this.Reference(x => x.ParentItem, "Родительский элемент").Column("parentitem").Fetch();
        }
    }

    // ReSharper disable once UnusedMember.Global
    public class NsiItemNhMapping : ClassMapping<NsiItem>
    {
        public NsiItemNhMapping()
        {
            this.Schema(NsiItemMap.SchemaName);
        }
    }
}