namespace Bars.Gkh.Migration.Version_2015031600
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015031600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migration.Version_2015022401.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (!Database.TableExists("CLW_NOTIFICATION"))
            {
                Database.AddTable("CLW_NOTIFICATION",
                    new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                    new Column("SEND_DATE", DbType.DateTime),
                    new Column("ELIMINATION_DATE", DbType.DateTime),
                    new Column("ELIMINATION_METHOD", DbType.String, 1000));

                Database.AddForeignKey("FK_CLW_NOTIF_DOC", "CLW_NOTIFICATION", "ID", "CLW_DOCUMENT", "ID");
                Database.AddRefColumn("CLW_NOTIFICATION", new RefColumn("FILE_ID", "CLW_NOTIF_FILE", "B4_FILE_INFO", "ID"));
            }

            if (!Database.TableExists("CLW_PRETENSION"))
            {
                Database.AddTable("CLW_PRETENSION",
                    new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                    new Column("SEND_DATE", DbType.DateTime),
                    new Column("REVIEW_DATE", DbType.DateTime),
                    new Column("SUM", DbType.Decimal),
                    new Column("PENALTY", DbType.Decimal),
                    new Column("SUM_PENALTY_CALC", DbType.String, 1000),
                    new Column("REQ_SATISFACTION", DbType.Int32, ColumnProperty.NotNull, 0));

                Database.AddForeignKey("FK_CLW_PRETENSION_DOC", "CLW_PRETENSION", "ID", "CLW_DOCUMENT", "ID");
                Database.AddRefColumn("CLW_PRETENSION", new RefColumn("FILE_ID", "CLW_PRETENSION_FILE", "B4_FILE_INFO", "ID"));
            }


            if (!Database.TableExists("CLW_ACT_VIOL_IDENTIF"))
            {
                Database.AddTable("CLW_ACT_VIOL_IDENTIF",
                    new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                    new Column("ACT_TYPE", DbType.Int32, ColumnProperty.NotNull, 0),
                    new Column("FACT_OF_RECEIVE", DbType.Int32, ColumnProperty.NotNull, 0),
                    new Column("FACT_OF_SIGN", DbType.Int32, ColumnProperty.NotNull, 0),
                    new Column("SEND_DATE", DbType.DateTime),
                    new Column("SIGN_DATE", DbType.DateTime),
                    new Column("FORM_DATE", DbType.DateTime),
                    new Column("SIGN_TIME", DbType.String, 50),
                    new Column("FORM_TIME", DbType.String, 50),
                    new Column("SIGN_PLACE", DbType.String, 500),
                    new Column("FORM_PLACE", DbType.String, 500),
                    new Column("PERSONS", DbType.String, 500));

                Database.AddForeignKey("FK_CLW_ACT_VIOL_ID_DOC", "CLW_ACT_VIOL_IDENTIF", "ID", "CLW_DOCUMENT", "ID");
            }

            if (!Database.TableExists("CLW_LAWSUIT"))
            {
                Database.AddTable("CLW_LAWSUIT",
                        new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                        new Column("DATE_CONSIDERAT", DbType.DateTime),
                        new Column("NUM_CONSIDERAT", DbType.String, 100),
                        new Column("DATE_END", DbType.DateTime),
                        new Column("BID_DATE", DbType.DateTime),
                        new Column("BID_NUMBER", DbType.String, 100),
                        new Column("DEBT_SUM", DbType.Decimal),
                        new Column("PENALTY_DEBT", DbType.Decimal),
                        new Column("DUTY", DbType.Decimal),
                        new Column("COSTS", DbType.Decimal),
                        new Column("WHO_CONSIDERED", DbType.Int16, ColumnProperty.NotNull, 0),
                        new Column("JURSECTOR_NUMBER", DbType.String, 100),
                        new Column("DATE_OF_ADOPTION", DbType.DateTime),
                        new Column("DATE_OF_REWIEW", DbType.DateTime),
                        new Column("SUSPENDED", DbType.Boolean, ColumnProperty.NotNull, false),
                        new Column("DETERMIN_NUMBER", DbType.String, 100),
                        new Column("DETERMIN_DATE", DbType.DateTime),
                        new Column("RESULT_CONSIDERATION", DbType.Int16, ColumnProperty.NotNull, 0),
                        new Column("LAW_TYPE_DOCUMENT", DbType.Int16, ColumnProperty.NotNull, 0),
                        new Column("DEBT_SUM_APPROV", DbType.Decimal),
                        new Column("PENALTY_DEBT_APPROV", DbType.Decimal),
                        new Column("CB_SIZE", DbType.Int16, ColumnProperty.NotNull, 0),
                        new Column("CB_DEBT_SUM", DbType.Decimal),
                        new Column("CB_PENALTY_DEBT", DbType.Decimal),
                        new Column("CB_FACT_INITIATED", DbType.Int16, ColumnProperty.NotNull, 0),
                        new Column("CB_DATE_INITIATED", DbType.DateTime),
                        new Column("CB_STATION_SSP", DbType.String, 500),
                        new Column("CB_DOCUMENT_TYPE", DbType.Int16, ColumnProperty.NotNull, 0),
                        new Column("CB_SUM_REPAYMENT", DbType.Decimal),
                        new Column("CB_DATE_DOC", DbType.DateTime),
                        new Column("CB_NUMBER_DOC", DbType.String, 100),
                        new Column("CB_SUM_STEP", DbType.Decimal),
                        new Column("CB_IS_STOPP", DbType.Boolean, ColumnProperty.NotNull, false),
                        new Column("CB_DATE_STOPP", DbType.DateTime),
                        new Column("CB_REASON_STOPP", DbType.Int16, ColumnProperty.NotNull, 0),
                        new Column("CB_REASON_STOPPDESC", DbType.String, 2000));

                Database.AddForeignKey("FK_CLW_LAWSUIT_DOC", "CLW_LAWSUIT", "ID", "CLW_DOCUMENT", "ID");
                Database.AddRefColumn("CLW_LAWSUIT", new RefColumn("FILE_ID", "CLW_LAWSUIT_FILE", "B4_FILE_INFO", "ID"));
                Database.AddRefColumn("CLW_LAWSUIT", new RefColumn("JURSECTOR_MU_ID", "CLW_LAWSUIT_JSM", "GKH_DICT_MUNICIPALITY", "ID"));
                Database.AddRefColumn("CLW_LAWSUIT", new RefColumn("CB_FILE_ID", "CLW_LAWSUIT_CBFILE", "B4_FILE_INFO", "ID"));

                Database.AddEntityTable("CLW_LAWSUIT_DOC",
                        new RefColumn("DOCUMENT_ID", ColumnProperty.NotNull, "CLW_LAWSUIT_DOC_D", "CLW_DOCUMENT", "ID"),
                        new RefColumn("FILE_ID", "CLW_LAWSUIT_DOC_FILE", "B4_FILE_INFO", "ID"),
                        new Column("DOC_NAME", DbType.String, 500),
                        new Column("DOC_DATE", DbType.DateTime),
                        new Column("DOC_NUMBER", DbType.String, 100),
                        new Column("DESCRIPTION", DbType.String, 2000));

                Database.AddEntityTable("CLW_LAWSUIT_COURT",
                        new RefColumn("DOCUMENT_ID", ColumnProperty.NotNull, "CLW_LAWSUIT_COURT_D", "CLW_DOCUMENT", "ID"),
                        new RefColumn("FILE_ID", "CLW_LAWSUIT_COURT_FILE", "B4_FILE_INFO", "ID"),
                        new Column("COURT_TYPE", DbType.Int16, ColumnProperty.NotNull, 0),
                        new Column("DOC_DATE", DbType.DateTime),
                        new Column("DOC_NUMBER", DbType.String, 100),
                        new Column("DESCRIPTION", DbType.String, 2000));
            }
        }

        public override void Down()
        {
            Database.RemoveTable("CLW_LAWSUIT_COURT");
            Database.RemoveTable("CLW_LAWSUIT_DOC");

            Database.RemoveConstraint("CLW_LAWSUIT", "FK_CLW_LAWSUIT_DOC");
            Database.RemoveColumn("CLW_LAWSUIT", "FILE_ID");
            Database.RemoveColumn("CLW_LAWSUIT", "JURSECTOR_MU_ID");
            Database.RemoveColumn("CLW_LAWSUIT", "CB_FILE_ID");

            Database.RemoveTable("CLW_LAWSUIT");

            Database.RemoveConstraint("CLW_ACT_VIOL_IDENTIF", "FK_CLW_ACT_VIOL_ID_DOC");
            Database.RemoveConstraint("CLW_PRETENSION", "FK_CLW_PRETENSION_DOC");
            Database.RemoveConstraint("CLW_NOTIFICATION", "FK_CLW_NOTIF_DOC");

            Database.RemoveColumn("CLW_PRETENSION", "FILE_ID");
            Database.RemoveColumn("CLW_NOTIFICATION", "FILE_ID");

            Database.RemoveTable("CLW_ACT_VIOL_IDENTIF");
            Database.RemoveTable("CLW_PRETENSION");
            Database.RemoveTable("CLW_NOTIFICATION");
        }
    }
}