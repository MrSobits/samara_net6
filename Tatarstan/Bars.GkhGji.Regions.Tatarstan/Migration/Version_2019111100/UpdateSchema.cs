namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2019111100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019111100")]
    [MigrationDependsOn(typeof(Version_2019110100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string PerfIndexTableName = "GJI_DICT_EFFEC_PERF_INDEX";
        private const string PerfIndexValueTableName = "GJI_EFFEC_PERF_INDEX_VALUE";

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(UpdateSchema.PerfIndexTableName,
                new Column("code", DbType.String, 300, ColumnProperty.NotNull),
                new Column("name", DbType.String, 500, ColumnProperty.NotNull),
                new Column("param_name", DbType.String, 500, ColumnProperty.NotNull),
                new Column("unit_measure", DbType.String, 300, ColumnProperty.NotNull));

            this.Database.AddEntityTable(UpdateSchema.PerfIndexValueTableName,
                new RefColumn("effec_perf_index_id", ColumnProperty.NotNull, 
                    UpdateSchema.PerfIndexValueTableName + "Foreign", UpdateSchema.PerfIndexTableName, "id"),
                new Column("calc_start_date", DbType.DateTime, ColumnProperty.NotNull),
                new Column("calc_end_date", DbType.DateTime, ColumnProperty.NotNull),
                new Column("value", DbType.Int32, ColumnProperty.NotNull)
                );
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(UpdateSchema.PerfIndexValueTableName);
            this.Database.RemoveTable(UpdateSchema.PerfIndexTableName);
        }
    }
}
