namespace Bars.Gkh.Migrations._2023.Version_2023050145
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050145")]

    [MigrationDependsOn(typeof(Version_2023050144.UpdateSchema))]

    /// Является Version_2022021501 из ядра
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName table = new SchemaQualifiedObjectName() { Name = "B4_FIAS_ADDRESS" };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.ChangeColumn(this.table, new Column("FLAT", DbType.String.WithSize(50)));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.ChangeColumn(this.table, new Column("FLAT", DbType.String.WithSize(10)));
        }
    }
}