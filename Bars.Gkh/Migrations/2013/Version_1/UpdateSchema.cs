// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateSchema.cs" company="">
//   
// </copyright>
// <summary>
//   The update schema.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bars.Gkh.Migrations.Version_1
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery(@"create sequence HIBERNATE_SEQUENCE minvalue 1 start with 1 increment by 1");
            }

            //-----Вид риска
            Database.AddEntityTable(
                "GKH_DICT_KIND_RISK",
                new Column("NAME", DbType.String, 50),
                new Column("CODE", DbType.String, 300),
                new Column("DESCRIPTION", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_KIND_RISK_NAME", false, "GKH_DICT_KIND_RISK", "NAME");
            Database.AddIndex("IND_GKH_KIND_RISK_CODE", false, "GKH_DICT_KIND_RISK", "CODE");
            //------

            //-----Группа капитальности
            Database.AddEntityTable(
                "GKH_DICT_CAPITAL_GROUP", 
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull), 
                new Column("DESCRIPTION", DbType.String, 1000),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_CAPITALGR_NAME", false, "GKH_DICT_CAPITAL_GROUP", "NAME");
            //-----

            //-----Конструктивный элемент
            Database.AddEntityTable(
                "GKH_DICT_CONST_ELEMENT", 
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull), 
                new Column("CODE", DbType.String, 300, ColumnProperty.NotNull), 
                new Column("GRP", DbType.String, 100), 
                new Column("LIFETIME", DbType.Decimal),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_CONSTEL_NAME", false, "GKH_DICT_CONST_ELEMENT", "NAME");
            Database.AddIndex("IND_GKH_CONSTEL_CODE", false, "GKH_DICT_CONST_ELEMENT", "CODE");
            //-----

            //-----Прибор учета
            Database.AddEntityTable(
                "GKH_DICT_METERING_DEVICE", 
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull), 
                new Column("DESCRIPTION", DbType.String, 1000),
                new Column("TYPE_ACCOUNTING", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("EXTERNAL_ID", DbType.String, 36),
                new Column("ACCURACY_CLASS", DbType.String, 30));
            Database.AddIndex("IND_GKH_MET_DEV_NAME", false, "GKH_DICT_METERING_DEVICE", "NAME");
            //-----

            //-----Должность
            Database.AddEntityTable(
                "GKH_DICT_POSITION", 
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull), 
                new Column("CODE", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_POSITION_NAME", false, "GKH_DICT_POSITION", "NAME");
            Database.AddIndex("IND_GKH_POSITION_CODE", false, "GKH_DICT_POSITION", "CODE");
            //-----

            //-----Материалы кровли
            Database.AddEntityTable(
                "GKH_DICT_ROOFING_MATERIAL", 
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_ROOF_MAT_NAME", false, "GKH_DICT_ROOFING_MATERIAL", "NAME");
            //-----

            //-----Материалы стен
            Database.AddEntityTable(
                "GKH_DICT_WALL_MATERIAL",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_WALL_MAT_NAME", false, "GKH_DICT_WALL_MATERIAL", "NAME");
            //-----

            //-----Форма собственности
            Database.AddEntityTable(
                "GKH_DICT_TYPE_OWNERSHIP", 
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_TYPE_OWNER_NAME", false, "GKH_DICT_TYPE_OWNERSHIP", "NAME");
            //-----

            //-----Тип обслуживания
            Database.AddEntityTable(
                "GKH_DICT_TYPE_SERVICE",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_TYPE_SERV_NAME", false, "GKH_DICT_TYPE_SERVICE", "NAME");
            //-----

            //-----Тип проекта
            Database.AddEntityTable(
                "GKH_DICT_TYPE_PROJ",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_TYPE_PROJ_NAME", false, "GKH_DICT_TYPE_PROJ", "NAME");
            Database.AddIndex("IND_GKH_TYPE_PROJ_CODE", false, "GKH_DICT_TYPE_PROJ", "CODE");
            //-----

            //-----Муниципальное образование
            Database.AddEntityTable(
                "GKH_DICT_MUNICIPALITY", 
                new Column("CITY", DbType.String, 30), 
                new Column("CODE", DbType.String, 30), 
                new Column("GRP", DbType.String, 30), 
                new Column("NAME", DbType.String, 300), 
                new Column("OKATO", DbType.String, 30), 
                new Column("DESCRIPTION", DbType.String, 300), 
                new Column("FEDERALNUMBER", DbType.String, 30), 
                new Column("RAION", DbType.String, 30), 
                new Column("RAION_SELECT", DbType.Boolean, ColumnProperty.NotNull, false), 
                new Column("CUT", DbType.String, 10),
                new Column("FIAS_ID", DbType.String, 36),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_MUNIC_NAME", false, "GKH_DICT_MUNICIPALITY", "NAME");
            Database.AddIndex("IND_GKH_MUNIC_CODE", false, "GKH_DICT_MUNICIPALITY", "CODE");

            //-----

            //-----Разрез финансирования муниципального образования
            Database.AddEntityTable(
                "GKH_DICT_MUNICIPAL_SOURCE", 
                new Column("MUNICIPALITY_ID", DbType.Int64, 22, ColumnProperty.NotNull), 
                new Column("ADD_EK", DbType.String, 50), 
                new Column("ADD_FK", DbType.String, 50), 
                new Column("ADD_KR", DbType.String, 50), 
                new Column("KCSR", DbType.String, 50), 
                new Column("KES", DbType.String, 50), 
                new Column("KFSR", DbType.String, 50), 
                new Column("KIF", DbType.String, 50), 
                new Column("KVR", DbType.String, 50), 
                new Column("KVSR", DbType.String, 50), 
                new Column("SOURCE_FINANCING", DbType.String, 50),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_MUNIC_SRC_MCP", false, "GKH_DICT_MUNICIPAL_SOURCE", "MUNICIPALITY_ID");
            Database.AddForeignKey("FK_GKH_MUNIC_SRC_MCP", "GKH_DICT_MUNICIPAL_SOURCE", "MUNICIPALITY_ID", "GKH_DICT_MUNICIPALITY", "ID");
            //-----

            //-----Период
            Database.AddEntityTable(
                "GKH_DICT_PERIOD", 
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull), 
                new Column("DATE_START", DbType.DateTime, ColumnProperty.NotNull), 
                new Column("DATE_END", DbType.DateTime),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_PERIOD_NAME", false, "GKH_DICT_PERIOD", "NAME");
            //-----

            //-----Программа переселения
            Database.AddEntityTable(
                "GKH_DICT_RESETTLE_PROGRAM",
                new Column("DESCRIPTION", DbType.String, 1000),
                new Column("MATCH_FEDERAL_LAW", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("PERIOD_ID", DbType.Int64, 22),
                new Column("STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("USE_IN_EXPORT", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("VISIBILITY", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_RESPROG_NAME", false, "GKH_DICT_RESETTLE_PROGRAM", "NAME");
            Database.AddIndex("IND_GKH_RESPROG_PRD", false, "GKH_DICT_RESETTLE_PROGRAM", "PERIOD_ID");
            Database.AddForeignKey("FK_GKH_RESPROG_PRD", "GKH_DICT_RESETTLE_PROGRAM", "PERIOD_ID", "GKH_DICT_PERIOD", "ID");
            //-----

            //-----Единица измерения
            Database.AddEntityTable(
                "GKH_DICT_UNITMEASURE", 
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull), 
                new Column("SHORT_NAME", DbType.String, 20, ColumnProperty.NotNull), 
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_UNITMEASURE_NAME", false, "GKH_DICT_UNITMEASURE", "NAME");

            //-----

            //-----Вид оснащения
            Database.AddEntityTable(
                "GKH_DICT_KINDEQUIPMENT", 
                new Column("UNIT_MEASURE_ID", DbType.Int64, 22, ColumnProperty.NotNull), 
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull), 
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_KINDEQUIPMENT_NAME", false, "GKH_DICT_KINDEQUIPMENT", "NAME");
            Database.AddIndex("IND_GKH_KINDEQUIPMENT_UNIT", false, "GKH_DICT_KINDEQUIPMENT", "UNIT_MEASURE_ID");
            Database.AddForeignKey("FK_GKH_KINDEQUIPMENT_UNIT", "GKH_DICT_KINDEQUIPMENT", "UNIT_MEASURE_ID", "GKH_DICT_UNITMEASURE", "ID");

            //-----

            //-----Специальность
            Database.AddEntityTable(
                "GKH_DICT_SPECIALTY", 
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull), 
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_SPECIALTY_NAME", false, "GKH_DICT_SPECIALTY", "NAME");
            //-----

            //-----Работа
            Database.AddEntityTable(
                "GKH_DICT_WORK", 
                new Column("UNIT_MEASURE_ID", DbType.Int64, 22, ColumnProperty.NotNull), 
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull), 
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("CONSISTENT185FZ", DbType.Boolean, ColumnProperty.NotNull, false), 
                new Column("DATE_END", DbType.DateTime),
                new Column("TYPE_WORK", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("EXTERNAL_ID", DbType.String, 36),
                new Column("NORMATIVE", DbType.Decimal),
                new Column("CODE", DbType.String, 10));
            Database.AddIndex("IND_GKH_WORK_NAME", false, "GKH_DICT_WORK", "NAME");
            Database.AddIndex("IND_GKH_WORK_CODE", false, "GKH_DICT_WORK", "CODE");
            Database.AddIndex("IND_GKH_WORK_UNIT", false, "GKH_DICT_WORK", "UNIT_MEASURE_ID");
            Database.AddForeignKey("FK_GKH_WORK_UNIT", "GKH_DICT_WORK", "UNIT_MEASURE_ID", "GKH_DICT_UNITMEASURE", "ID");
            //-----

            //-----Замена шаблона
            Database.AddEntityTable(
                "GKH_TEMPLATE_REPLACEMENT",
                new Column("CODE", DbType.String, ColumnProperty.NotNull),
                new Column("FILE_INFO_ID", DbType.Int64, 22));
            Database.AddIndex("IND_GKH_TEMPLATE_REPL_CODE", false, "GKH_TEMPLATE_REPLACEMENT", "CODE");
            Database.AddIndex("IND_GKH_TEMPLATE_REPL_FILE", false, "GKH_TEMPLATE_REPLACEMENT", "FILE_INFO_ID");
            Database.AddForeignKey("FK_GKH_TEMPLATE_REPL_FILE", "GKH_TEMPLATE_REPLACEMENT", "FILE_INFO_ID", "B4_FILE_INFO", "ID");
            //------

            //-----Зональная жилищная инспекция
            Database.AddEntityTable(
                "GKH_DICT_ZONAINSP",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("ZONE_NAME", DbType.String, 300),
                new Column("SHORT_NAME", DbType.String, 300),
                new Column("ADDRESS", DbType.String, 300),
                new Column("PHONE", DbType.String, 50),
                new Column("EMAIL", DbType.String, 50),
                new Column("OKATO", DbType.String, 30),
                new Column("EXTERNAL_ID", DbType.String, 36),
                new Column("NAME_SECOND", DbType.String, 300),
                new Column("ZONE_NAME_SECOND", DbType.String, 300),
                new Column("SHORT_NAME_SECOND", DbType.String, 300),
                new Column("ADDRESS_SECOND", DbType.String, 300),
                new Column("NAME_GENETIVE", DbType.String, 300),
                new Column("NAME_DATIVE", DbType.String, 300),
                new Column("NAME_ACCUSATIVE", DbType.String, 300),
                new Column("NAME_ABLATIVE", DbType.String, 300),
                new Column("NAME_PREPOSITIONAL", DbType.String, 300),
                new Column("SHORT_NAME_GENETIVE", DbType.String, 300),
                new Column("SHORT_NAME_DATIVE", DbType.String, 300),
                new Column("SHORT_NAME_ACCUSATIVE", DbType.String, 300),
                new Column("SHORT_NAME_ABLATIVE", DbType.String, 300),
                new Column("SHORT_NAME_PREPOSITIONAL", DbType.String, 300));
            Database.AddIndex("IND_ZONAI_NSP_NAME", false, "GKH_DICT_ZONAINSP", "NAME");
            //-----

            //-----Сводная таблица между Зональной жилищной инспекцией и муниципальным образованием
            Database.AddEntityTable(
                "GKH_DICT_ZONAINSP_MUNIC",
                new Column("ZONAL_INSPECTION_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("MUNICIPALITY_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_ZONA_MUNIC_ZON", false, "GKH_DICT_ZONAINSP_MUNIC", "ZONAL_INSPECTION_ID");
            Database.AddIndex("IND_DICT_ZONAINSP_MUNIC_MCP", false, "GKH_DICT_ZONAINSP_MUNIC", "MUNICIPALITY_ID");
            Database.AddForeignKey("FK_ZONA_MUNIC_MCP", "GKH_DICT_ZONAINSP_MUNIC", "MUNICIPALITY_ID", "GKH_DICT_MUNICIPALITY", "ID");
            Database.AddForeignKey("FK_ZONA_MUNIC_ZON", "GKH_DICT_ZONAINSP_MUNIC", "ZONAL_INSPECTION_ID", "GKH_DICT_ZONAINSP", "ID");
            //-----

            //-----Инспекторы
            Database.AddEntityTable(
                "GKH_DICT_INSPECTOR",
                new Column("CODE", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODEEDO", DbType.Int32),
                new Column("POSITION", DbType.String, 300),
                new Column("FIO", DbType.String, 300, ColumnProperty.NotNull),
                new Column("SHORTFIO", DbType.String, 100),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("IS_HEAD", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("FIO_GENITIVE", DbType.String, 300),
                new Column("FIO_DATIVE", DbType.String, 300),
                new Column("FIO_ACCUSATIVE", DbType.String, 300),
                new Column("FIO_ABLATIVE", DbType.String, 300),
                new Column("FIO_PREPOSITIONAL", DbType.String, 300),
                new Column("POSITION_GENITIVE", DbType.String, 300),
                new Column("POSITION_DATIVE", DbType.String, 300),
                new Column("POSITION_ACCUSATIVE", DbType.String, 300),
                new Column("POSITION_ABLATIVE", DbType.String, 300),
                new Column("POSITION_PREPOSITIONAL", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36),
                new Column("PHONE", DbType.String, 300));
            Database.AddIndex("IND_GKH_INSPECTOR_FIO", false, "GKH_DICT_INSPECTOR", "FIO");
            Database.AddIndex("IND_GKH_INSPECTOR_CODE", false, "GKH_DICT_INSPECTOR", "CODE");
            //-----

            //-----Сводная таблица между Зональной жилищной инспекцией и инспектором
            Database.AddEntityTable(
                "GKH_DICT_ZONAINSP_INSPECT",
                new Column("INSPECTOR_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("ZONAL_INSPECTION_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_ZONA_INS_INSP", false, "GKH_DICT_ZONAINSP_INSPECT", "INSPECTOR_ID");
            Database.AddIndex("IND_ZONA_INS_ZON", false, "GKH_DICT_ZONAINSP_INSPECT", "ZONAL_INSPECTION_ID");
            Database.AddForeignKey("FK_ZONA_INS_INSP", "GKH_DICT_ZONAINSP_INSPECT", "INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID");
            Database.AddForeignKey("FK_ZONA_INS_ZON", "GKH_DICT_ZONAINSP_INSPECT", "ZONAL_INSPECTION_ID", "GKH_DICT_ZONAINSP", "ID");
            //-----            
            
            // Дальнейшее использование
            Database.AddEntityTable(
                "GKH_DICT_FURTHER_USE",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_FURTHER_USE_NAME", false, "GKH_DICT_FURTHER_USE", "NAME");
            Database.AddIndex("IND_GKH_FURTHER_USE_CODE", false, "GKH_DICT_FURTHER_USE", "CODE");
            // ----

            // Основание нецелесообразности
            Database.AddEntityTable(
                "GKH_DICT_REAS_INEXPEDIENT",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_REAS_IN_NAME", false, "GKH_DICT_REAS_INEXPEDIENT", "NAME");
            Database.AddIndex("IND_GKH_REAS_IN_CODE", false, "GKH_DICT_REAS_INEXPEDIENT", "CODE");
            // ----

            // Источник по программе переселения
            Database.AddEntityTable(
                "GKH_DICT_RESETTLE_SOURCE",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_RESP_SRC_NAME", false, "GKH_DICT_RESETTLE_SOURCE", "NAME");
            Database.AddIndex("IND_GKH_RESP_SRC_CODE", false, "GKH_DICT_RESETTLE_SOURCE", "CODE");
            // ----

            //-----Вид деятельности страховой организации
            Database.AddEntityTable(
                "GKH_DICT_BELAY_KIND_ACTIV",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 1000),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_BELAY_KIND_NAME", false, "GKH_DICT_BELAY_KIND_ACTIV", "NAME");
            Database.AddIndex("IND_GKH_BELAY_KIND_CODE", false, "GKH_DICT_BELAY_KIND_ACTIV", "CODE");
            //------

            //-----Организационно правовая форма
            Database.AddEntityTable(
                "GKH_DICT_ORG_FORM", 
                new Column("CODE", DbType.String, 50),
                new Column("NAME", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_ORG_FORM_NAME", false, "GKH_DICT_ORG_FORM", "NAME");
            Database.AddIndex("IND_GKH_ORG_FORM_CODE", false, "GKH_DICT_ORG_FORM", "CODE");
            //-----

            //-----Учебное заведение
            Database.AddEntityTable(
                "GKH_DICT_INSTITUTIONS",
                new Column("NAME", DbType.String, 50),
                new Column("ABBREVIATION", DbType.String, 300),
                new Column("ADDRESS_ID", DbType.Int64, 22),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_INST_NAME", false, "GKH_DICT_INSTITUTIONS", "NAME");
            Database.AddIndex("IND_GKH_INST_ADDR", false, "GKH_DICT_INSTITUTIONS", "ADDRESS_ID");
            Database.AddForeignKey("FK_GKH_INST_ADDR", "GKH_DICT_INSTITUTIONS", "ADDRESS_ID", "B4_FIAS_ADDRESS", "ID");
            //-----

            //-----справочник видов работ текущего ремонта
            Database.AddEntityTable(
                "GKH_DICT_WORK_CUR_REPAIR",
                new Column("NAME", DbType.String, 300),
                new Column("CODE", DbType.String, 300),
                new Column("UNIT_MEASURE_ID", DbType.Int64, 22),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_WORK_CUR_REP_NAME", false, "GKH_DICT_WORK_CUR_REPAIR", "NAME");
            Database.AddIndex("IND_GKH_WORK_CUR_REP_CODE", false, "GKH_DICT_WORK_CUR_REPAIR", "CODE");
            Database.AddIndex("IND_GKH_WORK_CUR_REP_UM", false, "GKH_DICT_WORK_CUR_REPAIR", "UNIT_MEASURE_ID");
            Database.AddForeignKey("FK_GKH_WORK_CUR_REP_UM", "GKH_DICT_WORK_CUR_REPAIR", "UNIT_MEASURE_ID", "GKH_DICT_UNITMEASURE", "ID");
            //------

            //-----Жилой дом
            Database.AddEntityTable(
                "GKH_REALITY_OBJECT", 
                new Column("MUNICIPALITY_ID", DbType.Int64, 22, ColumnProperty.NotNull), 
                new Column("TYPE_OWNERSHIP_ID", DbType.Int64, 22), 
                new Column("CAPITAL_GROUP_ID", DbType.Int64, 22), 
                new Column("ROOFING_MATERIAL_ID", DbType.Int64, 22), 
                new Column("WALL_MATERIAL_ID", DbType.Int64, 22), 
                new Column("CONDITION_HOUSE", DbType.Int32, 4, ColumnProperty.NotNull, 10), 
                new Column("HAVING_BASEMENT", DbType.Int32, 4, ColumnProperty.NotNull, 30), 
                new Column("HEATING_SYSTEM", DbType.Int32, 4, ColumnProperty.NotNull, 10), 
                new Column("TYPE_HOUSE", DbType.Int32, 4, ColumnProperty.NotNull, 10), 
                new Column("TYPE_ROOF", DbType.Int32, 4, ColumnProperty.NotNull, 10), 
                new Column("ADDRESS", DbType.String, 1000), 
                new Column("AREA_BASEMENT", DbType.Decimal), 
                new Column("AREA_LIVING", DbType.Decimal), 
                new Column("AREA_LIV_NOT_LIV_MKD", DbType.Decimal), 
                new Column("AREA_LIVING_OWNED", DbType.Decimal), 
                new Column("AREA_MKD", DbType.Decimal),
                new Column("DATE_DEMOLITION", DbType.DateTime),
                new Column("FLOORS", DbType.Int32), 
                new Column("MAXIMUM_FLOORS", DbType.Int32), 
                new Column("IS_INSURED_OBJECT", DbType.Boolean, ColumnProperty.NotNull, false), 
                new Column("NOTATION", DbType.String, 1000), 
                new Column("NUMBER_APARTMENTS", DbType.Int32), 
                new Column("NUMBER_ENTRANCES", DbType.Int32), 
                new Column("NUMBER_LIFTS", DbType.Int32), 
                new Column("NUMBER_LIVING", DbType.Int32), 
                new Column("PHYSICAL_WEAR", DbType.Decimal), 
                new Column("SERIES_HOME", DbType.String, 50),
                new Column("DATE_COMMISSIONING", DbType.DateTime),
                new Column("DATE_LAST_RENOVATION", DbType.DateTime),
                new Column("FIAS_ADDRESS_ID", DbType.Int64, 22),
                new Column("EXTERNAL_ID", DbType.String, 36),
                new Column("DESCRIPTION", DbType.String, 1000),
                new Column("CODE_ERC", DbType.String, 250),
                new Column("FEDERAL_NUMBER", DbType.String, 300),
                new Column("GKH_CODE", DbType.String, 100),
                new Column("WEB_CAMERA_URL", DbType.String, 1000),
                new Column("DATE_TECH_INS", DbType.DateTime),
                new Column("RESIDENTS_EVICTED", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddIndex("IND_GKH_REALITY_OBJECT_CGR", false, "GKH_REALITY_OBJECT", "CAPITAL_GROUP_ID");
            Database.AddIndex("IND_GKH_REALITY_OBJECT_MCP", false, "GKH_REALITY_OBJECT", "MUNICIPALITY_ID");
            Database.AddIndex("IND_GKH_REALITY_OBJECT_RMT", false, "GKH_REALITY_OBJECT", "ROOFING_MATERIAL_ID");
            Database.AddIndex("IND_GKH_REALITY_OBJECT_OWN", false, "GKH_REALITY_OBJECT", "TYPE_OWNERSHIP_ID");
            Database.AddIndex("IND_GKH_REALITY_OBJECT_ADR", false, "GKH_REALITY_OBJECT", "FIAS_ADDRESS_ID");
            Database.AddForeignKey("FK_GKH_REALITY_OBJECT_ADR", "GKH_REALITY_OBJECT", "FIAS_ADDRESS_ID", "B4_FIAS_ADDRESS", "ID");
            Database.AddForeignKey("FK_GKH_REALITY_OBJECT_CGR", "GKH_REALITY_OBJECT", "CAPITAL_GROUP_ID", "GKH_DICT_CAPITAL_GROUP", "ID");
            Database.AddForeignKey("FK_GKH_REALITY_OBJECT_MCP", "GKH_REALITY_OBJECT", "MUNICIPALITY_ID", "GKH_DICT_MUNICIPALITY", "ID");
            Database.AddForeignKey("FK_GKH_REALITY_OBJECT_RMT", "GKH_REALITY_OBJECT", "ROOFING_MATERIAL_ID", "GKH_DICT_ROOFING_MATERIAL", "ID");
            Database.AddForeignKey("FK_GKH_REALITY_OBJECT_OWN", "GKH_REALITY_OBJECT", "TYPE_OWNERSHIP_ID", "GKH_DICT_TYPE_OWNERSHIP", "ID");

            //-----

            //-----Члены совета Многоквартирного дома (МКД)
            Database.AddEntityTable(
                "GKH_OBJ_COUNCILLORS",
                new Column("REALITY_OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FIO", DbType.String, 300),
                new Column("POST", DbType.Int32, 4, ColumnProperty.NotNull),
                new Column("PHONE", DbType.String, 50),
                new Column("EMAIL", DbType.String, 100),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_OBJ_COUNCIL_RO", false, "GKH_OBJ_COUNCILLORS", "REALITY_OBJECT_ID");
            Database.AddForeignKey("FK_GKH_OBJ_COUNCIL_RO", "GKH_OBJ_COUNCILLORS", "REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID");
            //------

            //-----Протокол собрания жильцов
            Database.AddEntityTable(
                "GKH_OBJ_PROTOCOL_MT",
                new Column("REALITY_OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("DOCUMENT_NAME", DbType.String, 300),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("DATE_FROM", DbType.DateTime),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_OBJ_PRTCL_MT_FILE", false, "GKH_OBJ_PROTOCOL_MT", "FILE_ID");
            Database.AddIndex("IND_GKH_OBJ_PRTCL_MT_RO", false, "GKH_OBJ_PROTOCOL_MT", "REALITY_OBJECT_ID");
            Database.AddForeignKey("FK_GKH_OBJ_PRTCL_MT_FILE", "GKH_OBJ_PROTOCOL_MT", "FILE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_GKH_OBJ_PRTCL_MT_RO", "GKH_OBJ_PROTOCOL_MT", "REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID");
            //------

            //-----Сведения по квартирам жилого дома
            Database.AddEntityTable(
                "GKH_OBJ_APARTMENT_INFO", 
                new Column("REALITY_OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull), 
                new Column("AREA_LIVING", DbType.Decimal), 
                new Column("AREA_TOTAL", DbType.Decimal), 
                new Column("COUNT_PEOPLE", DbType.Int32),
                new Column("PRIVATIZED", DbType.Int32, 4, ColumnProperty.NotNull, 30),
                new Column("PHONE", DbType.String, 50),
                new Column("EXTERNAL_ID", DbType.String, 36),
                new Column("NUM_APARTMENT", DbType.String, 20),
                new Column("FIO_OWNER", DbType.String, 500));
            Database.AddIndex("IND_GKH_OBJ_APARTINFO_RO", false, "GKH_OBJ_APARTMENT_INFO", "REALITY_OBJECT_ID");
            Database.AddForeignKey("FK_GKH_OBJ_APARTINFO_RO", "GKH_OBJ_APARTMENT_INFO", "REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID");

            //-----

            //-----Текущий ремонт жилого дома
            Database.AddEntityTable(
                "GKH_OBJ_CURENT_REPAIR", 
                new Column("REALITY_OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull), 
                new Column("FACT_DATE", DbType.DateTime), 
                new Column("FACT_SUM", DbType.Decimal), 
                new Column("FACT_WORK", DbType.Decimal), 
                new Column("PLAN_DATE", DbType.DateTime), 
                new Column("PLAN_SUM", DbType.Decimal), 
                new Column("PLAN_WORK", DbType.Decimal), 
                new Column("UNIT_MEASURE", DbType.String, 50),
                new Column("WORK_KIND_ID", DbType.Int64, 22),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_OBJ_CUR_REP_RO", false, "GKH_OBJ_CURENT_REPAIR", "REALITY_OBJECT_ID");
            Database.AddIndex("IND_GKH_OBJ_WORK_KIND", false, "GKH_OBJ_CURENT_REPAIR", "WORK_KIND_ID");
            Database.AddForeignKey("FK_GKH_OBJ_WORK_KIND", "GKH_OBJ_CURENT_REPAIR", "WORK_KIND_ID", "GKH_DICT_WORK_CUR_REPAIR", "ID");
            Database.AddForeignKey("FK_GKH_OBJ_CUR_REP_RO", "GKH_OBJ_CURENT_REPAIR", "REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID");

            //-----

            //-----Сведения о помещениях жилого дома
            Database.AddEntityTable(
                "GKH_OBJ_HOUSE_INFO", 
                new Column("REALITY_OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull), 
                new Column("DATE_REG", DbType.DateTime), 
                new Column("DATE_REG_OWNER", DbType.DateTime), 
                new Column("KIND_RIGHT", DbType.Int32, 4, ColumnProperty.NotNull, 10), 
                new Column("NUM", DbType.String, 50), 
                new Column("NUM_REG_RIGHT", DbType.String, 50), 
                new Column("NAME", DbType.String, 50), 
                new Column("OWNER", DbType.String, 50), 
                new Column("TOTAL_AREA", DbType.Decimal),
                new Column("UNIT_MEASURE_ID", DbType.Int64, 22),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_OBJ_HOUSE_INFO_N", false, "GKH_OBJ_HOUSE_INFO", "NAME");
            Database.AddIndex("IND_GKH_OBJ_HOUSE_INFO_RO", false, "GKH_OBJ_HOUSE_INFO", "REALITY_OBJECT_ID");
            Database.AddIndex("IND_GKH_OBJ_HOUSE_INFO_UM", false, "GKH_OBJ_HOUSE_INFO", "UNIT_MEASURE_ID");
            Database.AddForeignKey("FK_GKH_OBJ_HOUSE_INFO_UM", "GKH_OBJ_HOUSE_INFO", "UNIT_MEASURE_ID", "GKH_DICT_UNITMEASURE", "ID");
            Database.AddForeignKey("FK_GKH_OBJ_HOUSE_INFO_RO", "GKH_OBJ_HOUSE_INFO", "REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID");

            //-----

            //-----Фото-архив жилого дома
            Database.AddEntityTable(
                "GKH_OBJ_IMAGE", 
                new Column("REALITY_OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull), 
                new Column("FILE_ID", DbType.Int64), 
                new Column("DATE_IMAGE", DbType.DateTime), 
                new Column("DESCRIPTION", DbType.String, 300), 
                new Column("IMAGES_GROUP", DbType.Int32, 4, ColumnProperty.NotNull, 10), 
                new Column("TO_PRINT", DbType.Boolean, ColumnProperty.NotNull, false), 
                new Column("NAME", DbType.String, 100),
                new Column("PERIOD_ID", DbType.Int64, 22),
                new Column("WORK_ID", DbType.Int64, 22),
                new Column("EXTERNAL_ID", DbType.String, 36)); 
            Database.AddIndex("IND_GKH_OBJ_IMAGE_NAME", false, "GKH_OBJ_IMAGE", "NAME");
            Database.AddIndex("IND_GKH_OBJ_IMAGE_RO", false, "GKH_OBJ_IMAGE", "REALITY_OBJECT_ID");
            Database.AddIndex("IND_GKH_OBJ_IMAGE_DOC", false, "GKH_OBJ_IMAGE", "FILE_ID");
            Database.AddIndex("IND_GKH_OBJ_IMAGE_PR", false, "GKH_OBJ_IMAGE", "PERIOD_ID");
            Database.AddIndex("IND_GKH_OBJ_IMAGE_WORK", false, "GKH_OBJ_IMAGE", "WORK_ID");
            Database.AddForeignKey("FK_GKH_OBJ_IMAGE_WORK", "GKH_OBJ_IMAGE", "WORK_ID", "GKH_DICT_WORK", "ID");
            Database.AddForeignKey("FK_GKH_OBJ_IMAGE_PR", "GKH_OBJ_IMAGE", "PERIOD_ID", "GKH_DICT_PERIOD", "ID");
            Database.AddForeignKey("FK_GKH_OBJ_IMAGE_RO", "GKH_OBJ_IMAGE", "REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID");
            Database.AddForeignKey("FK_GKH_OBJ_IMAGE_FILE", "GKH_OBJ_IMAGE", "FILE_ID", "B4_FILE_INFO", "ID");

            //-----

            //-----Земельный участок жилого дома
            Database.AddEntityTable(
                "GKH_OBJ_LAND", 
                new Column("REALITY_OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull), 
                new Column("FILE_ID", DbType.Int64), 
                new Column("CADASTR_NUM", DbType.String, 300), 
                new Column("DATE_LAST_REG", DbType.DateTime), 
                new Column("DOCUMENT_DATE", DbType.DateTime), 
                new Column("DOCUMENT_NUM", DbType.String, 300), 
                new Column("DOCUMENT_NAME", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_OBJ_LAND_RO", false, "GKH_OBJ_LAND", "REALITY_OBJECT_ID");
            Database.AddIndex("IND_GKH_OBJ_LAND_FILE", false, "GKH_OBJ_LAND", "FILE_ID");
            Database.AddForeignKey("FK_GKH_OBJ_LAND_RO", "GKH_OBJ_LAND", "REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID");
            Database.AddForeignKey("FK_GKH_OBJ_LAND_FILE", "GKH_OBJ_LAND", "FILE_ID", "B4_FILE_INFO", "ID");

            //-----

            //-----Приборы учета жилого дома
            Database.AddEntityTable(
                "GKH_OBJ_METERING_DEVICE", 
                new Column("REALITY_OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull), 
                new Column("METERING_DEVICE_ID", DbType.Int64, 22, ColumnProperty.NotNull), 
                new Column("DESCRIPTION", DbType.String, 1000),
                new Column("DATE_REGISTRATION", DbType.DateTime),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_OBJ_MET_DEV_RO", false, "GKH_OBJ_METERING_DEVICE", "REALITY_OBJECT_ID");
            Database.AddIndex("IND_GKH_OBJ_MET_DEV_MDV", false, "GKH_OBJ_METERING_DEVICE", "METERING_DEVICE_ID");
            Database.AddForeignKey("FK_GKH_OBJ_MET_DEV_RO", "GKH_OBJ_METERING_DEVICE", "REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID");
            Database.AddForeignKey("FK_GKH_OBJ_MET_DEV_MDV", "GKH_OBJ_METERING_DEVICE", "METERING_DEVICE_ID", "GKH_DICT_METERING_DEVICE", "ID");

            //-----

            //-----Конструктивные элементы жилого дома
            Database.AddEntityTable(
                "GKH_OBJ_CONST_ELEMENT", 
                new Column("REALITY_OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull), 
                new Column("CONST_ELEMENT_ID", DbType.Int64), 
                new Column("LAST_YEAR_OVERHAUL", DbType.Int32),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_OBJ_CONSTEL_RO", false, "GKH_OBJ_CONST_ELEMENT", "REALITY_OBJECT_ID");
            Database.AddIndex("IND_GKH_OBJ_CONSTEL_CEL", false, "GKH_OBJ_CONST_ELEMENT", "CONST_ELEMENT_ID");
            Database.AddForeignKey("FK_GKH_OBJ_CONSTELEMENT_OBJ", "GKH_OBJ_CONST_ELEMENT", "REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID");
            Database.AddForeignKey("FK_GKH_OBJ_CONSTEL_CEL", "GKH_OBJ_CONST_ELEMENT", "CONST_ELEMENT_ID", "GKH_DICT_CONST_ELEMENT", "ID");

            //-----

            //-----Аварийность жилого дома
            Database.AddEntityTable(
                "GKH_EMERGENCY_OBJECT",
                new Column("REALTITY_OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("REASON_INEXPEDIENT_ID", DbType.Int64, 22),
                new Column("FURTHER_USE_ID", DbType.Int64, 22),
                new Column("FILE_INFO_ID", DbType.Int64, 22),
                new Column("CADASTRAL_NUMBER", DbType.String, 300),
                new Column("ACTUAL_INFO_DATE", DbType.DateTime),
                new Column("DEMOLITION_DATE", DbType.DateTime),
                new Column("DESCRIPTION", DbType.String, 300),
                new Column("DOCUMENT_NAME", DbType.String, 100),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("IS_REPAIR_EXPEDIENT", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("LAND_AREA", DbType.Decimal),
                new Column("RESETTLEMENT_FLAT_AREA", DbType.Decimal),
                new Column("CONDITION_HOUSE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("RESETTLEMENT_FLAT_AMOUNT", DbType.Int32),
                new Column("RESETTLEMENT_PROG_ID", DbType.Int64, 22),
                new Column("STATE_ID", DbType.Int64, 22),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_EMER_RO", false, "GKH_EMERGENCY_OBJECT", "REALTITY_OBJECT_ID");
            Database.AddIndex("IND_GKH_EMER_USE", false, "GKH_EMERGENCY_OBJECT", "FURTHER_USE_ID");
            Database.AddIndex("IND_GKH_EMER_RIN", false, "GKH_EMERGENCY_OBJECT", "REASON_INEXPEDIENT_ID");
            Database.AddIndex("IND_GKH_EMER_INF", false, "GKH_EMERGENCY_OBJECT", "FILE_INFO_ID");
            Database.AddIndex("IND_GKH_EMER_PROG", false, "GKH_EMERGENCY_OBJECT", "RESETTLEMENT_PROG_ID");
            Database.AddIndex("IND_GKH_EMER_ST", false, "GKH_EMERGENCY_OBJECT", "STATE_ID");
            Database.AddForeignKey("FK_GKH_EMER_RO", "GKH_EMERGENCY_OBJECT", "REALTITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID");
            Database.AddForeignKey("FK_GKH_EMER_RIN", "GKH_EMERGENCY_OBJECT", "REASON_INEXPEDIENT_ID", "GKH_DICT_REAS_INEXPEDIENT", "ID");
            Database.AddForeignKey("FK_GKH_EMER_USE", "GKH_EMERGENCY_OBJECT", "FURTHER_USE_ID", "GKH_DICT_FURTHER_USE", "ID");
            Database.AddForeignKey("FK_GKH_EMER_INF", "GKH_EMERGENCY_OBJECT", "FILE_INFO_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_GKH_EMER_PROG", "GKH_EMERGENCY_OBJECT", "RESETTLEMENT_PROG_ID", "GKH_DICT_RESETTLE_PROGRAM", "ID");
            Database.AddForeignKey("FK_GKH_EMER_ST", "GKH_EMERGENCY_OBJECT", "STATE_ID", "B4_STATE", "ID");
            //-----

            //-----Программа переселения
            Database.AddEntityTable(
                "GKH_EMERGENCY_RESETPROG",
                new Column("EMERGENCY_OBJ_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("RESETTLEMENT_SOURCE_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("AREA", DbType.Decimal),
                new Column("COST", DbType.Decimal),
                new Column("COUNT_RESIDENTS", DbType.Int32),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_EMER_RESP_RO", false, "GKH_EMERGENCY_RESETPROG", "EMERGENCY_OBJ_ID");
            Database.AddIndex("IND_GKH_EMER_RESP_SRC", false, "GKH_EMERGENCY_RESETPROG", "RESETTLEMENT_SOURCE_ID");
            Database.AddForeignKey("FK_GKH_EMER_RESP_RO", "GKH_EMERGENCY_RESETPROG", "EMERGENCY_OBJ_ID", "GKH_EMERGENCY_OBJECT", "ID");
            Database.AddForeignKey("FK_GKH_EMER_RESP_SRC", "GKH_EMERGENCY_RESETPROG", "RESETTLEMENT_SOURCE_ID", "GKH_DICT_RESETTLE_SOURCE", "ID");
            //-----

            //-----Контрагенты
            Database.AddEntityTable(
                "GKH_CONTRAGENT", 
                new Column("PARENT_ID", DbType.Int64, 22), 
                new Column("CONTRAGENT_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10), 
                new Column("ADDRESS_OUT_SUBJECT", DbType.String, 500), 
                new Column("FACT_ADDRESS", DbType.String, 500), 
                new Column("JURIDICAL_ADDRESS", DbType.String, 500), 
                new Column("MAILING_ADDRESS", DbType.String, 500), 
                new Column("DATE_TERMINATION", DbType.DateTime), 
                new Column("DESCRIPTION", DbType.String, 1000), 
                new Column("EMAIL", DbType.String, 50), 
                new Column("INN", DbType.String, 15), 
                new Column("KPP", DbType.String, 15), 
                new Column("IS_SITE", DbType.Boolean, ColumnProperty.NotNull, false), 
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull), 
                new Column("OFFICIAL_WEBSITE", DbType.String, 100), 
                new Column("OGRN", DbType.String, 15), 
                new Column("OGRN_REG", DbType.String, 15),
                new Column("PHONE", DbType.String, 50), 
                new Column("PHONE_DISPATCH_SERVICE", DbType.String, 50), 
                new Column("SUBSCRIBER_BOX", DbType.String, 300), 
                new Column("TWEETER_ACCOUNT", DbType.String, 300), 
                new Column("YEAR_REG", DbType.Int32), 
                new Column("ACTIVITY_TERMINATION", DbType.Int32, 4, ColumnProperty.NotNull, 10), 
                new Column("ACTIVITY_DATE_END", DbType.DateTime), 
                new Column("ACTIVITY_DATE_START", DbType.DateTime), 
                new Column("ACTIVITY_DESCRIPTION", DbType.String, 500),
                new Column("ORG_LEGAL_FORM_ID", DbType.Int64, 22),
                new Column("FIAS_FACT_ADDRESS_ID", DbType.Int64, 22),
                new Column("FIAS_JUR_ADDRESS_ID", DbType.Int64, 22),
                new Column("FIAS_MAIL_ADDRESS_ID", DbType.Int64, 22),
                new Column("FIAS_OUT_ADDRESS_ID", DbType.Int64, 22),
                new Column("OKPO", DbType.Int32, 10),
                new Column("OKVED", DbType.String, 50),
                new Column("SHORT_NAME", DbType.String, 300),
                new Column("MUNICIPALITY_ID", DbType.Int64, 22),
                new Column("EXTERNAL_ID", DbType.String, 36),
                new Column("NAME_GENITIVE", DbType.String, 300),
                new Column("NAME_DATIVE", DbType.String, 300),
                new Column("NAME_ACCUSATIVE", DbType.String, 300),
                new Column("NAME_ABLATIVE", DbType.String, 300),
                new Column("NAME_PREPOSITIONAL", DbType.String, 300));
            Database.AddIndex("IND_GKH_CONTR_NAME", false, "GKH_CONTRAGENT", "NAME");
            Database.AddIndex("IND_GKH_CONTR_INN", false, "GKH_CONTRAGENT", "INN");
            Database.AddIndex("IND_GKH_CONTR_SNAME", false, "GKH_CONTRAGENT", "SHORT_NAME");
            Database.AddIndex("IND_GKH_CONTR_PRT", false, "GKH_CONTRAGENT", "PARENT_ID");
            Database.AddIndex("ind_gkh_contr_orgf", false, "GKH_CONTRAGENT", "ORG_LEGAL_FORM_ID");
            Database.AddIndex("IND_GKH_CONTR_OADR", false, "GKH_CONTRAGENT", "FIAS_OUT_ADDRESS_ID");
            Database.AddIndex("IND_GKH_CONTR_FADR", false, "GKH_CONTRAGENT", "FIAS_FACT_ADDRESS_ID");
            Database.AddIndex("IND_GKH_CONTR_JADR", false, "GKH_CONTRAGENT", "FIAS_JUR_ADDRESS_ID");
            Database.AddIndex("IND_GKH_CONTR_MADR", false, "GKH_CONTRAGENT", "FIAS_MAIL_ADDRESS_ID");
            Database.AddIndex("IND_GKH_CONTR_MCP", false, "GKH_CONTRAGENT", "MUNICIPALITY_ID");
            Database.AddForeignKey("FK_GKH_CONTR_MCP", "GKH_CONTRAGENT", "MUNICIPALITY_ID", "GKH_DICT_MUNICIPALITY", "ID");
            Database.AddForeignKey("FK_GKH_CONTR_OADR", "GKH_CONTRAGENT", "FIAS_OUT_ADDRESS_ID", "B4_FIAS_ADDRESS", "ID");
            Database.AddForeignKey("FK_GKH_CONTR_MADR", "GKH_CONTRAGENT", "FIAS_MAIL_ADDRESS_ID", "B4_FIAS_ADDRESS", "ID");
            Database.AddForeignKey("FK_GKH_CONTR_JADR", "GKH_CONTRAGENT", "FIAS_JUR_ADDRESS_ID", "B4_FIAS_ADDRESS", "ID");
            Database.AddForeignKey("FK_GKH_CONTR_FADR", "GKH_CONTRAGENT", "FIAS_FACT_ADDRESS_ID", "B4_FIAS_ADDRESS", "ID");
            Database.AddForeignKey("FK_GKH_CONTR_PRT", "GKH_CONTRAGENT", "PARENT_ID", "GKH_CONTRAGENT", "ID");
            Database.AddForeignKey("fk_gkh_contr_orgf", "GKH_CONTRAGENT", "ORG_LEGAL_FORM_ID", "GKH_DICT_ORG_FORM", "ID");
            //------

            //-----Банк контрагента
            Database.AddEntityTable(
                "GKH_CONTRAGENT_BANK", 
                new Column("CONTRAGENT_ID", DbType.Int64, 22), 
                new Column("BIK", DbType.String, 50), 
                new Column("CORR_ACCOUNT", DbType.String, 50), 
                new Column("SETTL_ACCOUNT", DbType.String, 50), 
                new Column("DESCRIPTION", DbType.String, 1000), 
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull), 
                new Column("OKONH", DbType.String, 50), 
                new Column("OKPO", DbType.String, 50),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_CONTR_BANK_N", false, "GKH_CONTRAGENT_BANK", "NAME");
            Database.AddIndex("IND_GKH_CONTR_BANK_CTR", false, "GKH_CONTRAGENT_BANK", "CONTRAGENT_ID");
            Database.AddForeignKey("FK_GKH_CONTR_BANK_CTR", "GKH_CONTRAGENT_BANK", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");

            //------

            //-----Контакт контрагента
            Database.AddEntityTable(
                "GKH_CONTRAGENT_CONTACT", 
                new Column("CONTRAGENT_ID", DbType.Int64, 22), 
                new Column("ANNOTATION", DbType.String, 500), 
                new Column("DATE_END_WORK", DbType.DateTime), 
                new Column("DATE_START_WORK", DbType.DateTime), 
                new Column("EMAIL", DbType.String, 50), 
                new Column("NAME", DbType.String, 100, ColumnProperty.NotNull), 
                new Column("SURNAME", DbType.String, 100, ColumnProperty.NotNull), 
                new Column("PATRONYMIC", DbType.String, 100, ColumnProperty.NotNull), 
                new Column("FULL_NAME", DbType.String, 300, ColumnProperty.NotNull), 
                new Column("PHONE", DbType.String, 50), 
                new Column("POSITION_ID", DbType.Int64, 22), 
                new Column("ORDER_DATE", DbType.DateTime), 
                new Column("ORDER_NAME", DbType.String, 100), 
                new Column("ORDER_NUM", DbType.String, 50),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_CONTR_CONTACT_NAME", false, "GKH_CONTRAGENT_CONTACT", "NAME");
            Database.AddIndex("IND_GKH_CONTR_CONTACT_FNAME", false, "GKH_CONTRAGENT_CONTACT", "FULL_NAME");
            Database.AddIndex("IND_GKH_CONTR_CONTACT_CTR", false, "GKH_CONTRAGENT_CONTACT", "CONTRAGENT_ID");
            Database.AddIndex("IND_GKH_CONTR_CONTACT_POS", false, "GKH_CONTRAGENT_CONTACT", "POSITION_ID");
            Database.AddForeignKey("FK_GKH_CONTR_CONTACT_CTR", "GKH_CONTRAGENT_CONTACT", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            Database.AddForeignKey("FK_GKH_CONTR_CONTACT_POS", "GKH_CONTRAGENT_CONTACT", "POSITION_ID", "GKH_DICT_POSITION", "ID");

            //------

            //-----Управляющая организация
            Database.AddEntityTable(
                "GKH_MANAGING_ORGANIZATION", 
                new Column("CONTRAGENT_ID", DbType.Int64, 22, ColumnProperty.NotNull), 
                new Column("DESCRIPTION", DbType.String, 500), 
                new Column("ORG_STATE_ROLE", DbType.Int32, 4, ColumnProperty.NotNull, 10), 
                new Column("COUNT_MO", DbType.Int32), 
                new Column("COUNT_OFFICES", DbType.Int32), 
                new Column("COUNT_SRF", DbType.Int32), 
                new Column("IS_TRANSFER_MANAGE_TSJ", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("MEMBER_RANKING", DbType.Int32, 4, ColumnProperty.NotNull, 10), 
                new Column("NUMBER_EMPLOYEES", DbType.Int32), 
                new Column("OFFICIAL_SITE", DbType.String, 100),
                new Column("SHARE_MO", DbType.Decimal), 
                new Column("SHARE_SF", DbType.Decimal), 
                new Column("TYPE_MANAGEMENT", DbType.Int32, 4, ColumnProperty.NotNull, 10), 
                new Column("ACTIVITY_TERMINATION", DbType.Int32, 4, ColumnProperty.NotNull, 10), 
                new Column("ACTIVITY_DATE_END", DbType.DateTime),
                new Column("ACTIVITY_DESCRIPTION", DbType.String, 500),
                new Column("OFFICIAL_SITE_731", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_MORG_CTR", false, "GKH_MANAGING_ORGANIZATION", "CONTRAGENT_ID");
            Database.AddForeignKey("FK_GKH_MORG_CTR", "GKH_MANAGING_ORGANIZATION", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            //------

            //-----"Режим работы управляющей организации"
            Database.AddEntityTable(
                "GKH_MAN_ORG_WORK",
                new Column("TYPE_MODE", DbType.Int32, 4, ColumnProperty.NotNull),
                new Column("TYPE_DAY", DbType.Int32, 4, ColumnProperty.NotNull),
                new Column("START_DATE", DbType.DateTime),
                new Column("END_DATE", DbType.DateTime),
                new Column("PAUSE", DbType.String, 50),
                new Column("AROUND_CLOCK", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("MAN_ORG_ID", DbType.Int64, 22),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_MORG_WORK_MO", false, "GKH_MAN_ORG_WORK", "MAN_ORG_ID");
            Database.AddForeignKey("FK_GKH_MORG_WORK_MO", "GKH_MAN_ORG_WORK", "MAN_ORG_ID", "GKH_MANAGING_ORGANIZATION", "ID");
            //-----

            //-----Обслуживающая организация (Поставщик комунальных услуг)
            Database.AddEntityTable(
                "GKH_SERVICE_ORGANIZATION",
                new Column("CONTRAGENT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 500), 
                new Column("ORG_STATE_ROLE", DbType.Int32, 4, ColumnProperty.NotNull, 10), 
                new Column("ACTIVITY_TERMINATION", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("DESCRIPTION_TERM", DbType.String, 500),
                new Column("DATE_TERMINATION", DbType.DateTime),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_SERV_ORG_CTR", false, "GKH_SERVICE_ORGANIZATION", "CONTRAGENT_ID");
            Database.AddForeignKey("FK_GKH_SERV_ORG_CTR", "GKH_SERVICE_ORGANIZATION", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            //------

            // базовый договор управления
            Database.AddEntityTable(
                "GKH_MORG_CONTRACT",
                new Column("MANAG_ORG_ID", DbType.Int64, 22),
                new Column("FILE_INFO_ID", DbType.Int64, 22),
                new Column("TYPE_CONTRACT", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("DOCUMENT_NAME", DbType.String, 300),
                new Column("DOCUMENT_NUM", DbType.String, 300),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("START_DATE", DbType.DateTime),
                new Column("END_DATE", DbType.DateTime),
                new Column("PLANNED_END_DATE", DbType.DateTime),
                new Column("NOTE", DbType.String, 300),
                new Column("TERMINATE_REASON", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_MORG_CN_MORG", false, "GKH_MORG_CONTRACT", "MANAG_ORG_ID");
            Database.AddIndex("IND_GKH_MORG_CN_FILE", false, "GKH_MORG_CONTRACT", "FILE_INFO_ID");
            Database.AddForeignKey("FK_GKH_MORG_CN_MORG", "GKH_MORG_CONTRACT", "MANAG_ORG_ID", "GKH_MANAGING_ORGANIZATION", "ID");
            Database.AddForeignKey("FK_GKH_MORG_CN_FILE", "GKH_MORG_CONTRACT", "FILE_INFO_ID", "B4_FILE_INFO", "ID");

            //-----договор управления жск/тсж
            Database.AddTable(
                "GKH_MORG_JSKTSJ_CONTRACT",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("MAN_ORG_TRANSFERRED_MANAG_ID", DbType.Int64, 22),
                new Column("IS_TRANSFER_MANAGEMENT", DbType.Int32, 4, ColumnProperty.NotNull, 30));
            Database.AddIndex("IND_GKH_MORG_JSKTSJ_CN_MO", false, "GKH_MORG_JSKTSJ_CONTRACT", "MAN_ORG_TRANSFERRED_MANAG_ID");
            Database.AddForeignKey("FK_GKH_MORG_JSKTSJ_CN", "GKH_MORG_JSKTSJ_CONTRACT", "ID", "GKH_MORG_CONTRACT", "ID");
            Database.AddForeignKey("FK_GKH_MORG_JSKTSJ_CN_MO", "GKH_MORG_JSKTSJ_CONTRACT", "MAN_ORG_TRANSFERRED_MANAG_ID", "GKH_MANAGING_ORGANIZATION", "ID");
            //------

            //------договор управления ук и жск/тсж
            Database.AddTable(
                "GKH_MORG_CONTRACT_JSKTSJ",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("MAN_ORG_JSK_TSJ_ID", DbType.Int64, 22, ColumnProperty.NotNull));
            Database.AddIndex("IND_GKH_MORG_CN_MJSKTSJ", false, "GKH_MORG_CONTRACT_JSKTSJ", "MAN_ORG_JSK_TSJ_ID");
            Database.AddForeignKey("FK_GKH_MORG_CN_JSKTSJ", "GKH_MORG_CONTRACT_JSKTSJ", "ID", "GKH_MORG_CONTRACT", "ID");
            Database.AddForeignKey("FK_GKH_MORG_CN_MJSKTSJ", "GKH_MORG_CONTRACT_JSKTSJ", "MAN_ORG_JSK_TSJ_ID", "GKH_MANAGING_ORGANIZATION", "ID");
            //------

            //------договор управления ук и собственников
            Database.AddTable(
                "GKH_MORG_CONTRACT_OWNERS",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("CONTRACT_FOUNDATION", DbType.Int32, 4, ColumnProperty.NotNull, 10));
            Database.AddForeignKey("FK_GKH_MORG_CN_OWNERS_ID", "GKH_MORG_CONTRACT_OWNERS", "ID", "GKH_MORG_CONTRACT", "ID");
            //-------

            //-----договор непосредственное управление
            Database.AddTable(
                "GKH_OBJ_DIRECT_MANAG_CNRT",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddForeignKey("FK_GKH_DIRECT_MORG_CON", "GKH_OBJ_DIRECT_MANAG_CNRT", "ID", "GKH_MORG_CONTRACT", "ID");
            //------

            //-----жилой дом договора
            Database.AddEntityTable(
                "GKH_MORG_CONTRACT_REALOBJ",
                new Column("REALITY_OBJ_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("MAN_ORG_CONTRACT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_MORG_CN_RO_CON", false, "GKH_MORG_CONTRACT_REALOBJ", "MAN_ORG_CONTRACT_ID");
            Database.AddIndex("IND_GKH_MORG_CN_RO_RO", false, "GKH_MORG_CONTRACT_REALOBJ", "REALITY_OBJ_ID");
            Database.AddForeignKey("FK_GKH_MORG_CN_RO_CON", "GKH_MORG_CONTRACT_REALOBJ", "MAN_ORG_CONTRACT_ID", "GKH_MORG_CONTRACT", "ID");
            Database.AddForeignKey("FK_GKH_MORG_CN_RO_RO", "GKH_MORG_CONTRACT_REALOBJ", "REALITY_OBJ_ID", "GKH_REALITY_OBJECT", "ID");
            // -----

            //-----Обслуживающая организация жилого дома
            Database.AddEntityTable(
                "GKH_OBJ_SERVICE_ORG",
                new Column("REALITY_OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull), 
                new Column("FILE_ID", DbType.Int64, 22), 
                new Column("DESCRIPTION", DbType.String, 500), 
                new Column("DOCUMENT_DATE", DbType.DateTime), 
                new Column("DOCUMENT_NAME", DbType.String, 100), 
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("EXTERNAL_ID", DbType.String, 36),
                new Column("CONTRAGENT_ID", DbType.Int64, 22),
                new Column("TYPE_SERV_ORG", DbType.Int32, 4, ColumnProperty.NotNull, 10));
            Database.AddIndex("IND_GKH_OBJ_SERV_ORG_RO", false, "GKH_OBJ_SERVICE_ORG", "REALITY_OBJECT_ID");
            Database.AddIndex("IND_GKH_OBJ_SERVICE_ORG_DOC", false, "GKH_OBJ_SERVICE_ORG", "FILE_ID");
            Database.AddIndex("IND_GKH_OBJ_SERVORG_CTRN", false, "GKH_OBJ_SERVICE_ORG", "CONTRAGENT_ID");
            Database.AddForeignKey("FK_GKH_OBJ_SERVORG_CTRN", "GKH_OBJ_SERVICE_ORG", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            Database.AddForeignKey("FK_GKH_OBJ_SERV_ORG_RO", "GKH_OBJ_SERVICE_ORG", "REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID");
            Database.AddForeignKey("FK_GKH_OBJ_SERV_ORG_FILE", "GKH_OBJ_SERVICE_ORG", "FILE_ID", "B4_FILE_INFO", "ID");
            //------

            //-----Жилой дом управляющей организации
            Database.AddEntityTable(
                "GKH_MAN_ORG_REAL_OBJ",
                new Column("MANAG_ORG_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("REALITY_OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("DATE_START", DbType.DateTime),
                new Column("DATE_END", DbType.DateTime),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_MORG_RO_MORG", false, "GKH_MAN_ORG_REAL_OBJ", "MANAG_ORG_ID");
            Database.AddIndex("IND_GKH_MORG_RO_RO", false, "GKH_MAN_ORG_REAL_OBJ", "REALITY_OBJECT_ID");
            Database.AddForeignKey("FK_GKH_MORG_RO_MORG", "GKH_MAN_ORG_REAL_OBJ", "MANAG_ORG_ID", "GKH_MANAGING_ORGANIZATION", "ID");
            Database.AddForeignKey("FK_GKH_MORG_RO_RO", "GKH_MAN_ORG_REAL_OBJ", "REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID");
            //-----

            //-----Претензия к управляющей организации
            Database.AddEntityTable(
                "GKH_MAN_ORG_CLAIM",
                new Column("MAN_ORG_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("DATE_CLAIM", DbType.DateTime), 
                new Column("AMOUNT", DbType.Decimal), 
                new Column("CONTENT", DbType.String, 500),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_MORG_CL_MAO", false, "GKH_MAN_ORG_CLAIM", "MAN_ORG_ID");
            Database.AddForeignKey("FK_GKH_MORG_CL_MAO", "GKH_MAN_ORG_CLAIM", "MAN_ORG_ID", "GKH_MANAGING_ORGANIZATION", "ID");

            //-------

            //-----Документы управляющей организации
            Database.AddEntityTable(
                "GKH_MAN_ORG_DOC",
                new Column("MAN_ORG_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_ID", DbType.Int64, 22), 
                new Column("DESCRIPTION", DbType.String, 500), 
                new Column("DOCUMENT_DATE", DbType.DateTime), 
                new Column("DOCUMENT_NAME", DbType.String, 100),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_MORG_DOC_MAO", false, "GKH_MAN_ORG_DOC", "MAN_ORG_ID");
            Database.AddIndex("IND_GKH_MORG_DOC_FILE", false, "GKH_MAN_ORG_DOC", "FILE_ID");
            Database.AddForeignKey("FK_GKH_MORG_DOC_MAO", "GKH_MAN_ORG_DOC", "MAN_ORG_ID", "GKH_MANAGING_ORGANIZATION", "ID");
            Database.AddForeignKey("FK_GKH_MORG_DOC_FILE", "GKH_MAN_ORG_DOC", "FILE_ID", "B4_FILE_INFO", "ID");
            //-------

            //-----Членство управляющей организации
            Database.AddEntityTable(
                "GKH_MAN_ORG_MEMBERSHIP",
                new Column("MAN_ORG_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("ADDRESS", DbType.String, 1000), 
                new Column("DATE_START", DbType.DateTime), 
                new Column("DATE_END", DbType.DateTime), 
                new Column("NAME", DbType.String, 100),
                new Column("OFFICIAL_SITE", DbType.String, 100),
                new Column("DOCUMENT_NUM", DbType.String, 100),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_MORG_MEM_NAME", false, "GKH_MAN_ORG_MEMBERSHIP", "NAME");
            Database.AddIndex("IND_GKH_MORG_MEM_MAO", false, "GKH_MAN_ORG_MEMBERSHIP", "MAN_ORG_ID");
            Database.AddForeignKey("FK_GKH_MORG_MEM_MAO", "GKH_MAN_ORG_MEMBERSHIP", "MAN_ORG_ID", "GKH_MANAGING_ORGANIZATION", "ID");
            //-------

            //-----Услуга управляющей организации
            Database.AddEntityTable(
                "GKH_MAN_ORG_SERVICE",
                new Column("MAN_ORG_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("NAME", DbType.String, 100, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_MORG_SERV_NAME", false, "GKH_MAN_ORG_SERVICE", "NAME");
            Database.AddIndex("IND_GKH_MORG_SERV_MAO", false, "GKH_MAN_ORG_SERVICE", "MAN_ORG_ID");
            Database.AddForeignKey("FK_GKH_MORG_SERV_MAO", "GKH_MAN_ORG_SERVICE", "MAN_ORG_ID", "GKH_MANAGING_ORGANIZATION", "ID");
            //-------

            // Страхование деятельности управляющей организации
            Database.AddEntityTable(
                "GKH_BELAY_MORG_ACTIVITY",
                new Column("MANORG_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_BEL_MORGAC_ORG", false, "GKH_BELAY_MORG_ACTIVITY", "MANORG_ID");
            Database.AddForeignKey("FK_GKH_BEL_MORGAC_ORG", "GKH_BELAY_MORG_ACTIVITY", "MANORG_ID", "GKH_MANAGING_ORGANIZATION", "ID");
            // -----

            //-----Документы обслуживающей организации
            Database.AddEntityTable(
                "GKH_SERV_ORG_DOC",
                new Column("SERV_ORG_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_ID", DbType.Int64, 22), 
                new Column("DESCRIPTION", DbType.String, 500), 
                new Column("DOCUMENT_DATE", DbType.DateTime), 
                new Column("DOCUMENT_NAME", DbType.String, 100),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_SERV_ORG_DOC_SEO", false, "GKH_SERV_ORG_DOC", "SERV_ORG_ID");
            Database.AddIndex("IND_GKH_SERV_ORG_DOC_FILE", false, "GKH_SERV_ORG_DOC", "FILE_ID");
            Database.AddForeignKey("FK_GKH_SERV_ORG_DOC_SEO", "GKH_SERV_ORG_DOC", "SERV_ORG_ID", "GKH_SERVICE_ORGANIZATION", "ID");
            Database.AddForeignKey("FK_GKH_SERV_ORG_DOC_FILE", "GKH_SERV_ORG_DOC", "FILE_ID", "B4_FILE_INFO", "ID");
            //-------

            //-----Услуга обслуживающей организации
            Database.AddEntityTable(
                "GKH_SERV_ORG_SERVICE",
                new Column("SERV_ORG_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("TYPE_SERVICE_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_SERV_ORG_SERV_SEO", false, "GKH_SERV_ORG_SERVICE", "SERV_ORG_ID");
            Database.AddIndex("IND_GKH_SERV_ORG_SERV_TYS", false, "GKH_SERV_ORG_SERVICE", "TYPE_SERVICE_ID");
            Database.AddForeignKey("FK_GKH_SERV_ORG_SERV_TYS", "GKH_SERV_ORG_SERVICE", "TYPE_SERVICE_ID", "GKH_DICT_TYPE_SERVICE", "ID");
            Database.AddForeignKey("FK_GKH_SERV_ORG_SERV_SEO", "GKH_SERV_ORG_SERVICE", "SERV_ORG_ID", "GKH_SERVICE_ORGANIZATION", "ID");
            //-------

            //-----Органы местного самоуправления
            Database.AddEntityTable(
                "GKH_LOCAL_GOVERNMENT",
                new Column("CONTRAGENT_ID", DbType.Int64, 22, ColumnProperty.NotNull), 
                new Column("DESCRIPTION", DbType.String, 500), 
                new Column("ORG_STATE_ROLE", DbType.Int32, 4, ColumnProperty.NotNull, 10), 
                new Column("EMAIL", DbType.String, 50), 
                new Column("NAME_DEP_GKH", DbType.String, 300), 
                new Column("OFFICIAL_SITE", DbType.String, 50), 
                new Column("PHONE", DbType.String, 50),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_LOCALG_CTR", false, "GKH_LOCAL_GOVERNMENT", "CONTRAGENT_ID");
            Database.AddForeignKey("FK_GKH_LOCALG_CTR", "GKH_LOCAL_GOVERNMENT", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            //------

            //-----МО органа местного самоуправления
            Database.AddEntityTable(
                "GKH_LOCAL_GOV_MUNICIP",
                new Column("LOCAL_GOV_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("MUNICIPALITY_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_LOCALG_MUNIC_LG", false, "GKH_LOCAL_GOV_MUNICIP", "LOCAL_GOV_ID");
            Database.AddIndex("IND_GKH_LOCALG_MUNIC_MCP", false, "GKH_LOCAL_GOV_MUNICIP", "MUNICIPALITY_ID");
            Database.AddForeignKey("FK_GKH_LOCALG_MUNIC_MCP", "GKH_LOCAL_GOV_MUNICIP", "MUNICIPALITY_ID", "GKH_DICT_MUNICIPALITY", "ID");
            Database.AddForeignKey("FK_GKH_LOCALG_MUNIC_LG", "GKH_LOCAL_GOV_MUNICIP", "LOCAL_GOV_ID", "GKH_LOCAL_GOVERNMENT", "ID");
            //-----

            //-----"Режим работы органа местного самоуправления"
            Database.AddEntityTable(
                "GKH_LOCGOV_WORK",
                new Column("TYPE_MODE", DbType.Int32, 4, ColumnProperty.NotNull),
                new Column("TYPE_DAY", DbType.Int32, 4, ColumnProperty.NotNull),
                new Column("START_DATE", DbType.DateTime),
                new Column("END_DATE", DbType.DateTime),
                new Column("PAUSE", DbType.String, 50),
                new Column("AROUND_CLOCK", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("LOCAL_GOVERNMENT_ID", DbType.Int64, 22),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_LOCGOV_WORK_LG", false, "GKH_LOCGOV_WORK", "LOCAL_GOVERNMENT_ID");
            Database.AddForeignKey("FK_GKH_LOCGOV_WORK_LG", "GKH_LOCGOV_WORK", "LOCAL_GOVERNMENT_ID", "GKH_LOCAL_GOVERNMENT", "ID");
            //-----

            //-----Страховые организации
            Database.AddEntityTable(
                "GKH_BELAY_ORGANIZATION",
                new Column("CONTRAGENT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 500), 
                new Column("ORG_STATE_ROLE", DbType.Int32, 4, ColumnProperty.NotNull, 10), 
                new Column("ACTIVITY_TERMINATION", DbType.Int32, 4, ColumnProperty.NotNull, 10), 
                new Column("ACTIVITY_DESCRIPTION", DbType.String, 500),
                new Column("DATE_TERMINATION", DbType.DateTime),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_BEL_ORG_CTR", false, "GKH_BELAY_ORGANIZATION", "CONTRAGENT_ID");
            Database.AddForeignKey("FK_GKH_BEL_ORG_CTR", "GKH_BELAY_ORGANIZATION", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            //------

            //------Страховой полис
            Database.AddEntityTable(
                "GKH_BELAY_MANORG_POLICY",
                new Column("BELAY_MANORG_ACTIVITY_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("BELAY_ORGANIZATION_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("BELAY_ORG_KIND_ACTIV_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("DOCUMENT_NUM", DbType.String, 300),
                new Column("DOCUMENT_START_DATE", DbType.DateTime),
                new Column("DOCUMENT_END_DATE", DbType.DateTime),
                new Column("LIMIT_MANORG_HOME", DbType.Decimal),
                new Column("LIMIT_MANORG_INSURED", DbType.Decimal),
                new Column("LIMIT_CIVIL", DbType.Decimal),
                new Column("LIMIT_CIVIL_INSURED", DbType.Decimal),
                new Column("LIMIT_CIVIL_ONE", DbType.Decimal),
                new Column("POLICY_ACTION", DbType.Int32, 4, ColumnProperty.NotNull),
                new Column("BELAY_SUM", DbType.Decimal),
                new Column("CAUSE", DbType.String, 1000),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_BEL_MORGP_ACT", false, "GKH_BELAY_MANORG_POLICY", "BELAY_MANORG_ACTIVITY_ID");
            Database.AddIndex("IND_GKH_BEL_MORGP_BO", false, "GKH_BELAY_MANORG_POLICY", "BELAY_ORGANIZATION_ID");
            Database.AddIndex("IND_GKH_BEL_MORGP_KA", false, "GKH_BELAY_MANORG_POLICY", "BELAY_ORG_KIND_ACTIV_ID");
            Database.AddForeignKey("FK_GKH_BEL_MORGP_ACT", "GKH_BELAY_MANORG_POLICY", "BELAY_MANORG_ACTIVITY_ID", "GKH_BELAY_MORG_ACTIVITY", "ID");
            Database.AddForeignKey("FK_GKH_BEL_MORGP_BO", "GKH_BELAY_MANORG_POLICY", "BELAY_ORGANIZATION_ID", "GKH_BELAY_ORGANIZATION", "ID");
            Database.AddForeignKey("FK_GKH_BEL_MORGP_KA", "GKH_BELAY_MANORG_POLICY", "BELAY_ORG_KIND_ACTIV_ID", "GKH_DICT_BELAY_KIND_ACTIV", "ID");
            //------

            //-----Застрахованный риск
            Database.AddEntityTable(
                "GKH_BELAY_POLICY_RISK",
                new Column("BELAY_POLICY_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("KIND_RISK_ID", DbType.Int64, 22),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_BEL_POL_RISK_POL", false, "GKH_BELAY_POLICY_RISK", "BELAY_POLICY_ID");
            Database.AddIndex("IND_GKH_BEL_POL_RISK_KR", false, "GKH_BELAY_POLICY_RISK", "KIND_RISK_ID");
            Database.AddForeignKey("FK_GKH_BEL_POL_RISK_POL", "GKH_BELAY_POLICY_RISK", "BELAY_POLICY_ID", "GKH_BELAY_MANORG_POLICY", "ID");
            Database.AddForeignKey("FK_GKH_BEL_POL_RISK_KR", "GKH_BELAY_POLICY_RISK", "KIND_RISK_ID", "GKH_DICT_KIND_RISK", "ID");
            //-----

            //------Страховой случай
            Database.AddEntityTable(
                "GKH_BELAY_POLICY_EVENT",
                new Column("BELAY_POLICY_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_INFO_ID", DbType.Int64, 22),
                new Column("EVENT_DATE", DbType.DateTime),
                new Column("DESCRIPTION", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_BEL_POL_EV_POL", false, "GKH_BELAY_POLICY_EVENT", "BELAY_POLICY_ID");
            Database.AddIndex("IND_GKH_BEL_POL_EV_FIN", false, "GKH_BELAY_POLICY_EVENT", "FILE_INFO_ID");
            Database.AddForeignKey("FK_GKH_BEL_POL_EV_POL", "GKH_BELAY_POLICY_EVENT", "BELAY_POLICY_ID", "GKH_BELAY_MANORG_POLICY", "ID");
            Database.AddForeignKey("FK_GKH_BEL_POL_EV_FIN", "GKH_BELAY_POLICY_EVENT", "FILE_INFO_ID", "B4_FILE_INFO", "ID");
            //------

            //------Оплата договора
            Database.AddEntityTable(
                "GKH_BELAY_POLICY_PAYMENT",
                new Column("BELAY_POLICY_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_INFO_ID", DbType.Int64, 22),
                new Column("NAME", DbType.String, 300),
                new Column("PAYMENT_DATE", DbType.DateTime),
                new Column("DOCUMENT_NUM", DbType.String, 300),
                new Column("SUM", DbType.Decimal),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_BEL_POL_PAYM_NAME", false, "GKH_BELAY_POLICY_PAYMENT", "NAME");
            Database.AddIndex("IND_GKH_BEL_POL_PAYM_POL", false, "GKH_BELAY_POLICY_PAYMENT", "BELAY_POLICY_ID");
            Database.AddIndex("IND_GKH_BEL_POL_PAYM_FIN", false, "GKH_BELAY_POLICY_PAYMENT", "FILE_INFO_ID");
            Database.AddForeignKey("FK_GKH_BEL_POL_PAYM_POL", "GKH_BELAY_POLICY_PAYMENT", "BELAY_POLICY_ID", "GKH_BELAY_MANORG_POLICY", "ID");
            Database.AddForeignKey("FK_GKH_BEL_POL_PAYM_FIN", "GKH_BELAY_POLICY_PAYMENT", "FILE_INFO_ID", "B4_FILE_INFO", "ID");
            // -----

            // Страховой полис МКД
            Database.AddEntityTable(
                "GKH_BELAY_POLICY_MKD",
                new Column("REALITY_OBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("BELAY_POLICY_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("IS_EXCLUDED", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_BEL_POL_MKD_RO", false, "GKH_BELAY_POLICY_MKD", "REALITY_OBJECT_ID");
            Database.AddIndex("IND_GKH_BEL_POL_MKD_POL", false, "GKH_BELAY_POLICY_MKD", "BELAY_POLICY_ID");
            Database.AddForeignKey("FK_GKH_BEL_POL_MKD_RO", "GKH_BELAY_POLICY_MKD", "REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID");
            Database.AddForeignKey("FK_GKH_BEL_POL_MKD_POL", "GKH_BELAY_POLICY_MKD", "BELAY_POLICY_ID", "GKH_BELAY_MANORG_POLICY", "ID");
            // -----

            //-----Подрядчик
            Database.AddEntityTable(
                "GKH_BUILDER",
                new Column("CONTRAGENT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 500), 
                new Column("ORG_STATE_ROLE", DbType.Int32, 4, ColumnProperty.NotNull, 10), 
                new Column("ADVANCED_TECHNOLOGIES", DbType.Int32, 4, ColumnProperty.NotNull, 30), 
                new Column("CONSENT_INFO", DbType.Int32, 4, ColumnProperty.NotNull, 30), 
                new Column("WORK_WITHOUT_CONTR", DbType.Int32, 4, ColumnProperty.NotNull, 30), 
                new Column("RATING", DbType.Int32), 
                new Column("TAX_INFO_ADDRESS", DbType.String, 1000), 
                new Column("TAX_INFO_PHONE", DbType.String, 50), 
                new Column("FILE_ID", DbType.Int64, 22), 
                new Column("ACTIVITY_TERMINATION", DbType.Int32, 4, ColumnProperty.NotNull, 10), 
                new Column("ACTIVITY_DATE_END", DbType.DateTime), 
                new Column("ACTIVITY_DATE_START", DbType.DateTime),
                new Column("ACTIVITY_DESCRIPTION", DbType.String, 500),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_BUIL_FILE", false, "GKH_BUILDER", "FILE_ID");
            Database.AddIndex("IND_GKH_BUIL_CTR", false, "GKH_BUILDER", "CONTRAGENT_ID");
            Database.AddForeignKey("FK_GKH_BUIL_CTR", "GKH_BUILDER", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            Database.AddForeignKey("FK_GKH_BUIL_FILE", "GKH_BUILDER", "FILE_ID", "B4_FILE_INFO", "ID");
            //------

            //-----Займы подрядчиков 
            Database.AddEntityTable(
                "GKH_BUILDER_LOAN",
                new Column("BUILDER_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("LENDER_ID", DbType.Int64, 22), 
                new Column("DATE_ISSUE", DbType.DateTime), 
                new Column("DATE_PLAN_RETURN", DbType.DateTime), 
                new Column("AMOUNT", DbType.Decimal), 
                new Column("DESCRIPTION", DbType.String, 500), 
                new Column("DOCUMENT_DATE", DbType.DateTime), 
                new Column("DOCUMENT_NAME", DbType.String, 100),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_BUIL_LOAN_CTR", false, "GKH_BUILDER_LOAN", "LENDER_ID");
            Database.AddIndex("IND_GKH_BUIL_LOAN_BLD", false, "GKH_BUILDER_LOAN", "BUILDER_ID");
            Database.AddForeignKey("FK_GKH_BUIL_LOAN_BLD", "GKH_BUILDER_LOAN", "BUILDER_ID", "GKH_BUILDER", "ID");
            Database.AddForeignKey("FK_GKH_BUIL_LOAN_CTR", "GKH_BUILDER_LOAN", "LENDER_ID", "GKH_CONTRAGENT", "ID");
            //------

            //-----График погашения займов подрядчиков
            Database.AddEntityTable(
                "GKH_BUILDER_LOAN_REPAY",
                new Column("BUILDER_LOAN_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("DATE_REPAYMENT", DbType.DateTime), 
                new Column("AMOUNT", DbType.Decimal), 
                new Column("NAME", DbType.String, 300),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_BUIL_LOAN_REP_NAME", false, "GKH_BUILDER_LOAN_REPAY", "NAME");
            Database.AddIndex("IND_GKH_BUIL_LOAN_REP_BL", false, "GKH_BUILDER_LOAN_REPAY", "BUILDER_LOAN_ID");
            Database.AddForeignKey("FK_GKH_BUIL_LOAN_REP_BL", "GKH_BUILDER_LOAN_REPAY", "BUILDER_LOAN_ID", "GKH_BUILDER_LOAN", "ID");
            //------

            //-----Документы подрядчиков
            Database.AddEntityTable(
                "GKH_BUILDER_DOCUMENT",
                new Column("BUILDER_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("CONTRAGENT_ID", DbType.Int64, 22), 
                new Column("FILE_ID", DbType.Int64, 22), 
                new Column("PERIOD_ID", DbType.Int64, 22), 
                new Column("DESCRIPTION", DbType.String, 500), 
                new Column("DOCUMENT_DATE", DbType.DateTime), 
                new Column("DOCUMENT_NAME", DbType.String, 100), 
                new Column("DOCUMENT_NUM", DbType.String, 50), 
                new Column("DOCUMENT_EXIST", DbType.Int32, 4, ColumnProperty.NotNull, 30),
                new Column("DOCUMENT_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_BUIL_DOC_BLD", false, "GKH_BUILDER_DOCUMENT", "BUILDER_ID");
            Database.AddIndex("IND_GKH_BUIL_DOC_FILE", false, "GKH_BUILDER_DOCUMENT", "FILE_ID");
            Database.AddIndex("IND_GKH_BUIL_DOC_CTR", false, "GKH_BUILDER_DOCUMENT", "CONTRAGENT_ID");
            Database.AddIndex("IND_GKH_BUIL_DOC_PRD", false, "GKH_BUILDER_DOCUMENT", "PERIOD_ID");
            Database.AddForeignKey("FK_GKH_BUIL_DOC_PRD", "GKH_BUILDER_DOCUMENT", "PERIOD_ID", "GKH_DICT_PERIOD", "ID");
            Database.AddForeignKey("FK_GKH_BUIL_DOC_CTR", "GKH_BUILDER_DOCUMENT", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            Database.AddForeignKey("FK_GKH_BUIL_DOC_FILE", "GKH_BUILDER_DOCUMENT", "FILE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_GKH_BUIL_DOC_BLD", "GKH_BUILDER_DOCUMENT", "BUILDER_ID", "GKH_BUILDER", "ID");
            //------

            //-----Состав трудовых ресурсов подрядчиков
            Database.AddEntityTable(
                "GKH_BUILDER_WORKFORCE",
                new Column("BUILDER_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("SPECIALTY_ID", DbType.Int64, 22), 
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("DOCUMENT_DATE", DbType.DateTime), 
                new Column("DOCUMENT_QUALIFICATION", DbType.String, 300), 
                new Column("EMPLOYMENT_DATE", DbType.DateTime), 
                new Column("FIO", DbType.String, 300),
                new Column("POSITION", DbType.String, 100),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("INSTITUTIONS_ID", DbType.Int64, 22),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_BUIL_WORKF_BLD", false, "GKH_BUILDER_WORKFORCE", "BUILDER_ID");
            Database.AddIndex("IND_GKH_BUIL_WORKF_FILE", false, "GKH_BUILDER_WORKFORCE", "FILE_ID");
            Database.AddIndex("IND_GKH_BUIL_WORKF_SPEC", false, "GKH_BUILDER_WORKFORCE", "SPECIALTY_ID");
            Database.AddIndex("IND_GKH_BUIL_WORKF_INS", false, "GKH_BUILDER_WORKFORCE", "INSTITUTIONS_ID");
            Database.AddForeignKey("FK_GKH_BUIL_WORKF_SPEC", "GKH_BUILDER_WORKFORCE", "SPECIALTY_ID", "GKH_DICT_SPECIALTY", "ID");
            Database.AddForeignKey("FK_GKH_BUIL_WORKF_FILE", "GKH_BUILDER_WORKFORCE", "FILE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_GKH_BUIL_WORKF_BLD", "GKH_BUILDER_WORKFORCE", "BUILDER_ID", "GKH_BUILDER", "ID");
            Database.AddForeignKey("FK_GKH_BUIL_WORKF_INS", "GKH_BUILDER_WORKFORCE", "INSTITUTIONS_ID", "GKH_DICT_INSTITUTIONS", "ID");
            //------

            //----Производственная база подрядчиков
            Database.AddEntityTable(
                "GKH_BUILDER_PRODUCTBASE",
                new Column("BUILDER_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_ID", DbType.Int64, 22), 
                new Column("KIND_EQUIPMENT_ID", DbType.Int64, 22), 
                new Column("NOTATION", DbType.String, 300),
                new Column("VOLUME", DbType.Int32),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_BUIL_PROD_BLD", false, "GKH_BUILDER_PRODUCTBASE", "BUILDER_ID");
            Database.AddIndex("IND_GKH_BUIL_PROD_FILE", false, "GKH_BUILDER_PRODUCTBASE", "FILE_ID");
            Database.AddIndex("IND_GKH_BUIL_PROD_EQU", false, "GKH_BUILDER_PRODUCTBASE", "KIND_EQUIPMENT_ID");
            Database.AddForeignKey("FK_GKH_BUIL_PROD_EQU", "GKH_BUILDER_PRODUCTBASE", "KIND_EQUIPMENT_ID", "GKH_DICT_KINDEQUIPMENT", "ID");
            Database.AddForeignKey("FK_GKH_BUIL_PROD_FILE", "GKH_BUILDER_PRODUCTBASE", "FILE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_GKH_BUIL_PROD_BLD", "GKH_BUILDER_PRODUCTBASE", "BUILDER_ID", "GKH_BUILDER", "ID");
            //------

            //----Техника подрядчиков
            Database.AddEntityTable(
                "GKH_BUILDER_TECHNIQUE",
                new Column("BUILDER_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("NAME", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_BUIL_TECH_NAME", false, "GKH_BUILDER_TECHNIQUE", "NAME");
            Database.AddIndex("IND_GKH_BUIL_TECH_BLD", false, "GKH_BUILDER_TECHNIQUE", "BUILDER_ID");
            Database.AddIndex("IND_GKH_BUIL_TECH_FILE", false, "GKH_BUILDER_TECHNIQUE", "FILE_ID");
            Database.AddForeignKey("FK_GKH_BUIL_TECH_FILE", "GKH_BUILDER_TECHNIQUE", "FILE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_GKH_BUIL_TECH_BLD", "GKH_BUILDER_TECHNIQUE", "BUILDER_ID", "GKH_BUILDER", "ID");
            //------

            //----Сведения об участкии в СРО подрядчиков
            Database.AddEntityTable(
                "GKH_BUILDER_SROINFO",
                new Column("BUILDER_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("WORK_ID", DbType.Int64, 22),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_BUIL_SRO_BLD", false, "GKH_BUILDER_SROINFO", "BUILDER_ID");
            Database.AddIndex("IND_GKH_BUIL_SRO_FILE", false, "GKH_BUILDER_SROINFO", "FILE_ID");
            Database.AddIndex("IND_GKH_BUIL_SRO_WORK", false, "GKH_BUILDER_SROINFO", "WORK_ID");
            Database.AddForeignKey("FK_GKH_BUIL_SRO_WORK", "GKH_BUILDER_SROINFO", "WORK_ID", "GKH_DICT_WORK", "ID");
            Database.AddForeignKey("FK_GKH_BUIL_SRO_FILE", "GKH_BUILDER_SROINFO", "FILE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_GKH_BUIL_SRO_BLD", "GKH_BUILDER_SROINFO", "BUILDER_ID", "GKH_BUILDER", "ID");
            //------

            //----Отзывы заказчиков о подрядчиках
            Database.AddEntityTable(
                "GKH_BUILDER_FEEDBACK",
                new Column("BUILDER_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_ID", DbType.Int64, 22), 
                new Column("ASSESSMENT", DbType.Int32, 4, ColumnProperty.NotNull, 10), 
                new Column("AUTHOR", DbType.Int32, 4, ColumnProperty.NotNull, 10), 
                new Column("CONTENT", DbType.String, 500), 
                new Column("DOCUMENT_NAME", DbType.String, 100), 
                new Column("FEEDBACK_DATE", DbType.DateTime),
                new Column("ORGANIZATION_NAME", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_BUIL_FEEDB_BLD", false, "GKH_BUILDER_FEEDBACK", "BUILDER_ID");
            Database.AddIndex("IND_GKH_BUIL_FEEDB_FILE", false, "GKH_BUILDER_FEEDBACK", "FILE_ID");
            Database.AddForeignKey("FK_GKH_BUIL_FEEDB_FILE", "GKH_BUILDER_FEEDBACK", "FILE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_GKH_BUIL_FEEDB_BLD", "GKH_BUILDER_FEEDBACK", "BUILDER_ID", "GKH_BUILDER", "ID");
            //------

            //-----Поставщик коммунальных услуг
            Database.AddEntityTable(
                "GKH_SUPPLY_RESORG",
                new Column("CONTRAGENT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("ORG_STATE_ROLE", DbType.Int32, 4, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("ACTIVITY_TERMINATION", DbType.Int32, 4, ColumnProperty.NotNull),
                new Column("DESCRIPTION_TERM", DbType.String, 500),
                new Column("DATE_TERMINATION", DbType.DateTime),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_SUPPLY_CNTR", false, "GKH_SUPPLY_RESORG", "CONTRAGENT_ID");
            Database.AddForeignKey("FK_GKH_SUPPLY_CNTR", "GKH_SUPPLY_RESORG", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            //-----

            //-----Документы Поставщика коммунальных услуг
            Database.AddEntityTable(
                "GKH_SUPPLY_RESORG_DOC",
                new Column("SUPPLY_RESORG_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("DOCUMENT_NAME", DbType.String, 100),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_SUPPLY_DOC_RESORG", false, "GKH_SUPPLY_RESORG_DOC", "SUPPLY_RESORG_ID");
            Database.AddIndex("IND_GKH_SUPPLY_DOC_FILE", false, "GKH_SUPPLY_RESORG_DOC", "FILE_ID");
            Database.AddForeignKey("FK_GKH_SUPPLY_DOC_FILE", "GKH_SUPPLY_RESORG_DOC", "FILE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_GKH_SUPPLY_DOC_RESORG", "GKH_SUPPLY_RESORG_DOC", "SUPPLY_RESORG_ID", "GKH_SUPPLY_RESORG", "ID");
            //-----

            //-----Услуга Поставщика коммунальных услуг
            Database.AddEntityTable(
                "GKH_SUPPLY_RESORG_SERV",
                new Column("SUPPLY_RESORG_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("TYPE_SERVICE_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_SUPPLY_SERV_RESORG", false, "GKH_SUPPLY_RESORG_SERV", "SUPPLY_RESORG_ID");
            Database.AddIndex("IND_GKH_SUPPLY_SERV_TYPE", false, "GKH_SUPPLY_RESORG_SERV", "TYPE_SERVICE_ID");
            Database.AddForeignKey("FK_GKH_SUPPLY_SERV_TYPE", "GKH_SUPPLY_RESORG_SERV", "TYPE_SERVICE_ID", "GKH_DICT_TYPE_SERVICE", "ID");
            Database.AddForeignKey("FK_GKH_SUPPLY_SERV_RESORG", "GKH_SUPPLY_RESORG_SERV", "SUPPLY_RESORG_ID", "GKH_SUPPLY_RESORG", "ID");
            //-----

            //-----Оператор проекта БарсЖКХ
            Database.AddEntityTable(
                "GKH_OPERATOR",
                new Column("USER_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("TYPE_WORKPLACE", DbType.Int32, 4, ColumnProperty.NotNull),
                new Column("PHONE", DbType.String, 50),
                new Column("IS_ACTIVE", DbType.Boolean, ColumnProperty.NotNull, true),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_OPERATOR_USER", false, "GKH_OPERATOR", "USER_ID");
            Database.AddForeignKey("FK_GKH_OPERATOR_USER", "GKH_OPERATOR", "USER_ID", "B4_USER", "ID");
            //------

            //----Таблица связи Оператора с инспекторами
            Database.AddEntityTable(
               "GKH_OPERATOR_INSPECT",
               new Column("INSPECTOR_ID", DbType.Int64, 22, ColumnProperty.NotNull),
               new Column("OPERATOR_ID", DbType.Int64, 22, ColumnProperty.NotNull),
               new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_OPERATOR_INSP_OPER", false, "GKH_OPERATOR_INSPECT", "OPERATOR_ID");
            Database.AddIndex("IND_GKH_OPERATOR_INSP_INSP", false, "GKH_OPERATOR_INSPECT", "INSPECTOR_ID");
            Database.AddForeignKey("FK_GKH_OPERATOR_INSP_INSP", "GKH_OPERATOR_INSPECT", "INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID");
            Database.AddForeignKey("FK_GKH_OPERATOR_INSP_OPER", "GKH_OPERATOR_INSPECT", "OPERATOR_ID", "GKH_OPERATOR", "ID");
            //-----

            //-----Таблица связи Оператора с муниципальным образованием
            Database.AddEntityTable(
               "GKH_OPERATOR_MUNIC",
               new Column("MUNICIPALITY_ID", DbType.Int64, 22, ColumnProperty.NotNull),
               new Column("OPERATOR_ID", DbType.Int64, 22, ColumnProperty.NotNull),
               new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_OPERATOR_MU_MU", false, "GKH_OPERATOR_MUNIC", "MUNICIPALITY_ID");
            Database.AddIndex("IND_GKH_OPERATOR_MU_OPER", false, "GKH_OPERATOR_MUNIC", "OPERATOR_ID");
            Database.AddForeignKey("FK_GKH_OPERATOR_MU_OPER", "GKH_OPERATOR_MUNIC", "OPERATOR_ID", "GKH_OPERATOR", "ID");
            Database.AddForeignKey("FK_GKH_OPERATOR_MU_MU", "GKH_OPERATOR_MUNIC", "MUNICIPALITY_ID", "GKH_DICT_MUNICIPALITY", "ID");
            //-----

            //-----Таблица связи Оператора с контрагентом
            Database.AddEntityTable(
               "GKH_OPERATOR_CONTRAGENT",
               new Column("CONTRAGENT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
               new Column("OPERATOR_ID", DbType.Int64, 22, ColumnProperty.NotNull),
               new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GKH_OPERATOR_CTRN_OPER", false, "GKH_OPERATOR_CONTRAGENT", "OPERATOR_ID");
            Database.AddIndex("IND_GKH_OPERATOR_CTRN_CTRN", false, "GKH_OPERATOR_CONTRAGENT", "CONTRAGENT_ID");
            Database.AddForeignKey("FK_GKH_OPERATOR_CTRN_CTRN", "GKH_OPERATOR_CONTRAGENT", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            Database.AddForeignKey("FK_GKH_OPERATOR_CTRN_OPER", "GKH_OPERATOR_CONTRAGENT", "OPERATOR_ID", "GKH_OPERATOR", "ID");
            //------

            //-----Логи импортов
            Database.AddEntityTable(
                "GKH_LOG_IMPORT",
                new Column("OPERATOR_ID", DbType.Int64, 22),
                new Column("FILE_ID", DbType.Int64, 22),
                new Column("LOG_FILE_ID", DbType.Int64, 22),
                new Column("UPLOAD_DATE", DbType.DateTime, ColumnProperty.NotNull), 
                new Column("FILE_NAME", DbType.String, 256, ColumnProperty.NotNull), 
                new Column("IMPORT_KEY", DbType.String, 256, ColumnProperty.NotNull),
                new Column("COUNT_WARNING", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("COUNT_ERROR", DbType.Int64, 22, ColumnProperty.NotNull), 
                new Column("COUNT_IMPORTED_ROWS", DbType.Int64, 22, ColumnProperty.NotNull), 
                new Column("COUNT_CHANGED_ROWS", DbType.Int64, 22, ColumnProperty.NotNull), 
                new Column("COUNT_IMPORTED_FILE", DbType.Int64, 22, ColumnProperty.NotNull));
            Database.AddIndex("IND_GKH_LOGIMPORT_OP", false, "GKH_LOG_IMPORT", "OPERATOR_ID");
            Database.AddIndex("IND_GKH_LOGIMPORT_FILE", false, "GKH_LOG_IMPORT", "FILE_ID");
            Database.AddIndex("IND_GKH_LOGIMPORT_LFILE", false, "GKH_LOG_IMPORT", "LOG_FILE_ID");
            Database.AddForeignKey("FK_GKH_LOGIMPORT_LFILE", "GKH_LOG_IMPORT", "LOG_FILE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_GKH_LOGIMPORT_FILE", "GKH_LOG_IMPORT", "FILE_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_GKH_LOGIMPORT_OP", "GKH_LOG_IMPORT", "OPERATOR_ID", "GKH_OPERATOR", "ID");
            //-----

            //----темповые таблицы для конвертации
            Database.AddEntityTable(
                "CONVERTER_FILE_EXTERNAL",
                new Column("EXTERNAL_ID", DbType.String, 36),
                new Column("FILE_ID", DbType.Int64));

            Database.AddEntityTable(
                "CONVERTER_DISP_EXTERNAL",
                new Column("EXTERNAL_ID", DbType.String, 36),
                new Column("NEW_EXTERNAL_ID", DbType.String, 36));

            Database.AddEntityTable(
                "CONVERTER_STATE_EXTERNAL",
                new Column("EXTERNAL_ID", DbType.String, 36),
                new Column("STATE_ID", DbType.Int64));

            Database.AddEntityTable(
                "CONVERTER_USER_EXTERNAL",
                new Column("EXTERNAL_ID", DbType.String, 36),
                new Column("USER_ID", DbType.Int64));

            Database.AddEntityTable(
                "CONVERTER_ROLE_EXTERNAL",
                new Column("EXTERNAL_ID", DbType.String, 36),
                new Column("ROLE_ID", DbType.Int64));

            Database.AddEntityTable(
                "CONVERTER_UROLE_EXTERNAL",
                new Column("EXTERNAL_ID", DbType.String, 36),
                new Column("USER_ROLE_ID", DbType.Int64));
            //-----

        }

        public override void Down()
        {
            Database.RemoveConstraint("GKH_DICT_ZONAINSP_MUNIC", "FK_ZONA_MUNIC_MCP");
            Database.RemoveConstraint("GKH_DICT_ZONAINSP_MUNIC", "FK_ZONA_MUNIC_ZON");
            Database.RemoveConstraint("GKH_DICT_ZONAINSP_INSPECT", "FK_ZONA_INS_ZON");
            Database.RemoveConstraint("GKH_DICT_ZONAINSP_INSPECT", "FK_ZONA_INS_INSP");
            Database.RemoveConstraint("GKH_OBJ_DIRECT_MANAG_CNRT", "FK_GKH_DIRECT_MORG_CON");
            Database.RemoveConstraint("GKH_EMERGENCY_OBJECT", "FK_GKH_EMER_ST");
            Database.RemoveConstraint("GKH_LOG_IMPORT", "FK_GKH_LOGIMPORT_LFILE");
            Database.RemoveConstraint("GKH_LOG_IMPORT", "FK_GKH_LOGIMPORT_FILE");
            Database.RemoveConstraint("GKH_LOG_IMPORT", "FK_GKH_LOGIMPORT_OP");
            Database.RemoveConstraint("GKH_LOCAL_GOV_MUNICIP", "FK_GKH_LOCALG_MUNIC_MCP");
            Database.RemoveConstraint("GKH_LOCAL_GOV_MUNICIP", "FK_GKH_LOCALG_MUNIC_LG");
            Database.RemoveConstraint("GKH_OBJ_SERVICE_ORG", "FK_GKH_OBJ_SERVORG_CTRN");
            Database.RemoveConstraint("GKH_TEMPLATE_REPLACEMENT", "FK_GKH_TEMPLATE_REPL_FILE");
            Database.RemoveConstraint("GKH_DICT_INSTITUTIONS", "FK_GKH_INST_ADDR");
            Database.RemoveConstraint("GKH_LOCGOV_WORK", "FK_GKH_LOCGOV_WORK_LG");
            Database.RemoveConstraint("GKH_DICT_WORK_CUR_REPAIR", "FK_GKH_WORK_CR_UM");
            Database.RemoveConstraint("GKH_SUPPLY_RESORG", "FK_GKH_SUPPLY_CNTR");
            Database.RemoveConstraint("GKH_SUPPLY_RESORG_DOC", "FK_GKH_SUPPLY_DOC_RESORG");
            Database.RemoveConstraint("GKH_SUPPLY_RESORG_DOC", "FK_GKH_SUPPLY_DOC_FILE");
            Database.RemoveConstraint("GKH_SUPPLY_RESORG_SERV", "FK_GKH_SUPPLY_SERV_TYPE");
            Database.RemoveConstraint("GKH_SUPPLY_RESORG_SERV", "FK_GKH_SUPPLY_SERV_RESORG");
            Database.RemoveConstraint("GKH_BELAY_POLICY_RISK", "FK_GKH_BEL_POL_RISK_KR");
            Database.RemoveConstraint("GKH_MAN_ORG_WORK", "FK_GKH_MORG_WORK_MO");
            Database.RemoveConstraint("GKH_OBJ_IMAGE", "FK_GKH_OBJ_IMAGE_PR");
            Database.RemoveConstraint("GKH_OBJ_HOUSE_INFO", "FK_GKH_OBJ_HOUSE_INFO_UM");
            Database.RemoveConstraint("GKH_OPERATOR", "FK_GKH_OPERATOR_USER");
            Database.RemoveConstraint("GKH_OPERATOR_CONTRAGENT", "FK_GKH_OPERATOR_CTRN_OPER");
            Database.RemoveConstraint("GKH_OPERATOR_CONTRAGENT", "FK_GKH_OPERATOR_CTRN_CTRN");
            Database.RemoveConstraint("GKH_OPERATOR_MUNIC", "FK_GKH_OPERATOR_MU_MU");
            Database.RemoveConstraint("GKH_OPERATOR_MUNIC", "FK_GKH_OPERATOR_MU_OPER");
            Database.RemoveConstraint("GKH_OPERATOR_INSPECT", "FK_GKH_OPERATOR_INSP_INSP");
            Database.RemoveConstraint("GKH_OPERATOR_INSPECT", "FK_GKH_OPERATOR_INSP_OPER");
            Database.RemoveConstraint("GKH_OBJ_COUNCILLORS", "FK_GKH_OBJ_COUNCIL_RO");
            Database.RemoveConstraint("GKH_OBJ_PROTOCOL_MT", "FK_GKH_OBJ_PRTCL_MT_FILE");
            Database.RemoveConstraint("GKH_OBJ_PROTOCOL_MT", "FK_GKH_OBJ_PRTCL_MT_RO");
            Database.RemoveConstraint("GKH_DICT_ZONAINSP_MUNIC", "FK_DICT_ZONA_MUNIC_MCP");
            Database.RemoveConstraint("GKH_DICT_ZONAINSP_MUNIC", "FK_DICT_ZONA_MUNIC_ZON");
            Database.RemoveConstraint("GKH_DICT_ZONAINSP_INSPECT", "FK_DICT_ZONA_INSP_INSP");
            Database.RemoveConstraint("GKH_DICT_ZONAINSP_INSPECT", "FK_DICT_ZONA_INSP_ZON");
            Database.RemoveConstraint("GKH_MORG_CONTRACT", "FK_GKH_MORG_CN_MORG");
            Database.RemoveConstraint("GKH_MORG_CONTRACT", "FK_GKH_MORG_CN_FILE");
            Database.RemoveConstraint("GKH_MORG_JSKTSJ_CONTRACT", "FK_GKH_MORG_JSKTSJ_CN");
            Database.RemoveConstraint("GKH_MORG_JSKTSJ_CONTRACT", "FK_GKH_MORG_JSKTSJ_CN_MO");
            Database.RemoveConstraint("GKH_MORG_CONTRACT_JSKTSJ", "FK_GKH_MORG_CN_JSKTSJ");
            Database.RemoveConstraint("GKH_MORG_CONTRACT_JSKTSJ", "FK_GKH_MORG_CN_MJSKTSJ");
            Database.RemoveConstraint("GKH_MORG_CONTRACT_OWNERS", "FK_GKH_MORG_CN_OWNERS_ID");
            Database.RemoveConstraint("GKH_MORG_CONTRACT_RO_LIST", "FK_GKH_MORG_CN_RO_CON");
            Database.RemoveConstraint("GKH_MORG_CONTRACT_RO_LIST", "FK_GKH_MORG_CN_RO_RO");
            Database.RemoveConstraint("GKH_MAN_ORG_REAL_OBJ", "FK_GKH_MORG_RO_RO");
            Database.RemoveConstraint("GKH_MAN_ORG_REAL_OBJ", "FK_GKH_MORG_RO_MORG");
            Database.RemoveConstraint("GKH_DICT_RESETTLE_PROGRAM", "FK_GKH_RESPROG_PRD");
            Database.RemoveConstraint("GKH_EMERGENCY_OBJECT", "FK_GKH_EMER_PROG");
            Database.RemoveConstraint("GKH_BELAY_POLICY_RISK", "FK_GKH_BEL_POL_RISK_POL");
            Database.RemoveConstraint("GKH_BELAY_POLICY_EVENT", "FK_GKH_BEL_POL_EV_POL");
            Database.RemoveConstraint("GKH_BELAY_POLICY_EVENT", "FK_GKH_BEL_POL_EV_FIN");
            Database.RemoveConstraint("GKH_BELAY_POLICY_PAYMENT", "FK_GKH_BEL_POL_PAYM_POL");
            Database.RemoveConstraint("GKH_BELAY_POLICY_PAYMENT", "FK_GKH_BEL_POL_PAYM_FIN");
            Database.RemoveConstraint("GKH_BELAY_POLICY_MKD", "FK_GKH_BEL_POL_MKD_RO");
            Database.RemoveConstraint("GKH_BELAY_POLICY_MKD", "FK_GKH_BEL_POL_MKD_POL");
            Database.RemoveConstraint("GKH_BELAY_MANORG_POLICY", "FK_GKH_BEL_MORGP_ACT");
            Database.RemoveConstraint("GKH_BELAY_MANORG_POLICY", "FK_GKH_BEL_MORGP_BO");
            Database.RemoveConstraint("GKH_BELAY_MANORG_POLICY", "FK_GKH_BEL_MORGP_KA");
            Database.RemoveConstraint("GKH_BELAY_MORG_ACTIVITY", "FK_GKH_BEL_MORGAC_ORG");
            Database.RemoveConstraint("GKH_DICT_KINDEQUIPMENT", "FK_GKH_KINDEQUIPMENT_UNIT");
            Database.RemoveConstraint("GKH_DICT_WORK", "FK_GKH_WORK_UNIT");
            Database.RemoveConstraint("GKH_DICT_MUNICIPAL_SOURCE", "FK_GKH_MUNIC_SRC_MCP");
            Database.RemoveConstraint("GKH_REALITY_OBJECT", "FK_GKH_REALITY_OBJECT_CGR");
            Database.RemoveConstraint("GKH_REALITY_OBJECT", "FK_GKH_REALITY_OBJECT_MCP");
            Database.RemoveConstraint("GKH_REALITY_OBJECT", "FK_GKH_REALITY_OBJECT_RMT");
            Database.RemoveConstraint("GKH_REALITY_OBJECT", "FK_GKH_REALITY_OBJECT_OWN");
            Database.RemoveConstraint("GKH_REALITY_OBJECT", "FK_GKH_REALITY_OBJECT_ADR");
            Database.RemoveConstraint("GKH_OBJ_APARTMENT_INFO", "FK_GKH_OBJ_APARTINFO_RO");
            Database.RemoveConstraint("GKH_OBJ_CURENT_REPAIR", "FK_GKH_OBJ_CUR_REP_RO");
            Database.RemoveConstraint("GKH_OBJ_CURENT_REPAIR", "FK_GKH_OBJ_WORK_KIND");
            Database.RemoveConstraint("GKH_OBJ_HOUSE_INFO", "FK_GKH_OBJ_HOUSE_INFO_RO");
            Database.RemoveConstraint("GKH_OBJ_IMAGE", "FK_GKH_OBJ_IMAGE_RO");
            Database.RemoveConstraint("GKH_OBJ_IMAGE", "FK_GKH_OBJ_IMAGE_FILE");
            Database.RemoveConstraint("GKH_OBJ_IMAGE", "FK_GKH_OBJ_IMAGE_WORK");
            Database.RemoveConstraint("GKH_OBJ_LAND", "FK_GKH_OBJ_LAND_RO");
            Database.RemoveConstraint("GKH_OBJ_LAND", "FK_GKH_OBJ_LAND_FILE");
            Database.RemoveConstraint("GKH_OBJ_METERING_DEVICE", "FK_GKH_OBJ_MET_DEV_RO");
            Database.RemoveConstraint("GKH_OBJ_METERING_DEVICE", "FK_GKH_OBJ_MET_DEV_MDV");
            Database.RemoveConstraint("GKH_OBJ_CONST_ELEMENT", "FK_GKH_OBJ_CONSTELEMENT_OBJ");
            Database.RemoveConstraint("GKH_OBJ_CONST_ELEMENT", "FK_GKH_OBJ_CONSTEL_CEL");
            Database.RemoveConstraint("GKH_CONTRAGENT", "FK_GKH_CONTR_PRT");
            Database.RemoveConstraint("GKH_CONTRAGENT", "FK_GKH_CONTR_MADR");
            Database.RemoveConstraint("GKH_CONTRAGENT", "FK_GKH_CONTR_FADR");
            Database.RemoveConstraint("GKH_CONTRAGENT", "FK_GKH_CONTR_JADR");
            Database.RemoveConstraint("GKH_CONTRAGENT", "FK_GKH_CONTR_OADR");
            Database.RemoveConstraint("GKH_CONTRAGENT", "fk_gkh_contr_orgf");
            Database.RemoveConstraint("GKH_CONTRAGENT", "FK_GKH_CONTR_MCP");
            Database.RemoveConstraint("GKH_CONTRAGENT_BANK", "FK_GKH_CONTR_BANK_CTR");
            Database.RemoveConstraint("GKH_CONTRAGENT_CONTACT", "FK_GKH_CONTR_CONTACT_CTR");
            Database.RemoveConstraint("GKH_CONTRAGENT_CONTACT", "FK_GKH_CONTR_CONTACT_POS");
            Database.RemoveConstraint("GKH_MANAGING_ORGANIZATION", "FK_GKH_MORG_CTR");
            Database.RemoveConstraint("GKH_SERVICE_ORGANIZATION", "FK_GKH_SERV_ORG_CTR");
            Database.RemoveConstraint("GKH_OBJ_SERVICE_ORG", "FK_GKH_OBJ_SERV_ORG_RO");
            Database.RemoveConstraint("GKH_OBJ_SERVICE_ORG", "FK_GKH_OBJ_SERV_ORG_FILE");
            Database.RemoveConstraint("GKH_MAN_ORG_CLAIM", "FK_GKH_MORG_CL_MAO");
            Database.RemoveConstraint("GKH_MAN_ORG_DOC", "FK_GKH_MORG_DOC_MAO");
            Database.RemoveConstraint("GKH_MAN_ORG_DOC", "FK_GKH_MORG_DOC_FILE");
            Database.RemoveConstraint("GKH_MAN_ORG_MEMBERSHIP", "FK_GKH_MORG_MEM_MAO");
            Database.RemoveConstraint("GKH_MAN_ORG_SERVICE", "FK_GKH_MORG_SERV_MAO");
            Database.RemoveConstraint("GKH_SERV_ORG_DOC", "FK_GKH_SERV_ORG_DOC_SEO");
            Database.RemoveConstraint("GKH_SERV_ORG_DOC", "FK_GKH_SERV_ORG_DOC_FILE");
            Database.RemoveConstraint("GKH_SERV_ORG_SERVICE", "FK_GKH_SERV_ORG_SERV_SEO");
            Database.RemoveConstraint("GKH_SERV_ORG_SERVICE", "FK_GKH_SERV_ORG_SERV_TYS");
            Database.RemoveConstraint("GKH_LOCAL_GOVERNMENT", "FK_GKH_LOCALG_CTR");
            Database.RemoveConstraint("GKH_BELAY_ORGANIZATION", "FK_GKH_BEL_ORG_CTR");
            Database.RemoveConstraint("GKH_BUILDER", "FK_GKH_BUIL_FILE");
            Database.RemoveConstraint("GKH_BUILDER", "FK_GKH_BUIL_CTR");
            Database.RemoveConstraint("GKH_BUILDER_LOAN", "FK_GKH_BUIL_LOAN_CTR");
            Database.RemoveConstraint("GKH_BUILDER_LOAN", "FK_GKH_BUIL_LOAN_BLD");
            Database.RemoveConstraint("GKH_BUILDER_LOAN_REPAY", "FK_GKH_BUIL_LOAN_REP_BL");
            Database.RemoveConstraint("GKH_BUILDER_DOCUMENT", "FK_GKH_BUIL_DOC_PRD");
            Database.RemoveConstraint("GKH_BUILDER_DOCUMENT", "FK_GKH_BUIL_DOC_CTR");
            Database.RemoveConstraint("GKH_BUILDER_DOCUMENT", "FK_GKH_BUIL_DOC_FILE");
            Database.RemoveConstraint("GKH_BUILDER_DOCUMENT", "FK_GKH_BUIL_DOC_BLD");
            Database.RemoveConstraint("GKH_BUILDER_WORKFORCE", "FK_GKH_BUIL_WORKF_SPEC");
            Database.RemoveConstraint("GKH_BUILDER_WORKFORCE", "FK_GKH_BUIL_WORKF_FILE");
            Database.RemoveConstraint("GKH_BUILDER_WORKFORCE", "FK_GKH_BUIL_WORKF_BLD");
            Database.RemoveConstraint("GKH_BUILDER_WORKFORCE", "FK_GKH_BUIL_WORKF_INS");
            Database.RemoveConstraint("GKH_BUILDER_PRODUCTBASE", "FK_GKH_BUIL_PROD_EQU");
            Database.RemoveConstraint("GKH_BUILDER_PRODUCTBASE", "FK_GKH_BUIL_PROD_FILE");
            Database.RemoveConstraint("GKH_BUILDER_PRODUCTBASE", "FK_GKH_BUIL_PROD_BLD");
            Database.RemoveConstraint("GKH_BUILDER_TECHNIQUE", "FK_GKH_BUIL_TECH_FILE");
            Database.RemoveConstraint("GKH_BUILDER_TECHNIQUE", "FK_GKH_BUIL_TECH_BLD");
            Database.RemoveConstraint("GKH_BUILDER_SROINFO", "FK_GKH_BUIL_SRO_WORK");
            Database.RemoveConstraint("GKH_BUILDER_SROINFO", "FK_GKH_BUIL_SRO_FILE");
            Database.RemoveConstraint("GKH_BUILDER_SROINFO", "FK_GKH_BUIL_SRO_BLD");
            Database.RemoveConstraint("GKH_BUILDER_FEEDBACK", "FK_GKH_BUIL_FEEDB_FILE");
            Database.RemoveConstraint("GKH_BUILDER_FEEDBACK", "FK_GKH_BUIL_FEEDB_BLD");
            Database.RemoveConstraint("GKH_EMERGENCY_OBJECT", "FK_GKH_EMER_RO");
            Database.RemoveConstraint("GKH_EMERGENCY_OBJECT", "FK_GKH_EMER_RIN");
            Database.RemoveConstraint("GKH_EMERGENCY_OBJECT", "FK_GKH_EMER_USE");
            Database.RemoveConstraint("GKH_EMERGENCY_OBJECT", "FK_GKH_EMER_INF");
            Database.RemoveConstraint("GKH_EMERGENCY_RESETPROG", "FK_GKH_EMER_RESP_RO");
            Database.RemoveConstraint("GKH_EMERGENCY_RESETPROG", "FK_GKH_EMER_RESP_SRC");

            Database.RemoveTable("GKH_LOG_IMPORT");
            Database.RemoveTable("GKH_LOCAL_GOV_MUNICIP");
            Database.RemoveTable("CONVERTER_USER_EXTERNAL");
            Database.RemoveTable("CONVERTER_ROLE_EXTERNAL");
            Database.RemoveTable("CONVERTER_UROLE_EXTERNAL");
            Database.RemoveTable("CONVERTER_STATE_EXTERNAL");
            Database.RemoveTable("CONVERTER_DISP_EXTERNAL");
            Database.RemoveTable("CONVERTER_FILE_EXTERNAL");
            Database.RemoveTable("GKH_DICT_WORK_CUR_REPAIR");
            Database.RemoveTable("GKH_DICT_TYPE_PROJ");
            Database.RemoveTable("GKH_SUPPLY_RESORG");
            Database.RemoveTable("GKH_SUPPLY_RESORG_DOC");
            Database.RemoveTable("GKH_SUPPLY_RESORG_SERV");
            Database.RemoveTable("GKH_LOCGOV_WORK");
            Database.RemoveTable("GKH_DICT_KIND_RISK");
            Database.RemoveTable("GKH_DICT_INSTITUTIONS");
            Database.RemoveTable("GKH_OPERATOR");
            Database.RemoveTable("GKH_OPERATOR_INSPECT");
            Database.RemoveTable("GKH_OPERATOR_MUNIC");
            Database.RemoveTable("GKH_OPERATOR_CONTRAGENT");
            Database.RemoveTable("GKH_DICT_ORG_FORM");
            Database.RemoveTable("GKH_DICT_ZONAINSP_MUNIC");
            Database.RemoveTable("GKH_DICT_ZONAINSP_INSPECT");
            Database.RemoveTable("GKH_DICT_ZONAINSP"); 
            Database.RemoveTable("GKH_MAN_ORG_REAL_OBJ");
            Database.RemoveTable("GKH_DICT_RESETTLE_PROGRAM");
            Database.RemoveTable("GKH_DICT_BELAY_KIND_ACTIV");
            Database.RemoveTable("GKH_DICT_FURTHER_USE");
            Database.RemoveTable("GKH_DICT_REAS_INEXPEDIENT");
            Database.RemoveTable("GKH_DICT_RESETTLE_SOURCE");
            Database.RemoveTable("GKH_EMERGENCY_OBJECT");
            Database.RemoveTable("GKH_EMERGENCY_RESETPROG");
            Database.RemoveTable("GKH_RESETTLEMENT_PROGRAM");
            Database.RemoveTable("GKH_DICT_INSPECTOR");
            Database.RemoveTable("GKH_DICT_PERIOD");
            Database.RemoveTable("GKH_DICT_UNITMEASURE");
            Database.RemoveTable("GKH_DICT_SPECIALTY");
            Database.RemoveTable("GKH_DICT_KINDEQUIPMENT");
            Database.RemoveTable("GKH_DICT_WORK");
            Database.RemoveTable("GKH_DICT_CAPITAL_GROUP");
            Database.RemoveTable("GKH_DICT_CONST_ELEMENT");
            Database.RemoveTable("GKH_DICT_METERING_DEVICE");
            Database.RemoveTable("GKH_DICT_POSITION");
            Database.RemoveTable("GKH_DICT_ROOFING_MATERIAL");
            Database.RemoveTable("GKH_DICT_WALL_MATERIAL");
            Database.RemoveTable("GKH_DICT_TYPE_OWNERSHIP");
            Database.RemoveTable("GKH_DICT_TYPE_SERVICE");
            Database.RemoveTable("GKH_DICT_MUNICIPALITY");
            Database.RemoveTable("GKH_DICT_MUNICIPAL_SOURCE");
            Database.RemoveTable("GKH_OBJ_APARTMENT_INFO");
            Database.RemoveTable("GKH_OBJ_CURENT_REPAIR");
            Database.RemoveTable("GKH_OBJ_HOUSE_INFO");
            Database.RemoveTable("GKH_OBJ_IMAGE");
            Database.RemoveTable("GKH_OBJ_LAND");
            Database.RemoveTable("GKH_OBJ_METERING_DEVICE");
            Database.RemoveTable("GKH_OBJ_CONST_ELEMENT");
            Database.RemoveTable("GKH_OBJ_COUNCILLORS");
            Database.RemoveTable("GKH_OBJ_PROTOCOL_MT");
            Database.RemoveTable("GKH_OBJ_SERVICE_ORG");
            
            Database.RemoveTable("GKH_CONTRAGENT");
            Database.RemoveTable("GKH_CONTRAGENT_BANK");
            Database.RemoveTable("GKH_CONTRAGENT_CONTACT");
            Database.RemoveTable("GKH_MANAGING_ORGANIZATION");
            Database.RemoveTable("GKH_SERVICE_ORGANIZATION");
            Database.RemoveTable("GKH_MAN_ORG_CLAIM");
            Database.RemoveTable("GKH_MAN_ORG_DOC");
            Database.RemoveTable("GKH_MAN_ORG_MEMBERSHIP");
            Database.RemoveTable("GKH_MAN_ORG_SERVICE");
            Database.RemoveTable("GKH_MAN_ORG_WORK");
            Database.RemoveTable("GKH_BELAY_MORG_ACTIVITY");
            Database.RemoveTable("GKH_SERV_ORG_DOC");
            Database.RemoveTable("GKH_SERV_ORG_SERVICE");
            Database.RemoveTable("GKH_LOCAL_GOVERNMENT");
            Database.RemoveTable("GKH_BELAY_ORGANIZATION");
            Database.RemoveTable("GKH_BELAY_MANORG_POLICY");
            Database.RemoveTable("GKH_BELAY_POLICY_RISK");
            Database.RemoveTable("GKH_BELAY_POLICY_EVENT");
            Database.RemoveTable("GKH_BELAY_POLICY_PAYMENT");
            Database.RemoveTable("GKH_BELAY_POLICY_MKD");
            Database.RemoveTable("GKH_BUILDER");
            Database.RemoveTable("GKH_BUILDER_LOAN");
            Database.RemoveTable("GKH_BUILDER_LOAN_REPAY");
            Database.RemoveTable("GKH_BUILDER_DOCUMENT");
            Database.RemoveTable("GKH_BUILDER_WORKFORCE");
            Database.RemoveTable("GKH_BUILDER_PRODUCTBASE");
            Database.RemoveTable("GKH_BUILDER_TECHNIQUE");
            Database.RemoveTable("GKH_BUILDER_SROINFO");
            Database.RemoveTable("GKH_BUILDER_FEEDBACK");
            Database.RemoveTable("GKH_OBJ_DIRECT_MANAG_CNRT");
            Database.RemoveTable("GKH_MORG_JSKTSJ_CONTRACT");
            Database.RemoveTable("GKH_MORG_CONTRACT_JSKTSJ");
            Database.RemoveTable("GKH_MORG_CONTRACT_OWNERS");
            Database.RemoveTable("GKH_MORG_CONTRACT_REALOBJ");
            Database.RemoveTable("GKH_OPERATOR_CONTRAGENT");
            Database.RemoveTable("GKH_TEMPLATE_REPLACEMENT");
            Database.RemoveTable("GKH_REALITY_OBJECT");
            Database.RemoveTable("GKH_MORG_CONTRACT");

            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery(@"DROP SEQUENCE HIBERNATE_SEQUENCE");
            }
        }
    }
}