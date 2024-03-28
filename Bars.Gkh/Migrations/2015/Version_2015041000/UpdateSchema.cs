namespace Bars.Gkh.Migration.Version_2015041000
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015041000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2015040700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_MANORG_REQ_ANNEX",
                    new RefColumn("LIC_REQUEST_ID", ColumnProperty.NotNull, "GKH_MANORG_REQ_ANN_L", "GKH_MANORG_LIC_REQUEST", "ID"),
                    new Column("LIC_ANNEX_NAME", DbType.String, 200),
                    new Column("LIC_ANNEX_NUMBER", DbType.String, 100),
                    new Column("LIC_ANNEX_DATE", DbType.DateTime),
                    new Column("LIC_ANNEX_DESCR", DbType.String, 2000),
                    new RefColumn("LIC_ANNEX_FILE_ID", ColumnProperty.Null, "GKH_MANORG_REQ_ANN_F", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_MANORG_REQ_ANNEX");
        }
    }
}