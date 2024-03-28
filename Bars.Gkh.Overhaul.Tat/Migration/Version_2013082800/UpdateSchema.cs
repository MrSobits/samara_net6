namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013082800
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013082800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_1.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Перенесено в базовый модуль Overhaul, не удалять миграцию

            /*Database.AddEntityTable(
                "OVRHL_RO_STRUCT_EL",
                new RefColumn("RO_ID", ColumnProperty.NotNull, "RO_STRUCT_EL_RO", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("STRUCT_EL_ID", ColumnProperty.NotNull, "RO_STRUCT_EL_SE", "OVRHL_STRUCT_EL", "ID"),
                new RefColumn("UNIT_MEASURE_ID", ColumnProperty.NotNull, "RO_STRUCT_EL_UM", "GKH_DICT_UNITMEASURE", "ID"),
                new Column("VOLUME", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("REPAIRED", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("LAST_OVERHAUL_YEAR", DbType.Int64, ColumnProperty.NotNull, 0));

            Database.AddEntityTable(
               "OVRHL_RO_SE_VALUE",
               new RefColumn("ATR_ID", ColumnProperty.NotNull, "RO_SE_VAL_ATR", "OVRHL_STRUCT_EL_GROUP_ATR", "ID"),
               new RefColumn("OBJ_ID", ColumnProperty.NotNull, "RO_SE_VAL_OBJ", "OVRHL_RO_STRUCT_EL", "ID"),
               new Column("VALUE", DbType.String, ColumnProperty.NotNull, 1000));

            Database.AddEntityTable(
                "OVRHL_RO_STRUCT_EL_WORK",
                new RefColumn("RO_SE_ID", ColumnProperty.NotNull, "RO_SE_WORK_RO_SE", "OVRHL_RO_STRUCT_EL", "ID"),
                new RefColumn("WORK_ID", ColumnProperty.NotNull, "RO_SE_WORK_WORK", "GKH_DICT_WORK", "ID"),
                new Column("LAST_OVERHAUL_YEAR", DbType.Int64, ColumnProperty.NotNull, 0),
                new Column("VOLUME_REPAIR", DbType.Int64, ColumnProperty.NotNull),
                new Column("TYPE_REPAIR", DbType.Int64, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, ColumnProperty.NotNull, 2000));*/

            Database.AddEntityTable(
                "OVRHL_RO_STRUCT_EL_IN_PRG",
                new RefColumn("RO_SE_ID", ColumnProperty.NotNull, "RO_SE_INPRG_RO_SE", "OVRHL_RO_STRUCT_EL", "ID"),
                new Column("YEAR", DbType.Int64, ColumnProperty.NotNull, 0),
                new Column("SUM", DbType.Decimal, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_RO_STRUCT_EL_IN_PRG");
            /*Database.RemoveEntityTable("OVRHL_RO_STRUCT_EL_WORK");
            Database.RemoveEntityTable("OVRHL_RO_SE_VALUE");
            Database.RemoveEntityTable("OVRHL_RO_STRUCT_EL");*/
        }
    }
}