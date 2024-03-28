namespace Bars.GkhGji.Migrations.Version_2014051700
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014051700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014051600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_HEATING_INPUT_PERIOD",
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "GJI_HEAT_INPUT_MU", "GKH_DICT_MUNICIPALITY", "ID"),
                new Column("PERIOD_YEAR", DbType.Int32),
                new Column("PERIOD_MONTH", DbType.Byte));

            Database.AddEntityTable(
                "GJI_HEATING_INPUT_INFORMATION",
                new RefColumn("HEATINPUTPERIOD_ID", ColumnProperty.NotNull, "GJI_HEAT_INFO_PER", "GJI_HEATING_INPUT_PERIOD", "ID"),
                new Column("TYPE_HEAT_OBJ", DbType.Int32),
                new Column("HEAT_COUNT", DbType.Int32),
                new Column("HEAT_CENTRAL", DbType.Int32),
                new Column("HEAT_INDIVID", DbType.Int32),
                new Column("HEAT_PERCENT", DbType.Decimal),
                new Column("HEAT_NOHEATING", DbType.Int32));

            Database.AddRefColumn(
                "GJI_BOILER_ROOM",
                new RefColumn("MUNICIPALITY_ID", "GJI_BOIL_ROOM_MU", "GKH_DICT_MUNICIPALITY", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_HEATING_INPUT_PERIOD");

            Database.RemoveTable("GJI_HEATING_INPUT_INFORMATION");

            Database.RemoveColumn("GJI_BOILER_ROOM", "MUNICIPALITY_ID");
        }
    }
}