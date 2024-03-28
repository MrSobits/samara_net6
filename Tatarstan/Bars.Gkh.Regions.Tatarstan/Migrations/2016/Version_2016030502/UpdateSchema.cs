namespace Bars.Gkh.Regions.Tatarstan.Migrations._2016.Version_2016030502
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2016030502")]
    [MigrationDependsOn(typeof(Version_2016030100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_CONSTRUCT_OBJ_TYPEWORK",
                new Column("YEAR_BUILDING", DbType.Int32, ColumnProperty.NotNull),
                new Column("HAS_PSD", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("HAS_EXPERTISE", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("VOLUME", DbType.Decimal.WithSize(10, 2), ColumnProperty.Null),
                new Column("SUM", DbType.Decimal.WithSize(10, 2), ColumnProperty.Null),
                new Column("DESCRIPTION", DbType.String, 500, ColumnProperty.Null),
                new Column("DATE_START_WORK", DbType.DateTime, ColumnProperty.Null),
                new Column("DATE_END_WORK", DbType.DateTime, ColumnProperty.Null),
                new Column("VOLUME_COMPLETION", DbType.Decimal.WithSize(10, 2), ColumnProperty.Null),
                new Column("PERCENT_COMPLETION", DbType.Decimal.WithSize(10, 2), ColumnProperty.Null),
                new Column("COST_SUM", DbType.Decimal.WithSize(10, 2), ColumnProperty.Null),
                new Column("COUNT_WORKER", DbType.Decimal.WithSize(10, 2), ColumnProperty.Null),
                new RefColumn("OBJECT_ID", ColumnProperty.NotNull, "CO_TW_CO", "GKH_CONSTRUCTION_OBJECT", "ID"),
                new RefColumn("WORK_ID", ColumnProperty.NotNull, "CO_TW_WORK", "GKH_DICT_WORK", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_CONSTRUCT_OBJ_TYPEWORK");
        }
    }
}