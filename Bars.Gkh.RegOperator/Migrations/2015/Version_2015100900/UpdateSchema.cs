namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015100900
{
    using System;
    using System.Data;

    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015100900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015100500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_PERS_ACC_OWNER", "TOTAL_ACCOUNTS_COUNT", DbType.Int32, ColumnProperty.NotNull, 0);
            Database.AddColumn("REGOP_PERS_ACC_OWNER", "ACTIVE_ACCOUNTS_COUNT", DbType.Int32, ColumnProperty.NotNull, 0);

            var defaultStateId = 0L;
            using (
                var defaultStateReader =
                    Database.ExecuteQuery(
                        string.Format(
                            "SELECT ID FROM B4_STATE WHERE TYPE_ID = 'gkh_regop_personal_account' AND START_STATE = {0}",
                            ApplicationContext.Current.Configuration.DbDialect == DbDialect.PostgreSql ? "TRUE" : "1")))
            {
                if (defaultStateReader.Read())
                {
                    defaultStateId = defaultStateReader[0].ToLong();
                }
            }

            if (defaultStateId > 0)
            {
                Database.ExecuteNonQuery(
                    string.Format(
                        "UPDATE REGOP_PERS_ACC_OWNER OWNER SET "
                        + "TOTAL_ACCOUNTS_COUNT = (SELECT COUNT(ACC.*) FROM REGOP_PERS_ACC ACC WHERE ACC.ACC_OWNER_ID = OWNER.ID),"
                        + "ACTIVE_ACCOUNTS_COUNT = (SELECT COUNT(ACC.*) FROM REGOP_PERS_ACC ACC WHERE ACC.ACC_OWNER_ID = OWNER.ID AND ACC.STATE_ID = {0})",
                        defaultStateId));
            }
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PERS_ACC_OWNER", "ACTIVE_ACCOUNTS_COUNT");
            Database.RemoveColumn("REGOP_PERS_ACC_OWNER", "TOTAL_ACCOUNTS_COUNT");
        }
    }
}
