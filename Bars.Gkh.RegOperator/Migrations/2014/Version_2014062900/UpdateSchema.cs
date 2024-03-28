namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014062900
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014062900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014062700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_BANK_ACC_STMNT",
                new Column("OP_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("USER_LOGIN", DbType.String, 100),
                new Column("DOC_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DOC_SUM", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("DOC_NUM", DbType.String, 100),
                new Column("ACC_NUM", DbType.String, 100),
                new Column("P_DETAILS", DbType.String, 1000)
            );
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_BANK_ACC_STMNT");
        }
    }
}
