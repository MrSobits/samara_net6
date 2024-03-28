namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017040800
{
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция RegOperator 2017040800
    /// </summary>
    [Migration("2017040800")]
    [MigrationDependsOn(typeof(Version_2017030100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {          
            this.Database.AddIndex("IND_REGOP_PACC_W_RAA", false, "REGOP_PERS_ACC", "RAA_WALLET_ID");
            this.Database.AddForeignKey("FK_REGOP_PACC_W_RAA", "REGOP_PERS_ACC", "RAA_WALLET_ID", "REGOP_WALLET", "ID");           
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
        }
    }
}