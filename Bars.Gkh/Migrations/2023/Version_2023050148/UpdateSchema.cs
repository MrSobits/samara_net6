namespace Bars.Gkh.Migrations._2023.Version_2023050148
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050148")]

    [MigrationDependsOn(typeof(Version_2023050147.UpdateSchema))]

    /// Является Version_2022071900 из ядра
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName table = new SchemaQualifiedObjectName() { Name = "GKH_OPERATOR" };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn(this.table, new Column("MOBILE_APP_ACCESS_ENABLED", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(this.table, "MOBILE_APP_ACCESS_ENABLED");
        }
    }
}