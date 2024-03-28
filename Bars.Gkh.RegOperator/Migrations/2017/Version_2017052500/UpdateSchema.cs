namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017052500
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017052500")]
    [MigrationDependsOn(typeof(Version_2017052400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            if (this.Database.ColumnExists("REGOP_TRANSFER", "OWNER_ID"))
            {
                this.Database.RemoveColumn("REGOP_WALLET", "OWNER_ID");
            }
        }

        /// <inheritdoc/>
        public override void Down()
        {
        }
    }
}