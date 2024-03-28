namespace Bars.Gkh.Ris.Migrations.Version_2016052800
{
    using System.Data;

    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using B4.Modules.Ecm7.Framework;

    /// <summary>
    /// 2016 05 28 00
    /// </summary>
    [Migration("2016052800")]
    [MigrationDependsOn(typeof(Version_2016052400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Метод миграции на версию вперед
        /// </summary>
        public override void Up()
        {
            //this.Database.AddRisEntityTable("RIS_COMPLETED_WORK",
            //    new RefColumn("WORK_PLAN_ITEM_ID", "RISCOMPDWRK_WRK_PLN_ITM_ID", "RIS_WORKPLAN_ITEM", "ID"),
            //    new RefColumn("OBJECT_PHOTO_ID", "RISCOMPDWRK_OBJ_PHOTO_ID", "RIS_ATTACHMENT", "ID"),
            //    new RefColumn("ACT_FILE_ID", "RISCOMPDWRK_ACT_FILE_ID", "RIS_ATTACHMENT", "ID"),
            //    new Column("ACT_DATE", DbType.DateTime),
            //    new Column("ACT_NUMBER", DbType.String));
        }

        /// <summary>
        /// Метод миграции на версию назад
        /// </summary>
        public override void Down()
        {
            //this.Database.RemoveTable("RIS_WORKPLAN_ITEM");
        }
    }
}