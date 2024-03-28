namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2021102200
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2021102200")]
    [MigrationDependsOn(typeof(Version_2021083000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_CH_GIS_ERP", new Column("CTADDRESS", DbType.String, 500));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_GIS_ERP", "CTADDRESS");
        }
    }
}