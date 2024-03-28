namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014022601
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014022601")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014022600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_SUPP_ACC_OP_ACT",
                new RefColumn("SUPP_ACC_OP_ID", ColumnProperty.NotNull, "SUPP_ACC_OP_ACT_OP", "REGOP_RO_SUPP_ACC_OP", "ID"),
                new RefColumn("PERF_ACT_ID", ColumnProperty.NotNull, "SUPP_ACC_OP_ACT_ACT", "CR_OBJ_PERFOMED_WORK_ACT", "ID"));

            Database.AddUniqueConstraint("UNQ_SUPP_ACC_OP_OP_ID", "REGOP_SUPP_ACC_OP_ACT", "SUPP_ACC_OP_ID");
            Database.AddUniqueConstraint("UNQ_SUPP_ACC_OP_ACT_ID", "REGOP_SUPP_ACC_OP_ACT", "PERF_ACT_ID");

            Database.AddEntityTable("REGOP_PAY_ACC_OP_ACT",
                new RefColumn("PAY_ACC_OP_ID", ColumnProperty.NotNull, "PAY_ACC_OP_ACT_OP", "REGOP_RO_PAYMENT_ACC_OP", "ID"),
                new RefColumn("PERF_ACT_PAY_ID", ColumnProperty.NotNull, "PAY_ACC_OP_ACT_ACT", "CR_OBJ_PER_ACT_PAYMENT", "ID"));

            Database.AddUniqueConstraint("UNQ_PAY_ACC_OP_OP_ID", "REGOP_PAY_ACC_OP_ACT", "PAY_ACC_OP_ID");
            Database.AddUniqueConstraint("UNQ_PAY_ACC_OP_ACT_ID", "REGOP_PAY_ACC_OP_ACT", "PERF_ACT_PAY_ID");
        }

        public override void Down()
        {
            Database.RemoveConstraint("REGOP_PAY_ACC_OP_ACT", "UNQ_PAY_ACC_OP_OP_ID");
            Database.RemoveConstraint("REGOP_PAY_ACC_OP_ACT", "UNQ_PAY_ACC_OP_ACT_ID");

            Database.RemoveConstraint("REGOP_SUPP_ACC_OP_ACT", "UNQ_SUPP_ACC_OP_OP_ID");
            Database.RemoveConstraint("REGOP_SUPP_ACC_OP_ACT", "UNQ_SUPP_ACC_OP_ACT_ID");

            Database.RemoveTable("REGOP_SUPP_ACC_OP_ACT");
            Database.RemoveTable("REGOP_PAY_ACC_OP_ACT");
        }
    }
}
