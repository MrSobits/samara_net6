namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013091202
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013091202")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013091201.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Перенесено в базовый модуль Overhaul, не удалять эту миграцию

            // Для уничтожения данных
            /*Database.RemoveEntityTable("OVRHL_STRUCT_EL_WORK");

            // Расценки работ
            Database.AddEntityTable(
                "OVRHL_STRUCT_EL_WORK",
                new RefColumn("WORK_PRICE_ID", ColumnProperty.NotNull, "STR_EL_WRK_WRKPRICE", "OVRHL_DICT_WORK_PRICE", "ID"),
                new RefColumn("STRUCT_EL_ID", ColumnProperty.NotNull, "STR_EL_WRK_STR_EL", "OVRHL_STRUCT_EL", "ID"));*/
        }

        public override void Down()
        {
            // Для уничтожения данных
            /*Database.RemoveEntityTable("OVRHL_STRUCT_EL_WORK");

            // Расценки работ
            Database.AddEntityTable(
                "OVRHL_STRUCT_EL_WORK",
                new RefColumn("WORK_ID", ColumnProperty.NotNull, "STR_EL_WRK_WRK", "GKH_DICT_WORK", "ID"),
                new RefColumn("STRUCT_EL_ID", ColumnProperty.NotNull, "STR_EL_WRK_STR_EL", "OVRHL_STRUCT_EL", "ID"));*/
        }
    }
}