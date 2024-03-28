namespace Bars.Gkh.Migrations.Version_2015050500
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015050500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015043000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        #region Overrides of Migration

        public override void Up()
        {
            Database.AddEntityTable(
                "clw_court_order_doc",

                #region Lawsuit map

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
                new Column("CB_DOCUMENT_TYPE", DbType.Int16, ColumnProperty.NotNull, 0),
                new Column("CB_SUM_REPAYMENT", DbType.Decimal),
                new Column("CB_DATE_DOC", DbType.DateTime),
                new Column("CB_NUMBER_DOC", DbType.String, 100),
                new Column("CB_SUM_STEP", DbType.Decimal),
                new Column("CB_IS_STOPP", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("CB_DATE_STOPP", DbType.DateTime),
                new Column("CB_REASON_STOPP", DbType.Int16, ColumnProperty.NotNull, 0),
                new Column("CB_REASON_STOPPDESC", DbType.String, 2000),
                new RefColumn("FILE_ID", "CLW_COURT_CLM_FILE", "B4_FILE_INFO", "ID"),
                new RefColumn("JURSECTOR_MU_ID", "CLW_COURT_CL_JSM", "GKH_DICT_MUNICIPALITY", "ID"),
                new RefColumn("CB_FILE_ID", "CLW_COURT_CL_CBFILE", "B4_FILE_INFO", "ID"),
                new RefColumn("JINST_ID", "CLW_COURT_CL_JINST", "CLW_JUR_INSTITUTION", "ID"),
                new RefColumn("CB_SSP_JINST_ID", "CLW_COURT_CL_CB_SSP_JINST", "CLW_JUR_INSTITUTION", "ID"),
                new RefColumn("PETITION_ID", "CLW_COURT_CL_PET", "CLW_DICT_PETITION_TYPE", "ID"),

                #endregion

                new Column("claim_date", DbType.DateTime, ColumnProperty.NotNull),
                new Column("objection_arrived", DbType.Int32, ColumnProperty.NotNull),
                new RefColumn("document_id", ColumnProperty.Null, "clw_court_ord_doc_doc", "b4_file_info", "id"));
        }

        public override void Down()
        {
            Database.RemoveTable("clw_court_order_doc");
        }

        #endregion
    }
}