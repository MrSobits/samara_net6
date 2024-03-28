namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014102300
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014102300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014101801.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (!Database.IndexExists("ind_regop_money_op_op_null", "regop_money_operation"))
                Database.ExecuteNonQuery(@"create index ind_regop_money_op_op_null on regop_money_operation using btree (canceled_op_id) where canceled_op_id is null");

            if (!Database.IndexExists("ind_regop_money_op_op_nnull", "regop_money_operation"))
                Database.ExecuteNonQuery(@"create index ind_regop_money_op_op_nnull on regop_money_operation using btree (canceled_op_id) where canceled_op_id is not null");

            if (!Database.IndexExists("ind_regop_transfer_op_date_date", "regop_transfer"))
                Database.ExecuteNonQuery(@"create index ind_regop_transfer_op_date_date on regop_transfer using btree (date(operation_date))");
        }

        public override void Down()
        {
            Database.ExecuteNonQuery(@"drop index ind_regop_money_op_op_null");
            Database.ExecuteNonQuery(@"drop index ind_regop_money_op_op_nnull");
            Database.ExecuteNonQuery(@"drop index ind_regop_transfer_op_date_date");
        }
    }
}
