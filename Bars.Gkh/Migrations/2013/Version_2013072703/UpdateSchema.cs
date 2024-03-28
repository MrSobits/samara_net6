namespace Bars.Gkh.Migrations.Version_2013072703
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013072703")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013072702.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_DICT_TYPE_LIFT_CABIN",
               new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
               new Column("CODE", DbType.Int32),
               new Column("NORMATIVE_LIFETIME", DbType.Decimal));

            Database.AddEntityTable("GKH_OBJ_LIFT",
               new Column("DATE_COMMISSIONING", DbType.DateTime),
               new RefColumn("FILE_ID", "GKH_OBJ_LIFT_FILE", "B4_FILE_INFO", "ID"),
               new Column("SERIAL_NUMBER", DbType.String, 300),
               new Column("STOP_COUNT", DbType.Int32),
               new Column("ROOM_ENTRANCE", DbType.Int32),
               new Column("DOCUMENT_DATE", DbType.DateTime),
               new Column("DOCUMENT_NUM", DbType.String, 300), 
               new Column("WEAR", DbType.Decimal),
               new Column("IS_REPAIRED", DbType.Int32, 4, ColumnProperty.NotNull, 30), 
               new RefColumn("TYPE_LIFT_CABIN_ID", "GKH_OBJ_LIFT_CABIN", "GKH_DICT_TYPE_LIFT_CABIN", "ID"),
               new RefColumn("REALITY_OBJECT_ID", "GKH_LIFT_OBJ_RO", "GKH_REALITY_OBJECT", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_OBJ_LIFT");
            Database.RemoveTable("GKH_DICT_TYPE_LIFT_CABIN");
        }
    }
}