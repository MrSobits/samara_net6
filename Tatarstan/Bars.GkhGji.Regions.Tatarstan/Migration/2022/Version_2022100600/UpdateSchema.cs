namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022100600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022100600")]
    // объединение веток в одну
    [MigrationDependsOn(typeof(Version_2022093000.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2022100400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName violInfoTable = new SchemaQualifiedObjectName { Name = "GJI_VISIT_SHEET_VIOLATION_INFO" };
        private readonly SchemaQualifiedObjectName violTable = new SchemaQualifiedObjectName { Name = "GJI_VISIT_SHEET_VIOLATION" };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(this.violInfoTable.Name,
                new RefColumn(
                    "VISIT_SHEET_ID",
                    ColumnProperty.NotNull,
                    $"{this.violInfoTable.Name}_VISIT_SHEET",
                    "GJI_VISIT_SHEET",
                    "ID"),
                new RefColumn(
                    "REALITY_OBJECT_ID",
                    ColumnProperty.NotNull,
                    $"{this.violInfoTable.Name}_REALITY_OBJECT",
                    "GKH_REALITY_OBJECT",
                    "ID"),
                new Column("DESCRIPTION", DbType.String.WithSize(2000)));

            this.Database.AddEntityTable(this.violTable.Name,
                new RefColumn(
                    "VIOLATION_INFO_ID",
                    ColumnProperty.NotNull,
                    $"{this.violTable.Name}_VIOLATION_INFO",
                    this.violInfoTable.Name,
                    "ID"),
                new RefColumn("VIOLATION_ID",
                    ColumnProperty.NotNull,
                    $"{this.violTable.Name}_VIOLATION",
                    "GJI_DICT_VIOLATION",
                    "ID"),
                new Column("IS_THREAT_TO_LEGAL_PROTECTED_VALUES", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(this.violTable.Name);
            this.Database.RemoveTable(this.violInfoTable.Name);
        }
    }
}