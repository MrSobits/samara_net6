namespace Bars.GkhGji.Migrations._2024.Version_2024030131
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030131")]
    [MigrationDependsOn(typeof(Version_2024030130.UpdateSchema))]
    /// Является Version_2022080300 из ядра
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName table = new SchemaQualifiedObjectName()
        {
            Name = "GJI_DICT_EXECUTANT"
        };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn(this.table, new Column("ERKNM_CODE", DbType.String.WithSize(10)));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(this.table, "ERKNM_CODE");
        }
    }
}