using System.Data;
using global::Bars.B4.Modules.Ecm7.Framework;

namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014112800
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014112800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014112400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // cущность перенесся из модуля RF, пишу миграцию на создание таблицы, если вдруг в будущем  удалять модуль RF
            if (!Database.TableExists("RF_TRANSFER_CTR"))
            {
                Database.AddEntityTable(
                    "RF_TRANSFER_CTR",
                    new Column("STATE_ID", DbType.Int64, 22),
                    new Column("FILE_ID", DbType.Int64, 22),
                    new Column("DATE_FROM", DbType.Date),
                    new Column("DOCUMENT_NAME", DbType.String, 300),
                    new Column("DOCUMENT_NUM", DbType.String, 50),
                    new Column("PROGRAM_CR_ID", DbType.Int64, 22),
                    new Column("CONTRAGENT_BANK_ID", DbType.Int64, 22),
                    new Column("PERFOMER_NAME", DbType.String, 300),
                    new Column("BUDGET_MU", DbType.Decimal),
                    new Column("BUDGET_SUB", DbType.Decimal),
                    new Column("OWNER_RES", DbType.Decimal),
                    new Column("FUND_RES", DbType.Decimal),
                    new Column("OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                    new Column("FIN_SOURCE_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                    new Column("BUILDER_ID", DbType.Int64, 22),
                    new Column("TYPE_PAYMENT", DbType.Int16, ColumnProperty.NotNull, 10),
                    new Column("EXTERNAL_ID", DbType.String, 36),
                    new RefColumn("BUILD_CONTRACT_ID", ColumnProperty.NotNull, "RF_TRANSFER_CTR_TW_CTR", "CR_OBJ_BUILD_CONTRACT", "ID"), 
                    new Column("TYPE_PROGRAM_REQUEST", DbType.Int32, 4, ColumnProperty.NotNull, 10));

                Database.AddIndex("IND_CTR_TRANS_FS", false, "RF_TRANSFER_CTR", "FIN_SOURCE_ID");
                Database.AddIndex("IND_CTR_TRANS_BLDR", false, "RF_TRANSFER_CTR", "BUILDER_ID");
                Database.AddIndex("IND_CTR_TRANS_ST", false, "RF_TRANSFER_CTR", "STATE_ID");
                Database.AddIndex("IND_CTR_TRANS_PR", false, "RF_TRANSFER_CTR", "PROGRAM_CR_ID");
                Database.AddIndex("IND_CTR_TRANS_CB", false, "RF_TRANSFER_CTR", "CONTRAGENT_BANK_ID");
                Database.AddIndex("IND_CTR_TRANS_F", false, "RF_TRANSFER_CTR", "FILE_ID");

                Database.AddForeignKey("FK_RFTRANS_F", "RF_TRANSFER_CTR", "FILE_ID", "B4_FILE_INFO", "ID");
                Database.AddForeignKey("FK_RF_CTRTRANS_CB", "RF_TRANSFER_CTR", "CONTRAGENT_BANK_ID", "GKH_CONTRAGENT_BANK", "ID");
                Database.AddForeignKey("IND_RF_CTRTRANS_PR", "RF_TRANSFER_CTR", "PROGRAM_CR_ID", "CR_DICT_PROGRAM", "ID");
                Database.AddForeignKey("FK_RF_CTRTRANS_ST", "RF_TRANSFER_CTR", "STATE_ID", "B4_STATE", "ID");
                Database.AddForeignKey("FK_CTR_TRANS_FS", "RF_TRANSFER_CTR", "FIN_SOURCE_ID", "CR_DICT_FIN_SOURCE", "ID");
                Database.AddForeignKey("FK_CTR_TRANS_BLDR", "RF_TRANSFER_CTR", "BUILDER_ID", "GKH_BUILDER", "ID");
            }
        }

        public override void Down()
        {
            
        }
    }
}
