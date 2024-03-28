namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017082100
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017082100")]
    [MigrationDependsOn(typeof(Version_2017062000.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017062100.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017062700.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017081000.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017081400.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017080300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.ExecuteQuery(this.query);
        }

        private string query = @"
			update CLW_DEBTOR_CLAIM_WORK
            set DEBTOR_TYPE = 2
            where DEBTOR_TYPE=1;
			
			update CLW_DEBTOR_CLAIM_WORK
            set DEBTOR_TYPE = 1
            where DEBTOR_TYPE=0;";
    }
}