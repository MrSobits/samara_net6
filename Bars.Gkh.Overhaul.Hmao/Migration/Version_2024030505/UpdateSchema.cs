namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2024030505
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030505")]
    [MigrationDependsOn(typeof(Version_2024030504.UpdateSchema))]
    // Является Version_2022102100 из ядра
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName table = new SchemaQualifiedObjectName { Name = "OVRHL_ACTUALIZE_LOG" };
        private readonly SchemaQualifiedObjectName recordTable = new SchemaQualifiedObjectName { Name = "OVRHL_ACTUALIZE_LOG_RECORD" };
        private readonly Column column = new Column("INPUT_PARAMS", DbType.String.WithSize(1000));

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn(table, column);

            this.Database.AddEntityTable(recordTable.Name,
                new RefColumn(
                    "ACTUALIZE_LOG_ID",
                    ColumnProperty.NotNull,
                    $"{recordTable.Name}_ACTUALIZE_LOG",
                    table.Name,
                    "ID"),
                new Column("ACTION", DbType.String),
                new RefColumn(
                    "REALITY_OBJECT_ID",
                    $"{recordTable.Name}_REALITY_OBJECT",
                    "GKH_REALITY_OBJECT",
                    "ID"),
                new Column("WORK_CODE", DbType.String),
                new Column("CEO", DbType.String),
                new Column("PLAN_YEAR", DbType.Int32),
                new Column("CHANGE_PLAN_YEAR", DbType.Int32),
                new Column("PUBLISH_YEAR", DbType.Int32),
                new Column("CHANGE_PUBLISH_YEAR", DbType.Int32),
                new Column("VOLUME", DbType.Decimal),
                new Column("CHANGE_VOLUME", DbType.Decimal),
                new Column("SUM", DbType.Decimal),
                new Column("CHANGE_SUM", DbType.Decimal),
                new Column("NUMBER", DbType.Int32),
                new Column("CHANGE_NUMBER", DbType.Int32));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(table, column.Name);
            this.Database.RemoveTable(recordTable);
        }
    }
}