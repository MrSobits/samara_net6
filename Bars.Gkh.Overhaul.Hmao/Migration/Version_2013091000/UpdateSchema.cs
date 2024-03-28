namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013091000
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013091000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013090503.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Перенесено в базовый модуль Overhaul, не удалять эту миграцию
            //Database.AddColumn("OVRHL_RO_STRUCT_EL", new Column("NAME", DbType.String, ColumnProperty.Null));
        }

        public override void Down()
        {
            //Database.RemoveColumn("OVRHL_RO_STRUCT_EL", "NAME");
        }
    }
}