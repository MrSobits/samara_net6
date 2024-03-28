namespace Bars.GkhGji.Migrations.Version_2014052000
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014052000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014051700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_WORK_WINTER_MARK",
                new Column("WORKMARK_NAME", DbType.String, 200),
                new Column("WORKMARK_ROW", DbType.Int32),
                new Column("WORKMARK_MEASURE", DbType.String, 50),
                new Column("WORKMARK_OKEI", DbType.String, 50));

            Database.AddEntityTable(
                "GJI_WORK_WINTER_CONDITION",
                new RefColumn("HEATINPUTPERIOD_ID", ColumnProperty.NotNull, "GJI_WORK_WINT_PER", "GJI_HEATING_INPUT_PERIOD", "ID"),
                new RefColumn("WORKWINTERMARK_ID", "GJI_WORK_WINT_MARK", "GJI_WORK_WINTER_MARK", "ID"),
                new Column("WORKWINTER_TOTAL", DbType.Decimal),
                new Column("WORKWINTER_PREPTASK", DbType.Decimal),
                new Column("WORKWINTER_PREPWORK", DbType.Decimal),
                new Column("WORKWINTER_FINISHED", DbType.Decimal),
                new Column("WORKWINTER_PERCENT", DbType.Decimal));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_WORK_WINTER_CONDITION");

            Database.RemoveTable("GJI_WORK_WINTER_MARK");
        }
    }
}