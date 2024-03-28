namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022101100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Utils;

    [Migration("2022101100")]
    [MigrationDependsOn(typeof(Version_2022100600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly string oldTableName = "DECISION_KNM_ACTION";
        private readonly string newTableName = "GJI_DECISION_KNM_ACTION";

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.RenameTableAndSequence(this.oldTableName, this.newTableName);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RenameTableAndSequence(this.newTableName, this.oldTableName);
        }
    }
}