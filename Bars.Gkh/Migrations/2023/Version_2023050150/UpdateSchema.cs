namespace Bars.Gkh.Migrations._2023.Version_2023050150
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050150")]

    [MigrationDependsOn(typeof(Version_2023050149.UpdateSchema))]

    /// Является Version_2022090600 из ядра
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName table = new SchemaQualifiedObjectName()
        {
            Name = "GJI_DICT_FEATUREVIOL"
        };

        private readonly Column newColumn = new Column("IS_ACTUAL", DbType.Boolean, ColumnProperty.None, true);

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn(this.table, this.newColumn);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(this.table, this.newColumn.Name);
        }
    }
}