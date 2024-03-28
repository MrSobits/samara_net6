namespace Bars.GkhGji.Migrations._2024.Version_2024030121
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030121")]
    [MigrationDependsOn(typeof(Version_2024030120.UpdateSchema))]
    /// Является Version_2022040100 из ядра
    public class UpdateSchema : Migration
    {
        private string DocumentGjiTable => "GJI_DOCUMENT";

        private Column ErknmGuidColumn => new Column("ERKNM_GUID", DbType.String.WithSize(36));

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn(this.DocumentGjiTable, this.ErknmGuidColumn);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(this.DocumentGjiTable, this.ErknmGuidColumn.Name);
        }
    }
}