namespace Bars.Gkh.Migrations.Version_2013072503
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013072503")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013072502.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GKH_DICT_CHARACTS",
                new RefColumn("UNIT_MEASURE_ID", "GKH_DICT_CHARACTS_UM", "GKH_DICT_UNITMEASURE", "ID"),
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 10));

            Database.AddEntityTable(
               "GKH_DICT_CHARACT_ELEM",
               new RefColumn("CHARACT_ID", "GKH_DICT_CHARACTS_CH", "GKH_DICT_CHARACTS", "ID"),
               new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
               new Column("CODE", DbType.String, 300, ColumnProperty.NotNull));

            Database.AddEntityTable(
               "GKH_DICT_ELOBJ_CHARACT",
               new RefColumn("CHARACT_ID", "GKH_DICT_ELCHARACT_CH", "GKH_DICT_CHARACTS", "ID"),
               new RefColumn("ELOBJ_ID", "GKH_DICT_ELCHARACT_EL", "GKH_DICT_GROUP_ELEM_OBJ", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_DICT_ELOBJ_CHARACT");
            Database.RemoveTable("GKH_DICT_CHARACT_ELEM");
            Database.RemoveTable("GKH_DICT_CHARACTS");
        }
    }
}