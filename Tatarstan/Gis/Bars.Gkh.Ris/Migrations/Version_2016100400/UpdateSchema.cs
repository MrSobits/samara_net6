namespace Bars.Gkh.Ris.Migrations.Version_2016100400
{
    using System.Data;
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016100400")]
    [MigrationDependsOn(typeof(Version_2016082400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //this.Database.AddRisTable("HOME_PERIOD",
            //    new RefColumn("HOUSE_ID", "RIS_PERIOD_HOUSE_ID", "RIS_HOUSE", "ID"),
            //    new Column("FIAS_HOUSE_GUID", DbType.String),
            //    new Column("IS_UO", DbType.Boolean));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            //this.Database.RemoveTable("HOME_PERIOD");
        }
    }
}