namespace Bars.Gkh.Migrations._2023.Version_2023050134
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050134")]

    [MigrationDependsOn(typeof(Version_2023050133.UpdateSchema))]

    /// Является Version_2020082700 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_CONTRAGENT_CLW",
                new RefColumn("CONTRAGENT_ID", "GKH_CONTRAGENT_CLW_CONTRAGENT", "GKH_CONTRAGENT", "ID"));

            Database.AddEntityTable("GKH_CONTRAGENT_CLW_MUNICIPALITY",
                new RefColumn("CONTRAGENT_CLW_ID", "GKH_CONTRAGENT_CLW_MUNICIPALITY_CONTRAGENT_CLW", "GKH_CONTRAGENT_CLW", "ID"),
                new RefColumn("MUNICIPALITY_ID", "GKH_CONTRAGENT_CLW_MUNICIPALITY_MUNICIPALITY", "GKH_DICT_MUNICIPALITY", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_CONTRAGENT_CLW_MUNICIPALITY");
            Database.RemoveTable("GKH_CONTRAGENT_CLW");
        }
    }
}