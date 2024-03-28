namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013091600
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013091600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013091301.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Перенесено в базовый модуль Overhaul, не удалять миграцию

            /*Database.AddRefColumn("OVRHL_DICT_JOB", new RefColumn("UNIT_MEASURE_ID", ColumnProperty.Null, "OVRHL_JOB_UNITMEAS", "GKH_DICT_UNITMEASURE", "ID"));
            Database.RemoveRefColumn("OVRHL_DICT_WORK_PRICE", "UNIT_MEASURE_ID");*/
        }

        public override void Down()
        {
            /*Database.AddRefColumn("OVRHL_DICT_WORK_PRICE", new RefColumn("UNIT_MEASURE_ID", ColumnProperty.Null, "WRK_PRC_UNITMEAS", "GKH_DICT_UNITMEASURE", "ID"));
            Database.RemoveRefColumn("OVRHL_DICT_JOB", "UNIT_MEASURE_ID");*/
        }
    }
}