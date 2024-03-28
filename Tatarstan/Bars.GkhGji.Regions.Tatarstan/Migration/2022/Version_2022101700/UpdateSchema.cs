namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022101700
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Utils;

    [Migration("2022101700")]
    [MigrationDependsOn(typeof(Version_2022101100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly string oldTableName = "GJI_TASK_ACTIONISOLATED_PLANNED_ACTION";
        private readonly string newTableName = "GJI_TASK_ACTIONISOLATED_KNM_ACTION";

        private readonly string oldColumnName = "PLANNED_ACTION_ID";
        private readonly string newColumnName = "KNM_ACTION_ID";

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.RenameTableAndSequence(this.oldTableName, this.newTableName);
            this.Database.RenameColumn(this.newTableName, this.oldColumnName, this.newColumnName);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RenameColumn(this.newTableName, this.newColumnName, this.oldColumnName);
            this.Database.RenameTableAndSequence(this.newTableName, this.oldTableName);
        }
    }
}