namespace Bars.GkhGji.Regions.Nso.Migrations.Version_2014102100
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014102100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Nso.Migrations.Version_2014052800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_ACTCHECKRO_LTEXT",
                new RefColumn("ACTCHECK_RO_ID", ColumnProperty.NotNull, "GJI_ACTCHECKRO_LTEXT", "GJI_ACTCHECK_ROBJECT", "ID"),
                new Column("DESCRIPTION", DbType.Binary, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_ACTCHECKRO_LTEXT");
        }
    }
}