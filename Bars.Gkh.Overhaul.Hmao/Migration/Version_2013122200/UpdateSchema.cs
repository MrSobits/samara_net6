namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013122200
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013122200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013122100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Перенесено в базовый модуль Overhaul, не удалять эту миграцию

            //Database.AddColumn("OVRHL_STRUCT_EL_GROUP", new Column("USE_IN_CALC", DbType.Boolean, ColumnProperty.NotNull, true));
        }

        public override void Down()
        {
            //Database.RemoveColumn("OVRHL_STRUCT_EL_GROUP", "USE_IN_CALC");
        }
    }
}