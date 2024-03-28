namespace Bars.GkhGji.Migrations._2024.Version_2024030135
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030135")]
    [MigrationDependsOn(typeof(Version_2024030134.UpdateSchema))]
    /// Является Version_2022110800 из ядра
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName table = new SchemaQualifiedObjectName
        {
            Schema = "public",
            Name = "GJI_APPEAL_CITIZENS"
        };

        private readonly Column newColumn = new Column("IS_IDENTITY_VERIFIED", DbType.Boolean, ColumnProperty.None, false);

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