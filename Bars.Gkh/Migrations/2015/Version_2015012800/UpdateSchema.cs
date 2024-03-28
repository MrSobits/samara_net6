namespace Bars.Gkh.Migrations.Version_2015012800
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015012800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2015012300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (!Database.TableExists("OVRHL_DICT_CEO_GROUP_TYPE"))
            {
                Database.AddEntityTable("OVRHL_DICT_CEO_GROUP_TYPE",
                    new Column("GROUP_TYPE_CODE", DbType.String, 200, ColumnProperty.NotNull),
                    new Column("NAME", DbType.String, 300, ColumnProperty.NotNull));
            }
            
            if (!Database.TableExists("OVRHL_COMMON_ESTATE_OBJECT"))
            {
                Database.AddEntityTable("OVRHL_COMMON_ESTATE_OBJECT",
                    new RefColumn("CEO_GROUP_TYPE_ID", ColumnProperty.NotNull, "CEO_GROUP_TYPE",
                        "OVRHL_DICT_CEO_GROUP_TYPE",
                        "ID"),
                    new Column("CEO_CODE", DbType.String, 200, ColumnProperty.NotNull),
                    new Column("NAME", DbType.String, 500, ColumnProperty.NotNull),
                    new Column("SHORT_NAME", DbType.String, 300, ColumnProperty.NotNull),
                    new Column("IS_MATCH_HC", DbType.Boolean, ColumnProperty.NotNull, false),
                    new Column("WEIGHT", DbType.Int64, ColumnProperty.NotNull, 0),
                    new Column("INC_IN_SUBJ_PRG", DbType.Boolean, ColumnProperty.NotNull, false),
                    new Column("IS_ENG_NETWORK", DbType.Boolean, ColumnProperty.NotNull, false),
                    new Column("MULT_OBJECT", DbType.Boolean, ColumnProperty.NotNull, false),
                    new Column("IS_MAIN", DbType.Boolean, ColumnProperty.NotNull, false));
            }

            if (!Database.TableExists("OVRHL_STRUCT_EL_GROUP"))
            {
                // Группа конструктивных элементов
                Database.AddEntityTable("OVRHL_STRUCT_EL_GROUP",
                    new RefColumn("CMN_ESTATE_OBJ_ID", ColumnProperty.NotNull, "CEGRP_CMNESTOBJ",
                        "OVRHL_COMMON_ESTATE_OBJECT", "ID"),
                    new Column("NAME", DbType.String, 500, ColumnProperty.NotNull),
                    new Column("FORMULA", DbType.String, 500, ColumnProperty.NotNull),
                    new Column("FORMULA_NAME", DbType.String, 500, ColumnProperty.NotNull),
                    new Column("FORMULA_DESC", DbType.String, 500, ColumnProperty.Null),
                    new Column("FORMULA_PARAMS_BIN", DbType.Binary, ColumnProperty.Null),
                    new Column("REQUIRED", DbType.Boolean, ColumnProperty.NotNull, false),
                    new Column("USE_IN_CALC", DbType.Boolean, ColumnProperty.NotNull, true));
            }

            if (!Database.TableExists("OVRHL_STRUCT_EL_GROUP_ATR"))
            {
                //Атрибут группы конструктивных элементов
                Database.AddEntityTable("OVRHL_STRUCT_EL_GROUP_ATR",
                    new RefColumn("GROUP_ID", ColumnProperty.NotNull, "SE_GRP_ATR_GRP", "OVRHL_STRUCT_EL_GROUP", "ID"),
                    new Column("NAME", DbType.String, 500, ColumnProperty.NotNull),
                    new Column("IS_NEEDED", DbType.Boolean, ColumnProperty.NotNull, false),
                    new Column("ATR_TYPE", DbType.Int64, ColumnProperty.NotNull),
                    new Column("HINT", DbType.String, 3000));
            }

            if (!Database.TableExists("OVRHL_STRUCT_EL"))
            {
                //Конструктивные элементы
                Database.AddEntityTable("OVRHL_STRUCT_EL",
                    new RefColumn("GROUP_ID", ColumnProperty.NotNull, "SE_GRP", "OVRHL_STRUCT_EL_GROUP", "ID"),
                    new RefColumn("UNIT_MEASURE_ID", ColumnProperty.NotNull, "SE_UNIT_MSR", "GKH_DICT_UNITMEASURE", "ID"),
                    new RefColumn("NORM_DOC_ID", ColumnProperty.Null, "SE_NORM_DOC", "GKH_DICT_NORMATIVE_DOC", "ID"),
                    new Column("NAME", DbType.String, 500, ColumnProperty.NotNull),
                    new Column("SE_CODE", DbType.String, 300, ColumnProperty.NotNull),
                    new Column("LIFE_TIME", DbType.Int64, ColumnProperty.NotNull),
                    new Column("MUT_EXCLUS_GROUP", DbType.String, 300),
                    new Column("LIFE_TIME_AFTER_REPAIR", DbType.Int16, ColumnProperty.NotNull, 0),
                    new Column("CALCULATE_BY", DbType.Int32, ColumnProperty.NotNull, 0));
            }

            if (!Database.TableExists("OVRHL_REAL_ESTATE_TYPE"))
            {
                Database.AddEntityTable("OVRHL_REAL_ESTATE_TYPE",
                    new Column("NAME", DbType.String, 300, ColumnProperty.NotNull));
            }

            if (!Database.ColumnExists("OVRHL_REAL_ESTATE_TYPE", "MARG_REPAIR_COST"))
            {
                Database.AddColumn("OVRHL_REAL_ESTATE_TYPE", new Column("MARG_REPAIR_COST", DbType.Decimal));
            }

            if (!Database.TableExists("REAL_EST_TYPE_COMM_PARAM"))
            {
                Database.AddEntityTable("REAL_EST_TYPE_COMM_PARAM",
                    new Column("MIN", DbType.String, 500, ColumnProperty.NotNull),
                    new Column("MAX", DbType.String, 500, ColumnProperty.NotNull),
                    new Column("COMMON_PARAM_CODE", DbType.String, 500, ColumnProperty.Null),
                    new RefColumn("REAL_EST_TYPE_ID", ColumnProperty.NotNull, "COMM_PAR_REAL_EST_TYPE", "OVRHL_REAL_ESTATE_TYPE", "ID"));
            }

            if (!Database.TableExists("OVRHL_REALESTTYPE_PRIORITY"))
            {
                Database.AddEntityTable(
                    "OVRHL_REALESTTYPE_PRIORITY",
                    new Column("CODE", DbType.String, 300, ColumnProperty.NotNull),
                    new Column("WEIGHT", DbType.Int16, ColumnProperty.NotNull),
                    new RefColumn("REAL_ESTATE_TYPE_ID", "OVRHL_REALESTPROIR_REALEST", "OVRHL_REAL_ESTATE_TYPE", "ID"));
            }

            if (!Database.TableExists("OVRHL_REAL_EST_TYPE_RATE"))
            {
                Database.AddEntityTable(
                    "OVRHL_REAL_EST_TYPE_RATE",
                    new Column("SOCIALLY_ACCEPTABLE_RATE", DbType.Decimal),
                    new Column("NEED_FOR_FUNDING", DbType.Decimal),
                    new Column("TOTAL_AREA", DbType.Decimal),
                    new Column("REASONABLE_RATE", DbType.Decimal),
                    new Column("RATE_DEFICIT", DbType.Decimal),
                    new Column("YEAR", DbType.Int32, (object)2014),
                    new RefColumn("REAL_ESTATE_TYPE_ID", "OVRHL_RESTTPRT_RLST", "OVRHL_REAL_ESTATE_TYPE", "ID"));
            }

            if (!Database.TableExists("OVRHL_REALESTATEREALITYO"))
            {
                Database.AddEntityTable("OVRHL_REALESTATEREALITYO",
                    new RefColumn("RO_ID", "REALESTATEREALITYO_RO", "GKH_REALITY_OBJECT", "ID"),
                    new RefColumn("RET_ID", "REALESTATEREALITYO_RET", "OVRHL_REAL_ESTATE_TYPE", "ID"));
            }

            if (!Database.TableExists("REAL_EST_TYPE_STRUCT_EL"))
            {
                Database.AddEntityTable("REAL_EST_TYPE_STRUCT_EL",
                    new RefColumn("REAL_EST_TYPE_ID", ColumnProperty.NotNull, "STR_EL_REAL_EST_TYPE", "OVRHL_REAL_ESTATE_TYPE", "ID"),
                    new RefColumn("STRUCT_EL_ID", ColumnProperty.NotNull, "EST_TYPE_STRUCT_EL", "OVRHL_STRUCT_EL", "ID"));
            }

            if (!Database.ColumnExists("REAL_EST_TYPE_STRUCT_EL", "IS_EXISTS"))
            {
                Database.AddColumn("REAL_EST_TYPE_STRUCT_EL", new Column("IS_EXISTS", DbType.Boolean, true));
            }
        }

        public override void Down()
        {
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

            Database.RemoveTable("REAL_EST_TYPE_COMM_PARAM");

            Database.RemoveTable("OVRHL_REALESTTYPE_PRIORITY");

            Database.RemoveTable("OVRHL_REAL_ESTATE_TYPE");

            Database.RemoveTable("OVRHL_REAL_EST_TYPE_RATE");

            Database.RemoveTable("OVRHL_REALESTATEREALITYO");

            Database.RemoveTable("REAL_EST_TYPE_STRUCT_EL");
        }
    }
}