namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014102100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014102100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014052800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GJI_ACTCHECKRO_LTEXT",
                new RefColumn("ACTCHECK_RO_ID", ColumnProperty.NotNull, "GJI_ACTCHECKRO_LTEXT", "GJI_ACTCHECK_ROBJECT", "ID"),
                new Column("DESCRIPTION", DbType.Binary, ColumnProperty.Null));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_ACTCHECKRO_LTEXT");
        }
    }
}