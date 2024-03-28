namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2021033100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Collections.Generic;
    using System.Data;

    [Migration("2021033100")]
    [MigrationDependsOn(typeof(Version_2021033000.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {

           
            Database.AddRefColumn("GJI_SMEV_OW_PROPERTY", new RefColumn("ATT_FILE_ID", ColumnProperty.Null, "GJI_CH_SMEV_EXP_RESOLUTION_ATTFILEINFO_ID", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_SMEV_OW_PROPERTY", "ATT_FILE_ID");
        }


    }
}
