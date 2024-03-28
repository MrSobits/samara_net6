namespace Bars.Gkh.Migrations._2023.Version_2023050151
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050151")]

    [MigrationDependsOn(typeof(Version_2023050150.UpdateSchema))]

    /// Является Version_2022110300 из ядра
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName table = new SchemaQualifiedObjectName() { Name = "GKH_CONTRAGENT" };
        private readonly Column column = new Column("INCLUDE_IN_SOPR", DbType.Boolean, false);

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn(this.table, this.column);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(this.table, this.column.Name);
        }
    }
}