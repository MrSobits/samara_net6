namespace Bars.GkhDi.Migrations.Version_2013111100
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013111100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2013110700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("DI_DICT_WORK_PPR", new RefColumn("MEASURE_ID", ColumnProperty.Null, "DICT_WORK_PPR_M", "GKH_DICT_UNITMEASURE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("DI_DICT_WORK_PPR", "MEASURE_ID");
        }
    }
}