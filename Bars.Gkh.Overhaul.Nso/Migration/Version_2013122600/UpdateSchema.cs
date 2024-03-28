namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013122600
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013122600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013122401.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Перенесено в базовый модуль, не удалять миграцию
            /*Database.AddEntityTable("REAL_EST_TYPE_MU",
                new RefColumn("RET_ID", ColumnProperty.NotNull, "REAL_EST_TYPE_MU_RET", "OVRHL_REAL_ESTATE_TYPE", "ID"),
                new RefColumn("MU_ID", ColumnProperty.NotNull, "REAL_EST_TYPE_MU_MU", "GKH_DICT_MUNICIPALITY", "ID"));*/
        }

        public override void Down()
        {
            /*Database.RemoveEntityTable("REAL_EST_TYPE_MU");*/
        }
    }
}