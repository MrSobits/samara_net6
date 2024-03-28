namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2018032000
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2018032000")]
    [MigrationDependsOn(typeof(Version_2018031900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
           Database.AddColumn("GJI_CH_GIS_GMP", new Column("PROTOCOL_ID", DbType.Int32, ColumnProperty.None));
        }

        public override void Down()
        {           
            Database.RemoveColumn("GJI_CH_GIS_GMP", "PROTOCOL_ID");
        }
    }
}