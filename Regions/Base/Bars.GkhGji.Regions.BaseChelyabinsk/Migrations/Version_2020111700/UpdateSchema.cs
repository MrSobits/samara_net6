namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2020111700
{
    using B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2020111700")]
    [MigrationDependsOn(typeof(Version_2020101900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_APPCIT_ORDER", new Column("CONFIRMED", DbType.Int32, 4, ColumnProperty.NotNull, 30));
        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_APPCIT_ORDER", "CONFIRMED");
        }
    }
}