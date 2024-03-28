namespace Bars.GkhDi.Regions.Tatarstan.Migrations.Version_2014020600
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014020600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Regions.Tatarstan.Migrations.Version_1.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----Меры по снижению расходов
            Database.AddEntityTable(
                "DI_TAT_MEAS_RED_COSTS",
                new Column("MEASURE_NAME", DbType.String, 300, ColumnProperty.NotNull));
            Database.AddIndex("MEASURES_NAME", false, "DI_TAT_MEAS_RED_COSTS", "MEASURE_NAME");


            //-----Сущность для связи названия меры из справочника с работами по плану мер
            Database.AddEntityTable(
                "DI_TAT_PLAN_RED_MEAS_NAME",
                new RefColumn("MEASURE_RED_COSTS_ID", ColumnProperty.NotNull, "MEASURES_NAME", "DI_TAT_MEAS_RED_COSTS", "ID"),
                new RefColumn("PLAN_RED_EXPWORK_ID", ColumnProperty.NotNull, "PLAN_MEAS_NAME", "DI_DISINFO_RO_REDEXP_WORK", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("DI_TAT_PLAN_RED_MEAS_NAME");
            Database.RemoveTable("DI_TAT_MEAS_RED_COSTS");  
        }
    }
}