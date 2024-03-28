namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013091701
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013091701")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013091700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Перенесено в базовый модуль Overhaul, не удалять эту миграцию

            /*Database.AddEntityTable("OVRHL_DICT_WORK_TYPE_FIN",
                new RefColumn("WORK_ID", ColumnProperty.NotNull, "OVRHL_DICT_WRK_TFIN_WRK", "GKH_DICT_WORK", "ID"),
                new Column("TYPE_FIN_SOURCE", DbType.Int32, 4, ColumnProperty.NotNull, 10));*/
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_DICT_WORK_TYPE_FIN");
        }
    }
}