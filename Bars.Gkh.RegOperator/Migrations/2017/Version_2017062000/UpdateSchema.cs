namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017062000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция RegOperator 2017062000
    /// </summary>
    [Migration("2017062000")]
    [MigrationDependsOn(typeof(Version_2017060600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("CLW_DEBTOR_CLAIM_WORK", new Column("USER_ID", DbType.Int64));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("CLW_DEBTOR_CLAIM_WORK", "USER_ID");
        }
    }
}