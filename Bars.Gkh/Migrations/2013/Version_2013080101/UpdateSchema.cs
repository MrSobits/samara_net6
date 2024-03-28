namespace Bars.Gkh.Migrations.Version_2013080101
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013080101")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013080100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_OBJ_COMMET_DEV",
               new RefColumn("MET_DEVICE_ID", ColumnProperty.NotNull, "GKH_OBJ_COMMETDEV_MD", "GKH_DICT_METERING_DEVICE", "ID"),
               new RefColumn("REALITY_OBJECT_ID", ColumnProperty.NotNull, "GKH_OBJ_COMMETDEV_RO", "GKH_REALITY_OBJECT", "ID"),
               new RefColumn("ELEMENT_OBJECT_ID", ColumnProperty.NotNull, "GKH_OBJ_COMMETDEV_EO", "GKH_DICT_GROUP_ELEM_OBJ", "ID"),
               new RefColumn("FILE_INFO_ID", "GKH_OBJ_COMMETDEV_FILE", "B4_FILE_INFO", "ID"),
               new Column("DATE_INSTALLED", DbType.Date, ColumnProperty.NotNull),
               new Column("INVENTORY_NUMBER", DbType.Int32),
               new Column("ESTIMATE_WEAR", DbType.Decimal),
                new Column("DOCUMENT_DATE", DbType.DateTime),
               new Column("DOCUMENT_NUM", DbType.String, 300), 
               new Column("IS_REPAIRED", DbType.Int32, 4, ColumnProperty.NotNull, 30));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_OBJ_COMMET_DEV");
        }
    }
}
