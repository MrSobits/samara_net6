namespace Bars.Gkh.RegOperator.Migrations.Version_2014121200
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014121200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014121100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            /*
             * По ошибке 62973.
             * Перенес миграцию из модуля Bars.RegOperator
             * Так как к этому моменту столбец может быть создан, то проверяю его существование.
             */

            if (!Database.ColumnExists("GKH_CIT_SUG", "EXECUTOR_CR_FUND_ID"))
            {
                Database.AddRefColumn("GKH_CIT_SUG", new RefColumn("EXECUTOR_CR_FUND_ID", "GKH_CIT_SUG_CR_FUND", "GKH_CONTRAGENT_CONTACT", "ID"));
            }
        }

        public override void Down()
        {
            /* См. комментарий в методе Up() */

            if (Database.ColumnExists("GKH_CIT_SUG", "EXECUTOR_CR_FUND_ID"))
            {
                Database.RemoveColumn("GKH_CIT_SUG", "EXECUTOR_CR_FUND_ID");
            }
        }
    }
}