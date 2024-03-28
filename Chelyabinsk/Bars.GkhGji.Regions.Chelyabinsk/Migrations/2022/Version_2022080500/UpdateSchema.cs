namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2022080500
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2022080500")]
    [MigrationDependsOn(typeof(Version_2022071900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddRefColumn("GJI_CH_COURT_PRACTICE",
                new RefColumn("RES_DEC_IP", ColumnProperty.None, "GJI_CH_COURT_PRACTICE_RES_DEC_IP", "GJI_RESOLUTION_DECISION", "ID"));

          //  Database.RemoveColumn("GJI_CH_APPCIT_ADMONITION", "APP_DEC_ID");
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_COURT_PRACTICE", "RES_DEC_IP");
        }
    }
}