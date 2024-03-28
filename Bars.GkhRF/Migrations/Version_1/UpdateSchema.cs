// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateSchema.cs" company="">
//   
// </copyright>
// <summary>
//   The update schema.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bars.GkhRf.Migrations.Version_1
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----Оплата
            Database.AddEntityTable(
                "RF_PAYMENT",
                new Column("REALITY_OBJ_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_RF_PAYM_RO", false, "RF_PAYMENT", "REALITY_OBJ_ID");
            Database.AddForeignKey("FK_RF_PAYM_RO", "RF_PAYMENT", "REALITY_OBJ_ID", "GKH_REALITY_OBJECT", "ID");
            //-----

            //-----Оплата КР
            Database.AddEntityTable(
                "RF_PAYMENT_ITEM",
                new Column("PAYMENT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("MANAG_ORG_ID", DbType.Int64, 22),
                new Column("TYPE_PAYMENT", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("CHARGE_DATE", DbType.Date),
                new Column("BALANCE_IN", DbType.Decimal),
                new Column("BALANCE_OUT", DbType.Decimal),
                new Column("CHARGE_POPULATION", DbType.Decimal),
                new Column("PAID_POPULATION", DbType.Decimal),
                new Column("RECALCULATION", DbType.Decimal),
                new Column("TOTAL_AREA", DbType.Decimal),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_RF_PAYM_ITEM_MO", false, "RF_PAYMENT_ITEM", "MANAG_ORG_ID");
            Database.AddIndex("IND_RF_PAYM_ITEM_P", false, "RF_PAYMENT_ITEM", "PAYMENT_ID");
            Database.AddForeignKey("FK_RF_PAYM_ITEM_MO", "RF_PAYMENT_ITEM", "MANAG_ORG_ID", "GKH_MANAGING_ORGANIZATION", "ID");
            Database.AddForeignKey("FK_RF_PAYM_ITEM_P", "RF_PAYMENT_ITEM", "PAYMENT_ID", "RF_PAYMENT", "ID");
            //-----

            //-----Договор рег. фонда
            Database.AddEntityTable(
                "RF_CONTRACT",
                new Column("MANAG_ORG_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("DOCUMENT_DATE", DbType.Date),
                new Column("DATE_BEGIN", DbType.Date),
                new Column("DATE_END", DbType.Date),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_RF_CONTR_MO", false, "RF_CONTRACT", "MANAG_ORG_ID");
            Database.AddIndex("IND_RF_CONTR_FILE", false, "RF_CONTRACT", "FILE_ID");
            Database.AddForeignKey("FK_RF_CONTR_FILE", "RF_CONTRACT", "FILE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_RF_CONTR_MO", "RF_CONTRACT", "MANAG_ORG_ID", "GKH_MANAGING_ORGANIZATION", "ID");
            //-----

            //-----Объект договора рег. фонда
            Database.AddEntityTable(
                "RF_CONTRACT_OBJECT",
                new Column("CONTRACT_RF_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("REALITY_OBJ_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("TYPE_CONDITION", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("INCLUDE_DATE", DbType.Date),
                new Column("EXCLUDE_DATE", DbType.Date),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_RF_CONTR_RO_CON", false, "RF_CONTRACT_OBJECT", "CONTRACT_RF_ID");
            Database.AddIndex("IND_RF_CONTR_RO_RO", false, "RF_CONTRACT_OBJECT", "REALITY_OBJ_ID");
            Database.AddIndex("IND_RF_CONTR_RO_FILE", false, "RF_CONTRACT_OBJECT", "FILE_ID");
            Database.AddForeignKey("FK_RF_CONTR_RO_FILE", "RF_CONTRACT_OBJECT", "FILE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_RF_CONTR_RO_RO", "RF_CONTRACT_OBJECT", "REALITY_OBJ_ID", "GKH_REALITY_OBJECT", "ID");
            Database.AddForeignKey("FK_RF_CONTR_RO_CON", "RF_CONTRACT_OBJECT", "CONTRACT_RF_ID", "RF_CONTRACT", "ID");
            //-----

            //-----Перечисление средств рег. фонда
            Database.AddEntityTable(
                "RF_TRANSFER",
                new Column("CONTRACT_RF_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_RF_TRANS_CON", false, "RF_TRANSFER", "CONTRACT_RF_ID");
            Database.AddForeignKey("FK_RF_TRANS_CON", "RF_TRANSFER", "CONTRACT_RF_ID", "RF_CONTRACT", "ID");
            //-----

            //-----Запись перечисление средств рег. фонда
            Database.AddEntityTable(
                "RF_TRANSFER_RECORD",
                new Column("STATE_ID", DbType.Int64, 22),
                new Column("TRANSFER_RF_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("TRANSFER_DATE", DbType.Date),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("DATE_FROM", DbType.Date),
                new Column("DOCUMENT_NAME", DbType.String, 300),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_RF_TRANS_REC_ST", false, "RF_TRANSFER_RECORD", "STATE_ID");
            Database.AddIndex("IND_RF_TRANS_REC_TR", false, "RF_TRANSFER_RECORD", "TRANSFER_RF_ID");
            Database.AddIndex("IND_RF_TRANS_REC_F", false, "RF_TRANSFER_RECORD", "FILE_ID");
            Database.AddForeignKey("FK_RF_TRANS_REC_F", "RF_TRANSFER_RECORD", "FILE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_RF_TRANS_REC_TR", "RF_TRANSFER_RECORD", "TRANSFER_RF_ID", "RF_TRANSFER", "ID");
            Database.AddForeignKey("FK_RF_TRANS_REC_ST", "RF_TRANSFER_RECORD", "STATE_ID", "B4_STATE", "ID");
            //-----

            //-----Запись перечисление средств рег. фонда
            Database.AddEntityTable(
                "RF_TRANSFER_REC_OBJ",
                new Column("TRANSFER_RF_RECORD_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("REALITY_OBJ_ID", DbType.Int64, 22),
                new Column("SUM", DbType.Decimal),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_RF_TRANS_RECRO_T", false, "RF_TRANSFER_REC_OBJ", "TRANSFER_RF_RECORD_ID");
            Database.AddIndex("IND_RF_TRANS_RECRO_RO", false, "RF_TRANSFER_REC_OBJ", "REALITY_OBJ_ID");
            Database.AddForeignKey("FK_RF_TRANS_RECRO_RO", "RF_TRANSFER_REC_OBJ", "REALITY_OBJ_ID", "GKH_REALITY_OBJECT", "ID");
            Database.AddForeignKey("FK_RF_TRANS_RECRO_T", "RF_TRANSFER_REC_OBJ", "TRANSFER_RF_RECORD_ID", "RF_TRANSFER_RECORD", "ID");

            //-----

            //-----Заявка на перечисление средств
            Database.AddEntityTable(
                "RF_REQUEST_TRANSFER",
                new Column("STATE_ID", DbType.Int64, 22),
                new Column("CONTRACT_RF_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("PROGRAM_CR_ID", DbType.Int64, 22),
                new Column("CONTRAGENT_BANK_ID", DbType.Int64, 22),
                new Column("MANAGING_ORGANIZATION_ID", DbType.Int64, 22),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("TYPE_PROGRAM_REQUEST", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("PERFOMER_NAME", DbType.String, 300),
                new Column("DOCUMENT_NAME", DbType.String, 300),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("DATE_FROM", DbType.Date),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_RF_REQTRANS_ST", false, "RF_REQUEST_TRANSFER", "STATE_ID");
            Database.AddIndex("IND_RF_REQTRANS_CON", false, "RF_REQUEST_TRANSFER", "CONTRACT_RF_ID");
            Database.AddIndex("IND_RF_REQTRANS_PR", false, "RF_REQUEST_TRANSFER", "PROGRAM_CR_ID");
            Database.AddIndex("IND_RF_REQTRANS_CB", false, "RF_REQUEST_TRANSFER", "CONTRAGENT_BANK_ID");
            Database.AddIndex("IND_RF_REQTRANS_MO", false, "RF_REQUEST_TRANSFER", "MANAGING_ORGANIZATION_ID");
            Database.AddIndex("IND_RF_REQTRANS_F", false, "RF_REQUEST_TRANSFER", "FILE_ID");
            Database.AddForeignKey("FK_RF_REQTRANS_F", "RF_REQUEST_TRANSFER", "FILE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_RF_REQTRANS_MO", "RF_REQUEST_TRANSFER", "MANAGING_ORGANIZATION_ID", "GKH_MANAGING_ORGANIZATION", "ID");
            Database.AddForeignKey("FK_RF_REQTRANS_CB", "RF_REQUEST_TRANSFER", "CONTRAGENT_BANK_ID", "GKH_CONTRAGENT_BANK", "ID");
            Database.AddForeignKey("IND_RF_REQTRANS_PR", "RF_REQUEST_TRANSFER", "PROGRAM_CR_ID", "CR_DICT_PROGRAM", "ID");
            Database.AddForeignKey("FK_RF_REQTRANS_CON", "RF_REQUEST_TRANSFER", "CONTRACT_RF_ID", "RF_CONTRACT", "ID");
            Database.AddForeignKey("FK_RF_REQTRANS_ST", "RF_REQUEST_TRANSFER", "STATE_ID", "B4_STATE", "ID");
            //-----

            //-----Перечисление ден средств средств
            Database.AddEntityTable(
                "RF_TRANSFER_FUNDS",
                new Column("REQUEST_TRANSFER_RF_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("REALITY_OBJ_ID", DbType.Int64, 22),
                new Column("WORK_KIND", DbType.String, 300),
                new Column("PAY_ALLOCATE", DbType.String, 300),
                new Column("PERSONAL_ACCOUNT", DbType.String, 300),
                new Column("SUM", DbType.Decimal),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_RF_TRANS_FUN_R", false, "RF_TRANSFER_FUNDS", "REQUEST_TRANSFER_RF_ID");
            Database.AddIndex("IND_RF_TRANS_FUN_RO", false, "RF_TRANSFER_FUNDS", "REALITY_OBJ_ID");
            Database.AddForeignKey("FK_RF_TRANS_FUN_RO", "RF_TRANSFER_FUNDS", "REALITY_OBJ_ID", "GKH_REALITY_OBJECT", "ID");
            Database.AddForeignKey("FK_RF_TRANS_FUN_R", "RF_TRANSFER_FUNDS", "REQUEST_TRANSFER_RF_ID", "RF_REQUEST_TRANSFER", "ID");
            //-----
        }

        public override void Down()
        {
            Database.RemoveConstraint("RF_TRANSFER_FUNDS", "FK_RF_TRANS_FUN_RO");
            Database.RemoveConstraint("RF_TRANSFER_FUNDS", "FK_RF_TRANS_FUN_R");
            Database.RemoveConstraint("RF_REQUEST_TRANSFER", "FK_RF_REQTRANS_F");
            Database.RemoveConstraint("RF_REQUEST_TRANSFER", "FK_RF_REQTRANS_MO");
            Database.RemoveConstraint("RF_REQUEST_TRANSFER", "FK_RF_REQTRANS_CB");
            Database.RemoveConstraint("RF_REQUEST_TRANSFER", "IND_RF_REQTRANS_PR");
            Database.RemoveConstraint("RF_REQUEST_TRANSFER", "FK_RF_REQTRANS_CON");
            Database.RemoveConstraint("RF_REQUEST_TRANSFER", "FK_RF_REQTRANS_ST");
            Database.RemoveConstraint("RF_TRANSFER_REC_OBJ", "FK_RF_TRANS_RECRO_RO");
            Database.RemoveConstraint("RF_TRANSFER_REC_OBJ", "FK_RF_TRANS_RECRO_T");
            Database.RemoveConstraint("RF_TRANSFER_RECORD", "FK_RF_TRANS_REC_F");
            Database.RemoveConstraint("RF_TRANSFER_RECORD", "FK_RF_TRANS_REC_TR");
            Database.RemoveConstraint("RF_TRANSFER_RECORD", "FK_RF_TRANS_REC_ST");
            Database.RemoveConstraint("RF_TRANSFER", "FK_RF_TRANS_CON");
            Database.RemoveConstraint("RF_CONTRACT", "FK_RF_CONTR_FILE");
            Database.RemoveConstraint("RF_CONTRACT", "FK_RF_CONTR_MO");
            Database.RemoveConstraint("RF_CONTRACT_OBJECT", "FK_RF_CONTR_RO_FILE");
            Database.RemoveConstraint("RF_CONTRACT_OBJECT", "FK_RF_CONTR_RO_RO");
            Database.RemoveConstraint("RF_CONTRACT_OBJECT", "FK_RF_CONTR_RO_CON");
            Database.RemoveConstraint("RF_PAYMENT", "FK_RF_PAYM_RO");
            Database.RemoveConstraint("RF_PAYMENT_ITEM", "FK_RF_PAYM_ITEM_MO");
            Database.RemoveConstraint("RF_PAYMENT_ITEM", "FK_RF_PAYM_ITEM_P");

            Database.RemoveTable("RF_PAYMENT");
            Database.RemoveTable("RF_PAYMENT_ITEM");
            Database.RemoveTable("RF_CONTRACT");
            Database.RemoveTable("RF_CONTRACT_OBJECT");
            Database.RemoveTable("RF_TRANSFER");
            Database.RemoveTable("RF_TRANSFER_RECORD");
            Database.RemoveTable("RF_TRANSFER_REC_OBJ");
            Database.RemoveTable("RF_TRANSFER_FUNDS");
            Database.RemoveTable("RF_REQUEST_TRANSFER");
        }
    }
}