namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2021011500
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Collections.Generic;
    using System.Data;

    [Migration("2021011500")]
    [MigrationDependsOn(typeof(Version_2021011200.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_CH_SMEV_EMERGENCY_HOUSE", new RefColumn("MUNICIPALITY_ID", "GJI_CH_SMEV_EMERGENCY_HOUSE_MUNICIPALITY", "GKH_DICT_MUNICIPALITY", "ID"));
            Database.AddColumn("GJI_CH_SMEV_REDEVELOPMENT", new RefColumn("MUNICIPALITY_ID", "GJI_CH_SMEV_EMERGENCY_HOUSE_MUNICIPALITY", "GKH_DICT_MUNICIPALITY", "ID"));
            Database.AddColumn("GJI_SMEV_OW_PROPERTY", new RefColumn("MUNICIPALITY_ID", "GJI_SMEV_OW_PROPERTY_MUNICIPALITY", "GKH_DICT_MUNICIPALITY", "ID"));
            List<string> tableFilesList = new List<string>();
            tableFilesList.Add("GJI_CH_SMEV_EMERGENCY_HOUSE_FILE");
            tableFilesList.Add("GJI_CH_SMEV_REDEVELOPMENT_FILE");
            tableFilesList.Add("GJI_SMEV_OW_PROPERTY_FILE");
            foreach (string tab in tableFilesList)
            {
                DeleteQuery(tab);
            }
            List<string> tableList = new List<string>();
            tableList.Add("GJI_CH_SMEV_EMERGENCY_HOUSE");
            tableList.Add("GJI_CH_SMEV_REDEVELOPMENT");
            tableList.Add("GJI_SMEV_OW_PROPERTY");
            foreach (string tab in tableList)
            {
                DeleteQuery(tab);
            }
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_SMEV_OW_PROPERTY", "MUNICIPALITY_ID");
            Database.RemoveColumn("GJI_CH_SMEV_EMERGENCY_HOUSE", "MUNICIPALITY_ID");
            Database.RemoveColumn("GJI_CH_SMEV_REDEVELOPMENT", "MUNICIPALITY_ID");
        }

        private void DeleteQuery(string table)
        {
            this.Database.ExecuteQuery("DELETE FROM " + table);
        }

    }
}
