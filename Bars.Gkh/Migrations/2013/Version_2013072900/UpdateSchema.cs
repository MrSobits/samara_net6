namespace Bars.Gkh.Migrations.Version_2013072900
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013072900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013072703.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GKH_DICT_CONST_ELEMENT", new RefColumn("UNIT_MEASURE_ID", "GKH_DICT_CONST_EL_UNMES", "GKH_DICT_UNITMEASURE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_CONST_ELEMENT", "UNIT_MEASURE_ID");
        }
    }
}