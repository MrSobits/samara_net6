namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017062100
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017062100")]
    [MigrationDependsOn(typeof(_2016.Version_2016061600.UpdateSchema))]
    [MigrationDependsOn(typeof(_2017.Version_2017042200.UpdateSchema))]
    [MigrationDependsOn(typeof(_2017.Version_2017042201.UpdateSchema))]
    [MigrationDependsOn(typeof(_2017.Version_2017052200.UpdateSchema))]
    [MigrationDependsOn(typeof(_2017.Version_2017060600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_MONEY_OPERATION", new Column("USER_LOGIN", DbType.String, 100));
            this.Database.AddColumn("REGOP_DISTR_DETAIL", new Column("USER_LOGIN", DbType.String, 100));
        }

        /// <inheritdoc/>
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_MONEY_OPERATION", "USER_LOGIN");
            this.Database.RemoveColumn("REGOP_DISTR_DETAIL", "USER_LOGIN");
        }
    }
}