namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013110800
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013110800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013110701.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Перенесено в базовый модуль Overhaul, не удалять миграцию

            //Database.AddColumn("OVRHL_STRUCT_EL", new Column("LIFE_TIME_AFTER_REPAIR", DbType.Int16, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            //Database.RemoveColumn("OVRHL_STRUCT_EL", "LIFE_TIME_AFTER_REPAIR");
        }
    }
}