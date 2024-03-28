namespace Bars.Gkh.Overhaul.Migration.Version_2014013100
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014013100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Migration.Version_1.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (!Database.ColumnExists("OVRHL_COMMON_ESTATE_OBJECT", "IS_MAIN"))
            {
                Database.AddColumn("OVRHL_COMMON_ESTATE_OBJECT", new Column("IS_MAIN", DbType.Boolean, ColumnProperty.NotNull, false));
            }

            if (!Database.ColumnExists("OVRHL_RO_STRUCT_EL", "STATE_ID"))
            {
                Database.AddRefColumn("OVRHL_RO_STRUCT_EL", new RefColumn("STATE_ID", "OV_RO_SE_STATE", "B4_STATE", "ID"));
            }

            if (!Database.ColumnExists("OVRHL_STRUCT_EL_GROUP", "USE_IN_CALC"))
            {
                Database.AddColumn("OVRHL_STRUCT_EL_GROUP", new Column("USE_IN_CALC", DbType.Boolean, ColumnProperty.NotNull, true));
            }

            if (!Database.ColumnExists("OVRHL_DICT_PAYSIZE", "TYPE_INDICATOR"))
            {
                Database.AddColumn("OVRHL_DICT_PAYSIZE", new Column("TYPE_INDICATOR", DbType.Int64, ColumnProperty.NotNull, 20));
            }

            if (!Database.ColumnExists("ovrhl_dict_work_price", "SQUARE_METER_COST"))
            {
                Database.AddColumn("ovrhl_dict_work_price", new Column("SQUARE_METER_COST", DbType.Decimal, (object) 0m));
            }

            if (!Database.ColumnExists("OVRHL_STRUCT_EL", "CALCULATE_BY"))
            {
                Database.AddColumn("OVRHL_STRUCT_EL", new Column("CALCULATE_BY", DbType.Int32, ColumnProperty.NotNull, 0));
            }

            if (!Database.ColumnExists("OVRHL_STRUCT_EL", "LIFE_TIME_AFTER_REPAIR"))
            {
                Database.AddColumn("OVRHL_STRUCT_EL", new Column("LIFE_TIME_AFTER_REPAIR", DbType.Int16, ColumnProperty.NotNull, 0));
            }
        }

        public override void Down()
        {
            //down пустой, т.к. удаление происходит в миграции 1
        }
    }
}