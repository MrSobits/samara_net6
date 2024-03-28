namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2021011200
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Collections.Generic;
    using System.Data;

    [Migration("2021011200")]
    [MigrationDependsOn(typeof(Version_2020122900.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_FILE_REGISTER", new Column("DATE_FROM", DbType.DateTime, ColumnProperty.Null));
            Database.AddColumn("GJI_FILE_REGISTER", new Column("DATE_TO", DbType.DateTime, ColumnProperty.Null));
            Database.AddColumn("GJI_CH_SMEV_EXP_RESOLUTION", new RefColumn("MUNICIPALITY_ID", "GJI_CH_SMEV_EXP_RESOLUTION_MUNICIPALITY", "GKH_DICT_MUNICIPALITY", "ID"));
            Database.AddColumn("GJI_CH_SMEV_CHANGE_PREM_STATE", new RefColumn("MUNICIPALITY_ID", "GJI_CH_SMEV_CHANGE_PREM_STATE_MUNICIPALITY", "GKH_DICT_MUNICIPALITY", "ID"));
            Database.AddColumn("GJI_CH_SMEV_SOCIAL_HIRE", new RefColumn("MUNICIPALITY_ID", "GJI_CH_SMEV_SOCIAL_HIRE_MUNICIPALITY", "GKH_DICT_MUNICIPALITY", "ID"));
            List<string> tableFilesList = new List<string>();
            tableFilesList.Add("GJI_CH_SMEV_CHANGE_PREM_STATE_FILE");
            tableFilesList.Add("GJI_CH_SMEV_EXP_RESOLUTION_FILE");
            tableFilesList.Add("GJI_CH_SMEV_SOCIAL_HIRE_FILE");
            foreach (string tab in tableFilesList)
            {
                DeleteQuery(tab);
            }
            List<string> tableList = new List<string>();
            tableList.Add("GJI_CH_SMEV_EXP_RESOLUTION");
            tableList.Add("GJI_CH_SMEV_CHANGE_PREM_STATE");
            tableList.Add("GJI_CH_SMEV_SOCIAL_HIRE");
            foreach (string tab in tableList)
            {
                DeleteQuery(tab);
            }
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_FILE_REGISTER", "DATE_TO");
            Database.RemoveColumn("GJI_FILE_REGISTER", "DATE_FROM");
            Database.RemoveColumn("GJI_CH_SMEV_EXP_RESOLUTION", "MUNICIPALITY_ID");
            Database.RemoveColumn("GJI_CH_SMEV_CHANGE_PREM_STATE", "MUNICIPALITY_ID");
            Database.RemoveColumn("GJI_CH_SMEV_SOCIAL_HIRE", "MUNICIPALITY_ID");
        }

        private void DeleteQuery(string table)
        {
            this.Database.ExecuteQuery("DELETE FROM " + table);
        }

    }
}
