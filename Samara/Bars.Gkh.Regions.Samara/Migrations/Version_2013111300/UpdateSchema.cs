namespace Bars.Gkh.Regions.Samara.Migrations.Version_2013111300
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013111300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Regions.Samara.Migrations.Version_1.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveTable("GKH_SAM_RO_OWNERCODE");

            Database.AddEntityTable(
                "GKH_SAM_AP_INF_OWN_CODE",
                new Column("OWNER_CODE", DbType.String, 100, ColumnProperty.Null),
                new RefColumn("AP_INFO_ID", ColumnProperty.NotNull, "GKH_SAM_RO_AP_OC_RO", "GKH_OBJ_APARTMENT_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_SAM_AP_INF_OWN_CODE");

            Database.AddEntityTable(
                "GKH_SAM_RO_OWNERCODE",
                new Column("OWNER_CODE", DbType.String, 100, ColumnProperty.Null),
                new RefColumn("RO_ID", ColumnProperty.NotNull, "GKH_SAM_RO_OC_RO", "GKH_REALITY_OBJECT", "ID"));
        }
    }
}