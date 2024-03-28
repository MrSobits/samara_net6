namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2021040800
{
    using B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2021040800")]
    [MigrationDependsOn(typeof(Version_2021040700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_LICENSE_ACTION_FILE", new Column("FILE_NAME", DbType.String, 300, ColumnProperty.None));
        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_LICENSE_ACTION_FILE", "FILE_NAME");
        }
    }
}