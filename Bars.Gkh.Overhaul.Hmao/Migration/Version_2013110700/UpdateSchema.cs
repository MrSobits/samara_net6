namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013110700
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013110700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013110600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Перенесено в базовый модуль Overhaul, не удалять эту миграцию

            //Database.AddColumn("OVRHL_STRUCT_EL", new Column("IS_CALC_LIVEAREA", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        public override void Down()
        {
            //Database.RemoveColumn("OVRHL_STRUCT_EL", "IS_CALC_LIVEAREA");
        }
    }
}