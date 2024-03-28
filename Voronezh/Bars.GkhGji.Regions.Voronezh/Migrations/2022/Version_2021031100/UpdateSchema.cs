namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2021031100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Collections.Generic;
    using System.Data;

    [Migration("2021031100")]
    [MigrationDependsOn(typeof(Version_2021030100.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GJI_CH_SMEV_EXP_RESOLUTION", new RefColumn("RO_ID", ColumnProperty.None, "GJI_CH_SMEV_EXP_RESOLUTION_RO", "GKH_REALITY_OBJECT", "ID"));           
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_SMEV_EXP_RESOLUTION", "RO_ID");
        }


    }
}
