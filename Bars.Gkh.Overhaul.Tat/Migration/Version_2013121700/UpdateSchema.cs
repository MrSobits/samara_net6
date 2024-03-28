namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013121700
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013121700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013121600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Перенесено в базовый модуль Overhaul, не удалять миграцию

           //Database.AddColumn("OVRHL_DICT_PAYSIZE", new Column("TYPE_INDICATOR", DbType.Int64, ColumnProperty.NotNull, 20));         
        }

        public override void Down()
        {
            //Database.RemoveColumn("OVRHL_DICT_PAYSIZE", "TYPE_INDICATOR");
        }
    }
}