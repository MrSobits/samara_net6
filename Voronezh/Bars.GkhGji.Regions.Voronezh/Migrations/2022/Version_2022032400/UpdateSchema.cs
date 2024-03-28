namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2022032400
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2022032400")]
    [MigrationDependsOn(typeof(Version_2022031000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_CH_SMEV_MVD", new RefColumn("CONTRAGENT_ID", ColumnProperty.None, "GJI_CH_SMEV_MVD_CONTRAGENT_ID", "GKH_CONTRAGENT", "ID"));
            Database.AddColumn("GJI_CH_SMEV_MVD", new RefColumn("CONTACT_ID", ColumnProperty.None, "GJI_CH_SMEV_MVD_CONTACT_ID", "GKH_CONTRAGENT_CONTACT", "ID"));

        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_SMEV_MVD", "CONTACT_ID");
            Database.RemoveColumn("GJI_CH_SMEV_MVD", "CONTRAGENT_ID");
        }
    }
}