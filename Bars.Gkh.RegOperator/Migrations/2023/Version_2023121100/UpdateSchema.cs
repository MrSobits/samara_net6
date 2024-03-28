namespace Bars.Gkh.RegOperator.Migrations._2023.Version_2023121100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2023121100")]

    [MigrationDependsOn(typeof(Version_2023120800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("REGOP_DEBTOR", new Column("EXTRACT_DATE", DbType.DateTime));
        }
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_DEBTOR", "EXTRACT_DATE");
        }      
    }
}
