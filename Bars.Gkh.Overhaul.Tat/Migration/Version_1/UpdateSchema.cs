namespace Bars.Gkh.Overhaul.Tat.Migration.Version_1
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Перенесено в базовый модуль Overhaul, не удалять

            /*// Расценки работ
            Database.AddEntityTable(
                "OVRHL_DICT_WORK_PRICE",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new RefColumn("UNIT_MEASURE_ID", ColumnProperty.Null, "WRK_PRC_UNITMEAS", "GKH_DICT_UNITMEASURE", "ID"),
                new RefColumn("WORK_ID", ColumnProperty.NotNull, "WRK_PRC_WORK", "GKH_DICT_WORK", "ID"),
                new Column("NORMATIVE_COST", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("YEAR", DbType.Int16, ColumnProperty.NotNull));

            // Тип группы
            Database.AddEntityTable(
                "OVRHL_DICT_CEO_GROUP_TYPE",
                new Column("GROUP_TYPE_CODE", DbType.String, 200, ColumnProperty.NotNull),
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull));

            // Объект общего имущества
            Database.AddEntityTable(
                "OVRHL_COMMON_ESTATE_OBJECT",
                new Column("CEO_CODE", DbType.String, 200, ColumnProperty.NotNull),
                new RefColumn("CEO_GROUP_TYPE_ID", ColumnProperty.NotNull, "CEO_GROUP_TYPE", "OVRHL_DICT_CEO_GROUP_TYPE", "ID"),
                new Column("NAME", DbType.String, 500, ColumnProperty.NotNull),
                new Column("SHORT_NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("IS_MATCH_HC", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("INC_IN_SUBJ_PRG", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("IS_ENG_NETWORK", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("MULT_OBJECT", DbType.Boolean, ColumnProperty.NotNull, false));

            // Группа конструктивных элементов
            Database.AddEntityTable(
                "OVRHL_STRUCT_EL_GROUP",
                new RefColumn("CMN_ESTATE_OBJ_ID", ColumnProperty.NotNull, "CEGRP_CMNESTOBJ", "OVRHL_COMMON_ESTATE_OBJECT", "ID"),
                new Column("NAME", DbType.String, 500, ColumnProperty.NotNull),
                new Column("FORMULA", DbType.String, 500, ColumnProperty.NotNull),
                new Column("FORMULA_NAME", DbType.String, 500, ColumnProperty.NotNull),
                new Column("FORMULA_DESC", DbType.String, 500, ColumnProperty.Null),
                new Column("FORMULA_PARAMS", DbType.String, 500, ColumnProperty.Null));

            // Аттрибут группы конструктивных элементов
            Database.AddEntityTable(
                "OVRHL_STRUCT_EL_GROUP_ATR",
                new RefColumn("GROUP_ID", ColumnProperty.NotNull, "SE_GRP_ATR_GRP", "OVRHL_STRUCT_EL_GROUP", "ID"),
                new Column("NAME", DbType.String, 500, ColumnProperty.NotNull),
                new Column("IS_NEEDED", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("ATR_TYPE", DbType.Int64, ColumnProperty.NotNull));

            // Аттрибут группы конструктивных элементов
            Database.AddEntityTable(
                "OVRHL_STRUCT_EL",
                new RefColumn("GROUP_ID", ColumnProperty.NotNull, "SE_GRP", "OVRHL_STRUCT_EL_GROUP", "ID"),
                new Column("NAME", DbType.String, 500, ColumnProperty.NotNull),
                new Column("SE_CODE", DbType.String, 300, ColumnProperty.NotNull),
                new RefColumn("UNIT_MEASURE_ID", ColumnProperty.NotNull, "SE_UNIT_MSR", "GKH_DICT_UNITMEASURE", "ID"),
                new Column("LIFE_TIME", DbType.Int64, ColumnProperty.NotNull),
                new RefColumn("NORM_DOC_ID", ColumnProperty.Null, "SE_NORM_DOC", "GKH_DICT_NORMATIVE_DOC", "ID"));

            // Расценки работ
            Database.AddEntityTable(
                "OVRHL_STRUCT_EL_WORK",
                new RefColumn("WORK_ID", ColumnProperty.NotNull, "STR_EL_WRK_WRK", "GKH_DICT_WORK", "ID"),
                new RefColumn("STRUCT_EL_ID", ColumnProperty.NotNull, "STR_EL_WRK_STR_EL", "OVRHL_STRUCT_EL", "ID"));*/
        }

        public override void Down()
        {
            // Расценки работ
            Database.RemoveTable("OVRHL_STRUCT_EL_WORK");

            // Аттрибут группы конструктивных элементов
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
        }
    }
}