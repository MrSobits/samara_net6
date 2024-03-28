namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013122602
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013122602")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013122601.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery(@"ALTER TABLE OVRHL_SPECIAL_ACCOUNT ALTER COLUMN CREDIT_ORG_ID DROP NOT NULL;");
            }
            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery(@"ALTER TABLE OVRHL_SPECIAL_ACCOUNT MODIFY CREDIT_ORG_ID NULL");
            }
        }

        public override void Down()
        {
        }
    }
}