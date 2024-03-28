namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2019070300
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2019070300")]
    [MigrationDependsOn(typeof(Version_2019052800.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("CHARGE_ID", DbType.String, 100));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_GIS_GMP", "CHARGE_ID");
        }
    }
}


