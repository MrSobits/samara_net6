namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017021000
{
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция RegOperator 2017021000
    /// </summary>
    [Migration("2017021000")]
    [MigrationDependsOn(typeof(_2016.Version_2016122000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            // проставляем IsAffect трансферам займов
            this.Database.ExecuteNonQuery(
                @"with result as (select op.id from regop_money_operation op join regop_ro_loan l on l.c_guid = op.originator_guid)
                    update regop_transfer set is_affect = true 
                    where exists (select null from result op where op.id = op_id)");
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
        }
    }
}
