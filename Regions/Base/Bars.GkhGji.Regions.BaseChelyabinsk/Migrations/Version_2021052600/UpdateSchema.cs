namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2021052600
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2021052600")]
    [MigrationDependsOn(typeof(Version_2021040900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_LICENSE_ACTION", new Column("DECLINE_REASON", DbType.String, ColumnProperty.None, 4000));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_LICENSE_ACTION", "DECLINE_REASON");
        }

    }
}