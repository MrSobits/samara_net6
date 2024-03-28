namespace Bars.GkhGji.Migrations._2022.Version_2022060800
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Utils;

    [Migration("2022060800")]
    [MigrationDependsOn(typeof(Version_2022053100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName[] annexTables =
        {
            new SchemaQualifiedObjectName { Name = "GJI_PRESCRIPTION_ANNEX" },
            new SchemaQualifiedObjectName { Name = "GJI_PROTOCOL_ANNEX" }
        };

        private Column column = new Column("ERKNM_GUID", DbType.String.WithSize(36));

        /// <inheritdoc />
        public override void Up()
        {
            this.annexTables.ForEach(table => this.Database.AddColumn(table, this.column));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.annexTables.ForEach(table => this.Database.RemoveColumn(table, this.column.Name));
        }
    }
}