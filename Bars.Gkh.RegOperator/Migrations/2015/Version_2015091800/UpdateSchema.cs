namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015091800
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015091800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015082600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddIndex("ind_regop_transfer_op_id", false, "regop_transfer", "op_id");
            this.Database.AddIndex("ind_regop_transfer_originator_id", false, "regop_transfer", "originator_id");
            this.Database.AddIndex("ind_regop_transfer_source", false, "regop_transfer", "source_guid");
            this.Database.AddIndex("ind_regop_transfer_target", false, "regop_transfer", "target_guid");
            this.Database.AddIndex("ind_regop_calc_acc_ro_robj", false, "regop_calc_acc_ro", "ro_id");
            this.Database.AddIndex("ind_regop_calc_acc_ro_acc", false, "regop_calc_acc_ro", "account_id");
        }

        public override void Down()
        {
            this.Database.RemoveIndex("ind_regop_calc_acc_ro_acc", "regop_calc_acc_ro");
            this.Database.RemoveIndex("ind_regop_transfer_op_id", "regop_transfer");
            this.Database.RemoveIndex("ind_regop_transfer_originator_id", "regop_transfer");
            this.Database.RemoveIndex("ind_regop_transfer_source", "regop_transfer");
            this.Database.RemoveIndex("ind_regop_transfer_target", "regop_transfer");
            this.Database.RemoveIndex("ind_regop_calc_acc_ro_robj", "regop_calc_acc_ro");
        }
    }
}
