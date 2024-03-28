namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2022103100
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2022103100")]
    [MigrationDependsOn(typeof(Version_2022081100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddRefColumn("GJI_CH_COURT_PRACTICE", new RefColumn("APP_DEC_ID", ColumnProperty.None, "GJI_CH_COURT_PRACTICE_APP_DEC_ID", "GJI_APPCIT_DECISION", "ID"));
            Database.AddColumn("GJI_CH_COURT_PRACTICE", new Column("DISPUTE_RESULT", DbType.Int32, 4, ColumnProperty.NotNull, 10));
            this.Database.ExecuteQuery("UPDATE GJI_CH_COURT_PRACTICE SET DISPUTE_RESULT = CM_RESULT");
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_COURT_PRACTICE", "DISPUTE_RESULT");
            Database.RemoveColumn("GJI_CH_COURT_PRACTICE", "APP_DEC_ID");
        }
    }
}