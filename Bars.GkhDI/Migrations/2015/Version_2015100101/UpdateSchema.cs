namespace Bars.GkhDi.Migrations.Version_2015100101
{
    using System;
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Utils;
    using Bars.B4.Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015100101")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2015100100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.AlterColumnSetNullable("DI_DICT_PERIOD", "NAME", true);
            }

            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                if (Database.ExecuteScalar("SELECT count(*) FROM USER_TAB_COLUMNS WHERE TABLE_NAME = 'DI_DICT_PERIOD' and COLUMN_NAME ='NAME' AND Nullable='N'").ToLong() == 1)
                {
                    Database.AlterColumnSetNullable("DI_DICT_PERIOD", "NAME", true);
                }
            }
        }

        public override void Down()
        {
            Database.ExecuteNonQuery("UPDATE DI_DICT_PERIOD SET NAME = '' WHERE NAME is NULL");
            Database.AlterColumnSetNullable("DI_DICT_PERIOD", "NAME", false);
        }
    }

}
