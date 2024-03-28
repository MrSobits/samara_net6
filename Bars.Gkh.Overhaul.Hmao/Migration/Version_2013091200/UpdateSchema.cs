namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013091200
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013091200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013091101.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Перенесено в базовый модуль Overhaul, не удалять эту миграцию

            //Database.AddColumn("OVRHL_STRUCT_EL_GROUP", new Column("REQUIRED", DbType.Boolean, ColumnProperty.Null, false));
        }

        public override void Down()
        {
            //Database.RemoveColumn("OVRHL_STRUCT_EL_GROUP", "REQUIRED");
        }
    }
}