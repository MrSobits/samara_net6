namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2021041200
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2021041200")]
    [MigrationDependsOn(typeof(Version_2020111900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {

            Database.AddColumn("GJI_CH_SMEV_EGRUL", new RefColumn("XML_FILE", "GJI_CH_SMEV_SOC_GJI_CH_SMEV_EGRUL_XML_FILE_INFO_ID", "B4_FILE_INFO", "ID"));
            Database.AddColumn("GJI_CH_SMEV_EGRIP", new RefColumn("XML_FILE", "GJI_CH_SMEV_SOC_GJI_CH_SMEV_EGRIP_XML_FILE_INFO_ID", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_SMEV_EGRUL", "XML_FILE");
            Database.RemoveColumn("GJI_CH_SMEV_EGRIP", "XML_FILE");
        }

    }
}