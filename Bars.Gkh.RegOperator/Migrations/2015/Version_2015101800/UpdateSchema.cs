namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015101800
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2015.10.18.00
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015101800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015100900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            if (!this.Database.IndexExists("ind_regop_pacc_rpacex", "regop_pers_acc"))
            {
                this.Database.AddIndex("ind_regop_pacc_rpacex", false, "regop_pers_acc", "regop_pers_acc_extsyst");
            }
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            if (this.Database.IndexExists("ind_regop_pacc_rpacex", "regop_pers_acc"))
            {
                this.Database.RemoveIndex("ind_regop_pacc_rpacex", "regop_pers_acc");
            }
        }
    }
}
