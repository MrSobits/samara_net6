// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateSchema.cs" company="">
//   
// </copyright>
// <summary>
//   The update schema.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bars.GkhDi.Migrations.Version_1
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----Периодичность услуги
            Database.AddEntityTable(
                "DI_DICT_PERIODICITY",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 50),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_PERCITY_NAME", false, "DI_DICT_PERIODICITY", "NAME");
            Database.AddIndex("IND_DI_PERCITY_CODE", false, "DI_DICT_PERIODICITY", "CODE");
            //-----

            //-----Контролирующие органы
            Database.AddEntityTable("DI_DICT_SUPERVISORY_ORG",
                new Column("NAME", DbType.String, 300),
                new Column("CODE", DbType.String, 50),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_SUPERV_ORG_NAME", false, "DI_DICT_SUPERVISORY_ORG", "NAME");
            Database.AddIndex("IND_DI_SUPERV_ORG_CODE", false, "DI_DICT_SUPERVISORY_ORG", "CODE");
            //----

            //-----Система налогооблажения
            Database.AddEntityTable(
                "DI_DICT_TAX_SYSTEM",
                new Column("NAME", DbType.String, 300),
                new Column("SHORT_NAME", DbType.String, 50),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_TAX_SYS_NAME", false, "DI_DICT_TAX_SYSTEM", "NAME");
            //-----

            //-----Период раскрытия информации
            Database.AddEntityTable(
                "DI_DICT_PERIOD",
                new Column("NAME", DbType.String, 300),
                new Column("DATE_START", DbType.Date),
                new Column("DATE_END", DbType.Date), 
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_PERIOD_NAME", false, "DI_DICT_PERIOD", "NAME");
            //-----

            //-----Деятельность раскрытия информации
            Database.AddEntityTable(
                "DI_DISINFO",
                new Column("MANAG_ORG_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("PERIOD_DI_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_FUND_WITHOUT", DbType.Int64, 22),
                new Column("ADMIN_PERSONNEL", DbType.Int32),
                new Column("ENGINEER", DbType.Int32),
                new Column("WORK", DbType.Int32),
                new Column("DISMISSED_ADMIN_PERS", DbType.Int32),
                new Column("DISMISSED_ENGINEER", DbType.Int32),
                new Column("DISMISSED_WORK", DbType.Int32),
                new Column("UNHAPPY_EVENT_COUNT", DbType.Int32),
                new Column("TERMINATE_CONTRACT", DbType.Int32, ColumnProperty.NotNull, 30),
                new Column("MEMBERSHIP_UNIONS", DbType.Int32, ColumnProperty.NotNull, 30),
                new Column("FUNDS_INFO", DbType.Int32, ColumnProperty.NotNull, 30),
                new Column("ADMIN_RESPONSE", DbType.Int32, ColumnProperty.NotNull, 30),
                new Column("SIZE_PAYMENTS", DbType.Decimal),
                new Column("CONTRACT_AVAIL", DbType.Int32, 4, ColumnProperty.NotNull, 30), 
                new Column("NUMBER_CONTRACTS", DbType.Int32));
            Database.AddIndex("IND_DI_DISINFO_MORG", false, "DI_DISINFO", "MANAG_ORG_ID");
            Database.AddIndex("IND_DI_DISINFO_PERDI", false, "DI_DISINFO", "PERIOD_DI_ID");
            Database.AddIndex("IND_DI_DISINFO_FFUND", false, "DI_DISINFO", "FILE_FUND_WITHOUT");
            Database.AddForeignKey("FK_DI_DISINFO_FFUND", "DI_DISINFO", "FILE_FUND_WITHOUT", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_DI_DISINFO_PERDI", "DI_DISINFO", "PERIOD_DI_ID", "DI_DICT_PERIOD", "ID");
            Database.AddForeignKey("FK_DI_DISINFO_MORG", "DI_DISINFO", "MANAG_ORG_ID", "GKH_MANAGING_ORGANIZATION", "ID");
            //-----

            //-----Раскрытие информации в доме
            Database.AddEntityTable(
                "DI_DISINFO_REALOBJ",
                new Column("REALITY_OBJ_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("PERIOD_DI_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("NON_RESIDENT_PLACE", DbType.Int32, 4, ColumnProperty.NotNull, 30),
                new Column("PLACE_GENERAL_USE", DbType.Int32, 4, ColumnProperty.NotNull, 30),
                new Column("CLAIM_COMPENSATION", DbType.Decimal),
                new Column("CLAIM_NON_SERVICE", DbType.Decimal),
                new Column("CLAIM_NON_DELIVERY", DbType.Decimal),
                new Column("EXECUTION_WORK", DbType.String, 500),
                new Column("EXECUTION_OBLIGATION", DbType.String, 500),
                new Column("WORK_REPAIR", DbType.Decimal),
                new Column("WORK_LANDSCAPING", DbType.Decimal),
                new Column("SUBSIDIES", DbType.Decimal),
                new Column("CREDIT", DbType.Decimal),
                new Column("FINANCE_LEASING", DbType.Decimal),
                new Column("FINANCE_ENERGY", DbType.Decimal),
                new Column("OCCUPANT_CONTRIB", DbType.Decimal),
                new Column("OTHER_SOURCE", DbType.Decimal),
                new Column("REDUCTION_PAYMENT", DbType.Int32, ColumnProperty.NotNull, 30),
                new Column("DESCR_CATREP_SERV", DbType.String, 1500),
                new Column("DESCR_CATREP_TARIF", DbType.String, 1500));
            Database.AddIndex("IND_DI_DISINFRO_PERIOD", false, "DI_DISINFO_REALOBJ", "PERIOD_DI_ID");
            Database.AddIndex("IND_DI_DISINFO_RO_RO", false, "DI_DISINFO_REALOBJ", "REALITY_OBJ_ID");
            Database.AddForeignKey("FK_DI_DISINFO_RO_RO", "DI_DISINFO_REALOBJ", "REALITY_OBJ_ID", "GKH_REALITY_OBJECT", "ID");
            Database.AddForeignKey("FK_DI_DISINFRO_PERIOD", "DI_DISINFO_REALOBJ", "PERIOD_DI_ID", "DI_DICT_PERIOD", "ID");
            //-----

            // Таблица связи сведений об УО и раскрытия инф-ии в доме
            Database.AddEntityTable(
                "DI_DISINFO_RELATION",
                new Column("DISINFO_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("DISINFO_RO_ID", DbType.Int64, 22, ColumnProperty.NotNull));
            Database.AddIndex("IND_DI_DISINFO_REL_DI", false, "DI_DISINFO_RELATION", "DISINFO_ID");
            Database.AddIndex("IND_DI_DISINFO_REL_RO", false, "DI_DISINFO_RELATION", "DISINFO_RO_ID");
            Database.AddForeignKey("FK_DI_DISINFO_REL_RO", "DI_DISINFO_RELATION", "DISINFO_RO_ID", "DI_DISINFO_REALOBJ", "ID");
            Database.AddForeignKey("FK_DI_DISINFO_REL_DI", "DI_DISINFO_RELATION", "DISINFO_ID", "DI_DISINFO", "ID");
            //------

            //-----Сведения о фондах
            Database.AddEntityTable(
                "DI_DISINFO_FUNDS",
                new Column("DISINFO_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("DOCUMENT_NAME", DbType.String, 300),
                new Column("DOCUMENT_DATE", DbType.Date),
                new Column("SIZE_INFO", DbType.Decimal), 
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_DISINFO_FUN_DI", false, "DI_DISINFO_FUNDS", "DISINFO_ID");
            Database.AddForeignKey("FK_DI_DISINFO_FUN_DI", "DI_DISINFO_FUNDS", "DISINFO_ID", "DI_DISINFO", "ID");
            //-----

            //-----Общие сведения финансовой деятельности
            Database.AddEntityTable(
                "DI_DISINFO_FIN_ACTIVITY",
                new Column("VALUE_BLANK_ACTIVE", DbType.Decimal),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("CLAIM_DAMAGE", DbType.Decimal),
                new Column("FAILURE_SERVICE", DbType.Decimal),
                new Column("NON_DELIVERY_SERVICE", DbType.Decimal),
                new Column("DISINFO_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("BOOKKEEP_BALANCE", DbType.Int64, 22),
                new Column("BOOKKEEP_BALANCE_ANNEX", DbType.Int64, 22),
                new Column("TAX_SYSTEM_ID", DbType.Int64, 22),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_DISINFO_FA_DI", false, "DI_DISINFO_FIN_ACTIVITY", "DISINFO_ID");
            Database.AddIndex("IND_DI_DISINFO_FA_BB", false, "DI_DISINFO_FIN_ACTIVITY", "BOOKKEEP_BALANCE");
            Database.AddIndex("IND_DI_DISINFO_FA_BBA", false, "DI_DISINFO_FIN_ACTIVITY", "BOOKKEEP_BALANCE_ANNEX");
            Database.AddIndex("IND_DI_DISINFO_FA_TS", false, "DI_DISINFO_FIN_ACTIVITY", "TAX_SYSTEM_ID");
            Database.AddForeignKey("FK_DI_DISINFO_FA_TS", "DI_DISINFO_FIN_ACTIVITY", "TAX_SYSTEM_ID", "DI_DICT_TAX_SYSTEM", "ID");
            Database.AddForeignKey("FK_DI_DISINFO_FA_BBA", "DI_DISINFO_FIN_ACTIVITY", "BOOKKEEP_BALANCE_ANNEX", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_DI_DISINFO_FA_BB", "DI_DISINFO_FIN_ACTIVITY", "BOOKKEEP_BALANCE", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_DI_DISINFO_FA_DI", "DI_DISINFO_FIN_ACTIVITY", "DISINFO_ID", "DI_DISINFO", "ID");
            //-----

            //-----Коммунальные услуги финансовой деятельности
            Database.AddEntityTable(
                "DI_DISINFO_FINACT_COMMUN",
                new Column("EXACT", DbType.Decimal),
                new Column("INCOME_PROVIDING", DbType.Decimal),
                new Column("DEBT_POPULATION_START", DbType.Decimal),
                new Column("DEBT_POPULATION_END", DbType.Decimal),
                new Column("DEBT_MANORG_COMMUNAL", DbType.Decimal),
                new Column("PAID_METERING_DEVICE", DbType.Decimal),
                new Column("PAID_GENERAL_NEEDS", DbType.Decimal),
                new Column("PAYMENT_CLAIM", DbType.Decimal),
                new Column("TYPE_SERVICE_DI", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("DISINFO_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_DISINFO_FA_COM_DI", false, "DI_DISINFO_FINACT_COMMUN", "DISINFO_ID");
            Database.AddForeignKey("FK_DI_DISINFO_FA_COM_DI", "DI_DISINFO_FINACT_COMMUN", "DISINFO_ID", "DI_DISINFO", "ID");
            //-----

            //-----Управление домами по категориям финансовой деятельности
            Database.AddEntityTable(
                "DI_DISINFO_FINACT_CATEG",
                new Column("TYPE_CATEGORY_HOUSE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("INCOME_MANAGING", DbType.Decimal),
                new Column("INCOM_USE_GEN_PROPERTY", DbType.Decimal),
                new Column("EXPENSE_MANAGING", DbType.Decimal),
                new Column("EXACT_POPULATION", DbType.Decimal),
                new Column("DEBT_POPULATION_START", DbType.Decimal),
                new Column("DEBT_POPULATION_END", DbType.Decimal),
                new Column("DISINFO_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_DISINFO_FA_CAT_DI", false, "DI_DISINFO_FINACT_CATEG", "DISINFO_ID");
            Database.AddForeignKey("FK_DI_DISINFO_FA_CAT_DI", "DI_DISINFO_FINACT_CATEG", "DISINFO_ID", "DI_DISINFO", "ID");
            //-----

            //-----Управление по домам финансовой деятельности
            Database.AddEntityTable(
                "DI_DISINFO_FINACT_REALOBJ",
                new Column("PRESENTED_TO_REPAY", DbType.Decimal),
                new Column("RECEIVED_PROVIDED_SERV", DbType.Decimal),
                new Column("SUM_DEBT", DbType.Decimal),
                new Column("SUM_FACT_EXPENSE", DbType.Decimal),
                new Column("SUM_INCOME_MANAGE", DbType.Decimal),
                new Column("DISINFO_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("REALITY_OBJ_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_DISINFO_FA_RO_DI", false, "DI_DISINFO_FINACT_REALOBJ", "DISINFO_ID");
            Database.AddIndex("IND_DI_DISINFO_FA_RO_RO", false, "DI_DISINFO_FINACT_REALOBJ", "REALITY_OBJ_ID");
            Database.AddForeignKey("FK_DI_DISINFO_FA_RO_RO", "DI_DISINFO_FINACT_REALOBJ", "REALITY_OBJ_ID", "GKH_REALITY_OBJECT", "ID");
            Database.AddForeignKey("FK_DI_DISINFO_FA_RO_DI", "DI_DISINFO_FINACT_REALOBJ", "DISINFO_ID", "DI_DISINFO", "ID");
            //-----

            //-----Ремонт дома и благоустройство территории, средний срок обслуживания МКД финансовой деятельности
            Database.AddEntityTable(
                "DI_DISINFO_FIN_REPAIR_CAT",
                new Column("TYPE_CATEGORY_HOUSE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("WORK_REPAIR", DbType.Decimal),
                new Column("WORK_BEAUTIFICATION", DbType.Decimal),
                new Column("PERIOD_SERVICE", DbType.String, 50),
                new Column("DISINFO_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_DISINFO_FA_REP_DI", false, "DI_DISINFO_FIN_REPAIR_CAT", "DISINFO_ID");
            Database.AddForeignKey("FK_DI_DISINFO_FA_REP_DI", "DI_DISINFO_FIN_REPAIR_CAT", "DISINFO_ID", "DI_DISINFO", "ID");
            //-----

            //-----Объем привлеченных средств на ремонт и благоустройство финансовой деятельности
            Database.AddEntityTable(
                "DI_DISINFO_FIN_REPAIR_SRC",
                new Column("TYPE_SOURCE_FUNDS", DbType.Int32, 4, ColumnProperty.NotNull, 60),
                new Column("WORK_REPAIR", DbType.Decimal),
                new Column("DISINFO_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_DISINFO_FA_REPS_DI", false, "DI_DISINFO_FIN_REPAIR_SRC", "DISINFO_ID");
            Database.AddForeignKey("FK_DI_DISINFO_FA_REPS_DI", "DI_DISINFO_FIN_REPAIR_SRC", "DISINFO_ID", "DI_DISINFO", "ID");
            //-----

            //-----Административная ответсвенность
            Database.AddEntityTable(
                "DI_ADMIN_RESP",
                new Column("SUPERVISORY_ORG_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("DISINFO_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("AMOUNT_VIOLATION", DbType.Int32),
                new Column("SUM_PENALTY", DbType.Decimal),
                new Column("DATE_PAYMENT_PENALTY", DbType.Date),
                new Column("DATE_IMPOSITION_PENALTY", DbType.Date),
                new Column("TYPE_VIOLATION", DbType.String, 500),
                new Column("ACTIONS", DbType.String, 500),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("DOCUMENT_NAME", DbType.String, 300),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("DATE_FROM", DbType.Date), 
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_ADMIN_RESP_SO", false, "DI_ADMIN_RESP", "SUPERVISORY_ORG_ID");
            Database.AddIndex("IND_DI_ADMIN_RESP_DI", false, "DI_ADMIN_RESP", "DISINFO_ID");
            Database.AddIndex("IND_DI_ADMIN_RESP_FILE", false, "DI_ADMIN_RESP", "FILE_ID");
            Database.AddForeignKey("FK_DI_ADMIN_RESP_FILE", "DI_ADMIN_RESP", "FILE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_DI_ADMIN_RESP_DI", "DI_ADMIN_RESP", "DISINFO_ID", "DI_DISINFO", "ID");
            Database.AddForeignKey("FK_DI_ADMIN_RESP_SO", "DI_ADMIN_RESP", "SUPERVISORY_ORG_ID", "DI_DICT_SUPERVISORY_ORG", "ID");
            //-----

            //-----Аудиторские проверки финансовой деятельности
            Database.AddEntityTable(
                "DI_DISINFO_FINACT_AUDIT",
                new Column("MANAGING_ORG_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("YEAR", DbType.Int32, ColumnProperty.NotNull),
                new Column("TYPE_AUDIT_STATE", DbType.Int32, ColumnProperty.NotNull, 10), 
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_DISINFO_FA_AU_MO", false, "DI_DISINFO_FINACT_AUDIT", "MANAGING_ORG_ID");
            Database.AddIndex("IND_DI_DISINFO_FA_AU_F", false, "DI_DISINFO_FINACT_AUDIT", "FILE_ID");
            Database.AddForeignKey("FK_DI_DISINFO_FA_AU_F", "DI_DISINFO_FINACT_AUDIT", "FILE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_DI_DISINFO_FA_AU_MO", "DI_DISINFO_FINACT_AUDIT", "MANAGING_ORG_ID", "GKH_MANAGING_ORGANIZATION", "ID");
            //-----

            //-----Документы фин деятельности (сметы доходов и Заключение рев коммиссии) в разрезе по годам
            Database.AddEntityTable(
                "DI_DISINFO_FINACT_DOCYEAR",
                new Column("MANAGING_ORG_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("YEAR", DbType.Int32, ColumnProperty.NotNull),
                new Column("TYPE_DOC_BY_YEAR", DbType.Int32, ColumnProperty.NotNull, 10), 
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_DISINFO_FA_DY_MO", false, "DI_DISINFO_FINACT_DOCYEAR", "MANAGING_ORG_ID");
            Database.AddIndex("IND_DI_DISINFO_FA_DY_F", false, "DI_DISINFO_FINACT_DOCYEAR", "FILE_ID");
            Database.AddForeignKey("FK_DI_DISINFO_FA_DY_F", "DI_DISINFO_FINACT_DOCYEAR", "FILE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_DI_DISINFO_FA_DY_MO", "DI_DISINFO_FINACT_DOCYEAR", "MANAGING_ORG_ID", "GKH_MANAGING_ORGANIZATION", "ID");
            //-----

            //----- Документы сведений об УО
            Database.AddEntityTable(
                "DI_DISINFO_DOCUMENTS",
                new Column("DISINFO_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_PROJ_CONTR_ID", DbType.Int64, 22),
                new Column("FILE_COMMUNAL_ID", DbType.Int64, 22),
                new Column("FILE_APARTMENT_ID", DbType.Int64, 22),
                new Column("NOT_AVAILABLE", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("DESCR_PROJ_CONTR", DbType.String, 1500),
                new Column("DESCR_COMMUNAL_COST", DbType.String, 1500),
                new Column("DESCR_COMMUNAL_TARIF", DbType.String, 1500), 
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_DISINFO_DOC_DI", false, "DI_DISINFO_DOCUMENTS", "DISINFO_ID");
            Database.AddIndex("IND_DI_DISINFO_DOC_FP", false, "DI_DISINFO_DOCUMENTS", "FILE_PROJ_CONTR_ID");
            Database.AddIndex("IND_DI_DISINFO_DOC_FC", false, "DI_DISINFO_DOCUMENTS", "FILE_COMMUNAL_ID");
            Database.AddIndex("IND_DI_DISINFO_DOC_FA", false, "DI_DISINFO_DOCUMENTS", "FILE_APARTMENT_ID");
            Database.AddForeignKey("FK_DI_DISINFO_DOC_FA", "DI_DISINFO_DOCUMENTS", "FILE_APARTMENT_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_DI_DISINFO_DOC_FC", "DI_DISINFO_DOCUMENTS", "FILE_COMMUNAL_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_DI_DISINFO_DOC_FP", "DI_DISINFO_DOCUMENTS", "FILE_PROJ_CONTR_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_DI_DISINFO_DOC_DI", "DI_DISINFO_DOCUMENTS", "DISINFO_ID", "DI_DISINFO", "ID");
            //-----

            //----- Сведения об использование нежилых помещений
            Database.AddEntityTable(
                "DI_DISINFO_RO_NONRESPLACE",
                new Column("DISINFO_RO_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("TYPE_CONTRAGENT", DbType.Int32, ColumnProperty.NotNull, 30),
                new Column("AREA", DbType.Decimal),
                new Column("CONTRAGENT_NAME", DbType.String, 300),
                new Column("DOC_NUM_APARTMENT", DbType.String, 50),
                new Column("DOC_DATE_APARTMENT", DbType.Date),
                new Column("DOC_NAME_APARTMENT", DbType.String, 300),
                new Column("DOC_NUM_COMMUNAL", DbType.String, 50),
                new Column("DOC_DATE_COMMUNAL", DbType.Date),
                new Column("DOC_NAME_COMMUNAL", DbType.String, 300), 
                new Column("DATE_START", DbType.Date),
                new Column("DATE_END", DbType.Date), 
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_DISINFO_RO_NRP_DI", false, "DI_DISINFO_RO_NONRESPLACE", "DISINFO_RO_ID");
            Database.AddForeignKey("FK_DI_DISINFO_RO_NRP_DI", "DI_DISINFO_RO_NONRESPLACE", "DISINFO_RO_ID", "DI_DISINFO_REALOBJ", "ID");
            //-----

            //----- Приборы учета сведения об использование нежилых помещений
            Database.AddEntityTable(
                "DI_DISINFO_RONONRESP_METR",
                new Column("DI_NONRESPLACE_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("METERING_DEVICE_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_DISINFO_RONRP_NRP", false, "DI_DISINFO_RONONRESP_METR", "DI_NONRESPLACE_ID");
            Database.AddIndex("IND_DI_DISINFO_RONRP_MD", false, "DI_DISINFO_RONONRESP_METR", "METERING_DEVICE_ID");
            Database.AddForeignKey("FK_DI_DISINFO_RONRP_MD", "DI_DISINFO_RONONRESP_METR", "METERING_DEVICE_ID", "GKH_DICT_METERING_DEVICE", "ID");
            Database.AddForeignKey("FK_DI_DISINFO_RONRP_NRP", "DI_DISINFO_RONONRESP_METR", "DI_NONRESPLACE_ID", "DI_DISINFO_RO_NONRESPLACE", "ID");
            //-----

            //----- Фин показатели по коммунальным услугам
            Database.AddEntityTable(
                "DI_DISINFO_FINCOMMUN_RO",
                new Column("DISINFO_RO_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("TYPE_SERVICE_DI", DbType.Int32, ColumnProperty.NotNull, 30),
                new Column("PAID_OWNER", DbType.Decimal),
                new Column("DEBT_OWNER", DbType.Decimal),
                new Column("PAID_BY_INDICATOR", DbType.Decimal),
                new Column("PAID_BY_ACCOUNT", DbType.Decimal), 
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_DISINFO_FC_RO_DI", false, "DI_DISINFO_FINCOMMUN_RO", "DISINFO_RO_ID");
            Database.AddForeignKey("FK_DI_DISINFO_FC_RO_DI", "DI_DISINFO_FINCOMMUN_RO", "DISINFO_RO_ID", "DI_DISINFO_REALOBJ", "ID");
            //-----

            //----- Документы сведений об УО объекта недвижимости
            Database.AddEntityTable(
                "DI_DISINFO_DOC_RO",
                new Column("DISINFO_RO_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_ACT_STATE_ID", DbType.Int64, 22),
                new Column("FILE_CATREPAIR_ID", DbType.Int64, 22),
                new Column("FILE_PLAN_REPAIR_ID", DbType.Int64, 22),
                new Column("DESCR_ACT_STATE", DbType.String, 1000), 
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_DISINFO_DOCRO_DI", false, "DI_DISINFO_DOC_RO", "DISINFO_RO_ID");
            Database.AddIndex("IND_DI_DISINFO_DOCRO_FA", false, "DI_DISINFO_DOC_RO", "FILE_ACT_STATE_ID");
            Database.AddIndex("IND_DI_DISINFO_DOCRO_FC", false, "DI_DISINFO_DOC_RO", "FILE_CATREPAIR_ID");
            Database.AddIndex("IND_DI_DISINFO_DOCRO_FP", false, "DI_DISINFO_DOC_RO", "FILE_PLAN_REPAIR_ID");
            Database.AddForeignKey("FK_DI_DISINFO_DOCRO_FP", "DI_DISINFO_DOC_RO", "FILE_PLAN_REPAIR_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_DI_DISINFO_DOCRO_FC", "DI_DISINFO_DOC_RO", "FILE_CATREPAIR_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_DI_DISINFO_DOCRO_FA", "DI_DISINFO_DOC_RO", "FILE_ACT_STATE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_DI_DISINFO_DOCRO_DI", "DI_DISINFO_DOC_RO", "DISINFO_RO_ID", "DI_DISINFO_REALOBJ", "ID");
            //-----

            //----- Документы объекта недвижимости протоколы
            Database.AddEntityTable(
                "DI_DISINFO_DOC_PROT",
                new Column("REALITY_OBJ_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("YEAR", DbType.Int32, ColumnProperty.NotNull), 
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_DISINFO_DOC_PR_RO", false, "DI_DISINFO_DOC_PROT", "REALITY_OBJ_ID");
            Database.AddIndex("IND_DI_DISINFO_DOC_PR_F", false, "DI_DISINFO_DOC_PROT", "FILE_ID");
            Database.AddForeignKey("FK_DI_DISINFO_DOC_PR_F", "DI_DISINFO_DOC_PROT", "FILE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_DI_DISINFO_DOC_PR_RO", "DI_DISINFO_DOC_PROT", "REALITY_OBJ_ID", "GKH_REALITY_OBJECT", "ID");
            //-----

            // -----Сведения о договорах
            Database.AddEntityTable(
                "DI_DISINFO_ON_CONTRACTS",
                new Column("DISINFO_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("REALITY_OBJ_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("NAME", DbType.String, 300),
                new Column("NUM", DbType.String, 300),
                new Column("DATE_FROM", DbType.Date),
                new Column("DATE_START", DbType.Date),
                new Column("DATE_END", DbType.Date),
                new Column("PARTIES_CONTRACT", DbType.String, 1000),
                new Column("COST", DbType.Decimal),
                new Column("COMMENTS", DbType.String, 1000),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_DISINF_CON_N", false, "DI_DISINFO_ON_CONTRACTS", "NAME");
            Database.AddIndex("IND_DI_DISINF_CON_DI", false, "DI_DISINFO_ON_CONTRACTS", "DISINFO_ID");
            Database.AddIndex("IND_DI_DISINF_CON_RO", false, "DI_DISINFO_ON_CONTRACTS", "REALITY_OBJ_ID");
            Database.AddForeignKey("FK_DI_DISINF_CON_RO", "DI_DISINFO_ON_CONTRACTS", "REALITY_OBJ_ID", "GKH_REALITY_OBJECT", "ID");
            Database.AddForeignKey("FK_DI_DISINF_CON_DI", "DI_DISINFO_ON_CONTRACTS", "DISINFO_ID", "DI_DISINFO", "ID");
            // -----

            // -----Сведения об использовании мест общего пользования
            Database.AddEntityTable(
                "DI_DISINFO_COM_FACILS",
                new Column("DISINFO_RO_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("KIND_COMMON_FACILITIES ", DbType.String, 300),
                new Column("NUM", DbType.String, 300),
                new Column("DATE_FROM", DbType.Date),
                new Column("LESSEE", DbType.String, 300),
                new Column("TYPE_CONTRACT", DbType.Int32, ColumnProperty.NotNull, 10),
                new Column("DATE_START", DbType.Date),
                new Column("DATE_END", DbType.Date),
                new Column("COST_CONTRACT", DbType.Decimal),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_DISINFO_COMF_DRO", false, "DI_DISINFO_COM_FACILS", "DISINFO_RO_ID");
            Database.AddForeignKey("FK_DI_DISINFO_COMF_DRO", "DI_DISINFO_COM_FACILS", "DISINFO_RO_ID", "DI_DISINFO_REALOBJ", "ID");
            // -----

            //-----Шаблонная услуга
            Database.AddEntityTable(
                "DI_DICT_TEMPL_SERVICE",
                new Column("UNIT_MEASURE_ID", DbType.Int64, 22),
                new Column("NAME", DbType.String, 300),
                new Column("CODE", DbType.String, 50),
                new Column("CHARACTERISTIC", DbType.String, 300), 
                new Column("CHANGEABLE", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("IS_MANDATORY", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("TYPE_GROUP_SERVICE", DbType.Int32, ColumnProperty.NotNull, 10),
                new Column("KIND_SERVICE", DbType.Int32, ColumnProperty.NotNull, 10), 
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_TEM_SER_N", false, "DI_DICT_TEMPL_SERVICE", "NAME");
            Database.AddIndex("IND_DI_TEM_SER_UM", false, "DI_DICT_TEMPL_SERVICE", "UNIT_MEASURE_ID");
            Database.AddForeignKey("FK_DI_TEM_SER_UM", "DI_DICT_TEMPL_SERVICE", "UNIT_MEASURE_ID", "GKH_DICT_UNITMEASURE", "ID");
            //-----

            //-----Группы работ по ТО
            Database.AddEntityTable(
                "DI_DICT_GROUP_WORK_TO",
                new Column("NAME", DbType.String, 300),
                new Column("CODE", DbType.String, 300),
                new Column("TEMPLATE_SERVICE_ID", DbType.Int64, 22));
            Database.AddIndex("IND_DI_GRWORKTO_N", false, "DI_DICT_GROUP_WORK_TO", "NAME");
            Database.AddIndex("IND_DI_GRWORKTO_C", false, "DI_DICT_GROUP_WORK_TO", "CODE");
            Database.AddIndex("IND_DI_GRWORKTO_TS", false, "DI_DICT_GROUP_WORK_TO", "TEMPLATE_SERVICE_ID");
            Database.AddForeignKey("FK_DI_GRWORKTO_TS", "DI_DICT_GROUP_WORK_TO", "TEMPLATE_SERVICE_ID", "DI_DICT_TEMPL_SERVICE", "ID");
            //-----

            //-----Группы работ по ППР
            Database.AddEntityTable(
                "DI_DICT_GROUP_WORK_PPR",
                new Column("NAME", DbType.String, 300),
                new Column("CODE", DbType.String, 300));
            Database.AddIndex("IND_DI_GRWORK_PPR_N", false, "DI_DICT_GROUP_WORK_PPR", "NAME");
            Database.AddIndex("IND_DI_GRWORK_PPR_C", false, "DI_DICT_GROUP_WORK_PPR", "CODE");
            //-----


            //-----Работы по ППР
            Database.AddEntityTable(
                "DI_DICT_WORK_PPR",
                new Column("NAME", DbType.String, 300),
                new Column("GROUP_WORK_PPR_ID", DbType.Int64, 22));
            Database.AddIndex("IND_DI_WORK_PPR_N", false, "DI_DICT_WORK_PPR", "NAME");
            Database.AddIndex("IND_DI_WORK_PPR_GRP", false, "DI_DICT_WORK_PPR", "GROUP_WORK_PPR_ID");
            Database.AddForeignKey("FK_DI_WORK_PPR_GRP", "DI_DICT_WORK_PPR", "GROUP_WORK_PPR_ID", "DI_DICT_GROUP_WORK_PPR", "ID");
            //-----

            //-----Работы по ТО
            Database.AddEntityTable(
                "DI_DICT_WORK_TO",
                new Column("NAME", DbType.String, 300),
                new Column("GROUP_WORK_TO_ID", DbType.Int64, 22));
            Database.AddIndex("IND_DI_WORKTO_N", false, "DI_DICT_WORK_TO", "NAME");
            Database.AddIndex("IND_DI_WORKTO_GWT", false, "DI_DICT_WORK_TO", "GROUP_WORK_TO_ID");
            Database.AddForeignKey("FK_DI_WORKTO_GWT", "DI_DICT_WORK_TO", "GROUP_WORK_TO_ID", "DI_DICT_GROUP_WORK_TO", "ID");
            //-----

            //-----Базовая услуга
            Database.AddEntityTable(
                "DI_BASE_SERVICE",
                new Column("DISINFO_RO_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("TEMPLATE_SERVICE_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("PROVIDER_ID", DbType.Int64, 22),
                new Column("UNIT_MEASURE_ID", DbType.Int64, 22),
                new Column("PROFIT", DbType.Decimal), 
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_BASESERV_DRO", false, "DI_BASE_SERVICE", "DISINFO_RO_ID");
            Database.AddIndex("IND_DI_BASESERV_TS", false, "DI_BASE_SERVICE", "TEMPLATE_SERVICE_ID");
            Database.AddIndex("IND_DI_BASESERV_P", false, "DI_BASE_SERVICE", "PROVIDER_ID");
            Database.AddIndex("IND_DI_BASEEERV_UM", false, "DI_BASE_SERVICE", "UNIT_MEASURE_ID");
            Database.AddForeignKey("FK_DI_BASEEERV_UM", "DI_BASE_SERVICE", "UNIT_MEASURE_ID", "GKH_DICT_UNITMEASURE", "ID");
            Database.AddForeignKey("FK_DI_BASESERV_P", "DI_BASE_SERVICE", "PROVIDER_ID", "GKH_CONTRAGENT", "ID");
            Database.AddForeignKey("FK_DI_BASESERV_TS", "DI_BASE_SERVICE", "TEMPLATE_SERVICE_ID", "DI_DICT_TEMPL_SERVICE", "ID");
            Database.AddForeignKey("FK_DI_BASEEERV_DRO", "DI_BASE_SERVICE", "DISINFO_RO_ID", "DI_DISINFO_REALOBJ", "ID");
            //-----

            //-----Коммунальная услуга
            Database.AddTable(
                "DI_COMMUNAL_SERVICE",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("VOLUME_PURCHASED_RESOURCES", DbType.Decimal),
                new Column("PRICE_PURCHASED_RESOURCES", DbType.Decimal),
                new Column("KIND_ELECTRICITY_SUPPLY", DbType.Int32, ColumnProperty.Null, 10),
                new Column("TYPE_OF_PROVISION_SERVICE", DbType.Int32, ColumnProperty.NotNull, 10));
            Database.AddForeignKey("FK_DI_COMSERV_ID", "DI_COMMUNAL_SERVICE", "ID", "DI_BASE_SERVICE", "ID");
            //-----

            //-----Дополнительная услуга
            Database.AddTable(
                "DI_ADDITION_SERVICE",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("PERIODICITY_ID", DbType.Int64, 22),
                new Column("DOCUMENT", DbType.String, 300),
                new Column("DOCUMENT_NUMBER", DbType.String, 300),
                new Column("DOCUMENT_FROM", DbType.DateTime),
                new Column("DATE_START", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DATE_END", DbType.DateTime, ColumnProperty.NotNull),
                new Column("TOTAL", DbType.Decimal, ColumnProperty.NotNull));
            Database.AddIndex("IND_DI_ADDSERV_PER", false, "DI_ADDITION_SERVICE", "PERIODICITY_ID");
            Database.AddForeignKey("FK_DI_ADDSERV_PER", "DI_ADDITION_SERVICE", "PERIODICITY_ID", "DI_DICT_PERIODICITY", "ID");
            Database.AddForeignKey("FK_DI_ADDSERV_ID", "DI_ADDITION_SERVICE", "ID", "DI_BASE_SERVICE", "ID");
            //-----

            //-----Управление услуга
            Database.AddTable(
                "DI_CONTROL_SERVICE",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull));
            Database.AddForeignKey("FK_DI_CONTSERV_ID", "DI_CONTROL_SERVICE", "ID", "DI_BASE_SERVICE", "ID");
            //-----

            //-----Услуга ремонт
            Database.AddTable(
                "DI_REPAIR_SERVICE",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("TYPE_OF_PROVISION_SERVICE", DbType.Int32, ColumnProperty.NotNull, 10),
                new Column("SUM_WORK_TO", DbType.Decimal));
            Database.AddForeignKey("FK_DI_REPSER_ID", "DI_REPAIR_SERVICE", "ID", "DI_BASE_SERVICE", "ID");
            //-----

            //----Услуга кап ремонт
            Database.AddTable(
                "DI_CAP_REP_SERVICE",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("TYPE_OF_PROVISION_SERVICE", DbType.Int32, ColumnProperty.NotNull, 10),
                new Column("REGIONAL_FUND", DbType.Int32, ColumnProperty.Null, 10));
            Database.AddForeignKey("FK_DI_CAPREPSERV_ID", "DI_CAP_REP_SERVICE", "ID", "DI_BASE_SERVICE", "ID");
            //-----

            //-----Услуга Жилищная
            Database.AddTable(
                "DI_HOUSING_SERVICE",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("PERIODICITY_ID", DbType.Int64, 22),
                new Column("PROTOCOL", DbType.Int64, 22),
                new Column("EQUIPMENT", DbType.Int32, ColumnProperty.NotNull, 10),
                new Column("PROTOCOL_NUMBER", DbType.String, 300),
                new Column("PROTOCOL_FROM", DbType.DateTime), 
                new Column("TYPE_OF_PROVISION_SERVICE", DbType.Int32, ColumnProperty.NotNull, 10));
            Database.AddIndex("IND_DI_HOUSSERV_PER", false, "DI_HOUSING_SERVICE", "PERIODICITY_ID");
            Database.AddIndex("IND_DI_HOUSSERV_PR", false, "DI_HOUSING_SERVICE", "PROTOCOL");
            Database.AddForeignKey("FK_DI_HOUSSERV_PR", "DI_HOUSING_SERVICE", "PROTOCOL", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_DI_HOUSSERV_PER", "DI_HOUSING_SERVICE", "PERIODICITY_ID", "DI_DICT_PERIODICITY", "ID");
            Database.AddForeignKey("FK_DI_HOUSSERV_ID", "DI_HOUSING_SERVICE", "ID", "DI_BASE_SERVICE", "ID");
            //-----

            //-----Тарифы для потребителей
            Database.AddEntityTable(
                "DI_TARIFF_FCONSUMERS",
                new Column("BASE_SERVICE_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("DATE_START", DbType.Date),
                new Column("TARIFF_IS_SET_FOR", DbType.Int32, ColumnProperty.NotNull, 10),
                new Column("COST", DbType.Decimal),
                new Column("ORGANIZATION_SET_TARIFF", DbType.String, 300),
                new Column("COST_NIGHT", DbType.Decimal),
                new Column("TYPE_ORGAN_SET_TARIFF", DbType.Int32, 4, ColumnProperty.NotNull, 10), 
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_TAR_FCONS_BS", false, "DI_TARIFF_FCONSUMERS", "BASE_SERVICE_ID");
            Database.AddForeignKey("FK_DI_TAR_FCONS_BS", "DI_TARIFF_FCONSUMERS", "BASE_SERVICE_ID", "DI_BASE_SERVICE", "ID");
            //-----

            //-----Тарифы для РСО
            Database.AddEntityTable(
                "DI_TARIFF_FRSO",
                new Column("BASE_SERVICE_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("DATE_START", DbType.DateTime),
                new Column("NUMBER_NORMATIVE_LEGAL_ACT", DbType.String, 300),
                new Column("DATE_NORMATIVE_LEGAL_ACT", DbType.DateTime),
                new Column("ORGANIZATION_SET_TARIFF", DbType.String, 300),
                new Column("COST", DbType.Decimal),
                new Column("COST_NIGHT", DbType.Decimal), 
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_TAR_FRSO_BS", false, "DI_TARIFF_FRSO", "BASE_SERVICE_ID");
            Database.AddForeignKey("FK_DI_TAR_FRSO_BS", "DI_TARIFF_FRSO", "BASE_SERVICE_ID", "DI_BASE_SERVICE", "ID");
            //-----

            //-----Статьи затрат
            Database.AddEntityTable(
                "DI_COST_ITEM",
                new Column("BASE_SERVICE_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("NAME", DbType.String, 300),
                new Column("COST", DbType.Decimal), 
                new Column("COUNT", DbType.Decimal),
                new Column("SUM", DbType.Decimal),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_COST_ITEM_NAME", false, "DI_COST_ITEM", "NAME");
            Database.AddIndex("IND_DI_COST_ITEM_BS", false, "DI_COST_ITEM", "BASE_SERVICE_ID");
            Database.AddForeignKey("FK_DI_COST_ITEM_BS", "DI_COST_ITEM", "BASE_SERVICE_ID", "DI_BASE_SERVICE", "ID");
            //-----

            //-----Работа капремонта
            Database.AddEntityTable(
                "DI_CAPREPAIR_WORK",
                new Column("BASE_SERVICE_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("WORK_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("PLAN_VOLUME", DbType.Decimal),
                new Column("PLAN_COST", DbType.Decimal),
                new Column("FACT_VOLUME", DbType.Decimal),
                new Column("FACT_COST", DbType.Decimal), 
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_CAPREP_WORK_BS", false, "DI_CAPREPAIR_WORK", "BASE_SERVICE_ID");
            Database.AddIndex("IND_DI_CAPREP_WORK_W", false, "DI_CAPREPAIR_WORK", "WORK_ID");
            Database.AddForeignKey("FK_DI_CAPREP_WORK_W", "DI_CAPREPAIR_WORK", "WORK_ID", "GKH_DICT_WORK", "ID");
            Database.AddForeignKey("FK_DI_CAPREP_WORK_BS", "DI_CAPREPAIR_WORK", "BASE_SERVICE_ID", "DI_BASE_SERVICE", "ID");
            //-----

            //-----ППР список работ
            Database.AddEntityTable(
                "DI_REPAIR_WORK_LIST",
                new Column("BASE_SERVICE_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("GROUP_WORK_PPR_ID", DbType.Int64, 22),
                new Column("PLAN_VOLUME", DbType.Decimal),
                new Column("PLAN_COST", DbType.Decimal),
                new Column("FACT_VOLUME", DbType.Decimal),
                new Column("FACT_COST", DbType.Decimal),
                new Column("DATE_START", DbType.Date),
                new Column("DATE_END", DbType.Date), 
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_REPWORK_LIST_BS", false, "DI_REPAIR_WORK_LIST", "BASE_SERVICE_ID");
            Database.AddIndex("IND_DI_REPWORK_LIST_GWP", false, "DI_REPAIR_WORK_LIST", "GROUP_WORK_PPR_ID");
            Database.AddForeignKey("FK_DI_REPWORK_LIST_GWP", "DI_REPAIR_WORK_LIST", "GROUP_WORK_PPR_ID", "DI_DICT_GROUP_WORK_PPR", "ID");
            Database.AddForeignKey("FK_DI_REPWORK_LIST_BS", "DI_REPAIR_WORK_LIST", "BASE_SERVICE_ID", "DI_BASE_SERVICE", "ID");
            //-----

            //-----ППР детализация работ
            Database.AddEntityTable(
                "DI_REPAIR_WORK_DETAIL",
                new Column("BASE_SERVICE_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("WORK_PPR_ID", DbType.Int64, 22),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_REPWORK_DET_BS", false, "DI_REPAIR_WORK_DETAIL", "BASE_SERVICE_ID");
            Database.AddIndex("IND_DI_REPWORK_DET_WP", false, "DI_REPAIR_WORK_DETAIL", "WORK_PPR_ID");
            Database.AddForeignKey("FK_DI_REPWORK_DET_WP", "DI_REPAIR_WORK_DETAIL", "WORK_PPR_ID", "DI_DICT_WORK_PPR", "ID");
            Database.AddForeignKey("FK_DI_REPWORK_DET_BS", "DI_REPAIR_WORK_DETAIL", "BASE_SERVICE_ID", "DI_BASE_SERVICE", "ID");
            //-----

            //-----ТО список работ
            Database.AddEntityTable(
                "DI_REPAIR_WORK_TECH",
                new Column("BASE_SERVICE_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("WORK_TO_ID", DbType.Int64, 22),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_REPWORK_TEC_BS", false, "DI_REPAIR_WORK_TECH", "BASE_SERVICE_ID");
            Database.AddIndex("IND_DI_REPWORK_TEC_WTO", false, "DI_REPAIR_WORK_TECH", "WORK_TO_ID");
            Database.AddForeignKey("FK_DI_REPWORK_TEC_WTO", "DI_REPAIR_WORK_TECH", "WORK_TO_ID", "DI_DICT_WORK_TO", "ID");
            Database.AddForeignKey("FK_DI_REPWORK_TEC_BS", "DI_REPAIR_WORK_TECH", "BASE_SERVICE_ID", "DI_BASE_SERVICE", "ID");
            //-----

            //-----План работ по содержанию и ремонту
            Database.AddEntityTable(
                "DI_DISINFO_RO_SERV_REPAIR",
                new Column("DISINFO_RO_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("BASE_SERVICE_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_DISINFO_RSERV_DRO", false, "DI_DISINFO_RO_SERV_REPAIR", "DISINFO_RO_ID");
            Database.AddIndex("IND_DI_DISINFO_RSERV_BS", false, "DI_DISINFO_RO_SERV_REPAIR", "BASE_SERVICE_ID");
            Database.AddForeignKey("FK_DI_DISINFO_RSERV_BS", "DI_DISINFO_RO_SERV_REPAIR", "BASE_SERVICE_ID", "DI_BASE_SERVICE", "ID");
            Database.AddForeignKey("FK_DI_DISINFO_RSERV_DRO", "DI_DISINFO_RO_SERV_REPAIR", "DISINFO_RO_ID", "DI_DISINFO_REALOBJ", "ID");
            //-----

            //-----Работы по плану работ по содержанию и ремонту
            Database.AddEntityTable(
                "DI_DISINFO_RO_SRVREP_WORK",
                new Column("DISINFO_RO_SRVREP_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("REPAIR_WORK_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("PERIODICITY_ID", DbType.Int64, 22),
                new Column("DATA_COMPLETE", DbType.String),
                new Column("DATE_COMPLETE", DbType.Date),
                new Column("COST", DbType.Decimal),
                new Column("DATE_START", DbType.Date),
                new Column("DATE_END", DbType.Date),
                new Column("FACT_COST", DbType.Decimal),
                new Column("REASON_REJECTION", DbType.String, 500), 
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_DISINFO_RSERV_W_DRS", false, "DI_DISINFO_RO_SRVREP_WORK", "DISINFO_RO_SRVREP_ID");
            Database.AddIndex("IND_DI_DISINFO_RSERV_W_RW", false, "DI_DISINFO_RO_SRVREP_WORK", "REPAIR_WORK_ID");
            Database.AddIndex("IND_DI_DISINFO_RSERV_W_P", false, "DI_DISINFO_RO_SRVREP_WORK", "PERIODICITY_ID");
            Database.AddForeignKey("FK_DI_DISINFO_RSERV_W_P", "DI_DISINFO_RO_SRVREP_WORK", "PERIODICITY_ID", "DI_DICT_PERIODICITY", "ID");
            Database.AddForeignKey("FK_DI_DISINFO_RSERV_W_RW", "DI_DISINFO_RO_SRVREP_WORK", "REPAIR_WORK_ID", "DI_REPAIR_WORK_LIST", "ID");
            Database.AddForeignKey("FK_DI_DISINFO_RSERV_W_DRS", "DI_DISINFO_RO_SRVREP_WORK", "DISINFO_RO_SRVREP_ID", "DI_DISINFO_RO_SERV_REPAIR", "ID");
            //-----

            //-----План мер по снижению расходов
            Database.AddEntityTable(
                "DI_DISINFO_RO_REDUCT_EXP",
                new Column("DISINFO_RO_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("BASE_SERVICE_ID", DbType.Int64, 22),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_DISINFO_RREDEX_DRO", false, "DI_DISINFO_RO_REDUCT_EXP", "DISINFO_RO_ID");
            Database.AddIndex("IND_DI_DISINFO_RREDEX_BS", false, "DI_DISINFO_RO_REDUCT_EXP", "BASE_SERVICE_ID");
            Database.AddForeignKey("FK_DI_DISINFO_RREDEX_BS", "DI_DISINFO_RO_REDUCT_EXP", "BASE_SERVICE_ID", "DI_BASE_SERVICE", "ID");
            Database.AddForeignKey("FK_DI_DISINFO_RREDEX_DRO", "DI_DISINFO_RO_REDUCT_EXP", "DISINFO_RO_ID", "DI_DISINFO_REALOBJ", "ID");
            //-----

            //-----Работы по плану мер по снижению расходов
            Database.AddEntityTable(
                "DI_DISINFO_RO_REDEXP_WORK",
                new Column("DISINFO_RO_REDEXP_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("NAME", DbType.String, 300),
                new Column("DATE_COMPLETE", DbType.Date),
                new Column("PLAN_REDUCT_EXPENSE", DbType.Decimal),
                new Column("FACT_REDUCT_EXPENSE", DbType.Decimal),
                new Column("REASON_REJECTION", DbType.String, 500), 
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_DISINFO_RREDEX_N", false, "DI_DISINFO_RO_REDEXP_WORK", "NAME");
            Database.AddIndex("IND_DI_DISINFO_RREDEX_DRR", false, "DI_DISINFO_RO_REDEXP_WORK", "DISINFO_RO_REDEXP_ID");
            Database.AddForeignKey("FK_DI_DISINFO_RREDEX_DRR", "DI_DISINFO_RO_REDEXP_WORK", "DISINFO_RO_REDEXP_ID", "DI_DISINFO_RO_REDUCT_EXP", "ID");
            //-----

            //-----Сведения о случаях снижения платы
            Database.AddEntityTable(
                "DI_DISINFO_RO_REDUCT_PAY",
                new Column("DISINFO_RO_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("BASE_SERVICE_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("REASON_REDUCTION", DbType.String, 300),
                new Column("RECALC_SUM", DbType.Decimal),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("ORDER_DATE", DbType.Date),
                new Column("ORDER_NUM", DbType.String, 50), 
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_DISINFO_RREDP_DRO", false, "DI_DISINFO_RO_REDUCT_PAY", "DISINFO_RO_ID");
            Database.AddIndex("IND_DI_DISINFO_RREDP_BS", false, "DI_DISINFO_RO_REDUCT_PAY", "BASE_SERVICE_ID");
            Database.AddForeignKey("FK_DI_DISINFO_RREDP_BS", "DI_DISINFO_RO_REDUCT_PAY", "BASE_SERVICE_ID", "DI_BASE_SERVICE", "ID");
            Database.AddForeignKey("FK_DI_DISINFO_RREDP_DRO", "DI_DISINFO_RO_REDUCT_PAY", "DISINFO_RO_ID", "DI_DISINFO_REALOBJ", "ID");
            //-----

            //-----Сведения об оплатах коммунальных услуг
            Database.AddEntityTable(
                "DI_DISINFO_RO_PAY_COMMUN",
                new Column("DISINFO_RO_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("BASE_SERVICE_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("COUNTER_VALUE_START", DbType.Decimal),
                new Column("COUNTER_VALUE_END", DbType.Decimal),
                new Column("ACCRUAL", DbType.Decimal),
                new Column("PAYED", DbType.Decimal),
                new Column("DEBT", DbType.Decimal), 
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_DISINFO_RPAYCO_DRO", false, "DI_DISINFO_RO_PAY_COMMUN", "DISINFO_RO_ID");
            Database.AddIndex("IND_DI_DISINFO_RPAYCO_BS", false, "DI_DISINFO_RO_PAY_COMMUN", "BASE_SERVICE_ID");
            Database.AddForeignKey("FK_DI_DISINFO_RPAYCO_BS", "DI_DISINFO_RO_PAY_COMMUN", "BASE_SERVICE_ID", "DI_BASE_SERVICE", "ID");
            Database.AddForeignKey("FK_DI_DISINFO_RPAYCO_DRO", "DI_DISINFO_RO_PAY_COMMUN", "DISINFO_RO_ID", "DI_DISINFO_REALOBJ", "ID");
            //-----

            //-----Сведения об оплатах жилищных услуг
            Database.AddEntityTable(
                "DI_DISINFO_RO_PAY_HOUSING",
                new Column("DISINFO_RO_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("BASE_SERVICE_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("COUNTER_VALUE_START", DbType.Decimal),
                new Column("COUNTER_VALUE_END", DbType.Decimal),
                new Column("GENERAL_ACCRAUL", DbType.Decimal),
                new Column("COLLECTION", DbType.Decimal), 
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_DISINFO_RPAYHO_DRO", false, "DI_DISINFO_RO_PAY_HOUSING", "DISINFO_RO_ID");
            Database.AddIndex("IND_DI_DISINFO_RPAYHO_BS", false, "DI_DISINFO_RO_PAY_HOUSING", "BASE_SERVICE_ID");
            Database.AddForeignKey("FK_DI_DISINFO_RPAYHO_BS", "DI_DISINFO_RO_PAY_HOUSING", "BASE_SERVICE_ID", "DI_BASE_SERVICE", "ID");
            Database.AddForeignKey("FK_DI_DISINFO_RPAYHO_DRO", "DI_DISINFO_RO_PAY_HOUSING", "DISINFO_RO_ID", "DI_DISINFO_REALOBJ", "ID");
            //-----

            //-----Настраиваемые поля шаблонной услуги
            Database.AddEntityTable(
                "DI_TEMPL_SERV_OPT_FIELDS",
                new Column("TEMPLATE_SERVICE_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("NAME", DbType.String, 300),
                new Column("FIELD_NAME", DbType.String, 300),
                new Column("IS_HIDDEN", DbType.Boolean, ColumnProperty.NotNull, true));
            Database.AddIndex("IND_DI_TEM_SER_OPT_F_N", false, "DI_TEMPL_SERV_OPT_FIELDS", "NAME");
            Database.AddIndex("IND_DI_TEM_SER_OPT_F_TS", false, "DI_TEMPL_SERV_OPT_FIELDS", "TEMPLATE_SERVICE_ID");
            Database.AddForeignKey("FK_DI_TEM_SER_OPT_F_TS", "DI_TEMPL_SERV_OPT_FIELDS", "TEMPLATE_SERVICE_ID", "DI_DICT_TEMPL_SERVICE", "ID");
            //-----

            //-----Прочие услуги
            Database.AddEntityTable(
                "DI_OTHER_SERVICE",
                new Column("DISINFO_RO_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("NAME", DbType.String, 300),
                new Column("CODE", DbType.Int64, 22),
                new Column("UNIT_MEASURE", DbType.String, 300),
                new Column("TARIFF", DbType.Decimal),
                new Column("PROVIDER", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_OTHERSERV_NAME", false, "DI_OTHER_SERVICE", "NAME");
            Database.AddIndex("IND_DI_OTHERSERV_DRO", false, "DI_OTHER_SERVICE", "DISINFO_RO_ID");
            Database.AddForeignKey("FK_DI_OTHERSERV_DRO", "DI_OTHER_SERVICE", "DISINFO_RO_ID", "DI_DISINFO_REALOBJ", "ID");
            //------

            //-----Поставщики услуг услуги
            Database.AddEntityTable(
                "DI_SERVICE_PROVIDER",
                new Column("BASE_SERVICE_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("PROVIDER_ID", DbType.Int64, 22),
                new Column("DATE_START_CONTRACT", DbType.Date),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DI_SERVPROV_BS", false, "DI_SERVICE_PROVIDER", "BASE_SERVICE_ID");
            Database.AddIndex("IND_DI_SERVPROV_P", false, "DI_SERVICE_PROVIDER", "PROVIDER_ID");
            Database.AddForeignKey("FK_DI_SERVPROV_P", "DI_SERVICE_PROVIDER", "PROVIDER_ID", "GKH_CONTRAGENT", "ID");
            Database.AddForeignKey("FK_DI_SERVPROV_BS", "DI_SERVICE_PROVIDER", "BASE_SERVICE_ID", "DI_BASE_SERVICE", "ID");
            //-----

            //-----Группа домов
            Database.AddEntityTable(
                "DI_DISINFO_GROUP",
                new Column("DISINFO_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("NAME", DbType.String, 300));
            Database.AddIndex("IND_DI_DISINFO_GR_DI", false, "DI_DISINFO_GROUP", "DISINFO_ID");
            Database.AddForeignKey("FK_DI_DISINFO_GR_DI", "DI_DISINFO_GROUP", "DISINFO_ID", "DI_DISINFO", "ID");
            //-----

            //-----Таблица связи дома с группой
            Database.AddEntityTable(
                "DI_DISINFO_RO_GROUP",
                new Column("REALITY_OBJ_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("GROUP_ID", DbType.Int64, 22, ColumnProperty.NotNull));
            Database.AddIndex("IND_DI_ROGR_RO", false, "DI_DISINFO_RO_GROUP", "REALITY_OBJ_ID");
            Database.AddIndex("IND_DI_ROGR_GR", false, "DI_DISINFO_RO_GROUP", "GROUP_ID");
            Database.AddForeignKey("FK_DI_ROGR_GR", "DI_DISINFO_RO_GROUP", "GROUP_ID", "DI_DISINFO_GROUP", "ID");
            Database.AddForeignKey("FK_DI_ROGR_RO", "DI_DISINFO_RO_GROUP", "REALITY_OBJ_ID", "GKH_REALITY_OBJECT", "ID");
            //-----
        }

        public override void Down()
        {

            Database.RemoveConstraint("DI_DISINFO_RELATION", "FK_DI_DISINFO_REL_RO");
            Database.RemoveConstraint("DI_DISINFO_RELATION", "FK_DI_DISINFO_REL_DI");
            Database.RemoveConstraint("DI_ADDITION_SERVICE", "FK_DI_ADDSERV_PER");
            Database.RemoveConstraint("DI_DISINFO_RO_GROUP", "FK_DI_ROGR_RO");
            Database.RemoveConstraint("DI_DISINFO_RO_GROUP", "FK_DI_ROGR_GR");
            Database.RemoveConstraint("DI_DISINFO_GROUP", "FK_DI_DISINFO_GR_DI");
            Database.RemoveConstraint("DI_SERVICE_PROVIDER", "FK_DI_SERVPROV_P");
            Database.RemoveConstraint("DI_SERVICE_PROVIDER", "FK_DI_SERVPROV_BS");
            Database.RemoveConstraint("DI_OTHER_SERVICE", "FK_DI_OTHERSERV_DRO");
            Database.RemoveConstraint("DI_TEMPL_SERV_OPT_FIELDS", "FK_DI_TEM_SER_OPT_F_TS");
            Database.RemoveConstraint("DI_DISINFO_RO_PAY_HOUSING", "FK_DI_DISINFO_RPAYHO_BS");
            Database.RemoveConstraint("DI_DISINFO_RO_PAY_HOUSING", "FK_DI_DISINFO_RPAYHO_DRO");
            Database.RemoveConstraint("DI_DISINFO_RO_PAY_COMMUN", "FK_DI_DISINFO_RPAYCO_BS");
            Database.RemoveConstraint("DI_DISINFO_RO_PAY_COMMUN", "FK_DI_DISINFO_RPAYCO_DRO");
            Database.RemoveConstraint("DI_DISINFO_RO_REDUCT_PAY", "FK_DI_DISINFO_RREDP_BS");
            Database.RemoveConstraint("DI_DISINFO_RO_REDUCT_PAY", "FK_DI_DISINFO_RREDP_DRO");
            Database.RemoveConstraint("DI_DISINFO_RO_REDEXP_WORK", "FK_DI_DISINFO_RREDEX_DRR");
            Database.RemoveConstraint("DI_DISINFO_RO_REDUCT_EXP", "FK_DI_DISINFO_RREDEX_BS");
            Database.RemoveConstraint("DI_DISINFO_RO_REDUCT_EXP", "FK_DI_DISINFO_RREDEX_DRO");
            Database.RemoveConstraint("DI_DISINFO_RO_SRVREP_WORK", "FK_DI_DISINFO_RSERV_W_P");
            Database.RemoveConstraint("DI_DISINFO_RO_SRVREP_WORK", "FK_DI_DISINFO_RSERV_W_RW");
            Database.RemoveConstraint("DI_DISINFO_RO_SRVREP_WORK", "FK_DI_DISINFO_RSERV_W_DRS");
            Database.RemoveConstraint("DI_DISINFO_RO_SERV_REPAIR", "FK_DI_DISINFO_RSERV_BS");
            Database.RemoveConstraint("DI_DISINFO_RO_SERV_REPAIR", "FK_DI_DISINFO_RSERV_DRO");
            Database.RemoveConstraint("DI_REPAIR_WORK_TECH", "FK_DI_REPWORK_TEC_WTO");
            Database.RemoveConstraint("DI_REPAIR_WORK_TECH", "FK_DI_REPWORK_TEC_BS");
            Database.RemoveConstraint("DI_REPAIR_WORK_DETAIL", "FK_DI_REPWORK_DET_WP");
            Database.RemoveConstraint("DI_REPAIR_WORK_DETAIL", "FK_DI_REPWORK_DET_BS");
            Database.RemoveConstraint("DI_REPAIR_WORK_LIST", "FK_DI_REPWORK_LIST_GWP");
            Database.RemoveConstraint("DI_REPAIR_WORK_LIST", "FK_DI_REPWORK_LIST_BS");
            Database.RemoveConstraint("DI_CAPREPAIR_WORK", "FK_DI_CAPREP_WORK_W");
            Database.RemoveConstraint("DI_CAPREPAIR_WORK", "FK_DI_CAPREP_WORK_BS");
            Database.RemoveConstraint("DI_TARIFF_FRSO", "FK_DI_TAR_FRSO_BS");
            Database.RemoveConstraint("DI_COST_ITEM", "FK_DI_COST_ITEM_BS");
            Database.RemoveConstraint("DI_TARIFF_FCONSUMERS", "FK_DI_TAR_FCONS_BS");
            Database.RemoveConstraint("DI_COMMUNAL_SERVICE", "FK_DI_COMSERV_ID");
            Database.RemoveConstraint("DI_ADDITION_SERVICE", "FK_DI_ADDSERV_ID");
            Database.RemoveConstraint("DI_CONTROL_SERVICE", "FK_DI_CONTSERV_ID");
            Database.RemoveConstraint("DI_REPAIR_SERVICE", "FK_DI_REPSER_ID");
            Database.RemoveConstraint("DI_CAP_REP_SERVICE", "FK_DI_CAPREPSERV_ID");
            Database.RemoveConstraint("DI_HOUSING_SERVICE", "FK_DI_HOUSSERV_ID");
            Database.RemoveConstraint("DI_HOUSING_SERVICE", "FK_DI_HOUSSERV_PER");
            Database.RemoveConstraint("DI_BASE_SERVICE", "FK_DI_BASEEERV_UM");
            Database.RemoveConstraint("DI_BASE_SERVICE", "FK_DI_BASESERV_P");
            Database.RemoveConstraint("DI_BASE_SERVICE", "FK_DI_BASESERV_TS");
            Database.RemoveConstraint("DI_BASE_SERVICE", "FK_DI_BASEEERV_DRO");
            Database.RemoveConstraint("DI_DICT_WORK_TO", "FK_DI_WORKTO_GWT");
            Database.RemoveConstraint("DI_DICT_WORK_PPR", "FK_DI_WORK_PPR_GRP");
            Database.RemoveConstraint("DI_DICT_GROUP_WORK_TO", "FK_DI_GRWORKTO_TS");
            Database.RemoveConstraint("DI_DICT_TEMPL_SERVICE", "FK_DI_TEM_SER_UM");
            Database.RemoveConstraint("DI_DISINFO_ON_CONTRACTS", "FK_DI_DISINF_CON_RO");
            Database.RemoveConstraint("DI_DISINFO_ON_CONTRACTS", "FK_DI_DISINF_CON_DI");
            Database.RemoveConstraint("DI_DISINFO_COM_FACILS", "FK_DI_DISINFO_COMF_DRO");
            Database.RemoveConstraint("DI_DISINFO_DOC_PROT", "FK_DI_DISINFO_DOC_PR_F");
            Database.RemoveConstraint("DI_DISINFO_DOC_PROT", "FK_DI_DISINFO_DOC_PR_RO");
            Database.RemoveConstraint("DI_DISINFO_DOC_RO", "FK_DI_DISINFO_DOCRO_FP");
            Database.RemoveConstraint("DI_DISINFO_DOC_RO", "FK_DI_DISINFO_DOCRO_FC");
            Database.RemoveConstraint("DI_DISINFO_DOC_RO", "FK_DI_DISINFO_DOCRO_FA");
            Database.RemoveConstraint("DI_DISINFO_DOC_RO", "FK_DI_DISINFO_DOCRO_DI");
            Database.RemoveConstraint("DI_DISINFO_FINCOMMUN_RO", "FK_DI_DISINFO_FC_RO_DI");
            Database.RemoveConstraint("DI_DISINFO_RONONRESP_METR", "FK_DI_DISINFO_RONRP_MD");
            Database.RemoveConstraint("DI_DISINFO_RONONRESP_METR", "FK_DI_DISINFO_RONRP_NRP");
            Database.RemoveConstraint("DI_DISINFO_RO_NONRESPLACE", "FK_DI_DISINFO_RO_NRP_DI");
            Database.RemoveConstraint("DI_DISINFO_DOCUMENTS", "FK_DI_DISINFO_DOC_FA");
            Database.RemoveConstraint("DI_DISINFO_DOCUMENTS", "FK_DI_DISINFO_DOC_FC");
            Database.RemoveConstraint("DI_DISINFO_DOCUMENTS", "FK_DI_DISINFO_DOC_FP");
            Database.RemoveConstraint("DI_DISINFO_DOCUMENTS", "FK_DI_DISINFO_DOC_DI");
            Database.RemoveConstraint("DI_DISINFO_FINACT_DOCYEAR", "FK_DI_DISINFO_FA_DY_F");
            Database.RemoveConstraint("DI_DISINFO_FINACT_DOCYEAR", "FK_DI_DISINFO_FA_DY_MO");
            Database.RemoveConstraint("DI_DISINFO_FINACT_AUDIT", "FK_DDI_DISINFO_FINACT_AUDIT_F");
            Database.RemoveConstraint("DI_DISINFO_FINACT_AUDIT", "FK_DI_DISINFO_FA_AU_MO");
            Database.RemoveConstraint("DI_ADMIN_RESP", "FK_DI_ADMIN_RESP_FILE");
            Database.RemoveConstraint("DI_ADMIN_RESP", "FK_DI_ADMIN_RESP_DI");
            Database.RemoveConstraint("DI_ADMIN_RESP", "FK_DI_ADMIN_RESP_SO");
            Database.RemoveConstraint("DI_DISINFO_FIN_REPAIR_SRC", "FK_DI_DISINFO_FA_REPS_DI");
            Database.RemoveConstraint("DI_DISINFO_FIN_REPAIR_CAT", "FK_DI_DISINFO_FA_REP_DI");
            Database.RemoveConstraint("DI_DISINFO_FINACT_REALOBJ", "FK_DI_DISINFO_FA_RO_RO");
            Database.RemoveConstraint("DI_DISINFO_FINACT_REALOBJ", "FK_DI_DISINFO_FA_RO_DI");
            Database.RemoveConstraint("DI_DISINFO_FINACT_CATEG", "FK_DI_DISINFO_FA_CAT_DI");
            Database.RemoveConstraint("DI_DISINFO_FINACT_COMMUN", "FK_DI_DISINFO_FA_COM_DI");
            Database.RemoveConstraint("DI_DISINFO_FIN_ACTIVITY", "FK_DI_DISINFO_FA_TS");
            Database.RemoveConstraint("DI_DISINFO_FIN_ACTIVITY", "FK_DI_DISINFO_FA_BBA");
            Database.RemoveConstraint("DI_DISINFO_FIN_ACTIVITY", "FK_DI_DISINFO_FA_BB");
            Database.RemoveConstraint("DI_DISINFO_FIN_ACTIVITY", "FK_DI_DISINFO_FA_DI");
            Database.RemoveConstraint("DI_DISINFO_FUNDS", "FK_DI_DISINFO_FUN_DI");
            Database.RemoveConstraint("DI_DISINFO_REALOBJ", "FK_DI_DISINFO_RO_RO");
            Database.RemoveConstraint("DI_DISINFO_REALOBJ", "FK_DI_DISINFRO_PERIOD");
            Database.RemoveConstraint("DI_DISINFO", "FK_DI_DISINFO_FFUND");
            Database.RemoveConstraint("DI_DISINFO", "FK_DI_DISINFO_PERDI");
            Database.RemoveConstraint("DI_DISINFO", "FK_DI_DISINFO_MORG");

            Database.RemoveTable("DI_DICT_PERIOD");
            Database.RemoveTable("DI_DISINFO");
            Database.RemoveTable("DI_DISINFO_REALOBJ");
            Database.RemoveTable("DI_DISINFO");
            Database.RemoveTable("DI_DICT_TAX_SYSTEM");
            Database.RemoveTable("DI_DISINFO_FIN_ACTIVITY");
            Database.RemoveTable("DI_DISINFO_FINACT_COMMUN");
            Database.RemoveTable("DI_DISINFO_FINACT_CATEG");
            Database.RemoveTable("DI_DISINFO_FINACT_REALOBJ");
            Database.RemoveTable("DI_DISINFO_FIN_REPAIR_CAT");
            Database.RemoveTable("DI_DISINFO_FIN_REPAIR_SRC");
            Database.RemoveTable("DI_DICT_SUPERVISORY_ORG");
            Database.RemoveTable("DI_ADMIN_RESP");
            Database.RemoveTable("DI_DISINFO_FINACT_AUDIT");
            Database.RemoveTable("DI_DISINFO_FINACT_DOCYEAR");
            Database.RemoveTable("DI_DISINFO_DOCUMENTS");
            Database.RemoveTable("DI_DISINFO_RO_NONRESPLACE");
            Database.RemoveTable("DI_DISINFO_RONONRESP_METR");
            Database.RemoveTable("DI_DISINFO_FINCOMMUN_RO");
            Database.RemoveTable("DI_DISINFO_DOC_RO");
            Database.RemoveTable("DI_DISINFO_DOC_PROT");
            Database.RemoveTable("DI_DISINFO_ON_CONTRACTS");
            Database.RemoveTable("DI_DISINFO_COM_FACILS");
            Database.RemoveTable("DI_DICT_TEMPL_SERVICE");
            Database.RemoveTable("DI_DICT_PERIODICITY");
            Database.RemoveTable("DI_TARIFF_FCONSUMERS");
            Database.RemoveTable("DI_TARIFF_FRSO");
            Database.RemoveTable("DI_COST_ITEM");
            Database.RemoveTable("DI_COMMUNAL_SERVICE");
            Database.RemoveTable("DI_ADDITION_SERVICE");
            Database.RemoveTable("DI_CONTROL_SERVICE");
            Database.RemoveTable("DI_REPAIR_SERVICE");
            Database.RemoveTable("DI_CAP_REP_SERVICE");
            Database.RemoveTable("DI_HOUSING_SERVICE");
            Database.RemoveTable("DI_BASE_SERVICE");
            Database.RemoveTable("DI_CAPREPAIR_WORK");
            Database.RemoveTable("DI_REPAIR_WORK_LIST");
            Database.RemoveTable("DI_REPAIR_WORK_DETAIL");
            Database.RemoveTable("DI_REPAIR_WORK_TECH");
            Database.RemoveTable("DI_DISINFO_RO_SERV_REPAIR");
            Database.RemoveTable("DI_DISINFO_RO_SRVREP_WORK");
            Database.RemoveTable("DI_DISINFO_RO_REDUCT_EXP");
            Database.RemoveTable("DI_DISINFO_RO_REDEXP_WORK");
            Database.RemoveTable("DI_DISINFO_RO_REDUCT_PAY");
            Database.RemoveTable("DI_DISINFO_RO_PAY_COMMUN");
            Database.RemoveTable("DI_DISINFO_RO_PAY_HOUSING");
            Database.RemoveTable("DI_DICT_GROUP_WORK_TO");
            Database.RemoveTable("DI_DICT_GROUP_WORK_PPR");
            Database.RemoveTable("DI_DICT_WORK_PPR");
            Database.RemoveTable("DI_DICT_WORK_TO");
            Database.RemoveTable("DI_TEMPL_SERV_OPT_FIELDS");
            Database.RemoveTable("DI_OTHER_SERVICE");
            Database.RemoveTable("DI_SERVICE_PROVIDER");
            Database.RemoveTable("DI_DISINFO_GROUP");
            Database.RemoveTable("DI_DISINFO_RO_GROUP");
            Database.RemoveTable("DI_DISINFO_FUNDS");
            Database.RemoveTable("DI_DISINFO_RELATION");
                        
            
        }
    }
}