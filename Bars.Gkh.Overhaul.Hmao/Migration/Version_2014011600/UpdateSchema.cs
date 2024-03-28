namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2014011600
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014011600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2014011500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Перенесено в базовый модуль Overhaul, не удалять эту миграцию

            //Database.AddRefColumn("OVRHL_RO_STRUCT_EL", new RefColumn("STATE_ID", "OV_RO_SE_STATE", "B4_STATE", "ID"));
        }

        public override void Down()
        {
            //Database.RemoveRefColumn("OVRHL_RO_STRUCT_EL", "STATE_ID");
        }
    }
}