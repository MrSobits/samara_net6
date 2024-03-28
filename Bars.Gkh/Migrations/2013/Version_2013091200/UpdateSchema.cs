namespace Bars.Gkh.Migrations.Version_2013091200
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013091200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013091102.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                 "GKH_OBJ_RESORG",
                 new RefColumn("RESORG_ID", ColumnProperty.NotNull, "GKH_RO_RES_ORG", "GKH_SUPPLY_RESORG", "ID"),
                 new RefColumn("REALITY_OBJECT_ID", ColumnProperty.NotNull, "GKH_RO_SUP_ORG_RO", "GKH_REALITY_OBJECT", "ID"),
                 new Column("DATE_START", DbType.DateTime),
                 new Column("DATE_END", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_RO_SUP_RES_ORG");
        }
    }
}