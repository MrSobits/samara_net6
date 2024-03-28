namespace Bars.GkhGji.Migrations._2024.Version_2024030125
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.B4.Utils;
    using System.Data;

    [Migration("2024030125")]
    [MigrationDependsOn(typeof(Version_2024030124.UpdateSchema))]
    /// Является Version_2022041500 из ядра
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName table = new SchemaQualifiedObjectName { Name = "GJI_DICT_INSPECTION_BASE_TYPE" };

        private Column[] columns =
        {
            new Column("HAS_TEXT_FIELD", DbType.Boolean, ColumnProperty.NotNull, false),
            new Column("HAS_DATE", DbType.Boolean, ColumnProperty.NotNull, false),
            new Column("HAS_RISK_INDICATOR", DbType.Boolean, ColumnProperty.NotNull, false)
        };

        /// <inheritdoc />
        public override void Up()
        {
            this.columns.ForEach(column => this.Database.AddColumn(this.table, column));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.columns.ForEach(column => this.Database.RemoveColumn(this.table, column.Name));
        }
    }
}