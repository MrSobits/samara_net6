namespace Bars.Gkh1468.Migrations.Version_1
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----Структура паспорта
            Database.AddEntityTable(
                "GKH_PSTRUCT_PSTRUCT",
                new Column("NAME", DbType.String, 2000),
                new Column("VALID_START", DbType.DateTime, ColumnProperty.Null),
                new Column("VALID_END", DbType.DateTime, ColumnProperty.Null),
                new Column("PASSPORT_TYPE", DbType.Int16, ColumnProperty.Null));

            //-----Часть структуры паспорта
            Database.AddEntityTable(
                "GKH_PSTRUCT_PART",
                new RefColumn("PARENT_ID", ColumnProperty.Null, "GKH_PSTRUCT_PART_PARENT", "GKH_PSTRUCT_PART", "ID"),
                new RefColumn("PASSPORT_STRUCT_ID", ColumnProperty.Null, "GKH_PSTRUCT_PART_STRUCT", "GKH_PSTRUCT_PSTRUCT", "ID"),

                new Column("CODE", DbType.String, ColumnProperty.Null),
                new Column("NAME", DbType.String, 2000, ColumnProperty.Null),
                new Column("UO", DbType.Boolean, ColumnProperty.Null),
                new Column("PKU", DbType.Boolean, ColumnProperty.Null),
                new Column("PR", DbType.Boolean, ColumnProperty.Null),
                new Column("ORDER_NUM", DbType.String, ColumnProperty.Null),
                new Column("INTEGRATION_CODE", DbType.String, ColumnProperty.Null));

            //----Атрибут части
            Database.AddEntityTable(
                "GKH_PSTRUCT_META_ATTR",

                new RefColumn("PARENT_ID", ColumnProperty.Null, "GKH_PSTR_META_ATTR_PAR", "GKH_PSTRUCT_META_ATTR", "ID"),
                new RefColumn("PARENT_PART_ID", ColumnProperty.Null, "GKH_PSTR_META_ATTR_PARPART", "GKH_PSTRUCT_PART", "ID"),
                new RefColumn("UNIT_MEASURE_ID", ColumnProperty.Null, "GKH_PSTRUCT_META_ATTR_UNIT", "GKH_DICT_UNITMEASURE", "ID"),

                new Column("CODE", DbType.String, ColumnProperty.Null),
                new Column("NAME", DbType.String, 2000, ColumnProperty.Null),
                new Column("TYPE", DbType.Int16, ColumnProperty.Null),
                new Column("VALUE_TYPE", DbType.Int16, ColumnProperty.Null),

                new Column("VALIDATE_CHILDS", DbType.Boolean, ColumnProperty.Null),
                new Column("GROUP_TEXT", DbType.String, ColumnProperty.Null),
                new Column("ORDER_NUM", DbType.String, ColumnProperty.Null),
                new Column("INTEGRATION_CODE", DbType.String, ColumnProperty.Null),
                new Column("DICT_CODE", DbType.String, ColumnProperty.Null),

                new Column("MAX_LENGTH", DbType.Int16, ColumnProperty.Null),
                new Column("MIN_LENGTH", DbType.Int16, ColumnProperty.Null),
                new Column("PATTERN", DbType.String, ColumnProperty.Null),
                new Column("EXP", DbType.Int16, ColumnProperty.Null),
                new Column("REQUIRED", DbType.Boolean, ColumnProperty.Null),
                new Column("ALLOW_NEGATIVE", DbType.Boolean, ColumnProperty.Null));
            //-----

            //-----Поставщик коммунальных услуг
            Database.AddEntityTable(
                "GKH_PUBLIC_SERVORG",
                new Column("CONTRAGENT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("ORG_STATE_ROLE", DbType.Int32, 4, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("ACTIVITY_TERMINATION", DbType.Int32, 4, ColumnProperty.NotNull),
                new Column("DESCRIPTION_TERM", DbType.String, 500),
                new Column("DATE_TERMINATION", DbType.DateTime));
            Database.AddIndex("IND_GKH_PUBLSERV_CNTR", false, "GKH_PUBLIC_SERVORG", "CONTRAGENT_ID");
            Database.AddForeignKey("FK_GKH_PUBLSERV_CNTR", "GKH_PUBLIC_SERVORG", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            //-----

            Database.AddEntityTable("GKH_PUBLIC_SERVORG_MU",
                new RefColumn("PUBLIC_SERVORG_ID", ColumnProperty.NotNull, "GKH_PUBLIC_SERVORG_MU_PSO", "GKH_PUBLIC_SERVORG", "ID"),
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "GKH_PUBLIC_SERVORG_MU_MU", "GKH_DICT_MUNICIPALITY", "ID"));

            Database.AddEntityTable(
                "GKH_PUBLIC_SERV", 
                new Column("NAME", DbType.String, ColumnProperty.NotNull, 500));

            if (!Database.TableExists("GKH_DICT_MULTIGLOSSARY"))
            {
                //-----Универсальный справочник
                // Таблица перенесена в модуль GKH, условие для обратной совместимости
                Database.AddEntityTable("GKH_DICT_MULTIGLOSSARY",
                    new Column("CODE", DbType.String, 200, ColumnProperty.NotNull),
                    new Column("NAME", DbType.String, 2000, ColumnProperty.NotNull));
                Database.AddIndex("IND_GKH_DICT_MGLOSS_CODE", true, "GKH_DICT_MULTIGLOSSARY", "CODE");
            }


            if (!Database.TableExists("GKH_DICT_MULTIITEM"))
            {
                // Таблица перенесена в модуль GKH. Условие создано для обратной совместимости
                Database.AddEntityTable("GKH_DICT_MULTIITEM",
                    new RefColumn("GLOSSARY_ID", ColumnProperty.NotNull, "GKH_DICT_MULTIITEM_G", "GKH_DICT_MULTIGLOSSARY", "ID"),
                    new Column("KEY", DbType.String, 200, ColumnProperty.NotNull),
                    new Column("VALUE", DbType.String, 2000, ColumnProperty.NotNull));
                
            }
            //-----

            //-----Паспорт ОКИ
            Database.AddEntityTable(
                "GKH_OKI_PASSPORT",
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "OKI_PASSP_MUNIC", "GKH_DICT_MUNICIPALITY", "ID"),
                new RefColumn("STATE_ID", ColumnProperty.NotNull, "OKI_PASP_STATE", "B4_STATE", "ID"),
                new RefColumn("XML_ID", "OKI_PASP_XML", "B4_FILE_INFO", "ID"),
                new RefColumn("SIGNATURE_ID", "OKI_PASP_SIGN", "B4_FILE_INFO", "ID"),
                new RefColumn("PDF_ID", "OKI_PASP_PDF", "B4_FILE_INFO", "ID"),

                new Column("REP_YEAR", DbType.Int64, ColumnProperty.NotNull),
                new Column("REP_MONTH", DbType.Int64, ColumnProperty.NotNull),
                new Column("PERCENT", DbType.Decimal, ColumnProperty.NotNull, 0));

            //----- Провайдер Паспорта ОКИ
            Database.AddEntityTable(
                "GKH_OKI_PROV_PASSPORT",
                new RefColumn("OKI_PASP_ID", ColumnProperty.NotNull, "OKI_PROV_PASP_PASP", "GKH_OKI_PASSPORT", "ID"),
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "OKI_PROV_PASP_MU", "GKH_DICT_MUNICIPALITY", "ID"),
                new RefColumn("STATE_ID", ColumnProperty.NotNull, "OKI_PROV_PASP_STATE", "B4_STATE", "ID"),
                new RefColumn("CONTRAGENT_ID", ColumnProperty.NotNull, "OKI_PROV_PASP_CTRGNT", "GKH_CONTRAGENT", "ID"),
                new RefColumn("XML_ID", "OKI_PROV_PASP_XML", "B4_FILE_INFO", "ID"),
                new RefColumn("SIGNATURE_ID", "OKI_PROV_PASP_SIGN", "B4_FILE_INFO", "ID"),
                new RefColumn("PDF_ID", "OKI_PROV_PASP_PDF", "B4_FILE_INFO", "ID"),
                new RefColumn("PASSPORT_STRUCT_ID", "GKH_OKI_PROV_PASSP_STRUCT", "GKH_PSTRUCT_PSTRUCT", "ID"),

                new Column("REP_YEAR", DbType.Int64, ColumnProperty.NotNull),
                new Column("REP_MONTH", DbType.Int64, ColumnProperty.NotNull),
                new Column("CONTRAGENT_TYPE", DbType.Int64, ColumnProperty.NotNull),
                new Column("PERCENT", DbType.Decimal, ColumnProperty.NotNull, 0));

            //----- Паспорт дома
            Database.AddEntityTable(
                "GKH_HOUSE_PASSPORT",
                new RefColumn("RO_ID", ColumnProperty.NotNull, "HS_PASP_RO", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("STATE_ID", ColumnProperty.NotNull, "HS_PASP_STATE", "B4_STATE", "ID"),
                new RefColumn("XML_ID", "HS_PASP_XML", "B4_FILE_INFO", "ID"),
                new RefColumn("SIGNATURE_ID", "HS_PASP_SIGN", "B4_FILE_INFO", "ID"),
                new RefColumn("PDF_ID", "HS_PASP_PDF", "B4_FILE_INFO", "ID"),

                new Column("REP_YEAR", DbType.Int64, ColumnProperty.NotNull),
                new Column("REP_MONTH", DbType.Int64, ColumnProperty.NotNull),
                new Column("HOUSE_TYPE", DbType.Int64, ColumnProperty.NotNull),
                new Column("PERCENT", DbType.Decimal, ColumnProperty.NotNull, 0));

            //-----Провайдер паспорта дома
            Database.AddEntityTable(
                "GKH_HOUSE_PROV_PASSPORT",
                new RefColumn("HOUSE_PASP_ID", ColumnProperty.NotNull, "HS_PROV_PASP_PASP", "GKH_HOUSE_PASSPORT", "ID"),
                new RefColumn("RO_ID", ColumnProperty.NotNull, "HS_PROV_PASP_RO", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("STATE_ID", ColumnProperty.NotNull, "HS_PROV_PASP_STATE", "B4_STATE", "ID"),
                new RefColumn("CONTRAGENT_ID", ColumnProperty.NotNull, "HS_PROV_PASP_CTRGNT", "GKH_CONTRAGENT", "ID"),
                new RefColumn("XML_ID", "HS_PROV_PASP_XML", "B4_FILE_INFO", "ID"),
                new RefColumn("SIGNATURE_ID", "HS_PROV_PASP_SIGN", "B4_FILE_INFO", "ID"),
                new RefColumn("PDF_ID", "HS_PROV_PASP_PDF", "B4_FILE_INFO", "ID"),
                new RefColumn("PASSPORT_STRUCT_ID", "GKH_HOUSEPROV_PASSP_STRUCT", "GKH_PSTRUCT_PSTRUCT", "ID"),

                new Column("REP_YEAR", DbType.Int64, ColumnProperty.NotNull),
                new Column("REP_MONTH", DbType.Int64, ColumnProperty.NotNull),
                new Column("HOUSE_TYPE", DbType.Int64, ColumnProperty.NotNull),
                new Column("CONTRAGENT_TYPE", DbType.Int64, ColumnProperty.NotNull),
                new Column("PERCENT", DbType.Decimal, ColumnProperty.NotNull, 0));
            //-----

            Database.AddEntityTable("GKH_HOUSE_PROV_PASS_ROW",
                new RefColumn("META_ATTRIBUTE_ID", ColumnProperty.Null, "HOUSE_PROV_PASSP_ROW_ATTR", "GKH_PSTRUCT_META_ATTR", "ID"),
                new RefColumn("HOUSE_PROV_PASSPORT_ID", ColumnProperty.Null, "HOUSE_PROV_PASSP_ROW_PASSP", "GKH_HOUSE_PROV_PASSPORT", "ID"),
                new Column("VALUE", DbType.String, 2000, ColumnProperty.Null));

            Database.AddEntityTable("GKH_OKI_PROV_PASSPORT_ROW",
                new RefColumn("META_ATTRIBUTE_ID", ColumnProperty.Null, "OKI_PROV_PASSP_ROW_ATTR", "GKH_PSTRUCT_META_ATTR", "ID"),
                new RefColumn("OKI_PROV_PASSPORT_ID", ColumnProperty.Null, "OKI_PROV_PASSP_ROW_PASSP", "GKH_OKI_PROV_PASSPORT", "ID"),
                new Column("VALUE", DbType.String, 2000, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_SUPPLY_RESORG_MU");
            Database.RemoveTable("GKH_PUBLIC_SERVORG_MU");

            Database.RemoveConstraint("GKH_PUBLIC_SERVORG", "FK_GKH_PUBLSERV_CNTR");
            Database.RemoveTable("GKH_PUBLIC_SERVORG");

            Database.RemoveTable("GKH_PUBLIC_SERV");

            Database.RemoveTable("GKH_HOUSE_PROV_PASSPORT");
            Database.RemoveTable("GKH_HOUSE_PASSPORT");
            Database.RemoveTable("GKH_OKI_PROV_PASSPORT");
            Database.RemoveTable("GKH_OKI_PASSPORT");

            Database.RemoveTable("GKH_PSTRUCT_META_ATTR");
            Database.RemoveTable("GKH_PSTRUCT_PART");
            Database.RemoveTable("GKH_PSTRUCT_PSTRUCT");

            Database.RemoveTable("GKH_OKI_PROV_PASSPORT_ROW");
            Database.RemoveTable("GKH_HOUSE_PROV_PASS_ROW");
        }
    }
}