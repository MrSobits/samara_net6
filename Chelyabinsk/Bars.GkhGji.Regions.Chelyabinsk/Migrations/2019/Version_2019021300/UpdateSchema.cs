namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2019021300
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2019021300")]
    [MigrationDependsOn(typeof(Version_2019020100.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("GJI_CH_SMEV_EGRIP", "OKVED_NAMES");
            Database.AddColumn("GJI_CH_SMEV_EGRIP", new Column("OKVED_NAMES", DbType.String, 5000));
        }
        
        public override void Down()
        {

        }

    }
  
}
