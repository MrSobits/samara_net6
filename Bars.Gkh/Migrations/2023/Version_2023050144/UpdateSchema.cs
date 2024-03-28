namespace Bars.Gkh.Migrations._2023.Version_2023050144
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050144")]

    [MigrationDependsOn(typeof(Version_2023050143.UpdateSchema))]

    /// Является Version_2022021500 из ядра
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName table = new SchemaQualifiedObjectName() { Name = "GKH_DICT_MUNICIPALITY" };
        private Column column = new Column("CODE_GJI", DbType.String.WithSize(10));

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