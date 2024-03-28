namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013090503
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013090503")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013090502.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Перенесено в базовый модуль Overhaul, не удалять эту миграцию
            //Database.RemoveRefColumn("OVRHL_RO_STRUCT_EL", "UNIT_MEASURE_ID");
        }

        public override void Down()
        {
            //Database.AddRefColumn("OVRHL_RO_STRUCT_EL", new RefColumn("UNIT_MEASURE_ID", ColumnProperty.NotNull, "RO_STRUCT_EL_UM", "GKH_DICT_UNITMEASURE", "ID"));
        }
    }
}