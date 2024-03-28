namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014060900
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014060900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014060800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_LEGAL_ACC_OWN", new Column("PRINT_ACT", DbType.Boolean, false));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_LEGAL_ACC_OWN", "PRINT_ACT");
        }
    }
}
