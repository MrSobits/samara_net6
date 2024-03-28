namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013122401
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013122401")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013122400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery(@"ALTER TABLE OVRHL_ACC_BANK_STATEMENT ALTER COLUMN ACCOUNT_ID DROP NOT NULL;");
            }

            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery(@"ALTER TABLE OVRHL_ACC_BANK_STATEMENT MODIFY ACCOUNT_ID NULL");
            }
        }

        public override void Down()
        {

        }
    }
}