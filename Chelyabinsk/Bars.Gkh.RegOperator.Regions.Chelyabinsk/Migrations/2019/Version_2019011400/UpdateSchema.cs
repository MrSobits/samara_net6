using Bars.B4.Modules.Ecm7.Framework;

using System.Data;

namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk._2019.Version_2019011400
{
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019011400")]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
       
        public override void Up()
        {
            Database.AddEntityTable("CLW_LAWSUIT_DEBTWORK",
                   new Column("CB_SIZE", DbType.Int16, ColumnProperty.NotNull, 0),
                   new Column("CB_DEBT_SUM", DbType.Decimal),
                   new Column("CB_PENALTY_DEBT", DbType.Decimal),
                   new Column("CB_FACT_INITIATED", DbType.Int16, ColumnProperty.NotNull, 0),
                   new Column("CB_DATE_INITIATED", DbType.DateTime),
                   new Column("FACT_INITIATED_NOTE", DbType.String),
                   new RefColumn("CB_SSP_JINST_ID", "CLW_LAWSUITDBT_CB_SSP_JINST", "CLW_JUR_INSTITUTION", "ID"),
                   new Column("CB_SSP_DATE", DbType.DateTime),
                   new Column("CB_DOCUMENT_TYPE", DbType.Int16, ColumnProperty.NotNull, 0),
                   new Column("CB_SUM_REPAYMENT", DbType.Decimal),
                   new Column("CB_DATE_DOC", DbType.DateTime),
                   new Column("CB_NUMBER_DOC", DbType.String, 100),
                   new RefColumn("CB_FILE_ID", "CLW_LAWSUITDEBT_CBFILE", "B4_FILE_INFO", "ID"),
                   new Column("CB_SUM_STEP", DbType.Decimal),
                   new Column("CB_IS_STOPP", DbType.Boolean, ColumnProperty.NotNull, false),
                   new Column("CB_DATE_STOPP", DbType.DateTime),
                   new Column("CB_REASON_STOPP", DbType.Int16, ColumnProperty.NotNull, 0),
                   new Column("CB_REASON_STOPPDESC", DbType.String, 2000),
                   new Column("CB_DATE_DIRECTION_SSP", DbType.Date),
                   new RefColumn("OWNER_INFO", "CLW_LAWSUITDEBT_OWNER_INFO", "REGOP_LAWSUIT_OWNER_INFO", "ID"),
                   new RefColumn("LAWSUIT_ID", "CLW_LAWSUITDEBT_LSW", "CLW_LAWSUIT", "ID"));


            this.Database.AddEntityTable("CLW_LAWSUIT_DEBT_WORK_SSP_DOC",
                new RefColumn("LAWSUIT_DEBTWORKSSP_ID", "FK_LAWSUITDEBT_ID", "CLW_LAWSUIT_DEBTWORK", "ID"),
                new RefColumn("FILE_ID", "FK_LSD_FILE_ID", "B4_FILE_INFO", "ID"),
                new Column("DATE", DbType.DateTime, ColumnProperty.NotNull),
                new RefColumn("ROSP_ID", "CLW_LAWSUIT_DEBTWORKDOC_JINST_ID", "CLW_JUR_INSTITUTION", "ID"),
                new Column("COLLECT_DEBT_FROM", DbType.Int32),
                new Column("TYPE_DOC", DbType.Int16, ColumnProperty.NotNull, 0),
                new Column("NOTE", DbType.String, 255),
                new Column("NUMBER", DbType.Int32, ColumnProperty.NotNull));
       

        }

        public override void Down()
        {
            this.Database.RemoveTable("CLW_LAWSUIT_DEBT_WORK_SSP_DOC");
            this.Database.RemoveTable("CLW_LAWSUIT_DEBTWORK");
            
        }
    }
}