namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2021033000
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Collections.Generic;
    using System.Data;

    [Migration("2021033000")]
    [MigrationDependsOn(typeof(Version_2021032900.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {

           
            Database.AddRefColumn("GJI_CH_SMEV_EXP_RESOLUTION", new RefColumn("ANS_FILE_ID", ColumnProperty.Null, "GJI_CH_SMEV_EXP_RESOLUTION_FILEINFO_ID", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_CH_SMEV_CHANGE_PREM_STATE", new RefColumn("ANS_FILE_ID", ColumnProperty.Null, "GJI_CH_SMEV_CHANGE_PREM_STATE_FILEINFO_ID", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_SMEV_CHANGE_PREM_STATE", "ANS_FILE_ID");
            Database.RemoveColumn("GJI_CH_SMEV_EXP_RESOLUTION", "ANS_FILE_ID");
        }


    }
}
