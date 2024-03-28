namespace Bars.Gkh.Migrations._2016.Version_2016121700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2016121700")]
    [MigrationDependsOn(typeof(Version_2016121600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GKH_DICT_MODEL_LIFT",
                new Column("NAME", DbType.String, 300));

            this.Database.AddEntityTable(
                "GKH_DICT_TYPE_LIFT",
                new Column("NAME", DbType.String, 300));

            this.Database.AddEntityTable(
                "GKH_DICT_CABIN_LIFT",
                new Column("NAME", DbType.String, 300));

            this.Database.AddEntityTable(
                "GKH_DICT_LIFT_SHAFT",
                new Column("NAME", DbType.String, 300));

            this.Database.AddEntityTable(
                "GKH_DICT_DRIVE_DOORS",
                new Column("NAME", DbType.String, 300));

            this.Database.AddEntityTable(
                "GKH_DICT_MASH_ROOM",
                new Column("NAME", DbType.String, 300));

            this.Database.AddEntityTable(
                "GKH_RO_LIFT",
                new Column("PORCH_NUM", DbType.String, 100),
                new Column("LIFT_NUM", DbType.String, 100),
                new Column("FACTORY_NUM", DbType.String, 100),
                new Column("REG_NUM", DbType.String, 100),
                new Column("PERIOD_REPLACE", DbType.String, 100),

                new Column("YEAR_INSTALATION", DbType.Int32),
                new Column("YEAR_LAST_UP_REPAIR", DbType.Int32),
                new Column("YEAR_ESTIMATE", DbType.Int32),
                new Column("YEAR_PLAN_REP", DbType.Int32),
                new Column("STOP_COUNT", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("CAPACITY", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("COST", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("COST_ESTIMATE", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("SPEED_RISE", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("LIFE_TIME", DbType.DateTime),
                new Column("YEAR_EXPLOITATION", DbType.Int32),
                new Column("NUMBER_OF_STOREYS", DbType.Int32),
                new Column("DEPTH_LIFT_SHAFT", DbType.Decimal),
                new Column("WIDTH_LIFT_SHAFT", DbType.Decimal),
                new Column("HEIGHT_LIFT_SHAFT", DbType.Decimal),
                new Column("DEPTH_CABIN", DbType.Decimal),
                new Column("WIDTH_CABIN", DbType.Decimal),
                new Column("HEIGHT_CABIN", DbType.Decimal),
                new Column("WIDTH_OPENING_CABIN", DbType.Decimal),
                new Column("OWNER_LIFT", DbType.String, 100),
                new Column("AVAILABILITY_DEVICES", DbType.Int16, ColumnProperty.NotNull, 0),

                new RefColumn("RO_ID", ColumnProperty.NotNull, "GKH_RO_LIFT_ROID", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("LIFT_SHAFT_ID", ColumnProperty.Null, "GKH_RO_LIFT_SHAFTID", "GKH_DICT_LIFT_SHAFT", "ID"),
                new RefColumn("LIFT_DRIVE_DOORS_ID", ColumnProperty.Null, "GKH_RO_LIFT_D_DOORSID", "GKH_DICT_DRIVE_DOORS", "ID"),
                new RefColumn("LIFT_MASH_ROOM_ID", ColumnProperty.Null, "GKH_RO_LIFT_MASHROOMID", "GKH_DICT_MASH_ROOM", "ID"),
                new RefColumn("LIFT_TYPE_LIFT_ID", ColumnProperty.Null, "GKH_RO_LIFT_TYPEID", "GKH_DICT_TYPE_LIFT", "ID"),
                new RefColumn("LIFT_MODEL_LIFT_ID", ColumnProperty.Null, "GKH_RO_LIFT_MODELID", "GKH_DICT_MODEL_LIFT", "ID"),
                new RefColumn("LIFT_CONTRAGENT_ID", ColumnProperty.Null, "GKH_RO_LIFT_CTRID", "GKH_CONTRAGENT", "ID"),
                new RefColumn("LIFT_CABIN_ID", ColumnProperty.Null, "GKH_RO_LIFT_CABINID", "GKH_DICT_CABIN_LIFT", "ID")
                );

            this.Database.AddEntityTable(
                "GKH_RO_LIFT_SUM",
                new Column("HINGED", DbType.Int32),
                new Column("LOWERINGS", DbType.Int32),
                new Column("MJI_COUNT", DbType.Int32),
                new Column("MJI_PASSENGER", DbType.Int32),
                new Column("MJI_CARGO", DbType.Int32),
                new Column("MJI_MIXED", DbType.Int32),
                new Column("RISERS", DbType.Int32),
                new Column("SHAFT_COUNT", DbType.Int32),
                new Column("BTI_COUNT", DbType.Int32),
                new Column("BTI_PASSENGER", DbType.Int32),
                new Column("BTI_CARGO", DbType.Int32),
                new Column("BTI_MIXED", DbType.Int32),

                new RefColumn("RO_ID", ColumnProperty.NotNull, "GKH_RO_LIFT_SUM_ROID", "GKH_REALITY_OBJECT", "ID")
                );
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_RO_LIFT");
            this.Database.RemoveTable("GKH_RO_LIFT_SUM");

            this.Database.RemoveTable("GKH_DICT_MODEL_LIFT");
            this.Database.RemoveTable("GKH_DICT_TYPE_LIFT");
            this.Database.RemoveTable("GKH_DICT_CABIN_LIFT");
            this.Database.RemoveTable("GKH_DICT_LIFT_SHAFT");
            this.Database.RemoveTable("GKH_DICT_DRIVE_DOORS");
            this.Database.RemoveTable("GKH_DICT_MASH_ROOM");
        }
    }
}