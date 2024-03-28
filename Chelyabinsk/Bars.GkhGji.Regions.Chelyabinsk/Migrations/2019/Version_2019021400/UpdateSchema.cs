namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2019021400
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2019021400")]
    [MigrationDependsOn(typeof(Version_2019021300.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {           
            Database.AddColumn("GJI_CH_SMEV_EGRIP", new Column("CITY_NAME", DbType.String, 500));
            Database.AddColumn("GJI_CH_SMEV_EGRIP", new Column("CITY_TYPE", DbType.String, 500));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_SMEV_EGRIP", "CITY_NAME");
            Database.RemoveColumn("GJI_CH_SMEV_EGRIP", "CITY_TYPE");
        }

    }

}