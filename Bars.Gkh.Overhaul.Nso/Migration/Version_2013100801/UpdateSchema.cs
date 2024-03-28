namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013100801
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013100801")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013100800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Миграция перенесена в базовый модуль Overhaul, не удалять

            /*Database.AddRefColumn("OVRHL_DICT_WORK_PRICE",
                new RefColumn("MUNICIPALITY_ID", "OVRHL_DICT_WRK_PRC_MU", "GKH_DICT_MUNICIPALITY ", "ID"));*/
        }

        public override void Down()
        {
            //Database.RemoveRefColumn("OVRHL_DICT_WORK_PRICE", "MUNICIPALITY_ID");
        }
    }
}