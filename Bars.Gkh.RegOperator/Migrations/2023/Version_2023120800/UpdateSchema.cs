namespace Bars.Gkh.RegOperator.Migrations._2023.Version_2023120800
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2023120800")]

    [MigrationDependsOn(typeof(Version_2023101800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("REGOP_PAYMENT_DOC_SNAPSHOT", new Column("OWNER_INN", DbType.String, 20));
        }
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_PAYMENT_DOC_SNAPSHOT", "OWNER_INN");
        }      
    }
}
