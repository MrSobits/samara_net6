namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2019011700
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2019011700")]
    [MigrationDependsOn(typeof(Version_2018072400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
           Database.AddColumn("GJI_CH_GIS_GMP", new Column("MANORG_LIC_REQUEST_ID", DbType.Int32, ColumnProperty.None));
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("REISSUANCE_ID", DbType.Int32, ColumnProperty.None));
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("LIC_REQ_TYPE", DbType.Int32, 4, ColumnProperty.None));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_GIS_GMP", "LIC_REQ_TYPE");
            Database.RemoveColumn("GJI_CH_GIS_GMP", "REISSUANCE_ID");
            Database.RemoveColumn("GJI_CH_GIS_GMP", "MANORG_LIC_REQUEST_ID");
        }
    }
}