namespace Bars.Gkh.Overhaul.Migration.Version_2014052100
{
    using Bars.Gkh.Utils;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014052100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Migration.Version_2014051400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.AlterColumnSetNullable("OVRHL_ACCOUNT", "REALITY_OBJECT_ID", true);
            }
            //Database.AlterColumnSetNullable<OracleDialect>("OVRHL_ACCOUNT", "REALITY_OBJECT_ID", true);

            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery(string.Format("declare l_nullable varchar2(1); " + "begin " + "select nullable into l_nullable from user_tab_columns " + "where table_name = '{0}' " + "and column_name = '{1}'; " + "if l_nullable = 'N' then " + "execute immediate 'alter table {0} modify ({1} null)'; " + "end if; " + "end;", "OVRHL_ACCOUNT", "REALITY_OBJECT_ID"));
            }


        }

        public override void Down()
        {
        }
    }
}