namespace Bars.Gkh.Migrations._2017.Version_2017070600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.FormatDataExport.Domain.Impl;

    [Migration("2017070600")]
    [MigrationDependsOn(typeof(Version_2017050800.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017052000.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017053100.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017062300.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017070300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private string tableName = FormatDataExportIncrementalService.TableName;

        public override void Up()
        {
            this.Database.AddTable(this.tableName,
                new Column("ENTITY_CODE", DbType.String, 50, ColumnProperty.NotNull),
                new Column("EXPORT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("ROW_ID", DbType.Int64, ColumnProperty.NotNull),
                new Column("HASH_SUM", DbType.Binary, ColumnProperty.NotNull));

            this.AddIndex("ENTITY_CODE");
            this.AddIndex("ROW_ID");
        }

        public override void Down()
        {
            this.Database.RemoveTable(this.tableName);
        }

        private void AddIndex(string columnName)
        {
            this.Database.AddIndex($"{columnName}_{this.tableName}_IDX", false, this.tableName, columnName);
        }
    }
}