namespace Bars.Gkh.Overhaul.Migration.Version_1
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // Хитрая миграция. Тут пока, что пусто потому что надо будет существующие модули ДПКР объединить
            // следовантельно все регионы накатят данную миграцию а потом волшебным образом здесь появится код 
            // Миграция перенесена в базовый модуль Overhaul, не удалять

            //Работы
            Database.AddEntityTable("OVRHL_DICT_JOB",
                new RefColumn("WORK_ID", ColumnProperty.NotNull, "OVRHL_DICT_JOB_WORK", "GKH_DICT_WORK", "ID"),
                new RefColumn("UNIT_MEASURE_ID", ColumnProperty.Null, "OVRHL_JOB_UNITMEAS", "GKH_DICT_UNITMEASURE", "ID"),
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull));

            // Расценки работ
            Database.AddEntityTable("OVRHL_DICT_WORK_PRICE",
                new RefColumn("JOB_ID", "WRK_PRC_JOB", "OVRHL_DICT_JOB", "ID"),
                new RefColumn("MUNICIPALITY_ID", "OVRHL_DICT_WRK_PRC_MU", "GKH_DICT_MUNICIPALITY", "ID"),
                new Column("NORMATIVE_COST", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("SQUARE_METER_COST", DbType.Decimal),
                new Column("YEAR", DbType.Int16, ColumnProperty.NotNull));

            // Тип группы ООИ
            //Database.AddEntityTable("OVRHL_DICT_CEO_GROUP_TYPE",
            //    new Column("GROUP_TYPE_CODE", DbType.String, 200, ColumnProperty.NotNull),
            //    new Column("NAME", DbType.String, 300, ColumnProperty.NotNull));

            // Объект общего имущества
            //Database.AddEntityTable("OVRHL_COMMON_ESTATE_OBJECT",
            //    new RefColumn("CEO_GROUP_TYPE_ID", ColumnProperty.NotNull, "CEO_GROUP_TYPE", "OVRHL_DICT_CEO_GROUP_TYPE", "ID"),
            //    new Column("CEO_CODE", DbType.String, 200, ColumnProperty.NotNull),
            //    new Column("NAME", DbType.String, 500, ColumnProperty.NotNull),
            //    new Column("SHORT_NAME", DbType.String, 300, ColumnProperty.NotNull),
            //    new Column("IS_MATCH_HC", DbType.Boolean, ColumnProperty.NotNull, false),
            //    new Column("WEIGHT", DbType.Int64, ColumnProperty.NotNull, 0),
            //    new Column("INC_IN_SUBJ_PRG", DbType.Boolean, ColumnProperty.NotNull, false),
            //    new Column("IS_ENG_NETWORK", DbType.Boolean, ColumnProperty.NotNull, false),
            //    new Column("MULT_OBJECT", DbType.Boolean, ColumnProperty.NotNull, false),
            //    new Column("IS_MAIN", DbType.Boolean, ColumnProperty.NotNull, false));

            // Группа конструктивных элементов
            //Database.AddEntityTable("OVRHL_STRUCT_EL_GROUP",
            //    new RefColumn("CMN_ESTATE_OBJ_ID", ColumnProperty.NotNull, "CEGRP_CMNESTOBJ", "OVRHL_COMMON_ESTATE_OBJECT", "ID"),
            //    new Column("NAME", DbType.String, 500, ColumnProperty.NotNull),
            //    new Column("FORMULA", DbType.String, 500, ColumnProperty.NotNull),
            //    new Column("FORMULA_NAME", DbType.String, 500, ColumnProperty.NotNull),
            //    new Column("FORMULA_DESC", DbType.String, 500, ColumnProperty.Null),
            //    new Column("FORMULA_PARAMS_BIN", DbType.Binary, ColumnProperty.Null),
            //    new Column("REQUIRED", DbType.Boolean, ColumnProperty.NotNull, false),
            //    new Column("USE_IN_CALC", DbType.Boolean, ColumnProperty.NotNull, true));

            //Атрибут группы конструктивных элементов
            //Database.AddEntityTable("OVRHL_STRUCT_EL_GROUP_ATR",
            //    new RefColumn("GROUP_ID", ColumnProperty.NotNull, "SE_GRP_ATR_GRP", "OVRHL_STRUCT_EL_GROUP", "ID"),
            //    new Column("NAME", DbType.String, 500, ColumnProperty.NotNull),
            //    new Column("IS_NEEDED", DbType.Boolean, ColumnProperty.NotNull, false),
            //    new Column("ATR_TYPE", DbType.Int64, ColumnProperty.NotNull),
            //    new Column("HINT", DbType.String, 3000));

            //Конструктивные элементы
            //Database.AddEntityTable("OVRHL_STRUCT_EL",
            //    new RefColumn("GROUP_ID", ColumnProperty.NotNull, "SE_GRP", "OVRHL_STRUCT_EL_GROUP", "ID"),
            //    new RefColumn("UNIT_MEASURE_ID", ColumnProperty.NotNull, "SE_UNIT_MSR", "GKH_DICT_UNITMEASURE", "ID"),
            //    new RefColumn("NORM_DOC_ID", ColumnProperty.Null, "SE_NORM_DOC", "GKH_DICT_NORMATIVE_DOC", "ID"),
            //    new Column("NAME", DbType.String, 500, ColumnProperty.NotNull),
            //    new Column("SE_CODE", DbType.String, 300, ColumnProperty.NotNull),
            //    new Column("LIFE_TIME", DbType.Int64, ColumnProperty.NotNull),
            //    new Column("MUT_EXCLUS_GROUP", DbType.String, 300),
            //    new Column("LIFE_TIME_AFTER_REPAIR", DbType.Int16, ColumnProperty.NotNull, 0),
            //    new Column("CALCULATE_BY", DbType.Int32, ColumnProperty.NotNull, 0));

            //Работы конструктивного элемента
            Database.AddEntityTable("OVRHL_STRUCT_EL_WORK",
                new RefColumn("JOB_ID", "STR_EL_WRK_JOB", "OVRHL_DICT_JOB", "ID"),
                new RefColumn("STRUCT_EL_ID", ColumnProperty.NotNull, "STR_EL_WRK_STR_EL", "OVRHL_STRUCT_EL", "ID"));

            //Конструктивный элемент жилого дома
            Database.AddEntityTable("OVRHL_RO_STRUCT_EL",
                new RefColumn("RO_ID", ColumnProperty.NotNull, "RO_STRUCT_EL_RO", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("STRUCT_EL_ID", ColumnProperty.NotNull, "RO_STRUCT_EL_SE", "OVRHL_STRUCT_EL", "ID"),
                new Column("NAME", DbType.String),
                new Column("VOLUME", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("REPAIRED", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("LAST_OVERHAUL_YEAR", DbType.Int64, ColumnProperty.NotNull, 0),
                new Column("WEAROUT", DbType.Decimal, ColumnProperty.NotNull, 0));

            //Атрибут КЭ жилого дома
            Database.AddEntityTable("OVRHL_RO_SE_VALUE",
               new RefColumn("ATR_ID", ColumnProperty.NotNull, "RO_SE_VAL_ATR", "OVRHL_STRUCT_EL_GROUP_ATR", "ID"),
               new RefColumn("OBJ_ID", ColumnProperty.NotNull, "RO_SE_VAL_OBJ", "OVRHL_RO_STRUCT_EL", "ID"),
               new Column("VALUE", DbType.String, ColumnProperty.NotNull, 1000));

            //Размер взноса на КР
            Database.AddEntityTable("OVRHL_DICT_PAYSIZE",
                new Column("TYPE_INDICATOR", DbType.Int64, ColumnProperty.NotNull, 20),
                new Column("PAYMENT_SIZE", DbType.Decimal, ColumnProperty.NotNull),
                new Column("DATE_START_PERIOD", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DATE_END_PERIOD", DbType.DateTime));

            //Связь размера взноса и МО
            Database.AddEntityTable("OVRHL_PAYSIZE_MU_RECORD",
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "OV_PAYSIZE_MUREC_MU", "GKH_DICT_MUNICIPALITY", "ID"),
                new RefColumn("PAYSIZECR_ID", ColumnProperty.NotNull, "OV_PAYSIZE_MUREC_PS", "OVRHL_DICT_PAYSIZE", "ID"));

            //Источник финансирования вида работ
            Database.AddEntityTable("OVRHL_DICT_WORK_TYPE_FIN",
                new RefColumn("WORK_ID", ColumnProperty.NotNull, "OVRHL_DICT_WRK_TFIN_WRK", "GKH_DICT_WORK", "ID"),
                new Column("TYPE_FIN_SOURCE", DbType.Int32, 4, ColumnProperty.NotNull, 10));
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_DICT_WORK_TYPE_FIN");

            // Связь размера взноса и МО
            Database.RemoveTable("OVRHL_PAYSIZE_MU_RECORD");

            // Размер взноса на КР
            Database.RemoveTable("OVRHL_DICT_PAYSIZE");

            // Атрибут КЭ жилого дома
            Database.RemoveTable("OVRHL_RO_SE_VALUE");

            // Конструктивный элемент жилого дома
            Database.RemoveTable("OVRHL_RO_STRUCT_EL");

            // Виды работ конструктивного элемента
            Database.RemoveTable("OVRHL_STRUCT_EL_WORK");

            // Конструктивные элементы
            Database.RemoveTable("OVRHL_STRUCT_EL");

            // Аттрибут группы конструктивных элементов
            Database.RemoveTable("OVRHL_STRUCT_EL_GROUP_ATR");

            // Группа конструктивных элементов
            Database.RemoveTable("OVRHL_STRUCT_EL_GROUP");

            // Объект общего имущества
            Database.RemoveTable("OVRHL_COMMON_ESTATE_OBJECT");

            // Расценки работ
            Database.RemoveTable("OVRHL_DICT_CEO_GROUP_TYPE");

            // Расценки работ
            Database.RemoveTable("GKH_DICT_WORK_PRICE");

            Database.RemoveTable("OVRHL_DICT_JOB");
        }
    }
}