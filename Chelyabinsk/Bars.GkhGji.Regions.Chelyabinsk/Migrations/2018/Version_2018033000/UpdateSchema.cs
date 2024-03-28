namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2018033000
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2018033000")]
    [MigrationDependsOn(typeof(Version_2018032000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("MESSAGEID", DbType.String, 36, ColumnProperty.Unique));
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("PAYT_REASON", DbType.String, 2, ColumnProperty.Null));
            Database.ChangeColumn("GJI_CH_GIS_GMP", new Column("KIO", DbType.String, 5));
        }

        public override void Down()
        {
            Database.ChangeColumn("GJI_CH_GIS_GMP", new Column("KIO", DbType.String, 20));
            Database.RemoveColumn("GJI_CH_GIS_GMP", "PAYT_REASON");
            Database.RemoveColumn("GJI_CH_GIS_GMP", "MESSAGEID");
        }
    }
}