namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014061200
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014061200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014061000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_PREV_WORK_PAY",
                new RefColumn("ACCOUNT_ID", ColumnProperty.NotNull, "REGOP_PREV_WORK_PAY_ACC", "REGOP_PERS_ACC", "ID"),
                new Column("OPERATION_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("SUM", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("P_GUID", DbType.String, 40, ColumnProperty.NotNull));

            Database.AddEntityTable("REGOP_SUSPACC_WORKPAY",
              new RefColumn("SUSPACC_ID", ColumnProperty.NotNull, "REGOP_SUSPACC_WORKPAY_SA", "REG_OP_SUSPEN_ACCOUNT", "ID"),
              new RefColumn("WORKPAY_ID", ColumnProperty.NotNull, "REGOP_SUSPACC_WORKPAY_WP", "REGOP_PREV_WORK_PAY", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_SUSPACC_WORKPAY");
            Database.RemoveTable("REGOP_PREV_WORK_PAY");
        }
    }
}
