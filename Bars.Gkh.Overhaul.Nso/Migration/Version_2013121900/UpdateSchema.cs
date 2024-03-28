namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013121900
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013121900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013121801.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // перенесли в модуль RegOperator
            //Database.AddEntityTable("OVRHL_REG_OP_PERS_ACC_MU",
            //    new RefColumn("REG_OP_ID", ColumnProperty.NotNull, "OV_REG_OP_PA_MU_OP", "OVRHL_REG_OPERATOR", "ID"),
            //    new RefColumn("MU_ID", ColumnProperty.NotNull, "OV_REG_OP_PA_MU_MU", "GKH_DICT_MUNICIPALITY", "ID"),
            //    new Column("OWNER_FIO", DbType.String, 150),
            //    new Column("PAID_CONTRIB", DbType.Decimal),
            //    new Column("CREDIT_CONTRIB", DbType.Decimal),
            //    new Column("CREDIT_PENALTY", DbType.Decimal),
            //    new Column("PAID_PENALTY", DbType.Decimal),
            //    new Column("SUM_LOCAL_BUD", DbType.Decimal),
            //    new Column("SUM_SUBJ_BUD", DbType.Decimal),
            //    new Column("SUM_FED_BUD", DbType.Decimal),
            //    new Column("SUM_ADOPT", DbType.Decimal),
            //    new Column("REPAY_SUM_ADOPT", DbType.Decimal)
            //   );
        }

        public override void Down()
        {
            //Database.RemoveEntityTable("OVRHL_REG_OP_PERS_ACC_MU");
        }
    }
}