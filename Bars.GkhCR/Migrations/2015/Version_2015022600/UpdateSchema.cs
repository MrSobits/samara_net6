namespace Bars.GkhCr.Migration.Version_2015022600
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;
    using Gkh;
    using Gkh.Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015022600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2015022000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("CR_OBJECT", new RefColumn("BEFORE_DELETE_PROGRAM_ID", "BEFORE_DEL_PROG", "CR_DICT_PROGRAM", "ID"));

            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.AlterColumnSetNullable("CR_OBJECT", "PROGRAM_ID", true);
            }

            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery(string.Format("declare l_nullable varchar2(1); " + "begin " + "select nullable into l_nullable from user_tab_columns " + "where table_name = '{0}' " + "and column_name = '{1}'; " + "if l_nullable = 'N' then " + "execute immediate 'alter table {0} modify ({1} null)'; " + "end if; " + "end;", "CR_OBJECT", "PROGRAM_ID"));
            }


            ViewManager.Drop(Database, "GkhCr", "DeleteViewCrObject");
            ViewManager.Create(Database, "GkhCr", "CreateViewCrObject");
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_OBJECT", "BEFORE_DELETE_PROGRAM_ID");
        }
    }
}