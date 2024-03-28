namespace Bars.Gkh.RegOperator.Migrations._2023.Version_2023082100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2023082100")]

    [MigrationDependsOn(typeof(_2023.Version_2023080900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("REGOP_BANK_ACC_STMNT", new Column("ROSP", DbType.Boolean, ColumnProperty.NotNull, false));
        }
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "ROSP");
        }      
    }
}
