namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2018040400
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2018040400")]
    [MigrationDependsOn(typeof(Version_2018033000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("GJI_CH_GIS_GMP", "ANSWER_GET");
            Database.RemoveColumn("GJI_CH_GIS_GMP", "PACKET_ID");
            Database.RemoveColumn("GJI_CH_GIS_GMP", "PURPOSE");
        }

        public override void Down()
        {
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("ANSWER_GET", DbType.String, 500));
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("PACKET_ID", DbType.String, ColumnProperty.None));
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("PURPOSE", DbType.String));
        }
    }
}