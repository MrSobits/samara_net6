namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2021083000
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2021083000")]
    [MigrationDependsOn(typeof(Version_2021082300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("GJI_CH_RESOLUTION_FIZ", "FLDOCTYPE_ID");
        }

        public override void Down()
        {
            
        }
    }
}