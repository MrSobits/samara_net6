namespace Bars.GkhGji.Regions.Smolensk.Migrations.Version_2014100700
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014100700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Smolensk.Migrations.Version_2014071700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_ACT_SURV_LONGDESC",
                new RefColumn("ACT_SURV_ID", ColumnProperty.NotNull, "GJI_ACT_S_LONGDESC", "GJI_ACTSURVEY", "ID"),
                new Column("DESCRIPTION", DbType.Binary, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_ACT_SURV_LONGDESC");
        }
    }
}