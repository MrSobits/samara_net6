namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2024030503
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030503")]
    [MigrationDependsOn(typeof(Version_2024030502.UpdateSchema))]
    // Является Version_2020100900 из ядра
    public class UpdateSchema : Migration
    {
        private const string TableName = "OVRHL_DPKR_DOCUMENTS";

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.RenameColumn(TableName, "STATE", "STATE_ID");
            this.Database.AddForeignKey("FK_DPKR_DOCUMENTS_STATE_ID", TableName, "STATE_ID", "B4_STATE", "ID", B4.Modules.Ecm7.Framework.ForeignKeyConstraint.SetNull);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RenameColumn(TableName, "STATE_ID", "STATE");
            this.Database.RemoveConstraint(TableName, "FK_DPKR_DOCUMENTS_STATE_ID");
        }
    }
}