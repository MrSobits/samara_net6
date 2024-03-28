namespace Bars.GkhCr.Migrations._2015.Version_2015092800
{
    using Bars.Gkh.Utils;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015092800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migration.Version_2015082400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                if (Database.ExecuteScalar("SELECT count(*) FROM USER_TAB_COLUMNS WHERE TABLE_NAME = 'CR_DICT_FIN_SOURCE' and COLUMN_NAME ='CODE' AND Nullable='N'").ToLong() == 1)
                {
                    Database.AlterColumnSetNullable("CR_DICT_FIN_SOURCE", "CODE", true);
                }
            }

            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.AlterColumnSetNullable("CR_DICT_FIN_SOURCE", "CODE", true);
            }
        }

        public override void Down()
        {

        }
    }
}
