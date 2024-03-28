namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016110200
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Миграция RegOperator 2016110200
    /// </summary>
    [Migration("2016110200")]
    [MigrationDependsOn(typeof(Version_2016103100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AlterColumnSetNullable("REGOP_PERS_ACC_RECALC_EVT", "PERS_ACC_ID", true);
            this.Database.ExecuteNonQuery("UPDATE REGOP_PERS_ACC_RECALC_EVT set PERS_ACC_ID = null where PERS_ACC_ID = 0");

            this.Database.AddForeignKey(
                "FK_REGOP_PERS_ACC_RECALC_EVT_ACC_ID", 
                "REGOP_PERS_ACC_RECALC_EVT", 
                "PERS_ACC_ID",
                "REGOP_PERS_ACC",
                "ID");

            this.Database.AddIndex("IND_REGOP_PERS_ACC_RECALC_EVT_ACC_ID", false, "REGOP_PERS_ACC_RECALC_EVT", "PERS_ACC_ID");
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveIndex("IND_REGOP_PERS_ACC_RECALC_EVT_ACC_ID", "REGOP_PERS_ACC_RECALC_EVT");
            this.Database.RemoveConstraint("REGOP_PERS_ACC_RECALC_EVT", "FK_REGOP_PERS_ACC_RECALC_EVT_ACC_ID");
            this.Database.ExecuteNonQuery("UPDATE REGOP_PERS_ACC_RECALC_EVT set PERS_ACC_ID = 0 where PERS_ACC_ID is null");
            this.Database.AlterColumnSetNullable("REGOP_PERS_ACC_RECALC_EVT", "PERS_ACC_ID", false);
        }
    }
}
