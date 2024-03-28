namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015022700
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015022700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015022500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("REGOP_BANK_ACC_STMNT",
                new RefColumn("SUSPENSE_ACC_ID", "BANK_ACC_STMN_SUSP_ACC", "REGOP_SUSPENSE_ACCOUNT", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "SUSPENSE_ACC_ID");
        }
    }
}
