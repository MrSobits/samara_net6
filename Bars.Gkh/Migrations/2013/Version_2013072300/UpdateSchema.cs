namespace Bars.Gkh.Migrations.Version_2013072300
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013072300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013060600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----Группы элементов объектов общего имущества
            Database.AddEntityTable(
                "GKH_DICT_GROUP_ELEM_OBJ",
                new Column("NAME", DbType.String, 300),
                new Column("TYPE_GROUP", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("REPAIR_RESOURCE", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("ACCEPT_PRESENCE", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("TYPE_ENGINEER_SYSTEM", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("METERING_INDICATOR", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36),
                new RefColumn("UNIT_MEASURE_ID", "GKH_GROUP_ELEM_OBJ_UM", "GKH_DICT_UNITMEASURE", "ID"));

            Database.AddIndex("IND_GKH_GROUP_ELEM_OBJ_NAME", false, "GKH_DICT_GROUP_ELEM_OBJ", "NAME");

            //-----Группы элементов объектов общего имущества
            Database.AddEntityTable(
                "GKH_DICT_GROUP_EL_WORK",
                new RefColumn("WORK_ID", "GKH_GROUP_EL_WORK_WORK", "GKH_DICT_WORK", "ID"),
                new RefColumn("GROUP_ELEMENT_ID", "GKH_GROUP_EL_WORK_ELEM", "GKH_DICT_GROUP_ELEM_OBJ", "ID"),
                new Column("EXTERNAL_ID", DbType.String, 36));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_DICT_GROUP_EL_WORK");
            Database.RemoveTable("GKH_DICT_GROUP_ELEM_OBJ");
        }
    }
}