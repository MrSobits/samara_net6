namespace Bars.GkhGji.Migrations._2024.Version_2024030129
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using System.Data;

    [Migration("2024030129")]
    [MigrationDependsOn(typeof(Version_2024030128.UpdateSchema))]
    /// Является Version_2022060600 из ядра
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName[] annexTables =
        {
            new SchemaQualifiedObjectName { Name = "GJI_ACTCHECK_ANNEX" },
            new SchemaQualifiedObjectName { Name = "GJI_PRESCRIPTION_ANNEX" },
            new SchemaQualifiedObjectName { Name = "GJI_PROTOCOL_ANNEX" }
        };

        private Column sendFileToErknmColumn = new Column("SEND_FILE_TO_ERKNM", DbType.Int32, ColumnProperty.NotNull, (int)YesNoNotSet.NotSet);

        /// <inheritdoc />
        public override void Up()
        {
            this.annexTables.ForEach(table => this.Database.AddColumn(table, this.sendFileToErknmColumn));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.annexTables.ForEach(table => this.Database.RemoveColumn(table, this.sendFileToErknmColumn.Name));
        }
    }
}