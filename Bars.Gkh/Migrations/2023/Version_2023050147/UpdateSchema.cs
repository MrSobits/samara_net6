namespace Bars.Gkh.Migrations._2023.Version_2023050147
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050147")]

    [MigrationDependsOn(typeof(Version_2023050146.UpdateSchema))]

    /// Является Version_2022040800 из ядра
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName table = new SchemaQualifiedObjectName() { Name = "GKH_DICT_INSPECTOR" };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddRefColumn
                (
                    this.table.Name,
                    new RefColumn("INSPECTOR_POSITION", $"{this.table.Name}_POSITION", "GJI_DICT_INSPECTOR_POSITIONS", "ID")
                );
            this.Database.AddColumn(this.table, new Column("IS_ACTIVE", DbType.Boolean, false));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(this.table, "INSPECTOR_POSITION");
            this.Database.RemoveColumn(this.table, "IS_ACTIVE");
        }
    }
}