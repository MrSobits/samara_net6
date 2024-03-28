// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateSchema.cs" company="">
//   
// </copyright>
// <summary>
//   The update schema.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bars.GkhGji.Migrations.Version_1
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----Справочник "Тематики обращений"
            Database.AddEntityTable(
                "GJI_DICT_STATEMENT_SUBJ",
                new Column("NAME", DbType.String, 300),
                new Column("CODE", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_STAT_SUBJ_NAME", false, "GJI_DICT_STATEMENT_SUBJ", "NAME");
            Database.AddIndex("IND_GJI_STAT_SUBJ_CODE", false, "GJI_DICT_STATEMENT_SUBJ", "CODE");
            //-----

            //-----Компетентная организация
            Database.AddEntityTable(
                "GJI_DICT_COMPETENT_ORG",
                new Column("CODE", DbType.String, 300),
                new Column("NAME", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_COMPET_ORG_NAME", false, "GJI_DICT_COMPETENT_ORG", "NAME");
            Database.AddIndex("IND_GJI_COMPET_ORG_CODE", false, "GJI_DICT_COMPETENT_ORG", "CODE");
            //-----

            //-----Содержание ответа
            Database.AddEntityTable(
                "GJI_DICT_ANSWER_CONTENT",
                new Column("CODE", DbType.String, 300),
                new Column("NAME", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_ANSWER_CONT_NAME", false, "GJI_DICT_ANSWER_CONTENT", "NAME");
            Database.AddIndex("IND_GJI_ANSWER_CONT_CODE", false, "GJI_DICT_ANSWER_CONTENT", "CODE");
            //-----

            //-----Справочник статья ТСЖ ГЖИ
            Database.AddEntityTable(
                "GJI_DICT_ARTICLE_TSJ",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("GROUP_NAME", DbType.String, 300),
                new Column("CODE", DbType.String, 50));
            Database.AddIndex("IND_GJI_ARTICLE_TSJ_NAME", false, "GJI_DICT_ARTICLE_TSJ", "NAME");
            Database.AddIndex("IND_GJI_ARTICLE_TSJ_CODE", false, "GJI_DICT_ARTICLE_TSJ", "CODE");
            //-----

            //-----Справочник вид протокола ТСЖ ГЖИ
            Database.AddEntityTable(
                "GJI_DICT_KIND_PROT",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 50));
            Database.AddIndex("IND_GJI_KIND_PROT_NAME", false, "GJI_DICT_KIND_PROT", "NAME");
            Database.AddIndex("IND_GJI_KIND_PROT_CODE", false, "GJI_DICT_KIND_PROT", "CODE");
            //-----

            //-----Вид работы
            Database.AddEntityTable(
                "GJI_DICT_KIND_WORK",
                new Column("CODE", DbType.String, 50),
                new Column("NAME", DbType.String, 300));
            Database.AddIndex("IND_GJI_KIND_WORK_NAME", false, "GJI_DICT_KIND_WORK", "NAME");
            Database.AddIndex("IND_GJI_KIND_WORK_CODE", false, "GJI_DICT_KIND_WORK", "CODE");
            //-----   

            //-----Период отопительного сезона
            Database.AddEntityTable(
                "GJI_DICT_HEATSEASONPERIOD",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("DATE_START", DbType.DateTime),
                new Column("DATE_END", DbType.DateTime));
            Database.AddIndex("IND_GJI_HEATSEAS_PER_NAME", false, "GJI_DICT_HEATSEASONPERIOD", "NAME");
            //-----

            //-----Справочник нарушений
            Database.AddEntityTable(
                "GJI_DICT_VIOLATION",
                new Column("GKRF", DbType.String, 300),
                new Column("CODEPIN", DbType.String, 300),
                new Column("PPRF25", DbType.String, 100),
                new Column("PPRF307", DbType.String, 100),
                new Column("PPRF491", DbType.String, 100),
                new Column("EXTERNAL_ID", DbType.String, 36),
                new Column("NAME", DbType.String, 1000),
                new Column("DESCRIPTION", DbType.String, 1000));
            Database.AddIndex("IND_GJI_VIOL_NAME", false, "GJI_DICT_VIOLATION", "NAME");
            Database.AddIndex("IND_GJI_VIOL_CODE", false, "GJI_DICT_VIOLATION", "CODEPIN");
            //-----

            //-----Характеристики нарушений
            Database.AddEntityTable(
                "GJI_DICT_FEATUREVIOL",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_FEAT_VIOL_NAME", false, "GJI_DICT_FEATUREVIOL", "NAME");
            Database.AddIndex("IND_GJI_FEAT_VIOL_CODE", false, "GJI_DICT_FEATUREVIOL", "CODE");
            //-----

            //-----Таблица связи Нарушений и характеристик нарушений
            Database.AddEntityTable(
                "GJI_DICT_VIOLATIONFEATURE",
                new Column("VIOLATION_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FEATUREVIOL_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_VIOLFEATURE_VIOL", false, "GJI_DICT_VIOLATIONFEATURE", "VIOLATION_ID");
            Database.AddIndex("IND_GJI_VIOLFEATURE_FEA", false, "GJI_DICT_VIOLATIONFEATURE", "FEATUREVIOL_ID");
            Database.AddForeignKey("FK_GJI_VIOLFEATURE_VIOL", "GJI_DICT_VIOLATIONFEATURE", "VIOLATION_ID", "GJI_DICT_VIOLATION", "ID");
            Database.AddForeignKey("FK_GJI_VIOLFEATURE_FEA", "GJI_DICT_VIOLATIONFEATURE", "FEATUREVIOL_ID", "GJI_DICT_FEATUREVIOL", "ID");
            //-----

            //-----Справочник инспектируемая часть
            Database.AddEntityTable(
                "GJI_DICT_INSPECTEDPART",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300));
            Database.AddIndex("IND_GJI_INSPART_NAME", false, "GJI_DICT_INSPECTEDPART", "NAME");
            Database.AddIndex("IND_GJI_INSPART_CODE", false, "GJI_DICT_INSPECTEDPART", "CODE");
            //-----

            //-----Справочник исполнитель документа ГЖИ
            Database.AddEntityTable(
                "GJI_DICT_EXECUTANT",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_EXEC_NAME", false, "GJI_DICT_EXECUTANT", "NAME");
            Database.AddIndex("IND_GJI_EXEC_CODE", false, "GJI_DICT_EXECUTANT", "CODE");
            //-----

            //-----Справочник санкции ГЖИ
            Database.AddEntityTable(
                "GJI_DICT_SANCTION",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_SANCTION_NAME", false, "GJI_DICT_SANCTION", "NAME");
            Database.AddIndex("IND_GJI_SANCTION_CODE", false, "GJI_DICT_SANCTION", "CODE");
            //-----

            //-----Типы обследования
            Database.AddEntityTable(
                "GJI_DICT_TYPESURVEY",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_TYPESURVEY_NAME", false, "GJI_DICT_TYPESURVEY", "NAME");
            Database.AddIndex("IND_GJI_TYPESURVEY_CODE", false, "GJI_DICT_TYPESURVEY", "CODE");
            //-----

            Database.AddEntityTable(
                "GJI_DICT_TASKS_INSPECTION",
                new Column("TYPE_SURVEY_GJI_ID", DbType.Int64, 22),
                new Column("NAME", DbType.String, 500));
            Database.AddIndex("IND_GJI_TASK_INS_TS", false, "GJI_DICT_TASKS_INSPECTION", "TYPE_SURVEY_GJI_ID");
            Database.AddForeignKey("FK_GJI_TASK_INS_TS", "GJI_DICT_TASKS_INSPECTION", "TYPE_SURVEY_GJI_ID", "GJI_DICT_TYPESURVEY", "ID");

            //-----Цели и задачи проверки
            Database.AddEntityTable(
                "GJI_DICT_GOALS_INSPECTION",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("TYPE_SURVEY_GJI_ID", DbType.Int64, 22, ColumnProperty.NotNull));
            Database.AddIndex("IND_GJI_GOALS_INS_NAME", false, "GJI_DICT_GOALS_INSPECTION", "NAME");
            Database.AddIndex("IND_GJI_GOALS_INS_TS", false, "GJI_DICT_GOALS_INSPECTION", "TYPE_SURVEY_GJI_ID");
            Database.AddForeignKey("FK_GJI_GOALS_INS_TS", "GJI_DICT_GOALS_INSPECTION", "TYPE_SURVEY_GJI_ID", "GJI_DICT_TYPESURVEY", "ID");
            //-----

            //-----Вид обследования
            Database.AddEntityTable(
                "GJI_DICT_KIND_INSPECTION",
                new Column("TYPE_SURVEY_GJI_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("TYPE_CHECK", DbType.Int32, 4, ColumnProperty.NotNull));
            Database.AddIndex("IND_GJI_KIND_INS_TC", false, "GJI_DICT_KIND_INSPECTION", "TYPE_CHECK");
            Database.AddIndex("IND_GJI_KIND_INS_TS", false, "GJI_DICT_KIND_INSPECTION", "TYPE_SURVEY_GJI_ID");
            Database.AddForeignKey("FK_GJI_KIND_INS_TS", "GJI_DICT_KIND_INSPECTION", "TYPE_SURVEY_GJI_ID", "GJI_DICT_TYPESURVEY", "ID");
            //-----

            //-----Правовое основание проверки
            Database.AddEntityTable(
                "GJI_DICT_LEGFOUND_INSPECT",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("TYPE_SURVEY_GJI_ID", DbType.Int64, 22, ColumnProperty.NotNull));
            Database.AddIndex("IND_GJI_LEGF_INS_NAME", false, "GJI_DICT_LEGFOUND_INSPECT", "NAME");
            Database.AddIndex("IND_GJI_LEGF_INS_TS", false, "GJI_DICT_LEGFOUND_INSPECT", "TYPE_SURVEY_GJI_ID");
            Database.AddForeignKey("FK_GJI_LEGF_INS_TS", "GJI_DICT_LEGFOUND_INSPECT", "TYPE_SURVEY_GJI_ID", "GJI_DICT_TYPESURVEY", "ID");
            //-----

            //-----Справочник статьи закона
            Database.AddEntityTable(
                "GJI_DICT_ARTICLELAW",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_ARTLAW_NAME", false, "GJI_DICT_ARTICLELAW", "NAME");
            Database.AddIndex("IND_GJI_ARTLAW_CODE", false, "GJI_DICT_ARTICLELAW", "CODE");
            //-----

            //-----Справочник виды суда
            Database.AddEntityTable(
                "GJI_DICT_COURT",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_COURT_NAME", false, "GJI_DICT_COURT", "NAME");
            Database.AddIndex("IND_GJI_COURT_CODE", false, "GJI_DICT_COURT", "CODE");
            //-----

            //-----Справочник решение суда
            Database.AddEntityTable(
                "GJI_DICT_COURTVERDICT",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_COURTVER_NAME", false, "GJI_DICT_COURTVERDICT", "NAME");
            Database.AddIndex("IND_GJI_COURTVER_CODE", false, "GJI_DICT_COURTVERDICT", "CODE");
            //-----

            //-----Справочник инстанции
            Database.AddEntityTable(
                "GJI_DICT_INSTANCE",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_INSTANCE_NAME", false, "GJI_DICT_INSTANCE", "NAME");
            Database.AddIndex("IND_GJI_INSTANCE_CODE", false, "GJI_DICT_INSTANCE", "CODE");
            //-----

            //-----План проверок юр. лиц
            Database.AddEntityTable(
                "GJI_DICT_PLANJURPERSON",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("DATE_START", DbType.DateTime),
                new Column("DATE_END", DbType.DateTime));
            Database.AddIndex("IND_GJI_PLANJUR_NAME", false, "GJI_DICT_PLANJURPERSON", "NAME");
            //-----

            //-----Предоставляемые документы
            Database.AddEntityTable(
                "GJI_DICT_PROVIDEDDOCUMENT",
                new Column("NAME", DbType.String, 500, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300));
            Database.AddIndex("IND_GJI_PROVDOC_NAME", false, "GJI_DICT_PROVIDEDDOCUMENT", "NAME");
            Database.AddIndex("IND_GJI_PROVDOC_CODE", false, "GJI_DICT_PROVIDEDDOCUMENT", "CODE");
            //-----

            //-----Эксперты
            Database.AddEntityTable(
                "GJI_DICT_EXPERT",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));

            Database.AddIndex("IND_GJI_EXPERT_NAME", false, "GJI_DICT_EXPERT", "NAME");
            Database.AddIndex("IND_GJI_EXPERT_CODE", false, "GJI_DICT_EXPERT", "CODE");
            //-----

            //-----План инспекционных проверок
            Database.AddEntityTable(
                "GJI_DICT_PLANINSCHECK",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("DATE_START", DbType.DateTime),
                new Column("DATE_END", DbType.DateTime));
            Database.AddIndex("IND_GJI_PLANINSCHECK_NAME", false, "GJI_DICT_PLANINSCHECK", "NAME");
            //-----

            //-----Источники поступлений
            Database.AddEntityTable(
                "GJI_DICT_REVENUESOURCE",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_REVEN_SRC_NAME", false, "GJI_DICT_REVENUESOURCE", "NAME");
            Database.AddIndex("IND_GJI_REVEN_SRC_CODE", false, "GJI_DICT_REVENUESOURCE", "CODE");
            //-----

            //-----Вид обращения
            Database.AddEntityTable(
                "GJI_DICT_KINDSTATEMENT",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_KINDSTAT_NAME", false, "GJI_DICT_KINDSTATEMENT", "NAME");
            Database.AddIndex("IND_GJI_KINDSTAT_CODE", false, "GJI_DICT_KINDSTATEMENT", "CODE");
            //-----

            //-----Резолюция
            Database.AddEntityTable(
                "GJI_DICT_RESOLVE",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_RESOLVE_NAME", false, "GJI_DICT_RESOLVE", "NAME");
            Database.AddIndex("IND_GJI_RESOLVE_CODE", false, "GJI_DICT_RESOLVE", "CODE");
            //-----

            //-----Форма поступления
            Database.AddEntityTable(
                "GJI_DICT_REVENUEFORM",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_REVEN_FORM_NAME", false, "GJI_DICT_REVENUEFORM", "NAME");
            Database.AddIndex("IND_GJI_REVEN_FORM_CODE", false, "GJI_DICT_REVENUEFORM", "CODE");
            //-----
            
            //-----Проверка ГЖИ
            Database.AddEntityTable(
                "GJI_INSPECTION",
                new Column("TYPE_BASE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("CONTRAGENT_ID", DbType.Int64, 22),
                new Column("TYPE_JUR_PERSON", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("INSPECTION_NUMBER", DbType.String, 20),
                new Column("INSPECTION_NUM", DbType.Int32),
                new Column("INSPECTION_YEAR", DbType.Int32, 4),
                new Column("EXTERNAL_ID", DbType.String, 36),
                new Column("PHYSICAL_PERSON", DbType.String, 300),
                new Column("PHYSICAL_PERSON_INFO", DbType.String, 500),
                new Column("DESCRIPTION", DbType.String, 500));
            Database.AddIndex("IND_GJI_INSPECT_CTR", false, "GJI_INSPECTION", "CONTRAGENT_ID");
            Database.AddForeignKey("FK_GJI_INSPECT_CTR", "GJI_INSPECTION", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            //-----

            //-----Инспекторы Основания проверки
            Database.AddEntityTable(
                "GJI_INSPECTION_INSPECTOR",
                new Column("INSPECTION_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("INSPECTOR_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_INSPECT_INS_INSN", false, "GJI_INSPECTION_INSPECTOR", "INSPECTION_ID");
            Database.AddIndex("IND_GJI_INSPECT_INS_INSR", false, "GJI_INSPECTION_INSPECTOR", "INSPECTOR_ID");
            Database.AddForeignKey("FK_GJI_INSPECT_INS_INSR", "GJI_INSPECTION_INSPECTOR", "INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_INS_INSN", "GJI_INSPECTION_INSPECTOR", "INSPECTION_ID", "GJI_INSPECTION", "ID");
            //-----

            //-----Основание проверки ГЖИ - Проверка юр. лиц
            Database.AddTable(
                "GJI_INSPECTION_JURPERSON",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("PLAN_ID", DbType.Int64, 22),
                new Column("DATE_START", DbType.DateTime),
                new Column("COUNT_DAYS", DbType.Int32),
                new Column("REASON", DbType.String, 500),
                new Column("TYPE_BASE_JURAL", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("TYPE_FACT", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("TYPE_FORM", DbType.Int32, 4, ColumnProperty.NotNull, 10));
            Database.AddIndex("IND_GJI_INSPECT_JPERS_PLAN", false, "GJI_INSPECTION_JURPERSON", "PLAN_ID");
            Database.AddForeignKey("FK_GJI_INSPECT_JPERS_INS", "GJI_INSPECTION_JURPERSON", "ID", "GJI_INSPECTION", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_JPERS_PLAN", "GJI_INSPECTION_JURPERSON", "PLAN_ID", "GJI_DICT_PLANJURPERSON", "ID");
            //-----

            //-----Основание проверки ГЖИ - Требование прокуратуры
            Database.AddTable(
                "GJI_INSPECTION_PROSCLAIM",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("ISSUED_CLAIM", DbType.String, 300),
                new Column("PROSCLAIM_DATE_CHECK", DbType.DateTime),
                new Column("PROSCLAIM_DATE", DbType.DateTime),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("DOCUMENT_NAME", DbType.String, 300),
                new Column("DOCUMENT_NUMBER", DbType.String, 50),
                new Column("DOCUMENT_DESCRIPTION", DbType.String, 500),
                new Column("TYPE_BASE_PROSCLAIM", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("TYPE_FORM", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("FILE_INFO_ID", DbType.Int64, 22));
            Database.AddIndex("IND_GJI_INSPECT_PROSCL_FILE", false, "GJI_INSPECTION_PROSCLAIM", "FILE_INFO_ID");
            Database.AddForeignKey("FK_GJI_INSPECT_PROSCL_FILE", "GJI_INSPECTION_PROSCLAIM", "FILE_INFO_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_PROSCL_INS", "GJI_INSPECTION_PROSCLAIM", "ID", "GJI_INSPECTION", "ID");
            //-----

            //-----Основание проверки ГЖИ - Инспекционная проверка
            Database.AddTable(
                "GJI_INSPECTION_INSCHECK",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("PLAN_ID", DbType.Int64, 22),
                new Column("INSCHECK_DATE", DbType.DateTime),
                new Column("DATE_START", DbType.DateTime),
                new Column("AREA", DbType.Decimal),
                new Column("TYPE_FACT", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("REASON", DbType.String, 500));
            Database.AddIndex("IND_GJI_INSPECT_INSCH_PLAN", false, "GJI_INSPECTION_INSCHECK", "PLAN_ID");
            Database.AddForeignKey("FK_GJI_INSPECT_INSCH_PLAN", "GJI_INSPECTION_INSCHECK", "PLAN_ID", "GJI_DICT_PLANINSCHECK", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_INSCH_INS", "GJI_INSPECTION_INSCHECK", "ID", "GJI_INSPECTION", "ID");
            //-----

            //-----Основание проверки - постановления прокуратуры
            Database.AddTable(
                "GJI_INSPECTION_RESOLPROS",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull));
            Database.AddForeignKey("FK_GJI_INSPECT_RESPROS_INS", "GJI_INSPECTION_RESOLPROS", "ID", "GJI_INSPECTION", "ID");
            //-----

            //-----Основание проверки ГЖИ - Обращение граждан
            Database.AddTable(
                "GJI_INSPECTION_STATEMENT",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("STATEMENT_DATE", DbType.DateTime),
                new Column("REVENUE_DATE", DbType.DateTime),
                new Column("REVENUE_SOURCE_ID", DbType.Int64, 22),
                new Column("REVENUE_SOURCE_NUMBER", DbType.String, 50),
                new Column("REVENUE_FORM_ID", DbType.Int64, 22),
                new Column("CHECK_TIME", DbType.DateTime),
                new Column("SURETY_ID", DbType.Int64, 22),
                new Column("SURETY_RESOLVE_ID", DbType.Int64, 22),
                new Column("SURETY_DATE", DbType.DateTime),
                new Column("EXECUTANT_ID", DbType.Int64, 22),
                new Column("TESTER_ID", DbType.Int64, 22),
                new Column("EXECUTE_DATE", DbType.DateTime),
                new Column("ZONAINSP_ID", DbType.Int64, 22),
                new Column("PERSON_INSPECTION", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("FORM_CHECK", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("MANAGING_ORG_ID", DbType.Int64, 22));
            Database.AddIndex("IND_GJI_INSPECT_STAT_RES", false, "GJI_INSPECTION_STATEMENT", "REVENUE_SOURCE_ID");
            Database.AddIndex("IND_GJI_INSPECT_STAT_REF", false, "GJI_INSPECTION_STATEMENT", "REVENUE_FORM_ID");
            Database.AddIndex("IND_GJI_INSPECT_STAT_SUR", false, "GJI_INSPECTION_STATEMENT", "SURETY_ID");
            Database.AddIndex("IND_GJI_INSPECT_STAT_SURR", false, "GJI_INSPECTION_STATEMENT", "SURETY_RESOLVE_ID");
            Database.AddIndex("IND_GJI_INSPECT_STAT_EXE", false, "GJI_INSPECTION_STATEMENT", "EXECUTANT_ID");
            Database.AddIndex("IND_GJI_INSPECT_STAT_TES", false, "GJI_INSPECTION_STATEMENT", "TESTER_ID");
            Database.AddIndex("IND_GJI_INSPECT_STAT_ZON", false, "GJI_INSPECTION_STATEMENT", "ZONAINSP_ID");
            Database.AddIndex("IND_GJI_INSPECT_STAT_MORG", false, "GJI_INSPECTION_STATEMENT", "MANAGING_ORG_ID");
            Database.AddForeignKey("FK_GJI_INSPECT_STAT_MORG", "GJI_INSPECTION_STATEMENT", "MANAGING_ORG_ID", "GKH_MANAGING_ORGANIZATION", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_STAT_ZON", "GJI_INSPECTION_STATEMENT", "ZONAINSP_ID", "GKH_DICT_ZONAINSP", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_STAT_EXE", "GJI_INSPECTION_STATEMENT", "EXECUTANT_ID", "GKH_DICT_INSPECTOR", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_STAT_TES", "GJI_INSPECTION_STATEMENT", "TESTER_ID", "GKH_DICT_INSPECTOR", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_STAT_SURR", "GJI_INSPECTION_STATEMENT", "SURETY_RESOLVE_ID", "GJI_DICT_RESOLVE", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_STAT_SUR", "GJI_INSPECTION_STATEMENT", "SURETY_ID", "GKH_DICT_INSPECTOR", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_STAT_REF", "GJI_INSPECTION_STATEMENT", "REVENUE_FORM_ID", "GJI_DICT_REVENUEFORM", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_STAT_RES", "GJI_INSPECTION_STATEMENT", "REVENUE_SOURCE_ID", "GJI_DICT_REVENUESOURCE", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_STAT_INS", "GJI_INSPECTION_STATEMENT", "ID", "GJI_INSPECTION", "ID");
            //-----

            //-----Проверка ГЖИ по обращениям граждан - Запрос
            Database.AddEntityTable(
                "GJI_STATEMENT_REQUEST",
                new Column("INSPECTION_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("COMPETENT_ORG_ID", DbType.Int64, 22),
                new Column("DOCUMENT_NAME", DbType.String, 300),
                new Column("DOCUMENT_NUM", DbType.String, 300),
                new Column("DOCUMENT_DATE", DbType.Date),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("PERFORMANCE_DATE", DbType.Date),
                new Column("PERFORMANCE_FACT_DATE", DbType.Date),
                new Column("FILE_INFO_ID", DbType.Int64, 22),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_STAT_REQ_INS", false, "GJI_STATEMENT_REQUEST", "INSPECTION_ID");
            Database.AddIndex("IND_GJI_STAT_REQ_CORG", false, "GJI_STATEMENT_REQUEST", "COMPETENT_ORG_ID");
            Database.AddForeignKey("FK_GJI_STAT_REQ_INS", "GJI_STATEMENT_REQUEST", "INSPECTION_ID", "GJI_INSPECTION", "ID");
            Database.AddForeignKey("FK_GJI_STAT_REQ_CORG", "GJI_STATEMENT_REQUEST", "COMPETENT_ORG_ID", "GJI_DICT_COMPETENT_ORG", "ID");
            Database.AddForeignKey("FK_GJI_STAT_REQ_FILE", "GJI_STATEMENT_REQUEST", "FILE_INFO_ID", "B4_FILE_INFO", "ID");
            //-----

            //-----Тематика обращения проверки по обращениям граждан
            Database.AddEntityTable(
                "GJI_STATEMENT_STATSUBJECT",
                new Column("INSPECTION_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("STATEMENT_SUBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_STAT_SUBJ_INS", false, "GJI_STATEMENT_STATSUBJECT", "INSPECTION_ID");
            Database.AddIndex("IND_GJI_STAT_SUBJ_SUBJ", false, "GJI_STATEMENT_STATSUBJECT", "STATEMENT_SUBJECT_ID");
            Database.AddForeignKey("FK_GJI_STAT_SUBJ_INS", "GJI_STATEMENT_STATSUBJECT", "INSPECTION_ID", "GJI_INSPECTION", "ID");
            Database.AddForeignKey("FK_GJI_STAT_SUBJ_SUBJ", "GJI_STATEMENT_STATSUBJECT", "STATEMENT_SUBJECT_ID", "GJI_DICT_STATEMENT_SUBJ", "ID");
            //-----

            //-----Проверка ГЖИ по обращениям граждан - Ответ
            Database.AddEntityTable(
                "GJI_STATEMENT_ANSWER",
                new Column("INSPECTION_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("ANSWER_CONTENT_ID", DbType.Int64, 22), // Содержание ответа
                new Column("REVENUE_SOURCE_ID", DbType.Int64, 22), // Адресат
                new Column("INSPECTOR_ID", DbType.Int64, 22), // Исполнитель
                new Column("DOCUMENT_NAME", DbType.String, 300),
                new Column("DOCUMENT_NUM", DbType.String, 300),
                new Column("DOCUMENT_DATE", DbType.Date),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("FILE_INFO_ID", DbType.Int64, 22),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_STAT_ANSW_INSN", false, "GJI_STATEMENT_ANSWER", "INSPECTION_ID");
            Database.AddIndex("IND_GJI_STAT_ANSW_CON", false, "GJI_STATEMENT_ANSWER", "ANSWER_CONTENT_ID");
            Database.AddIndex("IND_GJI_STAT_ANSW_REV", false, "GJI_STATEMENT_ANSWER", "REVENUE_SOURCE_ID");
            Database.AddIndex("IND_GJI_STAT_ANSW_INSR", false, "GJI_STATEMENT_ANSWER", "INSPECTOR_ID");
            Database.AddIndex("IND_GJI_STAT_ANSW_FILE", false, "GJI_STATEMENT_ANSWER", "FILE_INFO_ID");
            Database.AddForeignKey("FK_GJI_STAT_ANSW_INSN", "GJI_STATEMENT_ANSWER", "INSPECTION_ID", "GJI_INSPECTION", "ID");
            Database.AddForeignKey("FK_GJI_STAT_ANSW_CON", "GJI_STATEMENT_ANSWER", "ANSWER_CONTENT_ID", "GJI_DICT_ANSWER_CONTENT", "ID");
            Database.AddForeignKey("FK_GJI_STAT_ANSW_REV", "GJI_STATEMENT_ANSWER", "REVENUE_SOURCE_ID", "GJI_DICT_REVENUESOURCE", "ID");
            Database.AddForeignKey("FK_GJI_STAT_ANSW_INSR", "GJI_STATEMENT_ANSWER", "INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID");
            Database.AddForeignKey("FK_GJI_STAT_ANSW_FILE", "GJI_STATEMENT_ANSWER", "FILE_INFO_ID", "B4_FILE_INFO", "ID");
            //-----

            //-----Подготовка к отопительному сезону
            Database.AddEntityTable(
                "GJI_HEATSEASON",
                new Column("REALITY_OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("HEATSEASON_PERIOD_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("DATE_HEAT", DbType.DateTime),
                new Column("HEATING_SYSTEM", DbType.Int32, 4, ColumnProperty.NotNull, 10));
            Database.AddIndex("IND_GJI_HEATS_OBJ", false, "GJI_HEATSEASON", "REALITY_OBJECT_ID");
            Database.AddIndex("IND_GJI_HEATS_PRD", false, "GJI_HEATSEASON", "HEATSEASON_PERIOD_ID");
            Database.AddForeignKey("FK_GJI_HEATS_OBJ", "GJI_HEATSEASON", "REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID");
            Database.AddForeignKey("FK_GJI_HEATS_PRD", "GJI_HEATSEASON", "HEATSEASON_PERIOD_ID", "GJI_DICT_HEATSEASONPERIOD", "ID");
            //-----

            //-----Документ отопительного сезона
            Database.AddEntityTable(
                "GJI_HEATSEASON_DOCUMENT",
                new Column("HEATSEASON_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("TYPE_DOCUMENT", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("DOCUMENT_NUMBER", DbType.String, 50),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("STATE_ID", DbType.Int64, 22));
            Database.AddIndex("IND_GJI_HEATS_DOC_FILE", false, "GJI_HEATSEASON_DOCUMENT", "FILE_ID");
            Database.AddIndex("IND_GJI_HEATS_DOC_SEA", false, "GJI_HEATSEASON_DOCUMENT", "HEATSEASON_ID");
            Database.AddIndex("IND_GJI_HEATS_DOC_ST", false, "GJI_HEATSEASON_DOCUMENT", "STATE_ID");
            Database.AddForeignKey("FK_GJI_HEATS_DOC_ST", "GJI_HEATSEASON_DOCUMENT", "STATE_ID", "B4_STATE", "ID");
            Database.AddForeignKey("FK_GJI_HEATS_DOC_SEA", "GJI_HEATSEASON_DOCUMENT", "HEATSEASON_ID", "GJI_HEATSEASON", "ID");
            Database.AddForeignKey("FK_GJI_HEATS_DOC_FILE", "GJI_HEATSEASON_DOCUMENT", "FILE_ID", "B4_FILE_INFO", "ID");
            //-----

            //-----Основание проверки -  подготовка к отопительному сезону
            Database.AddTable(
                "GJI_INSPECTION_HEATSEASON",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("HEATSEASON_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull));
            Database.AddIndex("IND_GJI_INSPECT_HSEAS_SEA", false, "GJI_INSPECTION_HEATSEASON", "HEATSEASON_ID");
            Database.AddForeignKey("FK_GJI_INSPECT_HSEAS_SEA", "GJI_INSPECTION_HEATSEASON", "HEATSEASON_ID", "GJI_HEATSEASON", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_HSEAS_INS", "GJI_INSPECTION_HEATSEASON", "ID", "GJI_INSPECTION", "ID");
            //-----

            //-----Основание по умолчанию
            Database.AddTable(
                "GJI_INSPECTION_BASEDEF",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull));
            Database.AddForeignKey("FK_GJI_INSPECT_BDEF_ID", "GJI_INSPECTION_BASEDEF", "ID", "GJI_INSPECTION", "ID");
            //-----

            //-----Этап проверки ГЖИ
            Database.AddEntityTable(
                "GJI_INSPECTION_STAGE",
                new Column("INSPECTION_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("PARENT_ID", DbType.Int64, 22),
                new Column("TYPE_STAGE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("POSITION", DbType.Int32, ColumnProperty.NotNull, 1),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_INSPECT_STG_INS", false, "GJI_INSPECTION_STAGE", "INSPECTION_ID");
            Database.AddIndex("IND_GJI_INSPECT_STG_PRT", false, "GJI_INSPECTION_STAGE", "PARENT_ID");
            Database.AddForeignKey("FK_GJI_INSPECT_STG_INS", "GJI_INSPECTION_STAGE", "INSPECTION_ID", "GJI_INSPECTION", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_STG_PRT", "GJI_INSPECTION_STAGE", "PARENT_ID", "GJI_INSPECTION_STAGE", "ID");
            //-----

            //-----Проверяемые дома в инспекционной проверке
            Database.AddEntityTable(
                "GJI_INSPECTION_ROBJECT",
                new Column("INSPECTION_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("REALITY_OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_INSPECT_RO_INS", false, "GJI_INSPECTION_ROBJECT", "INSPECTION_ID");
            Database.AddIndex("IND_GJI_INSPECT_RO_OBJ", false, "GJI_INSPECTION_ROBJECT", "REALITY_OBJECT_ID");
            Database.AddForeignKey("FK_GJI_INSPECT_RO_INS", "GJI_INSPECTION_ROBJECT", "INSPECTION_ID", "GJI_INSPECTION", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_RO_OBJ", "GJI_INSPECTION_ROBJECT", "REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID");
            //-----

            //-----Документ ГЖИ
            Database.AddEntityTable(
                "GJI_DOCUMENT",
                new Column("INSPECTION_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("STATE_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("TYPE_DOCUMENT", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("DOCUMENT_NUMBER", DbType.String, 50),
                new Column("DOCUMENT_NUM", DbType.Int32),
                new Column("DOCUMENT_SUBNUM", DbType.Int32),
                new Column("DOCUMENT_YEAR", DbType.Int32),
                new Column("EXTERNAL_ID", DbType.String, 36),
                new Column("STAGE_ID", DbType.Int64,22));
            Database.AddIndex("IND_GJI_DOCUMENT_INS", false, "GJI_DOCUMENT", "INSPECTION_ID");
            Database.AddIndex("IND_GJI_DOCUMENT_STG", false, "GJI_DOCUMENT", "STAGE_ID");
            Database.AddIndex("IND_GJI_DOCUMENT_STT", false, "GJI_DOCUMENT", "STATE_ID");
            Database.AddForeignKey("FK_GJI_DOCUMENT_INS", "GJI_DOCUMENT", "INSPECTION_ID", "GJI_INSPECTION", "ID");
            Database.AddForeignKey("FK_GJI_DOCUMENT_STG", "GJI_DOCUMENT", "STAGE_ID", "GJI_INSPECTION_STAGE", "ID");
            Database.AddForeignKey("FK_GJI_DOCUMENT_STT", "GJI_DOCUMENT", "STATE_ID", "B4_STATE", "ID");
            //-----


            //-----Основание проверки ГЖИ - По распоряжению руководителя
            Database.AddTable(
                "GJI_INSPECTION_DISPHEAD",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("HEAD_ID", DbType.Int64, 22),
                new Column("PREV_DOCUMENT_ID", DbType.Int64, 22),
                new Column("DISPHEAD_DATE", DbType.DateTime),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("DOCUMENT_NAME", DbType.String, 300),
                new Column("DOCUMENT_NUMBER", DbType.String, 50),
                new Column("IS_INSPECTION_SURVEY", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("TYPE_BASE_DISPHEAD", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("TYPE_FORM", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("FILE_INFO_ID", DbType.Int64, 22));
            Database.AddIndex("IND_GJI_INSPECT_DH_HEAD", false, "GJI_INSPECTION_DISPHEAD", "HEAD_ID");
            Database.AddIndex("IND_GJI_INSPECT_DH_DOC", false, "GJI_INSPECTION_DISPHEAD", "PREV_DOCUMENT_ID");
            Database.AddIndex("IND_GJI_INSPECT_DH_FILE", false, "GJI_INSPECTION_DISPHEAD", "FILE_INFO_ID");
            Database.AddForeignKey("FK_GJI_INSPECT_DH_FILE", "GJI_INSPECTION_DISPHEAD", "FILE_INFO_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_DH_DOC", "GJI_INSPECTION_DISPHEAD", "PREV_DOCUMENT_ID", "GJI_DOCUMENT", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_DH_HEAD", "GJI_INSPECTION_DISPHEAD", "HEAD_ID", "GKH_DICT_INSPECTOR", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_DH_INS", "GJI_INSPECTION_DISPHEAD", "ID", "GJI_INSPECTION", "ID");
            //----

            //----Инспектор Документа ГЖИ
            Database.AddEntityTable(
                "GJI_DOCUMENT_INSPECTOR",
                new Column("DOCUMENT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("INSPECTOR_ID", DbType.Int64, 22),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_DOCUMENT_INS_DOC", false, "GJI_DOCUMENT_INSPECTOR", "DOCUMENT_ID");
            Database.AddIndex("IND_GJI_DOCUMENT_INS_INS", false, "GJI_DOCUMENT_INSPECTOR", "INSPECTOR_ID");
            Database.AddForeignKey("FK_GJI_DOCUMENT_INS_INS", "GJI_DOCUMENT_INSPECTOR", "INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID");
            Database.AddForeignKey("FK_GJI_DOCUMENT_INS_DOC", "GJI_DOCUMENT_INSPECTOR", "DOCUMENT_ID", "GJI_DOCUMENT", "ID");
            //-----

            //-----Связь документов ГЖИ родителя с дочерним
            Database.AddEntityTable(
                "GJI_DOCUMENT_CHILDREN",
                new Column("PARENT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("CHILDREN_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_DOCUMENT_CH_PAR", false, "GJI_DOCUMENT_CHILDREN", "PARENT_ID");
            Database.AddIndex("IND_GJI_DOCUMENT_CH_CHI", false, "GJI_DOCUMENT_CHILDREN", "CHILDREN_ID");
            Database.AddForeignKey("FK_GJI_DOCUMENT_CH_PAR", "GJI_DOCUMENT_CHILDREN", "PARENT_ID", "GJI_DOCUMENT", "ID");
            Database.AddForeignKey("FK_GJI_DOCUMENT_CH_CHI", "GJI_DOCUMENT_CHILDREN", "CHILDREN_ID", "GJI_DOCUMENT", "ID");
            //-----

            //-----Таблица связи Документов ГЖИ
            Database.AddEntityTable(
                "GJI_DOCUMENT_REFERENCE",
                new Column("DOCUMENT1_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("DOCUMENT2_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("TYPE_REFERENCE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_DOCUMENT_REF_D1", false, "GJI_DOCUMENT_REFERENCE", "DOCUMENT1_ID");
            Database.AddIndex("IND_GJI_DOCUMENT_REF_D2", false, "GJI_DOCUMENT_REFERENCE", "DOCUMENT2_ID");
            Database.AddForeignKey("FK_GJI_DOCUMENT_REF_D1", "GJI_DOCUMENT_REFERENCE", "DOCUMENT1_ID", "GJI_DOCUMENT", "ID");
            Database.AddForeignKey("FK_GJI_DOCUMENT_REF_D2", "GJI_DOCUMENT_REFERENCE", "DOCUMENT2_ID", "GJI_DOCUMENT", "ID");
            //-----

            //-----Распоряжение
            Database.AddTable(
                "GJI_DISPOSAL",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DATE_START", DbType.DateTime),
                new Column("DATE_END", DbType.DateTime),
                new Column("TYPE_DISPOSAL", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("TYPE_AGRPROSECUTOR", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("TYPE_AGRRESULT", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("TYPE_CHECK", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("ISSUED_DISPOSAL_ID", DbType.Int64, 22),
                new Column("RESPONSIBLE_EXECUT_ID", DbType.Int64, 22),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("OBJECT_VISIT_START", DbType.DateTime),
                new Column("OBJECT_VISIT_END", DbType.DateTime));
            Database.AddIndex("IND_GJI_DISP_ISS", false, "GJI_DISPOSAL", "ISSUED_DISPOSAL_ID");
            Database.AddIndex("IND_GJI_DISP_RES", false, "GJI_DISPOSAL", "RESPONSIBLE_EXECUT_ID");
            Database.AddForeignKey("FK_GJI_DISP_DOC", "GJI_DISPOSAL", "ID", "GJI_DOCUMENT", "ID");
            Database.AddForeignKey("FK_GJI_DISP_ISS", "GJI_DISPOSAL", "ISSUED_DISPOSAL_ID", "GKH_DICT_INSPECTOR", "ID");
            Database.AddForeignKey("FK_GJI_DISP_RES", "GJI_DISPOSAL", "RESPONSIBLE_EXECUT_ID", "GKH_DICT_INSPECTOR", "ID");
            //-----

            //-----Предоставляемый документ распоряжения
            Database.AddEntityTable(
                "GJI_DISPOSAL_PROVDOC",
                new Column("DISPOSAL_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("PROVIDED_DOC_ID", DbType.Int64, 22, ColumnProperty.NotNull));
            Database.AddIndex("IND_GJI_DISP_PROVD_DOC", false, "GJI_DISPOSAL_PROVDOC", "DISPOSAL_ID");
            Database.AddIndex("IND_GJI_DISP_PROVD_PRD", false, "GJI_DISPOSAL_PROVDOC", "PROVIDED_DOC_ID");
            Database.AddForeignKey("FK_GJI_DISP_PROVD_DOC", "GJI_DISPOSAL_PROVDOC", "DISPOSAL_ID", "GJI_DISPOSAL", "ID");
            Database.AddForeignKey("FK_GJI_DISP_PROVD_PRD", "GJI_DISPOSAL_PROVDOC", "PROVIDED_DOC_ID", "GJI_DICT_PROVIDEDDOCUMENT", "ID");
            //-----

            //-----Типы обследования распоряжения
            Database.AddEntityTable(
                "GJI_DISPOSAL_TYPESURVEY",
                new Column("DISPOSAL_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("TYPESURVEY_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_DISP_TS_TS", false, "GJI_DISPOSAL_TYPESURVEY", "TYPESURVEY_ID");
            Database.AddIndex("IND_GJI_DISP_TS_DOC", false, "GJI_DISPOSAL_TYPESURVEY", "DISPOSAL_ID");
            Database.AddForeignKey("FK_GJI_DISP_TS_DOC", "GJI_DISPOSAL_TYPESURVEY", "DISPOSAL_ID", "GJI_DISPOSAL", "ID");
            Database.AddForeignKey("FK_GJI_DISP_TS_TS", "GJI_DISPOSAL_TYPESURVEY", "TYPESURVEY_ID", "GJI_DICT_TYPESURVEY", "ID");
            //-----

            //----- Эксперты распоряжения
            Database.AddEntityTable(
                "GJI_DISPOSAL_EXPERT",
                new Column("DISPOSAL_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXPERT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_DISP_EXP_EXP", false, "GJI_DISPOSAL_EXPERT", "EXPERT_ID");
            Database.AddIndex("IND_GJI_DISP_EXP_DOC", false, "GJI_DISPOSAL_EXPERT", "DISPOSAL_ID");
            Database.AddForeignKey("FK_GJI_DISP_EXP_DOC", "GJI_DISPOSAL_EXPERT", "DISPOSAL_ID", "GJI_DISPOSAL", "ID");
            Database.AddForeignKey("FK_GJI_DISP_EXP_EXP", "GJI_DISPOSAL_EXPERT", "EXPERT_ID", "GJI_DICT_EXPERT", "ID");
            //-----

            //-----Приложения распоряжения
            Database.AddEntityTable(
                "GJI_DISPOSAL_ANNEX",
                new Column("DISPOSAL_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_DISP_ANNEX_FILE", false, "GJI_DISPOSAL_ANNEX", "FILE_ID");
            Database.AddIndex("IND_GJI_DISP_ANNEX_DOC", false, "GJI_DISPOSAL_ANNEX", "DISPOSAL_ID");
            Database.AddForeignKey("FK_GJI_DISP_ANNEX_DOC", "GJI_DISPOSAL_ANNEX", "DISPOSAL_ID", "GJI_DISPOSAL", "ID");
            Database.AddForeignKey("FK_GJI_DISP_ANNEX_FILE", "GJI_DISPOSAL_ANNEX", "FILE_ID", "B4_FILE_INFO", "ID");
            //-----

            //-----Нарушение проверки
            Database.AddEntityTable(
                "GJI_INSPECTION_VIOLATION",
                new Column("INSPECTION_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("REALITY_OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("VIOLATION_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("DATE_PLAN_REMOVAL", DbType.DateTime),
                new Column("DATE_FACT_REMOVAL", DbType.DateTime),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_INSPECT_VIO_INS", false, "GJI_INSPECTION_VIOLATION", "INSPECTION_ID");
            Database.AddIndex("IND_GJI_INSPECT_VIO_OBJ", false, "GJI_INSPECTION_VIOLATION", "REALITY_OBJECT_ID");
            Database.AddIndex("IND_GJI_INSPECT_VIO_VIO", false, "GJI_INSPECTION_VIOLATION", "VIOLATION_ID");
            Database.AddForeignKey("FK_GJI_INSPECT_VIO_INS", "GJI_INSPECTION_VIOLATION", "INSPECTION_ID", "GJI_INSPECTION", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_VIO_OBJ", "GJI_INSPECTION_VIOLATION", "REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_VIO_VIO", "GJI_INSPECTION_VIOLATION", "VIOLATION_ID", "GJI_DICT_VIOLATION", "ID");
            //-----

            //-----Этап нарушения в проверке ГЖИ
            Database.AddEntityTable(
                "GJI_INSPECTION_VIOL_STAGE",
                new Column("DOCUMENT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("INSPECTION_VIOL_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("TYPE_VIOL_STAGE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_INSPECT_VSTG_DOC", false, "GJI_INSPECTION_VIOL_STAGE", "DOCUMENT_ID");
            Database.AddIndex("IND_GJI_INSPECT_VSTG_VIO", false, "GJI_INSPECTION_VIOL_STAGE", "INSPECTION_VIOL_ID");
            Database.AddForeignKey("FK_GJI_INSPECT_VSTG_DOC", "GJI_INSPECTION_VIOL_STAGE", "DOCUMENT_ID", "GJI_DOCUMENT", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_VSTG_VIO", "GJI_INSPECTION_VIOL_STAGE", "INSPECTION_VIOL_ID", "GJI_INSPECTION_VIOLATION", "ID");
            //-----

            //-----Акт проверки
            Database.AddTable(
                "GJI_ACTCHECK",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("TYPE_ACTCHECK", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("AREA", DbType.Decimal),
                new Column("TO_PROSECUTOR", DbType.Int32, 4, ColumnProperty.NotNull, 30),
                new Column("DATE_TO_PROSECUTOR", DbType.DateTime),
                new Column("FLAT", DbType.String, 10));
            Database.AddForeignKey("FK_GJI_ACTC_DOC", "GJI_ACTCHECK", "ID", "GJI_DOCUMENT", "ID");
            //-----

            //-----Проверяемые дома в акте проверки
            Database.AddEntityTable(
                "GJI_ACTCHECK_ROBJECT",
                new Column("ACTCHECK_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("REALITY_OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("HAVE_VIOLATION", DbType.Int32, 4, ColumnProperty.NotNull, 30),
                new Column("EXTERNAL_ID", DbType.String, 36),
                new Column("DESCRIPTION", DbType.String, 1000));
            Database.AddIndex("IND_GJI_ACTC_RO_ACT", false, "GJI_ACTCHECK_ROBJECT", "ACTCHECK_ID");
            Database.AddIndex("IND_GJI_ACTC_RO_OBJ", false, "GJI_ACTCHECK_ROBJECT", "REALITY_OBJECT_ID");
            Database.AddForeignKey("FK_GJI_ACTC_RO_ACT", "GJI_ACTCHECK_ROBJECT", "ACTCHECK_ID", "GJI_ACTCHECK", "ID");
            Database.AddForeignKey("FK_GJI_ACTC_RO_OBJ", "GJI_ACTCHECK_ROBJECT", "REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID");
            //-----

            //-----Инспектируемые части в акте проверки
            Database.AddEntityTable(
                "GJI_ACTCHECK_INSPECTPART",
                new Column("ACTCHECK_ID", DbType.Int64, 22),
                new Column("INSPECTIONPART_ID", DbType.Int64, 22),
                new Column("CHARACTER", DbType.String, 300),
                new Column("DESCRIPTION", DbType.String, 500));
            Database.AddIndex("IND_GJI_ACTC_INS_ACT", false, "GJI_ACTCHECK_INSPECTPART", "INSPECTIONPART_ID");
            Database.AddIndex("IND_GJI_ACTC_INS_PART", false, "GJI_ACTCHECK_INSPECTPART", "ACTCHECK_ID");
            Database.AddForeignKey("FK_GJI_ACTC_INS_PART", "GJI_ACTCHECK_INSPECTPART", "INSPECTIONPART_ID", "GJI_DICT_INSPECTEDPART", "ID");
            Database.AddForeignKey("FK_GJI_ACTC_INS_ACT", "GJI_ACTCHECK_INSPECTPART", "ACTCHECK_ID", "GJI_ACTCHECK", "ID");
            //-----

            //-----Нарушения выявленные входе проверки заносятся в нарушения акта проверки
            Database.AddTable(
                "GJI_ACTCHECK_VIOLAT",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("ACTCHECK_ROBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("DATE_PLAN_REMOVAL", DbType.DateTime));
            Database.AddIndex("IND_GJI_ACTC_VIOL_OBJ", false, "GJI_ACTCHECK_VIOLAT", "ACTCHECK_ROBJECT_ID");
            Database.AddForeignKey("FK_GJI_ACTC_VIOL_STG", "GJI_ACTCHECK_VIOLAT", "ID", "GJI_INSPECTION_VIOL_STAGE", "ID");
            Database.AddForeignKey("FK_GJI_ACTC_VIOL_OBJ", "GJI_ACTCHECK_VIOLAT", "ACTCHECK_ROBJECT_ID", "GJI_ACTCHECK_ROBJECT", "ID");
            //-----

            //-----Определения акта проверки
            Database.AddEntityTable(
                "GJI_ACTCHECK_DEFINITION",
                new Column("ACTCHECK_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("ISSUED_DEFINITION_ID", DbType.Int64, 22),
                new Column("EXECUTION_DATE", DbType.DateTime),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("TYPE_DEFINITION", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_ACTC_DEF_DOC", false, "GJI_ACTCHECK_DEFINITION", "ACTCHECK_ID");
            Database.AddIndex("IND_GJI_ACTC_DEF_ISD", false, "GJI_ACTCHECK_DEFINITION", "ISSUED_DEFINITION_ID");
            Database.AddForeignKey("FK_GJI_ACTC_DEF_ISD", "GJI_ACTCHECK_DEFINITION", "ISSUED_DEFINITION_ID", "GKH_DICT_INSPECTOR", "ID");
            Database.AddForeignKey("FK_GJI_ACTC_DEF_DOC", "GJI_ACTCHECK_DEFINITION", "ACTCHECK_ID", "GJI_ACTCHECK", "ID");
            //-----

            //-----Лица, присутсвующие при проверке акта проверки (или свидетели)
            Database.AddEntityTable(
                "GJI_ACTCHECK_WITNESS",
                new Column("ACTCHECK_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("POSITION", DbType.String, 300),
                new Column("IS_FAMILIAR", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("FIO", DbType.String, ColumnProperty.NotNull, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_ACTC_WIT_DOC", false, "GJI_ACTCHECK_WITNESS", "ACTCHECK_ID");
            Database.AddForeignKey("FK_GJI_ACTC_WIT_DOC", "GJI_ACTCHECK_WITNESS", "ACTCHECK_ID", "GJI_ACTCHECK", "ID");
            //-----

            //-----Приложения акта проверки
            Database.AddEntityTable(
                "GJI_ACTCHECK_ANNEX",
                new Column("ACTCHECK_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_ACTC_ANNEX_FILE", false, "GJI_ACTCHECK_ANNEX", "FILE_ID");
            Database.AddIndex("IND_GJI_ACTC_ANNEX_DOC", false, "GJI_ACTCHECK_ANNEX", "ACTCHECK_ID");
            Database.AddForeignKey("FK_GJI_ACTC_ANNEX_DOC", "GJI_ACTCHECK_ANNEX", "ACTCHECK_ID", "GJI_ACTCHECK", "ID");
            Database.AddForeignKey("FK_GJI_ACTC_ANNEX_FILE", "GJI_ACTCHECK_ANNEX", "FILE_ID", "B4_FILE_INFO", "ID");
            //-----

            //-----Акт обследования
            Database.AddTable(
                "GJI_ACTSURVEY",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("AREA", DbType.Decimal),
                new Column("FLAT", DbType.String, 10),
                new Column("REASON", DbType.String, 300),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("FACT_SURVEYED", DbType.Int32, 4, ColumnProperty.NotNull, 10));
            Database.AddForeignKey("FK_GJI_ACTS_DOC", "GJI_ACTSURVEY", "ID", "GJI_DOCUMENT", "ID");
            //-----

            //-----Дома в акте обследования
            Database.AddEntityTable(
                "GJI_ACTSURVEY_ROBJECT",
                new Column("ACTSURVEY_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("REALITY_OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull));
            Database.AddIndex("IND_GJI_ACTS_RO_ACT", false, "GJI_ACTSURVEY_ROBJECT", "ACTSURVEY_ID");
            Database.AddIndex("IND_GJI_ACTS_RO_OBJ", false, "GJI_ACTSURVEY_ROBJECT", "REALITY_OBJECT_ID");
            Database.AddForeignKey("FK_GJI_ACTS_RO_ACT", "GJI_ACTSURVEY_ROBJECT", "ACTSURVEY_ID", "GJI_ACTSURVEY", "ID");
            Database.AddForeignKey("FK_GJI_ACTS_RO_OBJ", "GJI_ACTSURVEY_ROBJECT", "REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID");
            //-----

            //-----Сведения о собственниках в акте обследования
            Database.AddEntityTable(
                "GJI_ACTSURVEY_OWNER",
                new Column("ACTSURVEY_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FIO", DbType.String, 300, ColumnProperty.NotNull),
                new Column("POSITION", DbType.String, 300),
                new Column("WORK_PLACE", DbType.String, 300),
                new Column("DOCUMENT_NAME", DbType.String, 300));
            Database.AddIndex("IND_GJI_ACTS_OWNER_DOC", false, "GJI_ACTSURVEY_OWNER", "ACTSURVEY_ID");
            Database.AddForeignKey("FK_GJI_ACTS_OWNER_DOC", "GJI_ACTSURVEY_OWNER", "ACTSURVEY_ID", "GJI_ACTSURVEY", "ID");
            //-----

            //-----Приложения акта обследования
            Database.AddEntityTable(
                "GJI_ACTSURVEY_ANNEX",
                new Column("ACTSURVEY_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 500));
            Database.AddIndex("IND_GJI_ACTS_ANNEX_FILE", false, "GJI_ACTSURVEY_ANNEX", "FILE_ID");
            Database.AddIndex("IND_GJI_ACTS_ANNEX_DOC", false, "GJI_ACTSURVEY_ANNEX", "ACTSURVEY_ID");
            Database.AddForeignKey("FK_GJI_ACTS_ANNEX_DOC", "GJI_ACTSURVEY_ANNEX", "ACTSURVEY_ID", "GJI_ACTSURVEY", "ID");
            Database.AddForeignKey("FK_GJI_ACTS_ANNEX_FILE", "GJI_ACTSURVEY_ANNEX", "FILE_ID", "B4_FILE_INFO", "ID");
            //-----

            //-----Фото акта обследования
            Database.AddEntityTable(
                "GJI_ACTSURVEY_PHOTO",
                new Column("ACTSURVEY_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("IS_PRINT", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("IMAGE_GROUP", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("IMAGE_DATE", DbType.DateTime),
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 500));
            Database.AddIndex("IND_GJI_ACTS_PHOTO_NAME", false, "GJI_ACTSURVEY_PHOTO", "NAME");
            Database.AddIndex("IND_GJI_ACTS_PHOTO_FILE", false, "GJI_ACTSURVEY_PHOTO", "FILE_ID");
            Database.AddIndex("IND_GJI_ACTS_PHOTO_DOC", false, "GJI_ACTSURVEY_PHOTO", "ACTSURVEY_ID");
            Database.AddForeignKey("FK_GJI_ACTS_PHOTO_DOC", "GJI_ACTSURVEY_PHOTO", "ACTSURVEY_ID", "GJI_ACTSURVEY", "ID");
            Database.AddForeignKey("FK_GJI_ACTS_PHOTO_FILE", "GJI_ACTSURVEY_PHOTO", "FILE_ID", "B4_FILE_INFO", "ID");
            //-----

            //-----Инспектируемая часть акта обследования
            Database.AddEntityTable(
                "GJI_ACTSURVEY_INSPECTPART",
                new Column("ACTSURVEY_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("INSPECTIONPART_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("CHARACTER", DbType.String, 300),
                new Column("DESCRIPTION", DbType.String, 500));
            Database.AddIndex("IND_GJI_ACTS_INSPART_DOC", false, "GJI_ACTSURVEY_INSPECTPART", "ACTSURVEY_ID");
            Database.AddIndex("IND_GJI_ACTS_INSPART_INP", false, "GJI_ACTSURVEY_INSPECTPART", "INSPECTIONPART_ID");
            Database.AddForeignKey("FK_GJI_ACTS_INSPART_INP", "GJI_ACTSURVEY_INSPECTPART", "INSPECTIONPART_ID", "GJI_DICT_INSPECTEDPART", "ID");
            Database.AddForeignKey("FK_GJI_ACTS_INSPART_DOC", "GJI_ACTSURVEY_INSPECTPART", "ACTSURVEY_ID", "GJI_ACTSURVEY", "ID");
            //-----

            //-----Предписание
            Database.AddTable(
                "GJI_PRESCRIPTION",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("EXECUTANT_ID", DbType.Int64, 22),
                new Column("CONTRAGENT_ID", DbType.Int64, 22),
                new Column("PHYSICAL_PERSON", DbType.String, 300),
                new Column("PHYSICAL_PERSON_INFO", DbType.String, 500),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull));
            Database.AddIndex("IND_GJI_PRESCR_EXE", false, "GJI_PRESCRIPTION", "EXECUTANT_ID");
            Database.AddIndex("IND_GJI_PRESCR_CTR", false, "GJI_PRESCRIPTION", "CONTRAGENT_ID");
            Database.AddForeignKey("FK_GJI_PRESCR_DOC", "GJI_PRESCRIPTION", "ID", "GJI_DOCUMENT", "ID");
            Database.AddForeignKey("FK_GJI_PRESCR_EXE", "GJI_PRESCRIPTION", "EXECUTANT_ID", "GJI_DICT_EXECUTANT", "ID");
            Database.AddForeignKey("FK_GJI_PRESCR_CTR", "GJI_PRESCRIPTION", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            //-----

            //-----Указание к устранению нарушений входе проверки заносятся в нарушения предписания
            Database.AddTable(
                "GJI_PRESCRIPTION_VIOLAT",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("ACTION", DbType.String, 500));
            Database.AddForeignKey("FK_GJI_PRESCR_VIOL_STG", "GJI_PRESCRIPTION_VIOLAT", "ID", "GJI_INSPECTION_VIOL_STAGE", "ID");
            //-----

            //-----Приложения предписания
            Database.AddEntityTable(
                "GJI_PRESCRIPTION_ANNEX",
                new Column("PRESCRIPTION_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_PRESCR_ANNEX_FILE", false, "GJI_PRESCRIPTION_ANNEX", "FILE_ID");
            Database.AddIndex("IND_GJI_PRESCR_ANNEX_DOC", false, "GJI_PRESCRIPTION_ANNEX", "PRESCRIPTION_ID");
            Database.AddForeignKey("FK_GJI_PRESCR_ANNEX_DOC", "GJI_PRESCRIPTION_ANNEX", "PRESCRIPTION_ID", "GJI_PRESCRIPTION", "ID");
            Database.AddForeignKey("FK_GJI_PRESCR_ANNEX_FILE", "GJI_PRESCRIPTION_ANNEX", "FILE_ID", "B4_FILE_INFO", "ID");
            //-----

            //-----Протокол
            Database.AddTable(
                "GJI_PROTOCOL",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("EXECUTANT_ID", DbType.Int64, 22),
                new Column("CONTRAGENT_ID", DbType.Int64, 22),
                new Column("PHYSICAL_PERSON", DbType.String, 300),
                new Column("PHYSICAL_PERSON_INFO", DbType.String, 500),
                new Column("DATE_TO_COURT", DbType.DateTime),
                new Column("TO_COURT", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull));
            Database.AddIndex("IND_GJI_PROT_EXE", false, "GJI_PROTOCOL", "EXECUTANT_ID");
            Database.AddIndex("IND_GJI_PROT_CTR", false, "GJI_PROTOCOL", "CONTRAGENT_ID");
            Database.AddForeignKey("FK_GJI_PROT_DOC", "GJI_PROTOCOL", "ID", "GJI_DOCUMENT", "ID");
            Database.AddForeignKey("FK_GJI_PROT_EXE", "GJI_PROTOCOL", "EXECUTANT_ID", "GJI_DICT_EXECUTANT", "ID");
            Database.AddForeignKey("FK_GJI_PROT_CTR", "GJI_PROTOCOL", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            //-----

            //-----Наказание за нарушения входе проверки заносятся в нарушения протокола
            Database.AddTable(
                "GJI_PROTOCOL_VIOLAT",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 500));
            Database.AddForeignKey("FK_GJI_PROT_VIOLAT_STG", "GJI_PROTOCOL_VIOLAT", "ID", "GJI_INSPECTION_VIOL_STAGE", "ID");
            //-----

            //-----Приложения протокола
            Database.AddEntityTable(
                "GJI_PROTOCOL_ANNEX",
                new Column("PROTOCOL_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_PROT_ANNEX_FILE", false, "GJI_PROTOCOL_ANNEX", "FILE_ID");
            Database.AddIndex("IND_GJI_PROT_ANNEX_DOC", false, "GJI_PROTOCOL_ANNEX", "PROTOCOL_ID");
            Database.AddForeignKey("FK_GJI_PROT_ANNEX_DOC", "GJI_PROTOCOL_ANNEX", "PROTOCOL_ID", "GJI_PROTOCOL", "ID");
            Database.AddForeignKey("FK_GJI_PROT_ANNEX_FILE", "GJI_PROTOCOL_ANNEX", "FILE_ID", "B4_FILE_INFO", "ID");
            //-----

            //-----Определения протокола
            Database.AddEntityTable(
                "GJI_PROTOCOL_DEFINITION",
                new Column("PROTOCOL_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("ISSUED_DEFINITION_ID", DbType.Int64, 22),
                new Column("EXECUTION_DATE", DbType.DateTime),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("TYPE_DEFINITION", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_PROT_DEF_DOC", false, "GJI_PROTOCOL_DEFINITION", "PROTOCOL_ID");
            Database.AddIndex("IND_GJI_PROT_DEF_ISD", false, "GJI_PROTOCOL_DEFINITION", "ISSUED_DEFINITION_ID");
            Database.AddForeignKey("FK_GJI_PROT_DEF_ISD", "GJI_PROTOCOL_DEFINITION", "ISSUED_DEFINITION_ID", "GKH_DICT_INSPECTOR", "ID");
            Database.AddForeignKey("FK_GJI_PROT_DEF_DOC", "GJI_PROTOCOL_DEFINITION", "PROTOCOL_ID", "GJI_PROTOCOL", "ID");
            //-----

            //-----Постановление
            Database.AddTable(
                "GJI_RESOLUTION",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("EXECUTANT_ID", DbType.Int64, 22),
                new Column("MUNICIPALITY_ID", DbType.Int64, 22),
                new Column("CONTRAGENT_ID", DbType.Int64, 22),
                new Column("SANCTION_ID", DbType.Int64, 22),
                new Column("OFFICIAL_ID", DbType.Int64, 22),
                new Column("PHYSICAL_PERSON", DbType.String, 300),
                new Column("PHYSICAL_PERSON_INFO", DbType.String, 500),
                new Column("DELIVERY_DATE", DbType.DateTime),
                new Column("TYPE_INITIATIVE_ORG", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("SECTOR_NUMBER", DbType.String, 50),
                new Column("PENALTY_AMOUNT", DbType.Decimal),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DATE_TRANSFER_SSP", DbType.Date),
                new Column("PAIDED", DbType.Int32, 4, ColumnProperty.NotNull, 30),
                new Column("DOCUMENT_NUM_SSP", DbType.String, 300));
            Database.AddIndex("IND_GJI_RESOL_SNC", false, "GJI_RESOLUTION", "SANCTION_ID");
            Database.AddIndex("IND_GJI_RESOL_EXE", false, "GJI_RESOLUTION", "EXECUTANT_ID");
            Database.AddIndex("IND_GJI_RESOL_MCP", false, "GJI_RESOLUTION", "MUNICIPALITY_ID");
            Database.AddIndex("IND_GJI_RESOL_CTR", false, "GJI_RESOLUTION", "CONTRAGENT_ID");
            Database.AddIndex("IND_GJI_RESOL_OFC", false, "GJI_RESOLUTION", "OFFICIAL_ID");
            Database.AddForeignKey("FK_GJI_RESOL_DOC", "GJI_RESOLUTION", "ID", "GJI_DOCUMENT", "ID");
            Database.AddForeignKey("FK_GJI_RESOL_OFC", "GJI_RESOLUTION", "OFFICIAL_ID", "GKH_DICT_INSPECTOR", "ID");
            Database.AddForeignKey("FK_GJI_RESOL_SNC", "GJI_RESOLUTION", "SANCTION_ID", "GJI_DICT_SANCTION", "ID");
            Database.AddForeignKey("FK_GJI_RESOL_EXE", "GJI_RESOLUTION", "EXECUTANT_ID", "GJI_DICT_EXECUTANT", "ID");
            Database.AddForeignKey("FK_GJI_RESOL_MCP", "GJI_RESOLUTION", "MUNICIPALITY_ID", "GKH_DICT_MUNICIPALITY", "ID");
            Database.AddForeignKey("FK_GJI_RESOL_CTR", "GJI_RESOLUTION", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            //-----

            //-----Приложения постановления
            Database.AddEntityTable(
                "GJI_RESOLUTION_ANNEX",
                new Column("RESOLUTION_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_RESOL_ANNEX_FILE", false, "GJI_RESOLUTION_ANNEX", "FILE_ID");
            Database.AddIndex("IND_GJI_RESOL_ANNEX_DOC", false, "GJI_RESOLUTION_ANNEX", "RESOLUTION_ID");
            Database.AddForeignKey("FK_GJI_RESOL_ANNEX_DOC", "GJI_RESOLUTION_ANNEX", "RESOLUTION_ID", "GJI_RESOLUTION", "ID");
            Database.AddForeignKey("FK_GJI_RESOL_ANNEX_FILE", "GJI_RESOLUTION_ANNEX", "FILE_ID", "B4_FILE_INFO", "ID");
            //-----

            //-----Определения постановления
            Database.AddEntityTable(
                "GJI_RESOLUTION_DEFINITION",
                new Column("RESOLUTION_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("ISSUED_DEFINITION_ID", DbType.Int64, 22),
                new Column("EXECUTION_DATE", DbType.DateTime),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("TYPE_DEFINITION", DbType.Int32, 4, ColumnProperty.NotNull, 10));
            Database.AddIndex("IND_GJI_RESOL_DEF_DOC", false, "GJI_RESOLUTION_DEFINITION", "RESOLUTION_ID");
            Database.AddIndex("IND_GJI_RESOL_DEF_ISD", false, "GJI_RESOLUTION_DEFINITION", "ISSUED_DEFINITION_ID");
            Database.AddForeignKey("FK_GJI_RESOL_DEF_ISD", "GJI_RESOLUTION_DEFINITION", "ISSUED_DEFINITION_ID", "GKH_DICT_INSPECTOR", "ID");
            Database.AddForeignKey("FK_GJI_RESOL_DEF_DOC", "GJI_RESOLUTION_DEFINITION", "RESOLUTION_ID", "GJI_RESOLUTION", "ID");
            //-----

            //-----Акт проверки устранения нарушений
            Database.AddTable(
                "GJI_ACTREMOVAL",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("TYPE_REMOVAL", DbType.Int32, 4, ColumnProperty.NotNull, 30),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("AREA", DbType.Decimal),
                new Column("FLAT", DbType.String, 10));
            Database.AddForeignKey("FK_GJI_ACTR_DOC", "GJI_ACTREMOVAL", "ID", "GJI_DOCUMENT", "ID");
            //-----

            //-----Устранение нарушений входе проверки заносятся в нарушения акта устранения
            Database.AddTable(
                "GJI_ACTREMOVAL_VIOLAT",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DATE_FACT_REMOVAL", DbType.DateTime));
            Database.AddForeignKey("FK_GJI_ACTR_VIOL_STG", "GJI_ACTREMOVAL_VIOLAT", "ID", "GJI_INSPECTION_VIOL_STAGE", "ID");
            //-----

            //-----Дата и время проведения проверки
            Database.AddEntityTable(
                "GJI_ACTCHECK_PERIOD",
                new Column("ACTCHECK_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("DATE_CHECK", DbType.DateTime),
                new Column("DATE_START", DbType.DateTime),
                new Column("DATE_END", DbType.DateTime));
            Database.AddIndex("IND_GJI_ACTC_PERIOD_DOC", false, "GJI_ACTCHECK_PERIOD", "ACTCHECK_ID");
            Database.AddForeignKey("FK_GJI_ACTC_PERIOD_DOC", "GJI_ACTCHECK_PERIOD", "ACTCHECK_ID", "GJI_ACTCHECK", "ID");
            //-----

            //-----Статья закона в протоколе
            Database.AddEntityTable(
                "GJI_PROTOCOL_ARTLAW",
                new Column("PROTOCOL_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("ARTICLELAW_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_PROT_ARTLAW_DOC", false, "GJI_PROTOCOL_ARTLAW", "PROTOCOL_ID");
            Database.AddIndex("IND_GJI_PROT_ARTLAW_ARL", false, "GJI_PROTOCOL_ARTLAW", "ARTICLELAW_ID");
            Database.AddForeignKey("FK_GJI_PROT_ARTLAW_DOC", "GJI_PROTOCOL_ARTLAW", "PROTOCOL_ID", "GJI_PROTOCOL", "ID");
            Database.AddForeignKey("FK_GJI_PROT_ARTLAW_ARL", "GJI_PROTOCOL_ARTLAW", "ARTICLELAW_ID", "GJI_DICT_ARTICLELAW", "ID");
            //-----

            //-----Статья закона в предписании
            Database.AddEntityTable(
                "GJI_PRESCRIPTION_ARTLAW",
                new Column("PRESCRIPTION_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("ARTICLELAW_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_PRESCR_ARTLAW_DOC", false, "GJI_PRESCRIPTION_ARTLAW", "PRESCRIPTION_ID");
            Database.AddIndex("IND_GJI_PRESCR_ARTLAW_ARL", false, "GJI_PRESCRIPTION_ARTLAW", "ARTICLELAW_ID");
            Database.AddForeignKey("FK_GJI_PRESCR_ARTLAW_DOC", "GJI_PRESCRIPTION_ARTLAW", "PRESCRIPTION_ID", "GJI_PRESCRIPTION", "ID");
            Database.AddForeignKey("FK_GJI_PRESCR_ARTLAW_ARL", "GJI_PRESCRIPTION_ARTLAW", "ARTICLELAW_ID", "GJI_DICT_ARTICLELAW", "ID");
            //-----

            //-----Решение об отмене в предписании
            Database.AddEntityTable(
                "GJI_PRESCRIPTION_CANCEL",
                new Column("PRESCRIPTION_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("ISSUED_CANCEL_ID", DbType.Int64, 22),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("DATE_CANCEL", DbType.DateTime),
                new Column("IS_COURT", DbType.Int32, ColumnProperty.NotNull, 30),
                new Column("REASON", DbType.String, 500),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_PRESCR_CANCEL_DOC", false, "GJI_PRESCRIPTION_CANCEL", "PRESCRIPTION_ID");
            Database.AddIndex("IND_GJI_PRESCR_CANCEL_ISC", false, "GJI_PRESCRIPTION_CANCEL", "ISSUED_CANCEL_ID");
            Database.AddForeignKey("FK_GJI_PRESCR_CANCEL_DOC", "GJI_PRESCRIPTION_CANCEL", "PRESCRIPTION_ID", "GJI_PRESCRIPTION", "ID");
            Database.AddForeignKey("FK_GJI_PRESCR_CANCEL_ISC", "GJI_PRESCRIPTION_CANCEL", "ISSUED_CANCEL_ID", "GKH_DICT_INSPECTOR", "ID");
            //-----

            //-----Оплата штрафов в постановлении
            Database.AddEntityTable(
                "GJI_RESOLUTION_PAYFINE",
                new Column("RESOLUTION_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("KIND_PAY", DbType.Int32, ColumnProperty.NotNull, 10),
                new Column("AMOUNT", DbType.Decimal));
            Database.AddIndex("IND_GJI_RESOL_PAYF_DOC", false, "GJI_RESOLUTION_PAYFINE", "RESOLUTION_ID");
            Database.AddForeignKey("FK_GJI_RESOL_PAYF_DOC", "GJI_RESOLUTION_PAYFINE", "RESOLUTION_ID", "GJI_RESOLUTION", "ID");
            //-----

            //-----Оспаривание постановления
            Database.AddEntityTable(
                "GJI_RESOLUTION_DISPUTE",
                new Column("RESOLUTION_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("COURT_ID", DbType.Int64, 22),
                new Column("INSTANTION_ID", DbType.Int64, 22),
                new Column("COURTVERDICT_ID", DbType.Int64, 22),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("INSPECTOR_ID", DbType.Int64, 22),
                new Column("APPEAL", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_RESOL_DISP_DOC", false, "GJI_RESOLUTION_DISPUTE", "RESOLUTION_ID");
            Database.AddIndex("IND_GJI_RESOL_DISP_CRT", false, "GJI_RESOLUTION_DISPUTE", "COURT_ID");
            Database.AddIndex("IND_GJI_RESOL_DISP_INS", false, "GJI_RESOLUTION_DISPUTE", "INSTANTION_ID");
            Database.AddIndex("IND_GJI_RESOL_DISP_CRV", false, "GJI_RESOLUTION_DISPUTE", "COURTVERDICT_ID");
            Database.AddIndex("IND_GJI_RESOL_DISP_FILE", false, "GJI_RESOLUTION_DISPUTE", "FILE_ID");
            Database.AddIndex("IND_GJI_RESOL_DISP_LWR", false, "GJI_RESOLUTION_DISPUTE", "INSPECTOR_ID");
            Database.AddForeignKey("FK_GJI_RESOL_DISP_LWR", "GJI_RESOLUTION_DISPUTE", "INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID");
            Database.AddForeignKey("FK_GJI_RESOL_DISP_FILE", "GJI_RESOLUTION_DISPUTE", "FILE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_GJI_RESOL_DISP_CRV", "GJI_RESOLUTION_DISPUTE", "COURTVERDICT_ID", "GJI_DICT_COURTVERDICT", "ID");
            Database.AddForeignKey("FK_GJI_RESOL_DISP_INS", "GJI_RESOLUTION_DISPUTE", "INSTANTION_ID", "GJI_DICT_INSTANCE", "ID");
            Database.AddForeignKey("FK_GJI_RESOL_DISP_CRT", "GJI_RESOLUTION_DISPUTE", "COURT_ID", "GJI_DICT_COURT", "ID");
            Database.AddForeignKey("FK_GJI_RESOL_DISP_DOC", "GJI_RESOLUTION_DISPUTE", "RESOLUTION_ID", "GJI_RESOLUTION", "ID");
            //-----

            //-----Добавляем документ постановление прокуратуры
            Database.AddTable(
                "GJI_RESOLPROS",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("MUNICIPALITY_ID", DbType.Int64, 22),
                new Column("EXECUTANT_ID", DbType.Int64, 22),
                new Column("CONTRAGENT_ID", DbType.Int64, 22),
                new Column("PHYSICAL_PERSON", DbType.String, 300),
                new Column("PHYSICAL_PERSON_INFO", DbType.String, 500),
                new Column("DATE_SUPPLY", DbType.DateTime),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull));
            Database.AddIndex("IND_GJI_RESPROS_MCP", false, "GJI_RESOLPROS", "MUNICIPALITY_ID");
            Database.AddIndex("IND_GJI_RESPROS_EXE", false, "GJI_RESOLPROS", "EXECUTANT_ID");
            Database.AddIndex("IND_GJI_RESPROS_CTR", false, "GJI_RESOLPROS", "CONTRAGENT_ID");
            Database.AddForeignKey("FK_GJI_RESPROS_CTR", "GJI_RESOLPROS", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            Database.AddForeignKey("FK_GJI_RESPROS_EXE", "GJI_RESOLPROS", "EXECUTANT_ID", "GJI_DICT_EXECUTANT", "ID");
            Database.AddForeignKey("FK_GJI_RESPROS_MCP", "GJI_RESOLPROS", "MUNICIPALITY_ID", "GKH_DICT_MUNICIPALITY", "ID");
            Database.AddForeignKey("FK_GJI_RESPROS_DOC", "GJI_RESOLPROS", "ID", "GJI_DOCUMENT", "ID");
            //-----

            //-----Дома в постановлении прокуратуры
            Database.AddEntityTable(
                "GJI_RESOLPROS_ROBJECT",
                new Column("RESOLPROS_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("REALITY_OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull));
            Database.AddIndex("IND_GJI_RESPROS_RO_RES", false, "GJI_RESOLPROS_ROBJECT", "RESOLPROS_ID");
            Database.AddIndex("IND_GJI_RESPROS_RO_OBJ", false, "GJI_RESOLPROS_ROBJECT", "REALITY_OBJECT_ID");
            Database.AddForeignKey("FK_GJI_RESPROS_RO_RES", "GJI_RESOLPROS_ROBJECT", "RESOLPROS_ID", "GJI_RESOLPROS", "ID");
            Database.AddForeignKey("FK_GJI_RESPROS_RO_OBJ", "GJI_RESOLPROS_ROBJECT", "REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID");
            //-----

            //-----Статья закона в постановлении прокуратуры
            Database.AddEntityTable(
                "GJI_RESOLPROS_ARTLAW",
                new Column("RESOLPROS_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("ARTICLELAW_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 500));
            Database.AddIndex("IND_GJI_RESPROS_ARTLAW_DOC", false, "GJI_RESOLPROS_ARTLAW", "RESOLPROS_ID");
            Database.AddIndex("IND_GJI_RESPROS_ARTLAW_ARL", false, "GJI_RESOLPROS_ARTLAW", "ARTICLELAW_ID");
            Database.AddForeignKey("FK_GJI_RESPROS_ARTLAW_DOC", "GJI_RESOLPROS_ARTLAW", "RESOLPROS_ID", "GJI_RESOLPROS", "ID");
            Database.AddForeignKey("FK_GJI_RESPROS_ARTLAW_ARL", "GJI_RESOLPROS_ARTLAW", "ARTICLELAW_ID", "GJI_DICT_ARTICLELAW", "ID");
            //-----

            //-----Приложения в постановлении прокуратуры
            Database.AddEntityTable(
                "GJI_RESOLPROS_ANNEX",
                new Column("RESOLPROS_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 500));
            Database.AddIndex("IND_GJI_RESPROS_ANNEX_FILE", false, "GJI_RESOLPROS_ANNEX", "FILE_ID");
            Database.AddIndex("IND_GJI_RESPROS_ANNEX_DOC", false, "GJI_RESOLPROS_ANNEX", "RESOLPROS_ID");
            Database.AddForeignKey("FK_GJI_RESPROS_ANNEX_DOC", "GJI_RESOLPROS_ANNEX", "RESOLPROS_ID", "GJI_RESOLPROS", "ID");
            Database.AddForeignKey("FK_GJI_RESPROS_ANNEX_FILE", "GJI_RESOLPROS_ANNEX", "FILE_ID", "B4_FILE_INFO", "ID");
            //-----

            //-----Уведомление о начале предпринимательской деятельности
            Database.AddEntityTable(
                "GJI_BUISNES_NOTIF",
                new Column("CONTRAGENT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("TYPE_KIND_ACTIVITY", DbType.Int32, 4, ColumnProperty.NotNull),
                new Column("INCOMING_NUM", DbType.String, 300),
                new Column("DATE_BEGIN", DbType.Date),
                new Column("DATE_REGISTRATION", DbType.Date),
                new Column("DATE_NOTIFICATION", DbType.Date),
                new Column("IS_NOT_BUISNES", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("ACCEPTED_ORGANIZATION", DbType.String, 300),
                new Column("REG_NUM", DbType.String, 50),
                new Column("IS_ORIGINAL", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("REGISTERED", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddIndex("IND_GJI_BUIS_NOTIF_CON", false, "GJI_BUISNES_NOTIF", "CONTRAGENT_ID");
            Database.AddIndex("IND_GJI_BUIS_NOTIF_FILE", false, "GJI_BUISNES_NOTIF", "FILE_ID");
            Database.AddForeignKey("FK_GJI_BUIS_NOTIF_FILE", "GJI_BUISNES_NOTIF", "FILE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_GJI_BUIS_NOTIF_CON", "GJI_BUISNES_NOTIF", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            //-----

            //-----Услуги оказываемые юридическим лицом
            Database.AddEntityTable(
                "GJI_DICT_SERV_JURID",
                new Column("BUISNES_NOTIF_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("KIND_WORK_ID", DbType.Int64, 22, ColumnProperty.NotNull));
            Database.AddIndex("IND_GJI_SERV_JUR_BUIS", false, "GJI_DICT_SERV_JURID", "BUISNES_NOTIF_ID");
            Database.AddIndex("IND_GJI_SERV_JUR_KW", false, "GJI_DICT_SERV_JURID", "KIND_WORK_ID");
            Database.AddForeignKey("FK_GJI_SERV_JUR_KW", "GJI_DICT_SERV_JURID", "KIND_WORK_ID", "GJI_DICT_KIND_WORK", "ID");
            Database.AddForeignKey("FK_GJI_SERV_JUR_BUIS", "GJI_DICT_SERV_JURID", "BUISNES_NOTIF_ID", "GJI_BUISNES_NOTIF", "ID");
            //-----

            //-----Деятельность ТСЖ ГЖИ
            Database.AddEntityTable(
                "GJI_ACTIVITY_TSJ",
                new Column("MANAGING_ORG_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("STATE_ID", DbType.Int64, 22));
            Database.AddIndex("IND_GJI_ACT_TSJ_MORG", false, "GJI_ACTIVITY_TSJ", "MANAGING_ORG_ID");
            Database.AddIndex("IND_GJI_ACT_TSJ_STATE", false, "GJI_ACTIVITY_TSJ", "STATE_ID");
            Database.AddForeignKey("FK_GJI_ACT_TSJ_STATE", "GJI_ACTIVITY_TSJ", "STATE_ID", "B4_STATE", "ID");
            Database.AddForeignKey("FK_GJI_ACT_TSJ_MORG", "GJI_ACTIVITY_TSJ", "MANAGING_ORG_ID", "GKH_MANAGING_ORGANIZATION", "ID");
            //-----

            //-----Устав деятельности ТСЖ ГЖИ
            Database.AddEntityTable(
                "GJI_ACT_TSJ_STATUTE",
                new Column("ACTIVITY_TSJ_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("CONCLUSION_FILE", DbType.Int64, 22),
                new Column("STATUTE_FILE", DbType.Int64, 22),
                new Column("TYPE_CONCLUSION", DbType.Int32, 4, ColumnProperty.NotNull),
                new Column("STAT_PROVISION_DATE", DbType.Date),
                new Column("STAT_APPROVAL_DATE", DbType.Date),
                new Column("CONCLUSION_DATE", DbType.Date),
                new Column("CONCLUSION_NUM", DbType.String, 50),
                new Column("DESCRIPTION", DbType.String, 500));
            Database.AddIndex("IND_GJI_ACT_TSJ_ST_AC", false, "GJI_ACT_TSJ_STATUTE", "ACTIVITY_TSJ_ID");
            Database.AddIndex("IND_GJI_ACT_TSJ_ST_CF", false, "GJI_ACT_TSJ_STATUTE", "CONCLUSION_FILE");
            Database.AddIndex("IND_GJI_ACT_TSJ_ST_SF", false, "GJI_ACT_TSJ_STATUTE", "STATUTE_FILE");
            Database.AddForeignKey("FK_GJI_ACT_TSJ_ST_SF", "GJI_ACT_TSJ_STATUTE", "STATUTE_FILE", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_GJI_ACT_TSJ_ST_CF", "GJI_ACT_TSJ_STATUTE", "CONCLUSION_FILE", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_GJI_ACT_TSJ_ST_AC", "GJI_ACT_TSJ_STATUTE", "ACTIVITY_TSJ_ID", "GJI_ACTIVITY_TSJ", "ID");
            //-----

            //-----Статья устава деятельности ТСЖ
            Database.AddEntityTable(
                "GJI_ACT_TSJ_ARTICLE",
                new Column("ACT_TSJ_STATUATE_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("ARTICLE_TSJ_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("TYPE_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("PARAGRAPH", DbType.String, 300),
                new Column("IS_NONE", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddIndex("IND_GJI_ACT_TSJ_ART_STAT", false, "GJI_ACT_TSJ_ARTICLE", "ACT_TSJ_STATUATE_ID");
            Database.AddIndex("IND_GJI_ACT_TSJ_ART_ART", false, "GJI_ACT_TSJ_ARTICLE", "ARTICLE_TSJ_ID");
            Database.AddForeignKey("FK_GJI_ACT_TSJ_ART_ART", "GJI_ACT_TSJ_ARTICLE", "ARTICLE_TSJ_ID", "GJI_DICT_ARTICLE_TSJ", "ID");
            Database.AddForeignKey("FK_GJI_ACT_TSJ_ART_STAT", "GJI_ACT_TSJ_ARTICLE", "ACT_TSJ_STATUATE_ID", "GJI_ACT_TSJ_STATUTE", "ID");
            //-----

            //-----Протокол деятельности ТСЖ ГЖИ
            Database.AddEntityTable(
                "GJI_ACT_TSJ_PROTOCOL",
                new Column("ACTIVITY_TSJ_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("KIND_PROTOCOL_TSJ_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_BULLETIN_ID", DbType.Int64, 22),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("DOCUMENT_DATE", DbType.Date),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("PERC_PARTICIPANT", DbType.Decimal),
                new Column("VOTES_DATE", DbType.Date),
                new Column("COUNT_VOTES", DbType.Int32),
                new Column("GENERAL_COUNT_VOTES", DbType.Int32));
            Database.AddIndex("IND_GJI_ACT_TSJ_PROT_AC", false, "GJI_ACT_TSJ_PROTOCOL", "ACTIVITY_TSJ_ID");
            Database.AddIndex("IND_GJI_ACT_TSJ_PROT_KP", false, "GJI_ACT_TSJ_PROTOCOL", "KIND_PROTOCOL_TSJ_ID");
            Database.AddIndex("IND_GJI_ACT_TSJ_PROT_BF", false, "GJI_ACT_TSJ_PROTOCOL", "FILE_BULLETIN_ID");
            Database.AddIndex("IND_GJI_ACT_TSJ_PROT_FL", false, "GJI_ACT_TSJ_PROTOCOL", "FILE_ID");
            Database.AddForeignKey("FK_GJI_ACT_TSJ_PROT_FL", "GJI_ACT_TSJ_PROTOCOL", "FILE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_GJI_ACT_TSJ_PROT_BF", "GJI_ACT_TSJ_PROTOCOL", "FILE_BULLETIN_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_GJI_ACT_TSJ_PROT_KP", "GJI_ACT_TSJ_PROTOCOL", "KIND_PROTOCOL_TSJ_ID", "GJI_DICT_KIND_PROT", "ID");
            Database.AddForeignKey("FK_GJI_ACT_TSJ_PROT_AC", "GJI_ACT_TSJ_PROTOCOL", "ACTIVITY_TSJ_ID", "GJI_ACTIVITY_TSJ", "ID");
            //-----

            //-----Дома протокола деятельности ТСЖ ГЖИ
            Database.AddEntityTable(
                "GJI_PROT_REAL_OBJ",
                new Column("PROTOCOL_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("REALITY_OBJ_ID", DbType.Int64, 22, ColumnProperty.NotNull));
            Database.AddIndex("IND_GJI_PROT_RO_PROT", false, "GJI_PROT_REAL_OBJ", "PROTOCOL_ID");
            Database.AddIndex("IND_GJI_PROT_RO_RO", false, "GJI_PROT_REAL_OBJ", "REALITY_OBJ_ID");
            Database.AddForeignKey("FK_GJI_PROT_RO_RO", "GJI_PROT_REAL_OBJ", "REALITY_OBJ_ID", "GKH_REALITY_OBJECT", "ID");
            Database.AddForeignKey("FK_GJI_PROT_RO_PROT", "GJI_PROT_REAL_OBJ", "PROTOCOL_ID", "GJI_ACT_TSJ_PROTOCOL", "ID");
            //-----

            //-----Дома деятельности ТСЖ ГЖИ
            Database.AddEntityTable(
                "GJI_ACT_TSJ_REAL_OBJ",
                new Column("ACTIVITY_TSJ_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("REALITY_OBJ_ID", DbType.Int64, 22, ColumnProperty.NotNull));
            Database.AddIndex("IND_GJI_ACT_TSJ_RO_ACT", false, "GJI_ACT_TSJ_REAL_OBJ", "ACTIVITY_TSJ_ID");
            Database.AddIndex("IND_GJI_ACT_TSJ_RO_RO", false, "GJI_ACT_TSJ_REAL_OBJ", "REALITY_OBJ_ID");
            Database.AddForeignKey("FK_GJI_ACT_TSJ_RO_RO", "GJI_ACT_TSJ_REAL_OBJ", "REALITY_OBJ_ID", "GKH_REALITY_OBJECT", "ID");
            Database.AddForeignKey("FK_GJI_ACT_TSJ_RO_ACT", "GJI_ACT_TSJ_REAL_OBJ", "ACTIVITY_TSJ_ID", "GJI_ACTIVITY_TSJ", "ID");
            //-----

            //-----Основание проверки деятельности ТСЖ
            Database.AddTable(
                "GJI_INSPECTION_ACTIVITY",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("ACTIVITY_TSJ_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull));
            Database.AddIndex("IND_GJI_INSPECT_ACT_ACT", false, "GJI_INSPECTION_ACTIVITY", "ACTIVITY_TSJ_ID");
            Database.AddForeignKey("FK_GJI_INSPECT_ACT_ID", "GJI_INSPECTION_ACTIVITY", "ACTIVITY_TSJ_ID", "GJI_ACTIVITY_TSJ", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_ACT_ACT", "GJI_INSPECTION_ACTIVITY", "ID", "GJI_INSPECTION", "ID");
            //-----

            //-----Представление
            Database.AddTable(
                "GJI_PRESENTATION",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("EXECUTANT_ID", DbType.Int64, 22),
                new Column("CONTRAGENT_ID", DbType.Int64, 22),
                new Column("OFFICIAL_ID", DbType.Int64, 22),
                new Column("PHYSICAL_PERSON", DbType.String, 300),
                new Column("PHYSICAL_PERSON_INFO", DbType.String, 500),
                new Column("TYPE_INITIATIVE_ORG", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull));
            Database.AddIndex("IND_GJI_PRESENT_EXE", false, "GJI_PRESENTATION", "EXECUTANT_ID");
            Database.AddIndex("IND_GJI_PRESENT_CTR", false, "GJI_PRESENTATION", "CONTRAGENT_ID");
            Database.AddIndex("IND_GJI_PRESENT_OFC", false, "GJI_PRESENTATION", "OFFICIAL_ID");
            Database.AddForeignKey("FK_GJI_PRESENT_DOC", "GJI_PRESENTATION", "ID", "GJI_DOCUMENT", "ID");
            Database.AddForeignKey("FK_GJI_PRESENT_OFC", "GJI_PRESENTATION", "OFFICIAL_ID", "GKH_DICT_INSPECTOR", "ID");
            Database.AddForeignKey("FK_GJI_PRESENT_EXE", "GJI_PRESENTATION", "EXECUTANT_ID", "GJI_DICT_EXECUTANT", "ID");
            Database.AddForeignKey("FK_GJI_PRESENT_CTR", "GJI_PRESENTATION", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            //-----

            //-----Приложения Представления
            Database.AddEntityTable(
                "GJI_PRESENTATION_ANNEX",
                new Column("PRESENTATION_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 500));
            Database.AddIndex("IND_GJI_PRESENT_ANNEX_FL", false, "GJI_PRESENTATION_ANNEX", "FILE_ID");
            Database.AddIndex("IND_GJI_PRESENT_ANNEX_D", false, "GJI_PRESENTATION_ANNEX", "PRESENTATION_ID");
            Database.AddForeignKey("FK_GJI_PRESENT_ANNEX_D", "GJI_PRESENTATION_ANNEX", "PRESENTATION_ID", "GJI_PRESENTATION", "ID");
            Database.AddForeignKey("FK_GJI_PRESENT_ANNEX_FL", "GJI_PRESENTATION_ANNEX", "FILE_ID", "B4_FILE_INFO", "ID");
            //-----

            Database.AddEntityTable(
                "GJI_DICT_ACREDT_FLAG",
                new Column("EXTERNAL_ID", DbType.String, 36),
                new Column("NAME", DbType.String, 250),
                new Column("CODE", DbType.Int16, ColumnProperty.NotNull),
                new Column("SYSTEM_VALUE", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddIndex("IND_GJI_ACREDT_FL_NAME", false, "GJI_DICT_ACREDT_FLAG", "NAME");
            Database.AddIndex("IND_GJI_ACREDT_FL_CODE", false, "GJI_DICT_ACREDT_FLAG", "CODE");

            Database.AddEntityTable(
                "GJI_APPEAL_CITIZENS",
                new Column("EXTERNAL_ID", DbType.String, 36),
                new Column("NUM", DbType.String, 500),
                new Column("YEAR", DbType.Int16, 4),
                new Column("STATUS", DbType.Int16, 4, ColumnProperty.NotNull, 0),
                new Column("ADDRESS_EDO", DbType.String, 2000),
                new Column("IS_EDO", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("EDO_ID", DbType.Int64, 22),
                new Column("CORRESPONDENT", DbType.String, 255),
                new Column("CORRESPONDENT_ADDRESS", DbType.String, 2000),
                new Column("EMAIL", DbType.String, 255),
                new Column("PHONE", DbType.String, 255),
                new Column("QUESTIONS_COUNT", DbType.Int32, 10),
                new Column("GJI_REDTAPE_FLAG_ID", DbType.Int64, 22),
                new Column("GJI_APPEAL_ID", DbType.Int16, 4),
                new Column("DESCRIPTION", DbType.String, 2000),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("FLAT_NUM", DbType.String, 255),
                new Column("MANAGING_ORG_ID", DbType.Int64, 22),
                new Column("GJI_DICT_KIND_ID", DbType.Int64, 22),
                new Column("GJI_NUMBER", DbType.String, 255),
                new Column("DATE_FROM", DbType.DateTime),
                new Column("CHECK_TIME", DbType.DateTime),
                new Column("DATE_ACTUAL", DbType.DateTime),
                new Column("DESC_LOCATION_PROBLEM", DbType.String, 255),
                new Column("DOCUMENT_NUMBER", DbType.String, 50),
                new Column("PREVIOUS_APPEAL_CITIZENS_ID", DbType.Int64, 22),
                new Column("STATE_ID", DbType.Int64, 22));
            Database.AddIndex("IND_GJI_APPEAL_CIT_ACR", false, "GJI_APPEAL_CITIZENS", "GJI_REDTAPE_FLAG_ID");
            Database.AddIndex("IND_GJI_APPEAL_CIT_MORG", false, "GJI_APPEAL_CITIZENS", "MANAGING_ORG_ID");
            Database.AddIndex("IND_GJI_APPEAL_CIT_KIND", false, "GJI_APPEAL_CITIZENS", "GJI_DICT_KIND_ID");
            Database.AddIndex("IND_GJI_APPEAL_CIT_PREV", false, "GJI_APPEAL_CITIZENS", "PREVIOUS_APPEAL_CITIZENS_ID");
            Database.AddIndex("IND_GJI_APPEAL_CIT_STATE", false, "GJI_APPEAL_CITIZENS", "STATE_ID");
            Database.AddForeignKey("FK_GJI_APPEAL_CIT_STATE", "GJI_APPEAL_CITIZENS", "STATE_ID", "B4_STATE", "ID");
            Database.AddForeignKey("FK_GJI_APPEAL_CIT_PREV", "GJI_APPEAL_CITIZENS", "PREVIOUS_APPEAL_CITIZENS_ID", "GJI_APPEAL_CITIZENS", "ID");
            Database.AddForeignKey("FK_GJI_APPEAL_CIT_KIND", "GJI_APPEAL_CITIZENS", "GJI_DICT_KIND_ID", "GJI_DICT_KINDSTATEMENT", "ID");
            Database.AddForeignKey("FK_GJI_APPEAL_CIT_MORG", "GJI_APPEAL_CITIZENS", "MANAGING_ORG_ID", "GKH_MANAGING_ORGANIZATION", "ID");
            Database.AddForeignKey("FK_GJI_APPEAL_CIT_ACR", "GJI_APPEAL_CITIZENS", "GJI_REDTAPE_FLAG_ID", "GJI_DICT_ACREDT_FLAG", "ID");

            Database.AddEntityTable(
                "GJI_BASESTAT_APPCIT",
                new Column("GJI_APPCIT_ID", DbType.Int64, 22),
                new Column("GJI_INSP_STAT_ID", DbType.Int64, 22),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_BASEST_APPCIT_APP", false, "GJI_BASESTAT_APPCIT", "GJI_APPCIT_ID");
            Database.AddIndex("IND_GJI_BASEST_APPCIT_ST", false, "GJI_BASESTAT_APPCIT", "GJI_INSP_STAT_ID");
            Database.AddForeignKey("FK_GJI_BASEST_APPCIT_ST", "GJI_BASESTAT_APPCIT", "GJI_INSP_STAT_ID", "GJI_INSPECTION_STATEMENT", "ID");
            Database.AddForeignKey("FK_GJI_BASEST_APPCIT_APP", "GJI_BASESTAT_APPCIT", "GJI_APPCIT_ID", "GJI_APPEAL_CITIZENS", "ID");

            Database.AddEntityTable(
                "GJI_REL_APPEAL_CITS",
                new Column("PARENT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("CHILDREN_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_REL_APP_PAR", false, "GJI_REL_APPEAL_CITS", "PARENT_ID");
            Database.AddIndex("IND_GJI_REL_APP_CHILD", false, "GJI_REL_APPEAL_CITS", "CHILDREN_ID");
            Database.AddForeignKey("FK_GJI_REL_APP_CHILD", "GJI_REL_APPEAL_CITS", "CHILDREN_ID", "GJI_APPEAL_CITIZENS", "ID");
            Database.AddForeignKey("FK_GJI_REL_APP_PAR", "GJI_REL_APPEAL_CITS", "PARENT_ID", "GJI_APPEAL_CITIZENS", "ID");

            Database.AddEntityTable(
                "GJI_APPCIT_RO",
                new Column("APPCIT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("REALITY_OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_APPCIT_RO_AP", false, "GJI_APPCIT_RO", "APPCIT_ID");
            Database.AddIndex("IND_GJI_APPCIT_RO_RO", false, "GJI_APPCIT_RO", "REALITY_OBJECT_ID");
            Database.AddForeignKey("FK_GJI_APPCIT_RO_RO", "GJI_APPCIT_RO", "REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID");
            Database.AddForeignKey("FK_GJI_APPCIT_RO_AP", "GJI_APPCIT_RO", "APPCIT_ID", "GJI_APPEAL_CITIZENS", "ID");

            Database.AddEntityTable(
                "GJI_APPCIT_STATSUBJ",
                new Column("APPCIT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("STATEMENT_SUBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_APPCIT_ST_APP", false, "GJI_APPCIT_STATSUBJ", "APPCIT_ID");
            Database.AddIndex("ind_appcitstat_stat", false, "GJI_APPCIT_STATSUBJ", "STATEMENT_SUBJECT_ID");
            Database.AddForeignKey("fk_appcitstat_stat", "GJI_APPCIT_STATSUBJ", "STATEMENT_SUBJECT_ID", "GJI_STATEMENT_STATSUBJECT", "ID");
            Database.AddForeignKey("FK_GJI_APPCIT_ST_APP", "GJI_APPCIT_STATSUBJ", "APPCIT_ID", "GJI_APPEAL_CITIZENS", "ID");

            Database.AddEntityTable(
                "GJI_APPEAL_SOURCES",
                new Column("EXTERNAL_ID", DbType.String, 36),
                new Column("REVENUE_DATE", DbType.DateTime),
                new Column("REVENUE_SOURCE_NUMBER", DbType.String, 50),
                new Column("REVENUE_SOURCE_ID", DbType.Int64, 22),
                new Column("REVENUE_FORM_ID", DbType.Int64, 22),
                new Column("APPCIT_ID", DbType.Int64, 22, ColumnProperty.NotNull));
            Database.AddIndex("IND_GJI_APPEAL_SRC_SOURS", false, "GJI_APPEAL_SOURCES", "REVENUE_SOURCE_ID");
            Database.AddIndex("IND_GJI_APPEAL_SRC_FORM", false, "GJI_APPEAL_SOURCES", "REVENUE_FORM_ID");
            Database.AddIndex("IND_GJI_APPEAL_SRC_APP", false, "GJI_APPEAL_SOURCES", "APPCIT_ID");
            Database.AddForeignKey("FK_GJI_APPEAL_SRC_APP", "GJI_APPEAL_SOURCES", "APPCIT_ID", "GJI_APPEAL_CITIZENS", "ID");
            Database.AddForeignKey("FK_GJI_APPEAL_SRC_FORM", "GJI_APPEAL_SOURCES", "REVENUE_FORM_ID", "GJI_DICT_REVENUEFORM", "ID");
            Database.AddForeignKey("FK_GJI_APPEAL_SRC_SOURS", "GJI_APPEAL_SOURCES", "REVENUE_SOURCE_ID", "GJI_DICT_REVENUESOURCE", "ID");

            Database.AddEntityTable(
                "GJI_STATEM_SOURS",
                new Column("EXTERNAL_ID", DbType.String, 36),
                new Column("REVENUE_DATE", DbType.DateTime),
                new Column("REVENUE_SOURCE_NUMBER", DbType.String, 50),
                new Column("REVENUE_SOURCE_ID", DbType.Int64, 22),
                new Column("REVENUE_FORM_ID", DbType.Int64, 22),
                new Column("INSPECTION_ID", DbType.Int64, 22, ColumnProperty.NotNull));
            Database.AddIndex("IND_GJI_STAT_SRC", false, "GJI_STATEM_SOURS", "REVENUE_SOURCE_ID");
            Database.AddIndex("IND_GJI_STAT_FORM", false, "GJI_STATEM_SOURS", "REVENUE_FORM_ID");
            Database.AddIndex("IND_GJI_STAT_INS", false, "GJI_STATEM_SOURS", "INSPECTION_ID");
            Database.AddForeignKey("FK_GJI_STAT_INS", "GJI_STATEM_SOURS", "INSPECTION_ID", "GJI_INSPECTION", "ID");
            Database.AddForeignKey("FK_GJI_STAT_FORM", "GJI_STATEM_SOURS", "REVENUE_FORM_ID", "GJI_DICT_REVENUEFORM", "ID");
            Database.AddForeignKey("FK_GJI_STAT_SRC", "GJI_STATEM_SOURS", "REVENUE_SOURCE_ID", "GJI_DICT_REVENUESOURCE", "ID");
        }

        public override void Down()
        {
            Database.RemoveConstraint("GJI_BUISNES_NOTIF", "FK_GJI_BUIS_NOTIF_ST");
            Database.RemoveConstraint("GJI_HEATSEASON_DOCUMENT", "FK_GJI_HEATS_DOC_ST");
            Database.RemoveConstraint("GJI_ACTREMOVAL_VIOLAT", "FK_GJI_ACTR_VIOL_STG");
            Database.RemoveConstraint("GJI_ACTCHECK_INSPECTPART", "FK_GJI_ACTC_INS_PART");
            Database.RemoveConstraint("GJI_STATEM_SOURS", "FK_GJI_STAT_INS");
            Database.RemoveConstraint("GJI_STATEM_SOURS", "FK_GJI_STAT_FORM");
            Database.RemoveConstraint("GJI_STATEM_SOURS", "FK_GJI_STAT_SRC");
            Database.RemoveConstraint("GJI_APPEAL_SOURCES", "FK_GJI_APPEAL_SRC_APP");
            Database.RemoveConstraint("GJI_APPEAL_SOURCES", "FK_GJI_APPEAL_SRC_FORM");
            Database.RemoveConstraint("GJI_APPEAL_SOURCES", "FK_GJI_APPEAL_SRC_SOURS");
            Database.RemoveConstraint("GJI_APPCIT_STATSUBJ", "fk_appcitstat_stat");
            Database.RemoveConstraint("GJI_APPCIT_STATSUBJ", "FK_GJI_APPCIT_ST_APP");
            Database.RemoveConstraint("GJI_APPCIT_RO", "FK_GJI_APPCIT_RO_RO");
            Database.RemoveConstraint("GJI_APPCIT_RO", "FK_GJI_APPCIT_RO_AP");
            Database.RemoveConstraint("GJI_REL_APPEAL_CITS", "FK_GJI_REL_APP_CHILD");
            Database.RemoveConstraint("GJI_REL_APPEAL_CITS", "FK_GJI_REL_APP_PAR");
            Database.RemoveConstraint("GJI_BASESTAT_APPCIT", "FK_GJI_BASEST_APPCIT_ST");
            Database.RemoveConstraint("GJI_BASESTAT_APPCIT", "FK_GJI_BASEST_APPCIT_APP");
            Database.RemoveConstraint("GJI_APPEAL_CITIZENS", "FK_GJI_APPEAL_CIT_STATE");
            Database.RemoveConstraint("GJI_APPEAL_CITIZENS", "FK_GJI_APPEAL_CIT_PREV");
            Database.RemoveConstraint("GJI_APPEAL_CITIZENS", "FK_GJI_APPEAL_CIT_KIND");
            Database.RemoveConstraint("GJI_APPEAL_CITIZENS", "FK_GJI_APPEAL_CIT_MORG");
            Database.RemoveConstraint("GJI_APPEAL_CITIZENS", "FK_GJI_APPEAL_CIT_ACR");
            Database.RemoveConstraint("GJI_INSPECTION_STATEMENT", "FK_GJI_INSPECT_STAT_MORG");
            Database.RemoveConstraint("GJI_INSPECTION_BASEDEF", "FK_GJI_INSPECT_BDEF_ID");
            Database.RemoveConstraint("GJI_ACTCHECK_INSPECTPART", "FK_GJI_ACTC_INS_ACT");
            Database.RemoveConstraint("GJI_INSPECTION_DISPHEAD", "FK_GJI_INSPECT_DH_FILE");
            Database.RemoveConstraint("GJI_INSPECTION_PROSCLAIM", "FK_GJI_INSPECT_PROSCL_FILE");
            Database.RemoveConstraint("GJI_DICT_TASKS_INSPECTION", "FK_GJI_TASK_INS_TS");
            Database.RemoveConstraint("GJI_DOCUMENT_INSPECTOR", "FK_GJI_DOCUMENT_INS_INS");
            Database.RemoveConstraint("GJI_DOCUMENT_INSPECTOR", "FK_GJI_DOCUMENT_INS_DOC");
            Database.RemoveConstraint("GJI_PRESENTATION", "FK_GJI_PRESENT_DOC");
            Database.RemoveConstraint("GJI_PRESENTATION", "FK_GJI_PRESENT_OFC");
            Database.RemoveConstraint("GJI_PRESENTATION", "FK_GJI_PRESENT_EXE");
            Database.RemoveConstraint("GJI_PRESENTATION", "FK_GJI_PRESENT_CTR");
            Database.RemoveConstraint("GJI_PRESENTATION_ANNEX", "FK_GJI_PRESENT_ANNEX_D");
            Database.RemoveConstraint("GJI_PRESENTATION_ANNEX", "FK_GJI_PRESENT_ANNEX_FL");
            Database.RemoveConstraint("GJI_RESOLUTION_DISPUTE", "FK_GJI_RESOL_DISP_LWR");
            Database.RemoveConstraint("GJI_STATEMENT_STATSUBJECT", "FK_GJI_STAT_SUBJ_INS");
            Database.RemoveConstraint("GJI_STATEMENT_STATSUBJECT", "FK_GJI_STAT_SUBJ_SUBJ");
            Database.RemoveConstraint("GJI_RESOLPROS_ROBJECT", "FK_GJI_RESPROS_RO_RES");
            Database.RemoveConstraint("GJI_RESOLPROS_ROBJECT", "FK_GJI_RESPROS_RO_OBJ");
            Database.RemoveConstraint("GJI_INSPECTION_ACTIVITY", "FK_GJI_INSPECT_ACT_ID");
            Database.RemoveConstraint("GJI_INSPECTION_ACTIVITY", "FK_GJI_INSPECT_ACT_ACT");
            Database.RemoveConstraint("GJI_STATEMENT_ANSWER", "FK_GJI_STAT_ANSW_INSN");
            Database.RemoveConstraint("GJI_STATEMENT_ANSWER", "FK_GJI_STAT_ANSW_CON");
            Database.RemoveConstraint("GJI_STATEMENT_ANSWER", "FK_GJI_STAT_ANSW_REV");
            Database.RemoveConstraint("GJI_STATEMENT_ANSWER", "FK_GJI_STAT_ANSW_INSR");
            Database.RemoveConstraint("GJI_STATEMENT_ANSWER", "FK_GJI_STAT_ANSW_FILE");
            Database.RemoveConstraint("GJI_STATEMENT_REQUEST", "FK_GJI_STAT_REQ_INS");
            Database.RemoveConstraint("GJI_STATEMENT_REQUEST", "FK_GJI_STAT_REQ_CORG");
            Database.RemoveConstraint("GJI_STATEMENT_REQUEST", "FK_GJI_STAT_REQ_FILE");
            Database.RemoveConstraint("GJI_ACTIVITY_TSJ", "FK_GJI_ACT_TSJ_MORG");
            Database.RemoveConstraint("GJI_ACT_TSJ_STATUTE", "FK_GJI_ACT_TSJ_ST_SF");
            Database.RemoveConstraint("GJI_ACT_TSJ_STATUTE", "FK_GJI_ACT_TSJ_ST_CF");
            Database.RemoveConstraint("GJI_ACT_TSJ_STATUTE", "FK_GJI_ACT_TSJ_ST_AC");
            Database.RemoveConstraint("GJI_ACT_TSJ_ARTICLE", "FK_GJI_ACT_TSJ_ART_ART");
            Database.RemoveConstraint("GJI_ACT_TSJ_ARTICLE", "FK_GJI_ACT_TSJ_ART_STAT");
            Database.RemoveConstraint("GJI_ACT_TSJ_PROTOCOL", "FK_GJI_ACT_TSJ_PROT_FL");
            Database.RemoveConstraint("GJI_ACT_TSJ_PROTOCOL", "FK_GJI_ACT_TSJ_PROT_BF");
            Database.RemoveConstraint("GJI_ACT_TSJ_PROTOCOL", "FK_GJI_ACT_TSJ_PROT_KP");
            Database.RemoveConstraint("GJI_ACT_TSJ_PROTOCOL", "FK_GJI_ACT_TSJ_PROT_AC");
            Database.RemoveConstraint("GJI_PROT_REAL_OBJ", "FK_GJI_PROT_RO_RO");
            Database.RemoveConstraint("GJI_PROT_REAL_OBJ", "FK_GJI_PROT_RO_PROT");
            Database.RemoveConstraint("GJI_ACT_TSJ_REAL_OBJ", "FK_GJI_ACT_TSJ_RO_RO");
            Database.RemoveConstraint("GJI_ACT_TSJ_REAL_OBJ", "FK_GJI_ACT_TSJ_RO_ACT");
            Database.RemoveConstraint("GJI_BUISNES_NOTIF", "FK_GJI_BUIS_NOTIF_FILE");
            Database.RemoveConstraint("GJI_BUISNES_NOTIF", "FK_GJI_BUIS_NOTIF_CON");
            Database.RemoveConstraint("GJI_DICT_SERV_JURID", "FK_GJI_SERV_JUR_KW");
            Database.RemoveConstraint("GJI_DICT_SERV_JURID", "FK_GJI_SERV_JUR_BUIS");
            Database.RemoveConstraint("GJI_INSPECTION_RESOLPROS", "FK_GJI_INSPECT_RESPROS_INS");
            Database.RemoveConstraint("GJI_INSPECTION_HEATSEASON", "FK_GJI_INSPECT_HSEAS_INS");
            Database.RemoveConstraint("GJI_INSPECTION_HEATSEASON", "FK_GJI_INSPECT_HSEAS_SEA");
            Database.RemoveConstraint("GJI_HEATSEASON", "FK_GJI_HEATS_OBJ");
            Database.RemoveConstraint("GJI_HEATSEASON", "FK_GJI_HEATS_PRD");
            Database.RemoveConstraint("GJI_HEATSEASON_DOCUMENT", "FK_GJI_HEATS_DOC_SEA");
            Database.RemoveConstraint("GJI_HEATSEASON_DOCUMENT", "FK_GJI_HEATS_DOC_FILE");
            Database.RemoveConstraint("GJI_RESOLPROS", "FK_GJI_RESPROS_CTR");
            Database.RemoveConstraint("GJI_RESOLPROS", "FK_GJI_RESPROS_EXE");
            Database.RemoveConstraint("GJI_RESOLPROS", "FK_GJI_RESPROS_MCP");
            Database.RemoveConstraint("GJI_RESOLPROS", "FK_GJI_RESPROS_DOC");
            Database.RemoveConstraint("GJI_RESOLPROS_ARTLAW", "FK_GJI_RESPROS_ARTLAW_DOC");
            Database.RemoveConstraint("GJI_RESOLPROS_ARTLAW", "FK_GJI_RESPROS_ARTLAW_ARL");
            Database.RemoveConstraint("GJI_RESOLPROS_ANNEX", "FK_GJI_RESPROS_ANNEX_DOC");
            Database.RemoveConstraint("GJI_RESOLPROS_ANNEX", "FK_GJI_RESPROS_ANNEX_FILE");
            Database.RemoveConstraint("GJI_DICT_VIOLATIONFEATURE", "FK_GJI_VIOLFEATURE_FEA");
            Database.RemoveConstraint("GJI_DICT_VIOLATIONFEATURE", "FK_GJI_VIOLFEATURE_VIOL");
            Database.RemoveConstraint("GJI_DOCUMENT_REFERENCE", "FK_GJI_DOCUMENT_REF_D1");
            Database.RemoveConstraint("GJI_DOCUMENT_REFERENCE", "FK_GJI_DOCUMENT_REF_D2");
            Database.RemoveConstraint("GJI_DICT_LEGFOUND_INSPECT", "FK_GJI_LEGF_INS_TS");
            Database.RemoveConstraint("GJI_DICT_KIND_INSPECTION", "FK_GJI_KIND_INS_TS");
            Database.RemoveConstraint("GJI_DICT_GOALS_INSPECTION", "FK_GJI_GOALS_INS_TS");
            Database.RemoveConstraint("GJI_INSPECTION", "FK_GJI_INSPECT_CTR");
            Database.RemoveConstraint("GJI_INSPECTION_DISPHEAD", "FK_GJI_INSPECT_DH_INS");
            Database.RemoveConstraint("GJI_INSPECTION_DISPHEAD", "FK_GJI_INSPECT_DH_HEAD");
            Database.RemoveConstraint("GJI_INSPECTION_DISPHEAD", "FK_GJI_INSPECT_DH_DOC");
            Database.RemoveConstraint("GJI_INSPECTION_PROSCLAIM", "FK_GJI_INSPECT_PROSCL_INS");
            Database.RemoveConstraint("GJI_INSPECTION_INSCHECK", "FK_GJI_INSPECT_INSCH_PLAN");
            Database.RemoveConstraint("GJI_INSPECTION_INSCHECK", "FK_GJI_INSPECT_INSCH_INS");
            Database.RemoveConstraint("GJI_INSPECTION_STATEMENT", "FK_GJI_INSPECT_STAT_EXE");
            Database.RemoveConstraint("GJI_INSPECTION_STATEMENT", "FK_GJI_INSPECT_STAT_TES");
            Database.RemoveConstraint("GJI_INSPECTION_STATEMENT", "FK_GJI_INSPECT_STAT_SURR");
            Database.RemoveConstraint("GJI_INSPECTION_STATEMENT", "FK_GJI_INSPECT_STAT_SUR");
            Database.RemoveConstraint("GJI_INSPECTION_STATEMENT", "FK_GJI_INSPECT_STAT_REF");
            Database.RemoveConstraint("GJI_INSPECTION_STATEMENT", "FK_GJI_INSPECT_STAT_RES");
            Database.RemoveConstraint("GJI_INSPECTION_STATEMENT", "FK_GJI_INSPECT_STAT_INS");
            Database.RemoveConstraint("GJI_INSPECTION_STATEMENT", "FK_GJI_INSPECT_STAT_ZON");
            Database.RemoveConstraint("GJI_INSPECTION_INSPECTOR", "FK_GJI_INSPECT_INS_INSR");
            Database.RemoveConstraint("GJI_INSPECTION_INSPECTOR", "FK_GJI_INSPECT_INS_INSN");
            Database.RemoveConstraint("GJI_ACTCHECK_PERIOD", "FK_GJI_ACTC_PERIOD_DOC");
            Database.RemoveConstraint("GJI_PROTOCOL_ARTLAW", "FK_GJI_PROT_ARTLAW_DOC");
            Database.RemoveConstraint("GJI_PROTOCOL_ARTLAW", "FK_GJI_PROT_ARTLAW_ARL");
            Database.RemoveConstraint("GJI_PRESCRIPTION_ARTLAW", "FK_GJI_PRESCR_ARTLAW_DOC");
            Database.RemoveConstraint("GJI_PRESCRIPTION_ARTLAW", "FK_GJI_PRESCR_ARTLAW_ARL");
            Database.RemoveConstraint("GJI_PRESCRIPTION_CANCEL", "FK_GJI_PRESCR_CANCEL_DOC");
            Database.RemoveConstraint("GJI_PRESCRIPTION_CANCEL", "FK_GJI_PRESCR_CANCEL_ISC");
            Database.RemoveConstraint("GJI_RESOLUTION_PAYFINE", "FK_GJI_RESOL_PAYF_DOC");
            Database.RemoveConstraint("GJI_RESOLUTION_DISPUTE", "FK_GJI_RESOL_DISP_CRV");
            Database.RemoveConstraint("GJI_RESOLUTION_DISPUTE", "FK_GJI_RESOL_DISP_INS");
            Database.RemoveConstraint("GJI_RESOLUTION_DISPUTE", "FK_GJI_RESOL_DISP_CRT");
            Database.RemoveConstraint("GJI_RESOLUTION_DISPUTE", "FK_GJI_RESOL_DISP_DOC");
            Database.RemoveConstraint("GJI_RESOLUTION_DISPUTE", "FK_GJI_RESOL_DISP_FILE");
            Database.RemoveConstraint("GJI_INSPECTION_VIOLATION", "FK_GJI_INSPECT_VIO_INS");
            Database.RemoveConstraint("GJI_INSPECTION_VIOLATION", "FK_GJI_INSPECT_VIO_OBJ");
            Database.RemoveConstraint("GJI_INSPECTION_VIOLATION", "FK_GJI_INSPECT_VIO_VIO");
            Database.RemoveConstraint("GJI_INSPECTION_VIOL_STAGE", "FK_GJI_INSPECT_VSTG_DOC");
            Database.RemoveConstraint("GJI_INSPECTION_VIOL_STAGE", "FK_GJI_INSPECT_VSTG_VIO");
            Database.RemoveConstraint("GJI_ACTCHECK", "FK_GJI_ACTC_DOC");
            Database.RemoveConstraint("GJI_ACTCHECK_ROBJECT", "FK_GJI_ACTC_RO_ACT");
            Database.RemoveConstraint("GJI_ACTCHECK_ROBJECT", "FK_GJI_ACTC_RO_OBJ");
            Database.RemoveConstraint("GJI_ACTCHECK_VIOLAT", "FK_GJI_ACTC_VIOL_STG");
            Database.RemoveConstraint("GJI_ACTCHECK_VIOLAT", "FK_GJI_ACTC_VIOL_OBJ");
            Database.RemoveConstraint("GJI_ACTCHECK_DEFINITION", "FK_GJI_ACTC_DEF_ISD");
            Database.RemoveConstraint("GJI_ACTCHECK_DEFINITION", "FK_GJI_ACTC_DEF_DOC");
            Database.RemoveConstraint("GJI_ACTCHECK_ANNEX", "FK_GJI_ACTC_ANNEX_DOC");
            Database.RemoveConstraint("GJI_ACTCHECK_ANNEX", "FK_GJI_ACTC_ANNEX_FILE");
            Database.RemoveConstraint("GJI_ACTCHECK_WITNESS", "FK_GJI_ACTC_WIT_DOC");
            Database.RemoveConstraint("GJI_ACTSURVEY", "FK_GJI_ACTS_DOC");
            Database.RemoveConstraint("GJI_ACTSURVEY_ROBJECT", "FK_GJI_ACTS_RO_ACT");
            Database.RemoveConstraint("GJI_ACTSURVEY_ROBJECT", "FK_GJI_ACTS_RO_OBJ");
            Database.RemoveConstraint("GJI_ACTSURVEY_OWNER", "FK_GJI_ACTS_OWNER_DOC");
            Database.RemoveConstraint("GJI_ACTSURVEY_ANNEX", "FK_GJI_ACTS_ANNEX_DOC");
            Database.RemoveConstraint("GJI_ACTSURVEY_ANNEX", "FK_GJI_ACTS_ANNEX_FILE");
            Database.RemoveConstraint("GJI_ACTSURVEY_PHOTO", "FK_GJI_ACTS_PHOTO_DOC");
            Database.RemoveConstraint("GJI_ACTSURVEY_PHOTO", "FK_GJI_ACTS_PHOTO_FILE");
            Database.RemoveConstraint("GJI_ACTSURVEY_INSPECTPART", "FK_GJI_ACTS_INSPART_INP");
            Database.RemoveConstraint("GJI_ACTSURVEY_INSPECTPART", "FK_GJI_ACTS_INSPART_DOC");
            Database.RemoveConstraint("GJI_PRESCRIPTION", "FK_GJI_PRESCR_DOC");
            Database.RemoveConstraint("GJI_PRESCRIPTION", "FK_GJI_PRESCR_EXE");
            Database.RemoveConstraint("GJI_PRESCRIPTION", "FK_GJI_PRESCR_CTR");
            Database.RemoveConstraint("GJI_PRESCRIPTION_VIOLAT", "FK_GJI_PRESCR_VIOL_STG");
            Database.RemoveConstraint("GJI_PRESCRIPTION_ANNEX", "FK_GJI_PRESCR_ANNEX_DOC");
            Database.RemoveConstraint("GJI_PRESCRIPTION_ANNEX", "FK_GJI_PRESCR_ANNEX_FILE");
            Database.RemoveConstraint("GJI_PROTOCOL", "FK_GJI_PROT_DOC");
            Database.RemoveConstraint("GJI_PROTOCOL", "FK_GJI_PROT_EXE");
            Database.RemoveConstraint("GJI_PROTOCOL", "FK_GJI_PROT_CTR");
            Database.RemoveConstraint("GJI_PROTOCOL_VIOLAT", "FK_GJI_PROT_VIOLAT_STG");
            Database.RemoveConstraint("GJI_PROTOCOL_ANNEX", "FK_GJI_PROT_ANNEX_DOC");
            Database.RemoveConstraint("GJI_PROTOCOL_ANNEX", "FK_GJI_PROT_ANNEX_FILE");
            Database.RemoveConstraint("GJI_PROTOCOL_DEFINITION", "FK_GJI_PROT_DEF_ISD");
            Database.RemoveConstraint("GJI_PROTOCOL_DEFINITION", "FK_GJI_PROT_DEF_DOC");
            Database.RemoveConstraint("GJI_RESOLUTION", "FK_GJI_RESOL_DOC");
            Database.RemoveConstraint("GJI_RESOLUTION", "FK_GJI_RESOL_SNC");
            Database.RemoveConstraint("GJI_RESOLUTION", "FK_GJI_RESOL_EXE");
            Database.RemoveConstraint("GJI_RESOLUTION", "FK_GJI_RESOL_MCP");
            Database.RemoveConstraint("GJI_RESOLUTION", "FK_GJI_RESOL_CTR");
            Database.RemoveConstraint("GJI_RESOLUTION", "FK_GJI_RESOL_OFC");
            Database.RemoveConstraint("GJI_RESOLUTION_ANNEX", "FK_GJI_RESOL_ANNEX_DOC");
            Database.RemoveConstraint("GJI_RESOLUTION_ANNEX", "FK_GJI_RESOL_ANNEX_FILE");
            Database.RemoveConstraint("GJI_RESOLUTION_DEFINITION", "FK_GJI_RESOL_DEF_ISD");
            Database.RemoveConstraint("GJI_RESOLUTION_DEFINITION", "FK_GJI_RESOL_DEF_DOC");
            Database.RemoveConstraint("GJI_ACTREMOVAL", "FK_GJI_ACTR_DOC");
            Database.RemoveConstraint("GJI_ACTREMOVAL_VIOLAT", "FK_GJI_ACTR_VIOLAT_STG");
            Database.RemoveConstraint("GJI_DOCUMENT", "FK_GJI_DOCUMENT_INS");
            Database.RemoveConstraint("GJI_DOCUMENT", "FK_GJI_DOCUMENT_STG");
            Database.RemoveConstraint("GJI_DOCUMENT", "FK_GJI_DOCUMENT_STT");
            Database.RemoveConstraint("GJI_DOCUMENT_CHILDREN", "FK_GJI_DOCUMENT_CH_PAR");
            Database.RemoveConstraint("GJI_DOCUMENT_CHILDREN", "FK_GJI_DOCUMENT_CH_CHI");
            Database.RemoveConstraint("GJI_DISPOSAL", "FK_GJI_DISP_DOC");
            Database.RemoveConstraint("GJI_DISPOSAL", "FK_GJI_DISP_ISS");
            Database.RemoveConstraint("GJI_DISPOSAL", "FK_GJI_DISP_RES");
            Database.RemoveConstraint("GJI_DISPOSAL_PROVDOC", "FK_GJI_DISP_PROVD_DOC");
            Database.RemoveConstraint("GJI_DISPOSAL_PROVDOC", "FK_GJI_DISP_PROVD_PRD");
            Database.RemoveConstraint("GJI_DISPOSAL_TYPESURVEY", "FK_GJI_DISP_TS_DOC");
            Database.RemoveConstraint("GJI_DISPOSAL_TYPESURVEY", "FK_GJI_DISP_TS_TS");
            Database.RemoveConstraint("GJI_DISPOSAL_EXPERT", "FK_GJI_DISP_EXP_DOC");
            Database.RemoveConstraint("GJI_DISPOSAL_EXPERT", "FK_GJI_DISP_EXP_EXP");
            Database.RemoveConstraint("GJI_DISPOSAL_ANNEX", "FK_GJI_DISP_ANNEX_DOC");
            Database.RemoveConstraint("GJI_DISPOSAL_ANNEX", "FK_GJI_DISP_ANNEX_FILE");
            Database.RemoveConstraint("GJI_INSPECTION_JURPERSON", "FK_GJI_INSPECT_JPERS_INS");
            Database.RemoveConstraint("GJI_INSPECTION_JURPERSON", "FK_GJI_INSPECT_JPERS_PLAN");
            Database.RemoveConstraint("GJI_INSPECTION_STAGE", "FK_GJI_INSPECT_STG_INS");
            Database.RemoveConstraint("GJI_INSPECTION_STAGE", "FK_GJI_INSPECT_STG_PRT");
            Database.RemoveConstraint("GJI_INSPECTION_ROBJECT", "FK_GJI_INSPECT_RO_INS");
            Database.RemoveConstraint("GJI_INSPECTION_ROBJECT", "FK_GJI_INSPECT_RO_OBJ");

            Database.RemoveTable("GJI_STATEM_SOURS");
            Database.RemoveTable("GJI_REL_APPEAL_CITS");
            Database.RemoveTable("GJI_APPCIT_RO");
            Database.RemoveTable("GJI_APPCIT_STATSUBJ");
            Database.RemoveTable("GJI_APPEAL_SOURCES");
            Database.RemoveTable("GJI_INSPECTION_BASEDEF");
            Database.RemoveTable("GJI_INSPECTION_BASEDEF");
            Database.RemoveTable("GJI_BASESTAT_APPCIT");
            Database.RemoveTable("GJI_APPEAL_CITIZENS");
            Database.RemoveTable("GJI_DICT_ACREDT_FLAG");
            Database.RemoveTable("GJI_ACTCHECK_INSPECTPART");
            Database.RemoveTable("GJI_INSPECTION_PROSCLAIM");
            Database.RemoveTable("GJI_INSPECTION_DISPHEAD");
            Database.RemoveTable("GJI_DICT_TASKS_INSPECTION");
            Database.RemoveTable("GJI_PRESENTATION");
            Database.RemoveTable("GJI_PRESENTATION_ANNEX");
            Database.RemoveTable("GJI_DICT_STATEMENT_SUBJ");
            Database.RemoveTable("GJI_STATEMENT_STATSUBJECT");
            Database.RemoveTable("GJI_RESOLPROS_ROBJECT");
            Database.RemoveTable("GJI_INSPECTION_ACTIVITY");
            Database.RemoveTable("GJI_DICT_COMPETENT_ORG");
            Database.RemoveTable("GJI_DICT_ANSWER_CONTENT");
            Database.RemoveTable("GJI_STATEMENT_REQUEST");
            Database.RemoveTable("GJI_STATEMENT_ANSWER");
            Database.RemoveTable("GJI_DICT_ARTICLE_TSJ");
            Database.RemoveTable("GJI_DICT_KIND_PROT");
            Database.RemoveTable("GJI_ACTIVITY_TSJ");
            Database.RemoveTable("GJI_ACT_TSJ_STATUTE");
            Database.RemoveTable("GJI_ACT_TSJ_ARTICLE");
            Database.RemoveTable("GJI_ACT_TSJ_PROTOCOL");
            Database.RemoveTable("GJI_PROT_REAL_OBJ");
            Database.RemoveTable("GJI_ACT_TSJ_REAL_OBJ");
            Database.RemoveTable("GJI_DICT_KIND_WORK");
            Database.RemoveTable("GJI_BUISNES_NOTIF");
            Database.RemoveTable("GJI_DICT_SERV_JURID"); 
            Database.RemoveTable("GJI_DICT_FEATUREVIOL");
            Database.RemoveTable("GJI_DICT_VIOLATIONFEATURE");
            Database.RemoveTable("GJI_DICT_HEATSEASONPERIOD");
            Database.RemoveTable("GJI_INSPECTION_RESOLPROS");
            Database.RemoveTable("GJI_INSPECTION_HEATSEASON");
            Database.RemoveTable("GJI_HEATSEASON");
            Database.RemoveTable("GJI_HEATSEASON_DOCUMENT");
            Database.RemoveTable("GJI_RESOLPROS");
            Database.RemoveTable("GJI_RESOLPROS_ARTLAW");
            Database.RemoveTable("GJI_RESOLPROS_ANNEX");
            Database.RemoveTable("GJI_DOCUMENT_REFERENCE");
            Database.RemoveTable("GJI_DICT_LEGFOUND_INSPECT");
            Database.RemoveTable("GJI_DICT_KIND_INSPECTION");
            Database.RemoveTable("GJI_DICT_GOALS_INSPECTION");
            Database.RemoveTable("GJI_DICT_REVENUESOURCE");
            Database.RemoveTable("GJI_DICT_REVENUEFORM");
            Database.RemoveTable("GJI_DICT_PLANINSCHECK");
            Database.RemoveTable("GJI_DICT_KINDSTATEMENT");
            Database.RemoveTable("GJI_DICT_RESOLVE");
            Database.RemoveTable("GJI_INSPECTION_INSPECTOR");
            Database.RemoveTable("GJI_INSPECTION_DISPHEAD");
            Database.RemoveTable("GJI_INSPECTION_PROSCLAIM");
            Database.RemoveTable("GJI_INSPECTION_INSCHECK");
            Database.RemoveTable("GJI_INSPECTION_STATEMENT");
            Database.RemoveTable("GJI_ACTCHECK_PERIOD");
            Database.RemoveTable("GJI_DICT_ARTICLELAW");
            Database.RemoveTable("GJI_DICT_COURT");
            Database.RemoveTable("GJI_DICT_COURTVERDICT");
            Database.RemoveTable("GJI_DICT_INSTANCE");
            Database.RemoveTable("GJI_PROTOCOL_ARTLAW");
            Database.RemoveTable("GJI_PRESCRIPTION_ARTLAW");
            Database.RemoveTable("GJI_PRESCRIPTION_CANCEL");
            Database.RemoveTable("GJI_RESOLUTION_PAYFINE");
            Database.RemoveTable("GJI_RESOLUTION_DISPUTE");
            Database.RemoveTable("GJI_DICT_VIOLATION");
            Database.RemoveTable("GJI_DICT_INSPECTEDPART");
            Database.RemoveTable("GJI_DICT_EXECUTANT");
            Database.RemoveTable("GJI_DICT_SANCTION");
            Database.RemoveTable("GJI_INSPECTION_VIOLATION");
            Database.RemoveTable("GJI_INSPECTION_VIOL_STAGE");
            Database.RemoveTable("GJI_ACTCHECK");
            Database.RemoveTable("GJI_ACTCHECK_ROBJECT");
            Database.RemoveTable("GJI_ACTCHECK_VIOLAT");
            Database.RemoveTable("GJI_ACTCHECK_DEFINITION");
            Database.RemoveTable("GJI_ACTCHECK_ANNEX");
            Database.RemoveTable("GJI_ACTCHECK_WITNESS");
            Database.RemoveTable("GJI_ACTSURVEY");
            Database.RemoveTable("GJI_ACTSURVEY_ROBJECT");
            Database.RemoveTable("GJI_ACTSURVEY_OWNER");
            Database.RemoveTable("GJI_ACTSURVEY_ANNEX");
            Database.RemoveTable("GJI_ACTSURVEY_PHOTO");
            Database.RemoveTable("GJI_ACTSURVEY_INSPECTPART");
            Database.RemoveTable("GJI_PRESCRIPTION");
            Database.RemoveTable("GJI_PRESCRIPTION_VIOLAT");
            Database.RemoveTable("GJI_PRESCRIPTION_ANNEX");
            Database.RemoveTable("GJI_PROTOCOL");
            Database.RemoveTable("GJI_PROTOCOL_VIOLAT");
            Database.RemoveTable("GJI_PROTOCOL_ANNEX");
            Database.RemoveTable("GJI_PROTOCOL_DEFINITION");
            Database.RemoveTable("GJI_RESOLUTION");
            Database.RemoveTable("GJI_RESOLUTION_ANNEX");
            Database.RemoveTable("GJI_RESOLUTION_DEFINITION");
            Database.RemoveTable("GJI_ACTREMOVAL");
            Database.RemoveTable("GJI_ACTREMOVAL_VIOLAT");
            Database.RemoveTable("GJI_DICT_PLANJURPERSON");
            Database.RemoveTable("GJI_INSPECTION");
            Database.RemoveTable("GJI_INSPECTION_JURPERSON");
            Database.RemoveTable("GJI_INSPECTION_STAGE");
            Database.RemoveTable("GJI_INSPECTION_ROBJECT");
            Database.RemoveTable("GJI_DICT_PROVIDEDDOCUMENT");
            Database.RemoveTable("GJI_DICT_TYPESURVEY");
            Database.RemoveTable("GJI_DICT_EXPERT");
            Database.RemoveTable("GJI_DOCUMENT");
            Database.RemoveTable("GJI_DOCUMENT_CHILDREN");
            Database.RemoveTable("gji_document_inspector");
            Database.RemoveTable("GJI_DISPOSAL");
            Database.RemoveTable("GJI_DISPOSAL_PROVDOC");
            Database.RemoveTable("GJI_DISPOSAL_TYPESURVEY");
            Database.RemoveTable("GJI_DISPOSAL_EXPERT");
            Database.RemoveTable("GJI_DISPOSAL_ANNEX");
        }
    }
}