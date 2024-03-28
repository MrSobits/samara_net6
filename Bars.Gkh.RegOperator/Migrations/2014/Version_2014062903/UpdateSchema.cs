namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014062903
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014062903")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014062902.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REG_OP_SUSPEN_ACCOUNT", new Column("C_GUID", DbType.String, 40, ColumnProperty.Null));
            Database.AddColumn("REGOP_RO_LOAN_PAYMENT", new Column("OP_GUID", DbType.String, 40, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("REG_OP_SUSPEN_ACCOUNT", "C_GUID");
            Database.RemoveColumn("REGOP_RO_LOAN_PAYMENT", "OP_GUID");
        }
    }
}
