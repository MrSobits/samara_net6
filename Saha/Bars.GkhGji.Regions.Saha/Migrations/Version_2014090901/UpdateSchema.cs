namespace Bars.GkhGji.Regions.Saha.Migrations.Version_2014090901
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014090901")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Saha.Migrations.Version_2014082800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_ACTCHECKRO_LTEXT",
                new RefColumn("ACTCHECK_RO_ID", ColumnProperty.NotNull, "GJI_ACTCHECKRO_LTEXT_ARO", "GJI_ACTCHECK_ROBJECT", "ID"),
                new Column("NOT_REV_VIOL", DbType.Binary, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_ACTCHECKRO_LTEXT");
        }
    }
}