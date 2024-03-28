namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2020031700
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2020031700")]
    [MigrationDependsOn(typeof(Version_2020022000.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_CH_GIS_ERP", new Column("ERPID", DbType.String));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_GIS_ERP", "ERPID");
        }
    }
}


