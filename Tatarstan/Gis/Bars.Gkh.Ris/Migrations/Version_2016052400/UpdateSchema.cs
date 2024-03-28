namespace Bars.Gkh.Ris.Migrations.Version_2016052400
{
    using System.Data;

    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using B4.Modules.Ecm7.Framework;

    [Migration("2016052400")]
    [MigrationDependsOn(typeof(Version_2016052000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Метод миграции на версию вперед
        /// </summary>
        public override void Up()
        {
            //this.Database.AddRisEntityTable("RIS_WORKPLAN",
            //    new RefColumn("WORKLIST_ID", "FK_WORKLIST_ID", "RIS_WORKLIST", "ID"),
            //    new Column("YEAR", DbType.Int16));

            //this.Database.AddRisEntityTable("RIS_WORKPLAN_ITEM",
            //    new RefColumn("WORKPLAN_ID", "FK_WORKPLAN_ID", "RIS_WORKPLAN", "ID"),
            //    new RefColumn("WORKLIST_ITEM_ID", "RIS_WORKLIST_ITEM", "RIS_WORKPLAN", "ID"),
            //    new Column("YEAR", DbType.Int16),
            //    new Column("MONTH", DbType.Int32),
            //    new Column("DATE_WORK", DbType.DateTime),
            //    new Column("COUNT_WORK", DbType.Int32));
        }

        /// <summary>
        /// Метод миграции на версию назад
        /// </summary>
        public override void Down()
        {
            //this.Database.RemoveTable("RIS_WORKPLAN_ITEM");
            //this.Database.RemoveTable("RIS_WORKPLAN");
        }
    }
}