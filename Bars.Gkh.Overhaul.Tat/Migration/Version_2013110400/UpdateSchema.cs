namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013110400
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013110400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013110103.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Перенесено в базовый модуль, не удалять миграцию
            /*Database.AddEntityTable("OVRHL_REALESTATEREALITYO",
                new RefColumn("RO_ID", "REALESTATEREALITYO_RO", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("RET_ID", "REALESTATEREALITYO_RET", "OVRHL_REAL_ESTATE_TYPE", "ID"));*/
        }

        public override void Down()
        {
            /*Database.RemoveEntityTable("OVRHL_REALESTATEREALITYO");*/
        }
    }
}