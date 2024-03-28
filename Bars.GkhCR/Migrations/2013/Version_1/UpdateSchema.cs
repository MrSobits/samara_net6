// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateSchema.cs" company="">
//   
// </copyright>
// <summary>
//   The update schema.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bars.GkhCr.Migrations.Version_1
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----Разрез финансирования по КР
            Database.AddEntityTable(
                "CR_DICT_FIN_SOURCE",
                new Column("TYPE_FIN_GROUP", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("TYPE_FINANCE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("CODE", DbType.String, 50),
                new Column("NAME", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_CR_FIN_SRC_NAME", false, "CR_DICT_FIN_SOURCE", "NAME");
            //-----

            //-----Субтаблица работ по источнику финансирования
            Database.AddEntityTable("CR_DICT_FIN_SOURCE_WORK",
                new Column("WORK_ID",DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FIN_SOURCE_ID",DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_CR_FINS_WORK_WORK", false, "CR_DICT_FIN_SOURCE_WORK", "WORK_ID");
            Database.AddIndex("IND_CR_FINS_WORK_FS", false, "CR_DICT_FIN_SOURCE_WORK", "FIN_SOURCE_ID");
            Database.AddForeignKey("FK_CR_FINS_WORK_WORK", "CR_DICT_FIN_SOURCE_WORK", "WORK_ID", "GKH_DICT_WORK", "ID");
            Database.AddForeignKey("FK_CR_FINS_WORK_FS", "CR_DICT_FIN_SOURCE_WORK", "FIN_SOURCE_ID", "CR_DICT_FIN_SOURCE", "ID");
            //-----

            //-----Программа капитального ремонта
            Database.AddEntityTable(
                "CR_DICT_PROGRAM",
                new Column("PERIOD_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("TYPE_VISIBILITY", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("TYPE_PROGRAM_CR", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("CODE", DbType.String, 50),
                new Column("USED_IN_EXPORT", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("NOT_ADD_HOME", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("NAME", DbType.String, 300),
                new Column("MATCH_FL", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("TYPE_PROGRAM_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_CR_PROG_NAME", false, "CR_DICT_PROGRAM", "NAME");
            Database.AddIndex("IND_CR_PROG_PER", false, "CR_DICT_PROGRAM", "PERIOD_ID");
            Database.AddForeignKey("FK_CR_PROG_PER", "CR_DICT_PROGRAM", "PERIOD_ID", "GKH_DICT_PERIOD", "ID");
            //-----

            //-----Субтаблица источников финансирования работы
            Database.AddEntityTable("CR_DICT_PROGCR_FIN_SOURCE",
                new Column("PROGRAM_ID",DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FIN_SOURCE_ID",DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_CR_PROG_FIN_SRC_PR", false, "CR_DICT_PROGCR_FIN_SOURCE", "PROGRAM_ID");
            Database.AddIndex("IND_CR_PROG_FIN_SRC_FS", false, "CR_DICT_PROGCR_FIN_SOURCE", "FIN_SOURCE_ID");
            Database.AddForeignKey("FK_CR_PROG_FIN_SRC_PR", "CR_DICT_PROGCR_FIN_SOURCE", "PROGRAM_ID", "CR_DICT_PROGRAM", "ID");
            Database.AddForeignKey("FK_CR_PROG_FIN_SRC_FS", "CR_DICT_PROGCR_FIN_SOURCE", "FIN_SOURCE_ID", "CR_DICT_FIN_SOURCE", "ID");
            //-----

            //-----Объект капитального ремонта
            Database.AddEntityTable("CR_OBJECT",
                new Column("STATE_ID", DbType.Int64, 22),
                new Column("PROGRAM_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("REALITY_OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("GJI_NUM", DbType.String, 50),
                new Column("PROGRAM_NUM", DbType.String, 50),
                new Column("DATE_END_BUILDER", DbType.Date),
                new Column("DATE_START_WORK", DbType.Date),
                new Column("DATE_END_WORK", DbType.Date),
                new Column("DATE_STOP_WORK_GJI", DbType.Date),
                new Column("DATE_CANCEL_REG", DbType.Date),
                new Column("DATE_ACCEPT_GJI", DbType.Date),
                new Column("DATE_ACCEPT_REG", DbType.Date),
                new Column("DATE_GJI_REG", DbType.Date),
                new Column("SUM_DEV_PSD", DbType.Decimal),
                new Column("SUM_SMR", DbType.Decimal),
                new Column("SUM_SMR_APPROVED", DbType.Decimal),
                new Column("SUM_TECH_INSP", DbType.Decimal),
                new Column("FEDERAL_NUM", DbType.String, 50),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_CR_OBJECT_PCR", false, "CR_OBJECT", "PROGRAM_ID");
            Database.AddIndex("IND_CR_OBJECT_RO", false, "CR_OBJECT", "REALITY_OBJECT_ID");
            Database.AddIndex("IND_CR_OBJECT_STATE", false, "CR_OBJECT", "STATE_ID");
            Database.AddForeignKey("FK_CR_OBJECT_PCR", "CR_OBJECT", "PROGRAM_ID", "CR_DICT_PROGRAM", "ID");
            Database.AddForeignKey("FK_CR_OBJECT_RO", "CR_OBJECT", "REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID");
            Database.AddForeignKey("FK_CR_OBJECT_STATE", "CR_OBJECT", "STATE_ID", "B4_STATE", "ID");
            //-----

            //-----Лицевой счет
            Database.AddEntityTable(
                "CR_OBJ_PERS_ACCOUNT",
                new Column("OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("TYPE_FIN_GROUP", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("CLOSED", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("ACCOUNT", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_CR_PERS_ACC_OCR", false, "CR_OBJ_PERS_ACCOUNT", "OBJECT_ID");
            Database.AddForeignKey("FK_CR_PERS_ACC_OCR", "CR_OBJ_PERS_ACCOUNT", "OBJECT_ID", "CR_OBJECT", "ID");
            //-----

            //-----Средства источника финансирования
            Database.AddEntityTable(
                "CR_OBJ_FIN_SOURCE_RES",
                new Column("OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FIN_SOURCE_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("BUDGET_MU", DbType.Decimal),
                new Column("BUDGET_SUB", DbType.Decimal),
                new Column("OWNER_RES", DbType.Decimal),
                new Column("FUND_RES", DbType.Decimal),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_CR_FIN_SOURCE_RES_OCR", false, "CR_OBJ_FIN_SOURCE_RES", "OBJECT_ID");
            Database.AddIndex("IND_CR_FIN_SOURCE_RES_FS", false, "CR_OBJ_FIN_SOURCE_RES", "FIN_SOURCE_ID");
            Database.AddForeignKey("FK_CR_FIN_SOURCE_RES_OCR", "CR_OBJ_FIN_SOURCE_RES", "OBJECT_ID", "CR_OBJECT", "ID");
            Database.AddForeignKey("FK_CR_FIN_SOURCE_RES_FS", "CR_OBJ_FIN_SOURCE_RES", "FIN_SOURCE_ID", "CR_DICT_FIN_SOURCE", "ID");
            //-----

            //-----Этапы работ КР
            Database.AddEntityTable("CR_DICT_STAGE_WORK",
                new Column("NAME", DbType.String, 300),
                new Column("CODE", DbType.String, 50),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_CR_STAGE_WORK_NAME", false, "CR_DICT_STAGE_WORK", "NAME");
            Database.AddIndex("IND_CR_STAGE_WORK_CODE", false, "CR_DICT_STAGE_WORK", "CODE");
            //-----

            //-----Вид работы
            Database.AddEntityTable(
                "CR_OBJ_TYPE_WORK",
                new Column("OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FIN_SOURCE_ID", DbType.Int64, 22),
                new Column("WORK_ID", DbType.Int64, 22),
                new Column("HAS_PSD", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("VOLUME", DbType.Decimal),
                new Column("SUM_MAT", DbType.Decimal),
                new Column("SUM", DbType.Decimal),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("DATE_START_WORK", DbType.Date),
                new Column("DATE_END_WORK", DbType.Date),
                new Column("VOLUME_COMPLETION", DbType.Decimal),
                new Column("FACT_SUM", DbType.Decimal),
                new Column("FACT_VOLUME", DbType.Decimal),
                new Column("MANUFACTURER_NAME", DbType.String, 300),
                new Column("PERCENT_COMPLETION", DbType.Decimal),
                new Column("COST_SUM", DbType.Decimal),
                new Column("COUNT_WORKER", DbType.Decimal),
                new Column("STAGE_WORK_CR_ID", DbType.Int64, 22),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_CR_TYPE_WORK_OCR", false, "CR_OBJ_TYPE_WORK", "OBJECT_ID");
            Database.AddIndex("IND_CR_TYPE_WORK_FS", false, "CR_OBJ_TYPE_WORK", "FIN_SOURCE_ID");
            Database.AddIndex("IND_CR_TYPE_WORK_WORK", false, "CR_OBJ_TYPE_WORK", "WORK_ID");
            Database.AddIndex("IND_CR_TYPE_WORK_SW", false, "CR_OBJ_TYPE_WORK", "STAGE_WORK_CR_ID");
            Database.AddForeignKey("FK_CR_TYPE_WORK_OCR", "CR_OBJ_TYPE_WORK", "OBJECT_ID", "CR_OBJECT", "ID");
            Database.AddForeignKey("FK_CR_TYPE_WORK_FS", "CR_OBJ_TYPE_WORK", "FIN_SOURCE_ID", "CR_DICT_FIN_SOURCE", "ID");
            Database.AddForeignKey("FK_CR_TYPE_WORK_WORK", "CR_OBJ_TYPE_WORK", "WORK_ID", "GKH_DICT_WORK", "ID");
            Database.AddForeignKey("FK_CR_TYPE_WORK_SW", "CR_OBJ_TYPE_WORK", "STAGE_WORK_CR_ID", "CR_DICT_STAGE_WORK", "ID");
            //-----

            //-----Договор подряда
            Database.AddEntityTable(
                "CR_OBJ_BUILD_CONTRACT",
                new Column("STATE_ID", DbType.Int64, 22),
                new Column("OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("INSPECTOR_ID", DbType.Int64, 22),
                new Column("BUILDER_ID", DbType.Int64, 22),
                new Column("DOCUMENT_FILE_ID", DbType.Int64, 22),
                new Column("PROTOCOL_FILE_ID", DbType.Int64, 22),
                new Column("TYPE_CONTRACT_BUILD", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("DATE_START_WORK", DbType.Date),
                new Column("DATE_END_WORK", DbType.Date),
                new Column("DATE_GJI", DbType.Date),
                new Column("DOCUMENT_DATE_FROM", DbType.Date),
                new Column("PROTOCOL_DATE_FROM", DbType.Date),
                new Column("DATE_CANCEL", DbType.Date),
                new Column("DATE_ACCEPT", DbType.Date),
                new Column("DOCUMENT_NAME", DbType.String, 300),
                new Column("PROTOCOL_NAME", DbType.String, 300),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("PROTOCOL_NUM", DbType.String, 50),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("SUM", DbType.Decimal),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_CR_BUILD_CN_OCR", false, "CR_OBJ_BUILD_CONTRACT", "OBJECT_ID");
            Database.AddIndex("IND_CR_BUILD_CN_INSP", false, "CR_OBJ_BUILD_CONTRACT", "INSPECTOR_ID");
            Database.AddIndex("IND_CR_BUILD_CN_BLDR", false, "CR_OBJ_BUILD_CONTRACT", "BUILDER_ID");
            Database.AddIndex("IND_CR_BUILD_CN_DFILE", false, "CR_OBJ_BUILD_CONTRACT", "DOCUMENT_FILE_ID");
            Database.AddIndex("IND_CR_BUILD_CN_PFILE", false, "CR_OBJ_BUILD_CONTRACT", "PROTOCOL_FILE_ID");
            Database.AddIndex("IND_CR_BUILD_CN_STATE", false, "CR_OBJ_BUILD_CONTRACT", "STATE_ID");
            Database.AddForeignKey("FK_CR_BUILD_CN_OCR", "CR_OBJ_BUILD_CONTRACT", "OBJECT_ID", "CR_OBJECT", "ID");
            Database.AddForeignKey("FK_CR_BUILD_CN_INSP", "CR_OBJ_BUILD_CONTRACT", "INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID");
            Database.AddForeignKey("FK_CR_BUILD_CN_BLDR", "CR_OBJ_BUILD_CONTRACT", "BUILDER_ID", "GKH_BUILDER", "ID");
            Database.AddForeignKey("FK_CR_BUILD_CN_DFILE", "CR_OBJ_BUILD_CONTRACT", "DOCUMENT_FILE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_CR_BUILD_CN_PFILE", "CR_OBJ_BUILD_CONTRACT", "PROTOCOL_FILE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_CR_BUILD_CN_STATE", "CR_OBJ_BUILD_CONTRACT", "STATE_ID", "B4_STATE", "ID");
            //-----

            //-----Дефектная ведомость
            Database.AddEntityTable(
                "CR_OBJ_DEFECT_LIST",
                new Column("STATE_ID", DbType.Int64, 22),
                new Column("OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("WORK_ID", DbType.Int64, 22),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("DOCUMENT_NAME", DbType.String, 300),
                new Column("DOCUMENT_DATE", DbType.Date),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_CR_DEFECT_LIST_OCR", false, "CR_OBJ_DEFECT_LIST", "OBJECT_ID");
            Database.AddIndex("IND_CR_DEFECT_LIST_WORK", false, "CR_OBJ_DEFECT_LIST", "WORK_ID");
            Database.AddIndex("IND_CR_DEFECT_LIST_FILE", false, "CR_OBJ_DEFECT_LIST", "FILE_ID");
            Database.AddIndex("IND_CR_DEFECT_LIST_STATE", false, "CR_OBJ_DEFECT_LIST", "STATE_ID");
            Database.AddForeignKey("FK_CR_DEFECT_LIST_OCR", "CR_OBJ_DEFECT_LIST", "OBJECT_ID", "CR_OBJECT", "ID");
            Database.AddForeignKey("FK_CR_DEFECT_LIST_WORK", "CR_OBJ_DEFECT_LIST", "WORK_ID", "GKH_DICT_WORK", "ID");
            Database.AddForeignKey("FK_CR_DEFECT_LIST_FILE", "CR_OBJ_DEFECT_LIST", "FILE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_CR_DEFECT_LIST_STATE", "CR_OBJ_DEFECT_LIST", "STATE_ID", "B4_STATE", "ID");
            //-----

            //-----Протокол(акт)
            Database.AddEntityTable(
                "CR_OBJ_PROTOCOL",
                new Column("STATE_ID", DbType.Int64, 22),
                new Column("OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("CONTRAGENT_ID", DbType.Int64, 22),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("TYPE_DOCUMENT_CR", DbType.Int32, 4),
                new Column("DOCUMENT_NAME", DbType.String, 300),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("COUNT_ACCEPT", DbType.Decimal),
                new Column("COUNT_VOTE", DbType.Decimal),
                new Column("COUNT_VOTE_GENERAL", DbType.Decimal),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("DATE_FROM", DbType.Date),
                new Column("GRADE_OCCUPANT", DbType.Int32),
                new Column("GRADE_CLIENT", DbType.Int32),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_CR_PROTOCOL_OCR", false, "CR_OBJ_PROTOCOL", "OBJECT_ID");
            Database.AddIndex("IND_CR_PROTOCOL_CONTR", false, "CR_OBJ_PROTOCOL", "CONTRAGENT_ID");
            Database.AddIndex("IND_CR_PROTOCOL_FILE", false, "CR_OBJ_PROTOCOL", "FILE_ID");
            Database.AddIndex("IND_CR_PROTOCOL_STATE", false, "CR_OBJ_PROTOCOL", "STATE_ID");
            Database.AddForeignKey("FK_CR_PROTOCOL_OCR", "CR_OBJ_PROTOCOL", "OBJECT_ID", "CR_OBJECT", "ID");
            Database.AddForeignKey("FK_CR_PROTOCOL_CONTR", "CR_OBJ_PROTOCOL", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            Database.AddForeignKey("FK_CR_PROTOCOL_FILE", "CR_OBJ_PROTOCOL", "FILE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_CR_PROTOCOL_STATE", "CR_OBJ_PROTOCOL", "STATE_ID", "B4_STATE", "ID");
            //-----

            //-----Договор объекта КР
            Database.AddEntityTable(
                "CR_OBJ_CONTRACT",
                new Column("STATE_ID", DbType.Int64, 22),
                new Column("OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("CONTRAGENT_ID", DbType.Int64, 22),
                new Column("FIN_SOURCE_ID", DbType.Int64, 22),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("TYPE_CONTRACT_OBJ", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("DOCUMENT_NAME", DbType.String, 300),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("DATE_FROM", DbType.Date),
                new Column("SUM", DbType.Decimal),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_CR_CONTRACT_OCR", false, "CR_OBJ_CONTRACT", "OBJECT_ID");
            Database.AddIndex("IND_CR_CONTRACT_CONTR", false, "CR_OBJ_CONTRACT", "CONTRAGENT_ID");
            Database.AddIndex("IND_CR_CONTRACT_FS", false, "CR_OBJ_CONTRACT", "FIN_SOURCE_ID");
            Database.AddIndex("IND_CR_CONTRACT_FILE", false, "CR_OBJ_CONTRACT", "FILE_ID");
            Database.AddIndex("IND_CR_CONTRACT_STATE", false, "CR_OBJ_CONTRACT", "STATE_ID");
            Database.AddForeignKey("FK_CR_CONTRACT_OCR", "CR_OBJ_CONTRACT", "OBJECT_ID", "CR_OBJECT", "ID");
            Database.AddForeignKey("FK_CR_CONTRACT_CONTR", "CR_OBJ_CONTRACT", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            Database.AddForeignKey("FK_CR_CONTRACT_FS", "CR_OBJ_CONTRACT", "FIN_SOURCE_ID", "CR_DICT_FIN_SOURCE", "ID");
            Database.AddForeignKey("FK_CR_CONTRACT_FILE", "CR_OBJ_CONTRACT", "FILE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_CR_CONTRACT_STATE", "CR_OBJ_CONTRACT", "STATE_ID", "B4_STATE", "ID");
            //-----

            //-----Документ работы объекта КР
            Database.AddEntityTable("CR_OBJ_DOCUMENT_WORK",
                new Column("OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("CONTRAGENT_ID", DbType.Int64, 22),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("DOCUMENT_NAME", DbType.String, 300),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("DATE_FROM", DbType.Date),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_CR_OBJ_DOC_WORK_NAME", false, "CR_OBJ_DOCUMENT_WORK", "DOCUMENT_NAME");
            Database.AddIndex("IND_CR_OBJ_DOC_WORK_OCR", false, "CR_OBJ_DOCUMENT_WORK", "OBJECT_ID");
            Database.AddIndex("IND_CR_OBJ_DOC_WORK_FILE", false, "CR_OBJ_DOCUMENT_WORK", "FILE_ID");
            Database.AddIndex("IND_CR_OBJ_DOC_WORK_CTR", false, "CR_OBJ_DOCUMENT_WORK", "CONTRAGENT_ID");
            Database.AddForeignKey("FK_CR_OBJ_DOC_WORK_OCR", "CR_OBJ_DOCUMENT_WORK", "OBJECT_ID", "CR_OBJECT", "ID");
            Database.AddForeignKey("FK_CR_OBJ_DOC_WORK_FILE", "CR_OBJ_DOCUMENT_WORK", "FILE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_CR_OBJ_DOC_WORK_CTR", "CR_OBJ_DOCUMENT_WORK", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            //-----

            //-----Сметный расчет по работе
            Database.AddEntityTable("CR_OBJ_ESTIMATE_CALC",
                new Column("STATE_ID", DbType.Int64, 22),
                new Column("OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("TYPE_WORK_CR_ID", DbType.Int64, 22),
                new Column("FILE_RES_STATMENT_ID", DbType.Int64, 22),
                new Column("FILE_ESTIMATE_ID", DbType.Int64, 22),
                new Column("FILE_ESTIMATE_FILE_ID", DbType.Int64, 22),
                new Column("RES_STAT_DOC_NAME", DbType.String, 300),
                new Column("RES_STAT_DOC_NUM", DbType.String, 50),
                new Column("RES_STAT_DOC_DATE", DbType.Date),
                new Column("ESTIMATE_DOC_NAME", DbType.String, 300),
                new Column("ESTIMATE_DOC_NUM", DbType.String, 50),
                new Column("ESTIMATE_DOC_DATE", DbType.Date),
                new Column("ESTIMATE_FILE_DOC_NAME", DbType.String, 300),
                new Column("ESTIMATE_FILE_DOC_NUM", DbType.String, 50),
                new Column("ESTIMATE_FILE_DOC_DATE", DbType.Date),
                new Column("NDS", DbType.Decimal),
                new Column("OTHER_COST", DbType.Decimal),
                new Column("TOTAL_ESTIMATE", DbType.Decimal),
                new Column("OVERHEAD_SUM", DbType.Decimal),
                new Column("ESTIMATE_PROFIT", DbType.Decimal),
                new Column("TOTAL_DIRECT_COST", DbType.Decimal),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_CR_OBJ_EST_CALC_RES", false, "CR_OBJ_ESTIMATE_CALC", "RES_STAT_DOC_NAME");
            Database.AddIndex("IND_CR_OBJ_EST_CALC_EST", false, "CR_OBJ_ESTIMATE_CALC", "ESTIMATE_DOC_NAME");
            Database.AddIndex("IND_CR_OBJ_EST_CALC_EF", false, "CR_OBJ_ESTIMATE_CALC", "ESTIMATE_FILE_DOC_NAME");
            Database.AddIndex("IND_CR_OBJ_EST_CALC_OCR", false, "CR_OBJ_ESTIMATE_CALC", "OBJECT_ID");
            Database.AddIndex("IND_CR_OBJ_EST_CALC_RES", false, "CR_OBJ_ESTIMATE_CALC", "FILE_RES_STATMENT_ID");
            Database.AddIndex("IND_CR_OBJ_EST_CALC_EST", false, "CR_OBJ_ESTIMATE_CALC", "FILE_ESTIMATE_ID");
            Database.AddIndex("IND_CR_OBJ_EST_CALC_EF", false, "CR_OBJ_ESTIMATE_CALC", "FILE_ESTIMATE_FILE_ID");
            Database.AddIndex("IND_CR_OBJ_EST_CALC_ST", false, "CR_OBJ_ESTIMATE_CALC", "STATE_ID");
            Database.AddForeignKey("FK_CR_OBJ_EST_CALC_OCR", "CR_OBJ_ESTIMATE_CALC", "OBJECT_ID", "CR_OBJECT", "ID");
            Database.AddForeignKey("FK_CR_OBJ_EST_CALC_RES", "CR_OBJ_ESTIMATE_CALC", "FILE_RES_STATMENT_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_CR_OBJ_EST_CALC_EST", "CR_OBJ_ESTIMATE_CALC", "FILE_ESTIMATE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_CR_OBJ_EST_CALC_EF", "CR_OBJ_ESTIMATE_CALC", "FILE_ESTIMATE_FILE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_CR_OBJ_EST_CALC_ST", "CR_OBJ_ESTIMATE_CALC", "STATE_ID", "B4_STATE", "ID");
            //-----

            //-----Смета
            Database.AddEntityTable("CR_EST_CALC_ESTIMATE",
                new Column("ESTIMATE_CALC_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("UNIT_MEASURE_ID", DbType.Int64, 22),
                new Column("DOCUMENT_NAME", DbType.String, 300),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("REASON", DbType.String, 500),
                new Column("BASE_SALARY", DbType.Decimal),
                new Column("MECH_WORK", DbType.Decimal),
                new Column("BASE_WORK", DbType.Decimal),
                new Column("TOTAL_COUNT", DbType.Decimal),
                new Column("TOTAL_COST", DbType.Decimal),
                new Column("ON_UNIT_COUNT", DbType.Decimal),
                new Column("ON_UNIT_COST", DbType.Decimal),
                new Column("MAT_COST", DbType.Decimal),
                new Column("MACHINE_OPERATING_COST", DbType.Decimal),
                new Column("MECH_SALARY", DbType.Decimal),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_CR_EST_CAL_EST_NAME", false, "CR_EST_CALC_ESTIMATE", "DOCUMENT_NAME");
            Database.AddIndex("IND_CR_EST_CAL_EST_EC", false, "CR_EST_CALC_ESTIMATE", "ESTIMATE_CALC_ID");
            Database.AddIndex("IND_CR_EST_CAL_EST_UM", false, "CR_EST_CALC_ESTIMATE", "UNIT_MEASURE_ID");
            Database.AddForeignKey("FK_CR_EST_CAL_EST_EC", "CR_EST_CALC_ESTIMATE", "ESTIMATE_CALC_ID", "CR_OBJ_ESTIMATE_CALC", "ID");
            Database.AddForeignKey("FK_CR_EST_CAL_EST_UM", "CR_EST_CALC_ESTIMATE", "UNIT_MEASURE_ID", "GKH_DICT_UNITMEASURE", "ID");
            //-----

            //-----Ведомость ресурсов
            Database.AddEntityTable("CR_EST_CALC_RES_STATEM",
                new Column("ESTIMATE_CALC_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("UNIT_MEASURE_ID", DbType.Int64, 22),
                new Column("DOCUMENT_NAME", DbType.String, 300),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("REASON", DbType.String, 300),
                new Column("TOTAL_COUNT", DbType.Decimal),
                new Column("TOTAL_COST", DbType.Decimal),
                new Column("ON_UNIT_COUNT", DbType.Decimal),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_CR_EST_CAL_RES_NAME", false, "CR_EST_CALC_RES_STATEM", "DOCUMENT_NAME");
            Database.AddIndex("IND_CR_EST_CAL_RES_EC", false, "CR_EST_CALC_RES_STATEM", "ESTIMATE_CALC_ID");
            Database.AddIndex("IND_CR_EST_CAL_RES_UM", false, "CR_EST_CALC_RES_STATEM", "UNIT_MEASURE_ID");
            Database.AddForeignKey("FK_CR_EST_CAL_RES_EC", "CR_EST_CALC_RES_STATEM", "ESTIMATE_CALC_ID", "CR_OBJ_ESTIMATE_CALC", "ID");
            Database.AddForeignKey("FK_CR_EST_CAL_RES_UM", "CR_EST_CALC_RES_STATEM", "UNIT_MEASURE_ID", "GKH_DICT_UNITMEASURE", "ID");
            //-----

            //-----Акт выполненных работ мониторинга СМР
            Database.AddEntityTable("CR_OBJ_PERFOMED_WORK_ACT",
                new Column("STATE_ID", DbType.Int64, 22),
                new Column("OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("TYPE_WORK_CR_ID", DbType.Int64, 22),
                new Column("VOLUME", DbType.Decimal),
                new Column("SUM", DbType.Decimal),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("DATE_FROM", DbType.Date),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_CR_OBJ_PER_WORK_AC_OCR", false, "CR_OBJ_PERFOMED_WORK_ACT", "OBJECT_ID");
            Database.AddIndex("IND_CR_OBJ_PER_WORK_AC_W", false, "CR_OBJ_PERFOMED_WORK_ACT", "TYPE_WORK_CR_ID");
            Database.AddIndex("IND_CR_OBJ_PER_WORK_AC_ST", false, "CR_OBJ_PERFOMED_WORK_ACT", "STATE_ID");
            Database.AddForeignKey("FK_CR_OBJ_PER_WORK_AC_OCR", "CR_OBJ_PERFOMED_WORK_ACT", "OBJECT_ID", "CR_OBJECT", "ID");
            Database.AddForeignKey("FK_CR_OBJ_PER_WORK_AC_W", "CR_OBJ_PERFOMED_WORK_ACT", "TYPE_WORK_CR_ID", "CR_OBJ_TYPE_WORK", "ID");
            Database.AddForeignKey("FK_CR_OBJ_PER_WORK_AC_ST", "CR_OBJ_PERFOMED_WORK_ACT", "STATE_ID", "B4_STATE", "ID");
            //-----

            //----- Запись акта выполненных работ мониторинга СМР
            Database.AddEntityTable("CR_OBJ_PERFOMED_WACT_REC",
                new Column("PERFOMED_ACT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("UNIT_MEASURE_ID", DbType.Int64, 22),
                new Column("DOCUMENT_NAME", DbType.String, 300),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("REASON", DbType.String, 500),
                new Column("BASE_SALARY", DbType.Decimal),
                new Column("MECH_WORK", DbType.Decimal),
                new Column("BASE_WORK", DbType.Decimal),
                new Column("TOTAL_COUNT", DbType.Decimal),
                new Column("TOTAL_COST", DbType.Decimal),
                new Column("ON_UNIT_COUNT", DbType.Decimal),
                new Column("ON_UNIT_COST", DbType.Decimal),
                new Column("MAT_COST", DbType.Decimal),
                new Column("MACHINE_OPERATING_COST", DbType.Decimal),
                new Column("MECH_SALARY", DbType.Decimal),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_CR_OBJ_PER_WAC_REC_PACT", false, "CR_OBJ_PERFOMED_WACT_REC", "PERFOMED_ACT_ID");
            Database.AddIndex("IND_CR_OBJ_PER_WAC_REC_UM", false, "CR_OBJ_PERFOMED_WACT_REC", "UNIT_MEASURE_ID");
            Database.AddIndex("IND_CR_OBJ_PERF_WACT_REC_NAME", false, "CR_OBJ_PERFOMED_WACT_REC", "DOCUMENT_NAME");
            Database.AddForeignKey("FK_CR_OBJ_PER_WAC_REC_PACT", "CR_OBJ_PERFOMED_WACT_REC", "PERFOMED_ACT_ID", "CR_OBJ_PERFOMED_WORK_ACT", "ID");
            Database.AddForeignKey("FK_CR_OBJ_PER_WAC_REC_UM", "CR_OBJ_PERFOMED_WACT_REC", "UNIT_MEASURE_ID", "GKH_DICT_UNITMEASURE", "ID");
            //-----

            //-----Банковская выписка
            Database.AddEntityTable("CR_OBJ_BANK_STATEMENT",
                new Column("OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("MANAG_ORG_ID", DbType.Int64, 22),
                new Column("CONTRAGENT_ID", DbType.Int64, 22),
                new Column("PERIOD_ID", DbType.Int64, 22),
                new Column("TYPE_FIN_GROUP", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("BUDGET_YEAR", DbType.Int32, 4),
                new Column("INCOMING_BALANCE", DbType.Decimal),
                new Column("OUTGOING_BALANCE", DbType.Decimal),
                new Column("PERSONAL_ACCOUNT", DbType.String, 50),
                new Column("OPER_LAST_DATE", DbType.Date),
                new Column("DOCUMENT_DATE", DbType.Date),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_CR_BANK_ST_OCR", false, "CR_OBJ_BANK_STATEMENT", "OBJECT_ID");
            Database.AddIndex("IND_CR_BANK_ST_MORG", false, "CR_OBJ_BANK_STATEMENT", "MANAG_ORG_ID");
            Database.AddIndex("IND_CR_BANK_ST_CTR", false, "CR_OBJ_BANK_STATEMENT", "CONTRAGENT_ID");
            Database.AddIndex("IND_CR_BANK_ST_PER", false, "CR_OBJ_BANK_STATEMENT", "PERIOD_ID");
            Database.AddForeignKey("FK_CR_BANK_ST_OCR", "CR_OBJ_BANK_STATEMENT", "OBJECT_ID", "CR_OBJECT", "ID");
            Database.AddForeignKey("FK_CR_BANK_ST_MORG", "CR_OBJ_BANK_STATEMENT", "MANAG_ORG_ID", "GKH_MANAGING_ORGANIZATION", "ID");
            Database.AddForeignKey("FK_CR_BANK_ST_CTR", "CR_OBJ_BANK_STATEMENT", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            Database.AddForeignKey("FK_CR_BANK_ST_PER", "CR_OBJ_BANK_STATEMENT", "PERIOD_ID", "GKH_DICT_PERIOD", "ID");
            //-----

            //-----Базовое платежное поручение
            Database.AddEntityTable("CR_PAYMENT_ORDER",
                new Column("BANK_STATEMENT_ID", DbType.Int64, 22),
                new Column("FIN_SOURCE_ID", DbType.Int64, 22),
                new Column("PAYER_CONTRAGENT_ID", DbType.Int64, 22),
                new Column("RECEIVER_CONTRAGENT_ID", DbType.Int64, 22),
                new Column("TYPE_PAYMENT_ORDER", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("PAY_PURPOSE", DbType.String, 300),
                new Column("BID_NUM", DbType.String, 50),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("BID_DATE", DbType.Date),
                new Column("DOCUMENT_DATE", DbType.Date),
                new Column("SUM", DbType.Decimal),
                new Column("REDIRECT_FUNDS", DbType.Decimal), 
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_CR_PAYMENT_BSTAT", false, "CR_PAYMENT_ORDER", "BANK_STATEMENT_ID");
            Database.AddIndex("IND_CR_PAYMENT_FS", false, "CR_PAYMENT_ORDER", "FIN_SOURCE_ID");
            Database.AddIndex("IND_CR_PAYMENT_PCONTR", false, "CR_PAYMENT_ORDER", "PAYER_CONTRAGENT_ID");
            Database.AddIndex("IND_CR_PAYMENT_RCONTR", false, "CR_PAYMENT_ORDER", "RECEIVER_CONTRAGENT_ID");
            Database.AddForeignKey("FK_CR_PAYMENT_BSTAT", "CR_PAYMENT_ORDER", "BANK_STATEMENT_ID", "CR_OBJ_BANK_STATEMENT", "ID");
            Database.AddForeignKey("FK_CR_PAYMENT_FS", "CR_PAYMENT_ORDER", "FIN_SOURCE_ID", "CR_DICT_FIN_SOURCE", "ID");
            Database.AddForeignKey("FK_CR_PAYMENT_PCONTR", "CR_PAYMENT_ORDER", "PAYER_CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            Database.AddForeignKey("FK_CR_PAYMENT_RCONTR", "CR_PAYMENT_ORDER", "RECEIVER_CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            //-----

            //-----Платежное поручение приход
            Database.AddTable("CR_PAYMENT_ORDER_IN",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique));
            Database.AddForeignKey("FK_CR_PAYMENT_ORDER_IN", "CR_PAYMENT_ORDER_IN", "ID", "CR_PAYMENT_ORDER", "ID");
            //-----

            //-----Платежное поручение расход
            Database.AddTable("CR_PAYMENT_ORDER_OUT",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique));
            Database.AddForeignKey("FK_CR_PAYMENT_ORDER_OUT", "CR_PAYMENT_ORDER_OUT", "ID", "CR_PAYMENT_ORDER", "ID");
            //-----

            //-----Участник квалификационного отбора
            Database.AddEntityTable("CR_DICT_QUAL_MEMBER",
                new Column("PERIOD_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("NAME", DbType.String, 300),
                new Column("IS_PRIMARY", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_CR_QUAL_MEM_PER", false, "CR_DICT_QUAL_MEMBER", "PERIOD_ID");
            Database.AddIndex("IND_CR_QUAL_MEM_NAME", false, "CR_DICT_QUAL_MEMBER", "NAME");
            Database.AddForeignKey("FK_CR_QUAL_MEM_PER", "CR_DICT_QUAL_MEMBER", "PERIOD_ID", "GKH_DICT_PERIOD", "ID");
            //-----

            //-----Квалификационный отбор
            Database.AddEntityTable("CR_QUALIFICATION",
                new Column("OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("BUILDER_ID",DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("SUM", DbType.Decimal),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_CR_QUAL_OCR", false, "CR_QUALIFICATION", "OBJECT_ID");
            Database.AddIndex("IND_CR_QUAL_BUILDER", false, "CR_QUALIFICATION", "BUILDER_ID");
            Database.AddForeignKey("FK_CR_QUAL_OCR", "CR_QUALIFICATION", "OBJECT_ID", "CR_OBJECT", "ID");
            Database.AddForeignKey("FK_CR_QUAL_BUILDER", "CR_QUALIFICATION", "BUILDER_ID", "GKH_BUILDER", "ID");
            //-----

            //-----Голос участника квалификационного отбора
            Database.AddEntityTable("CR_VOICE_QUAL_MEMBER",
                new Column("QUALIFICATION_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("QUAL_MEMBER_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("TYPE_ACCEPT_QUAL", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("REASON", DbType.String, 300),
                new Column("DOCUMENT_DATE", DbType.Date),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_CR_VQUALMEM_QUAL", false, "CR_VOICE_QUAL_MEMBER", "QUALIFICATION_ID");
            Database.AddIndex("IND_CR_VQUALMEM_QUALMEM", false, "CR_VOICE_QUAL_MEMBER", "QUAL_MEMBER_ID");
            Database.AddForeignKey("FK_CR_VQUALMEM_QUAL", "CR_VOICE_QUAL_MEMBER", "QUALIFICATION_ID", "CR_QUALIFICATION", "ID");
            Database.AddForeignKey("FK_CR_VQUALMEM_QUALMEM", "CR_VOICE_QUAL_MEMBER", "QUAL_MEMBER_ID", "CR_DICT_QUAL_MEMBER", "ID");
            //-----

            //-----Этапы работы капремонта
            Database.AddEntityTable("CR_DICT_WORK_STAGE_WORK",
                new Column("WORK_ID",DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("STAGE_WORK_ID",DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_CR_WORK_STG_W", false, "CR_DICT_WORK_STAGE_WORK", "WORK_ID");
            Database.AddIndex("IND_CR_WORK_STG_SW", false, "CR_DICT_WORK_STAGE_WORK", "STAGE_WORK_ID");
            Database.AddForeignKey("FK_CR_WORK_STG_W", "CR_DICT_WORK_STAGE_WORK", "WORK_ID", "GKH_DICT_WORK", "ID");
            Database.AddForeignKey("FK_CR_WORK_STG_SW", "CR_DICT_WORK_STAGE_WORK", "STAGE_WORK_ID", "CR_DICT_STAGE_WORK", "ID");
            //-----

            //-----Mониторинг СМР
            Database.AddEntityTable("CR_OBJ_MONITORING_CMP",
                new Column("OBJECT_ID", DbType.Int64, 22),
                new Column("STATE_ID", DbType.Int64, 22),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_CR_OBJ_MON_CMP_OBJ", false, "CR_OBJ_MONITORING_CMP", "OBJECT_ID");
            Database.AddIndex("IND_CR_OBJ_MON_CMP_ST", false, "CR_OBJ_MONITORING_CMP", "STATE_ID");
            Database.AddForeignKey("FK_CR_OBJ_MON_CMP_OBJ", "CR_OBJ_MONITORING_CMP", "OBJECT_ID", "CR_OBJECT", "ID");
            Database.AddForeignKey("FK_CR_OBJ_MON_CMP_ST", "CR_OBJ_MONITORING_CMP", "STATE_ID", "B4_STATE", "ID");
            //-----
        }

        public override void Down()
        {
            Database.RemoveConstraint("CR_OBJ_MONITORING_CMP", "FK_CR_OBJ_MON_CMP_OBJ");
            Database.RemoveConstraint("CR_OBJ_MONITORING_CMP", "FK_CR_OBJ_MON_CMP_ST");
            Database.RemoveConstraint("CR_OBJECT", "FK_CR_OBJECT_PCR");
            Database.RemoveConstraint("CR_OBJECT", "FK_CR_OBJECT_RO");
            Database.RemoveConstraint("CR_OBJECT", "FK_CR_OBJECT_STATE");
            Database.RemoveConstraint("CR_DICT_PROGRAM", "FK_CR_PROG_PER");
            Database.RemoveConstraint("CR_DICT_PROGCR_FIN_SOURCE", "FK_CR_PROG_FIN_SRC_PR");
            Database.RemoveConstraint("CR_DICT_PROGCR_FIN_SOURCE", "FK_CR_PROG_FIN_SRC_FS");
            Database.RemoveConstraint("CR_DICT_FIN_SOURCE_WORK", "FK_CR_FINS_WORK_WORK");
            Database.RemoveConstraint("CR_DICT_FIN_SOURCE_WORK", "FK_CR_FINS_WORK_FS");
            Database.RemoveConstraint("CR_OBJ_PERS_ACCOUNT", "FK_CR_PERS_ACC_OCR");
            Database.RemoveConstraint("CR_OBJ_FIN_SOURCE_RES", "FK_CR_FIN_SOURCE_RES_OCR");
            Database.RemoveConstraint("CR_OBJ_FIN_SOURCE_RES", "FK_CR_FIN_SOURCE_RES_FS");
            Database.RemoveConstraint("CR_OBJ_TYPE_WORK", "FK_CR_TYPE_WORK_OCR");
            Database.RemoveConstraint("CR_OBJ_TYPE_WORK", "FK_CR_TYPE_WORK_FS");
            Database.RemoveConstraint("CR_OBJ_TYPE_WORK", "FK_CR_TYPE_WORK_WORK");
            Database.RemoveConstraint("CR_OBJ_TYPE_WORK", "FK_CR_TYPE_WORK_SW");
            Database.RemoveConstraint("CR_OBJ_BUILD_CONTRACT", "FK_CR_BUILD_CN_OCR");
            Database.RemoveConstraint("CR_OBJ_BUILD_CONTRACT", "FK_CR_BUILD_CN_INSP");
            Database.RemoveConstraint("CR_OBJ_BUILD_CONTRACT", "FK_CR_BUILD_CN_BLDR");
            Database.RemoveConstraint("CR_OBJ_BUILD_CONTRACT", "FK_CR_BUILD_CN_DFILE");
            Database.RemoveConstraint("CR_OBJ_BUILD_CONTRACT", "FK_CR_BUILD_CN_PFILE");
            Database.RemoveConstraint("CR_OBJ_BUILD_CONTRACT", "FK_CR_BUILD_CN_STATE");
            Database.RemoveConstraint("CR_OBJ_DEFECT_LIST", "FK_CR_DEFECT_LIST_OCR");
            Database.RemoveConstraint("CR_OBJ_DEFECT_LIST", "FK_CR_DEFECT_LIST_WORK");
            Database.RemoveConstraint("CR_OBJ_DEFECT_LIST", "FK_CR_DEFECT_LIST_FILE");
            Database.RemoveConstraint("CR_OBJ_DEFECT_LIST", "FK_CR_DEFECT_LIST_STATE");
            Database.RemoveConstraint("CR_OBJ_PROTOCOL", "FK_CR_PROTOCOL_OCR");
            Database.RemoveConstraint("CR_OBJ_PROTOCOL", "FK_CR_PROTOCOL_CONTR");
            Database.RemoveConstraint("CR_OBJ_PROTOCOL", "FK_CR_PROTOCOL_FILE");
            Database.RemoveConstraint("CR_OBJ_CONTRACT", "FK_CR_CONTRACT_OCR");
            Database.RemoveConstraint("CR_OBJ_CONTRACT", "FK_CR_CONTRACT_CONTR");
            Database.RemoveConstraint("CR_OBJ_CONTRACT", "FK_CR_CONTRACT_FS");
            Database.RemoveConstraint("CR_OBJ_CONTRACT", "FK_CR_CONTRACT_FILE");
            Database.RemoveConstraint("CR_OBJ_CONTRACT", "FK_CR_CONTRACT_STATE");
            Database.RemoveConstraint("CR_OBJ_DOCUMENT_WORK", "FK_CR_OBJ_DOC_WORK_OCR");
            Database.RemoveConstraint("CR_OBJ_DOCUMENT_WORK", "FK_CR_OBJ_DOC_WORK_FILE");
            Database.RemoveConstraint("CR_OBJ_DOCUMENT_WORK", "FK_CR_OBJ_DOC_WORK_CTR");
            Database.RemoveConstraint("CR_EST_CALC_ESTIMATE", "FK_CR_EST_CAL_EST_EC");
            Database.RemoveConstraint("CR_EST_CALC_ESTIMATE", "FK_CR_EST_CAL_EST_UM");
            Database.RemoveConstraint("CR_EST_CALC_RES_STATEM", "FK_CR_EST_CAL_RES_EC");
            Database.RemoveConstraint("CR_EST_CALC_RES_STATEM", "FK_CR_EST_CAL_RES_UM");
            Database.RemoveConstraint("CR_OBJ_ESTIMATE_CALC", "FK_CR_OBJ_EST_CALC_OCR");
            Database.RemoveConstraint("CR_OBJ_ESTIMATE_CALC", "FK_CR_OBJ_EST_CALC_RES");
            Database.RemoveConstraint("CR_OBJ_ESTIMATE_CALC", "FK_CR_OBJ_EST_CALC_EST");
            Database.RemoveConstraint("CR_OBJ_ESTIMATE_CALC", "FK_CR_OBJ_EST_CALC_EF");
            Database.RemoveConstraint("CR_OBJ_ESTIMATE_CALC", "FK_CR_OBJ_EST_CALC_ST");
            Database.RemoveConstraint("CR_OBJ_PERFOMED_WORK_ACT", "FK_CR_OBJ_PER_WORK_AC_ST");
            Database.RemoveConstraint("CR_OBJ_PERFOMED_WORK_ACT", "FK_CR_OBJ_PER_WORK_AC_OCR");
            Database.RemoveConstraint("CR_OBJ_PERFOMED_WORK_ACT", "FK_CR_OBJ_PER_WORK_AC_W");
            Database.RemoveConstraint("CR_OBJ_PERFOMED_WACT_REC", "FK_CR_OBJ_PER_WAC_REC_PACT");
            Database.RemoveConstraint("CR_OBJ_PERFOMED_WACT_REC", "FK_CR_OBJ_PER_WAC_REC_UM");
            Database.RemoveConstraint("CR_OBJ_BANK_STATEMENT", "FK_CR_BANK_ST_OCR");
            Database.RemoveConstraint("CR_OBJ_BANK_STATEMENT", "FK_CR_BANK_ST_MORG");
            Database.RemoveConstraint("CR_OBJ_BANK_STATEMENT", "FK_CR_BANK_ST_CTR");
            Database.RemoveConstraint("CR_OBJ_BANK_STATEMENT", "FK_CR_BANK_ST_PER");
            Database.RemoveConstraint("CR_PAYMENT_ORDER", "FK_CR_PAYMENT_BSTAT");
            Database.RemoveConstraint("CR_PAYMENT_ORDER", "FK_CR_PAYMENT_FS");
            Database.RemoveConstraint("CR_PAYMENT_ORDER", "FK_CR_PAYMENT_PCONTR");
            Database.RemoveConstraint("CR_PAYMENT_ORDER", "FK_CR_PAYMENT_RCONTR");
            Database.RemoveConstraint("CR_PAYMENT_ORDER_IN", "FK_CR_PAYMENT_ORDER_IN");
            Database.RemoveConstraint("CR_PAYMENT_ORDER_OUT", "FK_CR_PAYMENT_ORDER_OUT");
            Database.RemoveConstraint("CR_DICT_QUAL_MEMBER", "FK_CR_QUAL_MEM_PER");
            Database.RemoveConstraint("CR_QUALIFICATION", "FK_CR_QUAL_OCR");
            Database.RemoveConstraint("CR_QUALIFICATION", "FK_CR_QUAL_BUILDER");
            Database.RemoveConstraint("CR_VOICE_QUAL_MEMBER", "FK_CR_VQUALMEM_QUAL");
            Database.RemoveConstraint("CR_VOICE_QUAL_MEMBER", "FK_CR_VQUALMEM_QUALMEM");
            Database.RemoveConstraint("CR_DICT_WORK_STAGE_WORK", "FK_CR_WORK_STG_W");
            Database.RemoveConstraint("CR_DICT_WORK_STAGE_WORK", "FK_CR_WORK_STG_SW");

            Database.RemoveTable("CR_DICT_QUAL_MEMBER");
            Database.RemoveTable("CR_QUALIFICATION");
            Database.RemoveTable("CR_VOICE_QUAL_MEMBER");
            Database.RemoveTable("CR_OBJECT");
            Database.RemoveTable("CR_DICT_PROGRAM");
            Database.RemoveTable("CR_DICT_FIN_SOURCE");
            Database.RemoveTable("CR_DICT_PROGCR_FIN_SOURCE");
            Database.RemoveTable("CR_DICT_FIN_SOURCE_WORK");
            Database.RemoveTable("CR_OBJ_PERS_ACCOUNT");
            Database.RemoveTable("CR_OBJ_FIN_SOURCE_RES");
            Database.RemoveTable("CR_OBJ_TYPE_WORK");
            Database.RemoveTable("CR_OBJ_BUILD_CONTRACT");
            Database.RemoveTable("CR_OBJ_QUALIFICATION");
            Database.RemoveTable("CR_OBJ_DEFECT_LIST");
            Database.RemoveTable("CR_OBJ_PROTOCOL");
            Database.RemoveTable("CR_OBJ_CONTRACT");
            Database.RemoveTable("CR_OBJ_DOCUMENT_WORK");
            Database.RemoveTable("CR_EST_CALC_ESTIMATE");
            Database.RemoveTable("CR_EST_CALC_RES_STATEM");
            Database.RemoveTable("CR_OBJ_ESTIMATE_CALC");
            Database.RemoveTable("CR_OBJ_PERFOMED_WORK_ACT");
            Database.RemoveTable("CR_OBJ_PERFOMED_WACT_REC");
            Database.RemoveTable("CR_OBJ_BANK_STATEMENT");
            Database.RemoveTable("CR_PAYMENT_ORDER");
            Database.RemoveTable("CR_PAYMENT_ORDER_IN");
            Database.RemoveTable("CR_PAYMENT_ORDER_OUT");
            Database.RemoveTable("CR_DICT_WORK_STAGE_WORK");
            Database.RemoveTable("CR_OBJ_MONITORING_CMP");
            Database.RemoveTable("cr_dict_stage_work");
        }
    }
}