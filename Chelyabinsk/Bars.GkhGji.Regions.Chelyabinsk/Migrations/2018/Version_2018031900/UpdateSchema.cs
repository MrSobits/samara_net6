namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2018031900
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2018031900")]
    [MigrationDependsOn(typeof(Version_2018031700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("UIN", DbType.String, 27, ColumnProperty.None));
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("RESOLUTION_ID", DbType.Int32, ColumnProperty.None));

        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_GIS_GMP", "UIN");
            Database.RemoveColumn("GJI_CH_GIS_GMP", "RESOLUTION_ID");
        }
    }
}