namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014102902
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014102902")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014102901.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("REGOP_UNACCEPT_PAY_PACKET", new RefColumn("BANK_DOC_ID", ColumnProperty.Null, "REGOP_UNACC_PAY_P_BD", "REGOP_BANK_DOC_IMPORT", "ID"));
            Database.AddRefColumn("REGOP_SUSPENSE_ACCOUNT", new RefColumn("BANK_DOC_ID", ColumnProperty.Null, "REGOP_SUSP_ACC_BD", "REGOP_BANK_DOC_IMPORT", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_SUSPENSE_ACCOUNT", "BANK_DOC_ID");
            Database.RemoveColumn("REGOP_UNACCEPT_PAY_PACKET", "BANK_DOC_ID");
        }
    }
}
