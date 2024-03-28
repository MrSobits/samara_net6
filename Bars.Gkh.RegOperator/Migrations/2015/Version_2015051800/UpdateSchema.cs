namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015051800
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Gkh.Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015051800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015051500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("REGOP_BANK_ACC_STMNT", new FileColumn("DOCUMENT_ID", "REGOP_BANK_STMNT_DOC"));
            Database.AddRefColumn("REGOP_SUSPENSE_ACCOUNT", new FileColumn("DOCUMENT_ID", "REGOP_SUSP_ACC_DOC"));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_SUSPENSE_ACCOUNT", "DOCUMENT_ID");
            Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "DOCUMENT_ID");
        }
    }
}
