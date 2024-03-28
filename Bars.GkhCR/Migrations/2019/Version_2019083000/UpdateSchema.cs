using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using Bars.Gkh.Utils;
using System.Data;

namespace Bars.GkhCr.Migrations._2019.Version_2019083000
{
    [Migration("2019083000")]
    [MigrationDependsOn(typeof(Version_2019073000.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("CR_OBJ_MASS_BUILD_CONTRACT",
                new Column("TYPE_CONTRACT_BUILD", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("DATE_START_WORK", DbType.Date),
                new Column("DATE_END_WORK", DbType.Date),
                new Column("DATE_GJI", DbType.Date),
                new Column("DOCUMENT_DATE_FROM", DbType.Date),
                new Column("PROTOCOL_DATE_FROM", DbType.Date),
                new Column("BUDGET_MO", DbType.Decimal),
                new Column("BUDGET_SUBJ", DbType.Decimal),
                new Column("OWNER_MEANS", DbType.Decimal),
                new Column("FUND_MEANS", DbType.Decimal),
                new Column("DATE_CANCEL", DbType.Date),
                new Column("DATE_ACCEPT", DbType.Date),
                new Column("DOCUMENT_NAME", DbType.String, 300),
                new Column("PROTOCOL_NAME", DbType.String, 300),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("PROTOCOL_NUM", DbType.String, 50),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("SUM", DbType.Decimal),
                new Column("USED_IN_EXPORT", DbType.Int32, 20, ColumnProperty.NotNull, 20),
                new Column("TERMINATION_DATE", DbType.Date),
                new Column("TERMINATION_REASON", DbType.String, 500),
                new Column("GUARANTEE_PERIOD", DbType.Int32),
                new Column("URL_RESULT_TRADING", DbType.String),
                new Column("TERMINATION_DOCUMENT_NUMBER", DbType.String),
                new Column("EXTERNAL_ID", DbType.String, 36),
                new RefColumn("TERMINATION_REASON_ID", "CR_OBJ_MASS_BUILD_CONTRACT_TERMINATION_REASON", "CR_DICT_TERMINATION_REASON", "ID"),
                new FileColumn("TERMINATION_DOCUMENT_FILE_ID", "CR_OBJ_MASS_BC_TERMINATION_DOCUMENT_FILE"),
                new RefColumn("CONTRAGENT_ID", "FK_CR_MASS_BUILD_CONTRAGENT_ID", "GKH_CONTRAGENT", "ID"),
                new RefColumn("STATE_ID", ColumnProperty.None, "FK_CR_MASS_BUILD_STATE", "B4_STATE", "ID"),
                new RefColumn("INSPECTOR_ID", ColumnProperty.None, "FK_CR_MASS_BUILD_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"),
                new RefColumn("BUILDER_ID", ColumnProperty.None, "FK_CR_MASS_BUILD_BUILDER", "GKH_BUILDER", "ID"),
                new RefColumn("PROGRAM_ID", ColumnProperty.None, "FK_CR_MASS_BUILD_PROGRAM_CR", "CR_DICT_PROGRAM", "ID"),
                new RefColumn("DOCUMENT_FILE_ID", ColumnProperty.None, "FK_CR_MASS_BUILD_CN_DFILE", "B4_FILE_INFO", "ID"),
                new RefColumn("PROTOCOL_FILE_ID", ColumnProperty.None, "FK_CR_MASS_BUILD_CN_PFILE", "B4_FILE_INFO", "ID"));

            this.Database.AddEntityTable("CR_MASS_BLD_CONTR_OBJ_CR",              
                new Column("SUM", DbType.Decimal, ColumnProperty.None),          
                new RefColumn("MASS_BC_ID", "FK_MBC_CR_OBJECT_CONTRACT", "CR_OBJ_MASS_BUILD_CONTRACT", "ID"),             
                new RefColumn("OBJECTCR_ID", "FK_MBC_CR_OBJECT_OBJECT", "CR_OBJECT", "ID"));

            this.Database.AddEntityTable("CR_MASS_BLD_CONTR_TYPE_WRK",
               new Column("SUM", DbType.Decimal, ColumnProperty.None),
               new RefColumn("MASS_BC_ID", "FK_MBC_TYPE_WORK_CONTRACT", "CR_OBJ_MASS_BUILD_CONTRACT", "ID"),
               new RefColumn("WORK_ID", "FK_MBC_TYPE_WORK_WORK", "GKH_DICT_WORK", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("CR_MASS_BLD_CONTR_TYPE_WRK");
            this.Database.RemoveTable("CR_MASS_BLD_CONTR_OBJ_CR");
            this.Database.RemoveTable("CR_OBJ_MASS_BUILD_CONTRACT");
        }
    }
}