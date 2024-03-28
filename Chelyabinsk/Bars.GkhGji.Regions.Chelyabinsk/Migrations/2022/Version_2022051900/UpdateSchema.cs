namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2022051900
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2022051900")]
    [MigrationDependsOn(typeof(Version_2022051800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddRefColumn("GJI_CH_COURT_PRACTICE", new RefColumn("REQUEST_ID", ColumnProperty.None, "GJI_VR_COURT_PRACTICE_LIC_STMNT", "GJI_MKD_LIC_STATEMENT", "ID"));
            Database.AddRefColumn("GJI_CH_COURT_PRACTICE", new RefColumn("APP_DEC_ID", ColumnProperty.None, "FK_GJI_VR_COURT_PRACTICE_APP_DEC_ID", "GJI_APPCIT_DECISION", "ID"));

        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_COURT_PRACTICE", "REQUEST_ID");
            Database.RemoveColumn("GJI_CH_COURT_PRACTICE", "APP_DEC_ID");
        }
    }
}