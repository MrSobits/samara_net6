namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016103100
{
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция RegOperator 2016103100
    /// </summary>
    [Migration("2016103100")]
    [MigrationDependsOn(typeof(Version_2016102700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.RemoveIndex("ind_regop_tr_op", "REGOP_TRANSFER");
            this.Database.RemoveIndex("idx_regop_transfer_tg", "REGOP_TRANSFER");
            this.Database.RemoveIndex("idx_regop_transfer_sg", "REGOP_TRANSFER");
            this.Database.RemoveIndex("ind_regop_transfer_transfer", "REGOP_TRANSFER");
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
        }
    }
}
