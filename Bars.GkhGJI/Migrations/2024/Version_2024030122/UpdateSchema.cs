namespace Bars.GkhGji.Migrations._2024.Version_2024030122
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030122")]
    [MigrationDependsOn(typeof(Version_2024030121.UpdateSchema))]
    /// Является Version_2022040101 из ядра
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName table = new SchemaQualifiedObjectName
        {
            Name = "GJI_DICT_INSPECTION_BASE_TYPE"
        };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn(this.table, new Column("CODE_ERKNM", DbType.String.WithSize(10)));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(this.table, "CODE_ERKNM");
        }
    }
}