namespace Bars.GkhGji.Migrations.Version_2015041000
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Gkh;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015041000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations._2014.Version_2014122400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddTable(
                 "GJI_INSPECTION_LIC_APP",
                 new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                 new Column("FORM_CHECK", DbType.Int32, 4, ColumnProperty.NotNull, 10));

            Database.AddRefColumn("GJI_INSPECTION_LIC_APP", new RefColumn("MAN_ORG_LIC_ID", ColumnProperty.NotNull, "GJI_INS_LIC_APP_LIC", "GKH_MANORG_LIC_REQUEST", "ID"));
        }

        public override void Down()
        {
            ViewManager.Drop(Database, "GkhGji");

            Database.RemoveColumn("GJI_INSPECTION_LIC_APP", "MAN_ORG_LIC_ID");
            Database.RemoveTable("GJI_INSPECTION_LIC_APP");
        }
    }
}