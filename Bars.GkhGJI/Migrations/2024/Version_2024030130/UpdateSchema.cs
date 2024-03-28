namespace Bars.GkhGji.Migrations._2024.Version_2024030130
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030130")]
    [MigrationDependsOn(typeof(Version_2024030129.UpdateSchema))]
    /// Является Version_2022060601 из ядра
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName actCheckAnnexTable = new SchemaQualifiedObjectName { Name = "GJI_ACTCHECK_ANNEX" };

        private Column column = new Column("ERKNM_GUID", DbType.String.WithSize(36));

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn(this.actCheckAnnexTable, this.column);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(this.actCheckAnnexTable, this.column.Name);
        }
    }
}