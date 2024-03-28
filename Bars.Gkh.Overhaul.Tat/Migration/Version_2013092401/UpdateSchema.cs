namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013092401
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013092401")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013092400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Перенесено в базовый модуль Overhaul, не удалять миграцию

            //Database.AddColumn("OVRHL_STRUCT_EL_GROUP_ATR", new Column("HINT", DbType.String, 3000));
        }

        public override void Down()
        {
            //Database.RemoveColumn("OVRHL_STRUCT_EL_GROUP_ATR", "HINT");
        }
    }
}