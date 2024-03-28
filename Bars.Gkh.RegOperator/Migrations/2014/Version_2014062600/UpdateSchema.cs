namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014062600
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014062600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014062200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            /*
             * По ошибке 62973.
             * Комментирую тело миграции, потому что миграция была ошибочно добавлена не в тот модуль.
             * На самом деле она должна содержаться в модуле Bars.Gkh. Туда ее и переношу.
             */
            
            // Database.AddRefColumn("GKH_CIT_SUG", new RefColumn("EXECUTOR_CR_FUND_ID", "GKH_CIT_SUG_CR_FUND", "GKH_CONTRAGENT_CONTACT", "ID"));
        }

        public override void Down()
        {
            /* См. комментарий в методе Up() */
            
            // Database.RemoveRefColumn("GKH_CIT_SUG", "EXECUTOR_CR_FUND_ID");
        }
    }
}
