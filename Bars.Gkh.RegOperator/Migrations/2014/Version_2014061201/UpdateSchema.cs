using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014061201
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014061201")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014061200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_FED_STANDARD_FEE_CR",
                new Column("VALUE", DbType.Decimal, ColumnProperty.NotNull, 0M),
                new Column("DATE_START", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DATE_END", DbType.DateTime));

            Database.AddEntityTable("REGOP_RO_SUBSIDY_ACCOUNT",
                new RefColumn("RO_ID", ColumnProperty.NotNull, "RO_SUBSIDY_ACC", "GKH_REALITY_OBJECT", "ID"),
                new Column("DATE_OPEN", DbType.DateTime, ColumnProperty.NotNull),
                new Column("ACC_NUM", DbType.String, 20));

            Database.AddEntityTable("REGOP_RO_SUBSIDY_ACC_OP",
                new Column("OPER_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OPER_SUM", DbType.Decimal, ColumnProperty.NotNull),
                new Column("OPER_TYPE", DbType.Int16, ColumnProperty.NotNull),
                new RefColumn("ACC_ID", ColumnProperty.NotNull, "RO_SUB_ACC_SUB_ACC", "REGOP_RO_SUBSIDY_ACCOUNT", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_RO_SUBSIDY_ACC_OP");
            Database.RemoveTable("REGOP_RO_SUBSIDY_ACCOUNT");
            Database.RemoveTable("REGOP_FED_STANDARD_FEE_CR");
        }
    }
}
