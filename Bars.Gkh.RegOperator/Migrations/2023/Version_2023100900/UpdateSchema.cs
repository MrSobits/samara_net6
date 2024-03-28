namespace Bars.Gkh.RegOperator.Migrations._2023.Version_2023100900
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2023100900")]

    [MigrationDependsOn(typeof(Version_2023100500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("REGOP_BANK_ACC_STMNT", new Column("PAYER_FULL", DbType.String));
        }
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "PAYER_FULL");
        }      
    }
}
