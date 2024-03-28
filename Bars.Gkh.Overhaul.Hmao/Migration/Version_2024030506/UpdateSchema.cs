namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2024030506
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030506")]
    [MigrationDependsOn(typeof(Version_2024030505.UpdateSchema))]
    // Является Version_2022103100 из ядра
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName table = new SchemaQualifiedObjectName { Name = "OVRHL_ACTUALIZE_LOG" };
        private readonly string columnName = "FILE_ID";

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.ChangeColumnNotNullable(table, columnName, false);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.ChangeColumnNotNullable(table, columnName, true);
        }
    }
}