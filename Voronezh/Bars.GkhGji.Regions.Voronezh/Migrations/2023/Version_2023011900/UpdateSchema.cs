namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2023011900
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023011900")]
    [MigrationDependsOn(typeof(Version_2023011600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_CH_SMEV_EGRN", new Column("PROTOCOL_OSP_REQUEST_ID", DbType.String));
        }

        public override void Down()
        {         
            Database.RemoveColumn("GJI_CH_SMEV_EGRN", "PROTOCOL_OSP_REQUEST_ID");
        }
    }
}