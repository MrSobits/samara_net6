namespace Bars.GkhGji.Migrations._2024.Version_2024030124
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.B4.Utils;
    using System.Data;

    [Migration("2024030124")]
    [MigrationDependsOn(typeof(Version_2024030123.UpdateSchema))]
    /// Является Version_2022041401 из ядра
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName[] tables = {
            new SchemaQualifiedObjectName(){Name = "GJI_INSPECTION_PROSCLAIM"},
            new SchemaQualifiedObjectName(){Name = "GJI_INSPECTION_DISPHEAD"},
            new SchemaQualifiedObjectName(){Name = "GJI_INSPECTION_STATEMENT"},
        };

        /// <inheritdoc />
        public override void Up()
        {
            this.tables.ForEach(table => this.Database.AddColumn(table, "INN", DbType.String, 12));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.tables.ForEach(table => this.Database.RemoveColumn(table, "INN"));
        }
    }
}