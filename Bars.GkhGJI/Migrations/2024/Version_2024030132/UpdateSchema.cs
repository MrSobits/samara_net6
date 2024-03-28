namespace Bars.GkhGji.Migrations._2024.Version_2024030132
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030132")]
    [MigrationDependsOn(typeof(Version_2024030131.UpdateSchema))]
    /// Является Version_2022090900 из ядра
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName table = new SchemaQualifiedObjectName()
        {
            Name = "GJI_DICT_VIOLATION"
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