namespace Bars.Gkh.Migrations.Version_2013072500
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013072500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013072402.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("GKH_DICT_GROUP_ELEM_OBJ", "METERING_INDICATOR");
            Database.RemoveColumn("GKH_DICT_GROUP_ELEM_OBJ", "TYPE_ENGINEER_SYSTEM");
            Database.RemoveColumn("GKH_DICT_GROUP_ELEM_OBJ", "UNIT_MEASURE_ID");
            Database.AddColumn("GKH_DICT_GROUP_ELEM_OBJ", new Column("SHORT_NAME", DbType.String, 300));
        }

        public override void Down()
        {
        }
    }
}