namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014082800
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014082800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014082500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_SALDO_CHANGE", new Column("OPER_DATE", DbType.DateTime, ColumnProperty.Null));
            Database.ExecuteNonQuery(
                "update REGOP_SALDO_CHANGE set OPER_DATE = OBJECT_CREATE_DATE where REGOP_SALDO_CHANGE is null");
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_SALDO_CHANGE", "OPER_DATE");
        }
    }
}
