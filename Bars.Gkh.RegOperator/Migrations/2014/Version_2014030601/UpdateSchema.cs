namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014030601
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014030601")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014030600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("REGOP_RO_SUPP_ACC", "SALDO");
        }

        public override void Down()
        {
            Database.AddColumn("REGOP_RO_SUPP_ACC", "SALDO", DbType.Decimal);
        }
    }
}
