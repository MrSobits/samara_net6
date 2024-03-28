namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2014012500
{
    using System;
    using System.Data;
    using B4.Utils;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014012500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2014012400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddTable("OVRHL_PR_DEC_LIST_SERVICES",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("DATE_START", DbType.DateTime),
                new Column("DATE_END", DbType.DateTime));

            Database.AddForeignKey("FK_OVRHL_PR_DEC_LIST_SERVICES", "OVRHL_PR_DEC_LIST_SERVICES", "ID", "OVRHL_PROP_OWN_DECISION_BASE", "ID");

            Database.AddTable("OVRHL_PR_DEC_MIN_FUND_SIZE",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("SUBJ_MIN_FUND_SIZE", DbType.Int64, 40),
                new Column("OWN_MIN_FUND_SIZE", DbType.Int64),
                new Column("DATE_START", DbType.DateTime),
                new Column("DATE_END", DbType.DateTime));

            Database.AddForeignKey("FK_OVRHL_PR_DEC_MIN_FUND_SIZE", "OVRHL_PR_DEC_MIN_FUND_SIZE", "ID", "OVRHL_PROP_OWN_DECISION_BASE", "ID");

            Database.AddEntityTable("OVRHL_PRDEC_SVC_WORK_FACT",
                new Column("FACT_YEAR", DbType.Int32, ColumnProperty.Null),
                new RefColumn("DECISION_ID", ColumnProperty.NotNull, "O_PRDEC_SVC_WF_D", "OVRHL_PR_DEC_LIST_SERVICES", "ID"),
                new RefColumn("WORK_ID", ColumnProperty.NotNull, "O_PRDEC_SVC_WF_WORK", "GKH_DICT_WORK", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_PRDEC_SVC_WORK_FACT");

            var listServIdsForDelete = GetIdsForDelete("OVRHL_PR_DEC_LIST_SERVICES");

            if (!listServIdsForDelete.IsEmpty())
                Database.ExecuteNonQuery(string.Format("delete from OVRHL_PR_DEC_LIST_SERVICES where id in ({0})", listServIdsForDelete));

            Database.RemoveConstraint("OVRHL_PR_DEC_LIST_SERVICES", "FK_OVRHL_PR_DEC_LIST_SERVICES");

            var minFundSizeIdsForDelete = GetIdsForDelete("OVRHL_PR_DEC_MIN_FUND_SIZE");

            if (!minFundSizeIdsForDelete.IsEmpty())
                Database.ExecuteNonQuery(string.Format("delete from OVRHL_PR_DEC_MIN_FUND_SIZE where id in ({0})", minFundSizeIdsForDelete));

            Database.RemoveConstraint("OVRHL_PR_DEC_MIN_FUND_SIZE", "FK_OVRHL_PR_DEC_MIN_FUND_SIZE");

            Database.RemoveTable("OVRHL_PR_DEC_MIN_FUND_SIZE");
            Database.RemoveTable("OVRHL_PR_DEC_LIST_SERVICES");
        }

        private string GetIdsForDelete(string tableName)
        {
            var idsForDelete = string.Empty;

            using (var dr = Database.ExecuteQuery("select id from " + tableName))
            {
                while (dr.Read())
                {
                    if (dr[0] is DBNull)
                        break;

                    int id;
                    if (!int.TryParse(dr[0].ToString(), out id) || id == 0)
                        continue;

                    if (!idsForDelete.IsEmpty())
                    {
                        idsForDelete += ",";
                    }

                    idsForDelete += id;
                }
            }

            return idsForDelete;
        }
    }
}