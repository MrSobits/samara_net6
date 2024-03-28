namespace Bars.Gkh.Ris.Migrations.Version_2016063001
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016063001")]
    [MigrationDependsOn(typeof(Version_2016063000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //this.Database.AddRisEntityTable("RIS_CR_PLAN",
            //    new Column("NAME", DbType.String, 500),
            //    new Column("MUNICIPALITY_CODE", DbType.String, 11),
            //    new Column("MUNICIPALITY_NAME", DbType.String, 500),
            //    new Column("START_MONTH_YEAR", DbType.Date),
            //    new Column("END_MONTH_YEAR", DbType.Date));

            //this.Database.AddRisEntityTable("RIS_CR_PLANWORK",
            //    new Column("PLAN_GUID", DbType.String, 50),
            //    new Column("BUILDING_FIAS_GUID", DbType.String, 50),
            //    new Column("WORK_KIND_CODE", DbType.String, 10),
            //    new Column("WORK_KIND_GUID", DbType.String, 50),
            //    new Column("END_MONTH_YEAR", DbType.String, 50),
            //    new Column("MUNICIPALITY_CODE", DbType.String, 11),
            //    new Column("MUNICIPALITY_NAME", DbType.String, 500)
            //    );
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            //this.Database.RemoveTable("RIS_CR_PLANWORK");
            //this.Database.RemoveTable("RIS_CR_PLAN");
        }
    }
}