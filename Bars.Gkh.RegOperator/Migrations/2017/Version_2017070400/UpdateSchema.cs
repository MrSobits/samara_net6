namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017070400
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017070400")]
    [MigrationDependsOn(typeof(Version_2017062900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            var baseTableName = new SchemaQualifiedObjectName { Name = "CHES_MATCH_ACC_OWNER", Schema = "IMPORT" };

            this.Database.RemoveConstraint(baseTableName, "FK_MATCH_ACC_OWNER_OWNER_ID");

            this.Database.AddForeignKey("FK_MATCH_ACC_OWNER_OWNER_ID", baseTableName, "OWNER_ID", "REGOP_PERS_ACC_OWNER", "ID");
        }

        /// <inheritdoc />
        public override void Down()
        {
        }
    }
}