namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2022110400
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2022110400")]
    [MigrationDependsOn(typeof(Version_2022103100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddRefColumn("GJI_CH_COURT_PRACTICE", new RefColumn("RES_DEF_IP", ColumnProperty.None, "GJI_CH_COURT_PRACTICE_RES_DEF_IP", "GJI_RESOLUTION_DEFINITION", "ID"));
            Database.AddRefColumn("GJI_CH_COURT_PRACTICE", new RefColumn("APP_DEF_ID", ColumnProperty.None, "GJI_CH_COURT_PRACTICE_APP_DEF_ID", "GJI_APPCIT_DEFINITION", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_COURT_PRACTICE", "APP_DEF_ID");
            Database.RemoveColumn("GJI_CH_COURT_PRACTICE", "RES_DEF_IP");
        }
    }
}