namespace Bars.GkhGji.Migrations._2024.Version_2024030134
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030134")]
    [MigrationDependsOn(typeof(Version_2024030133.UpdateSchema))]
    /// Является Version_2022100400 из ядра
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName table = new SchemaQualifiedObjectName
        {
            Schema = "public",
            Name = "GJI_DICT_STATEMENT_SUBJ"
        };

        private readonly Column newColumn = new Column("NEED_IN_SOPR", DbType.Boolean, ColumnProperty.NotNull, false);

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