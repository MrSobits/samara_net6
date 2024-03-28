namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017101000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017101000")]
    [MigrationDependsOn(typeof(Version_2017082100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("REGOP_PAYMENT_DOC_SNAPSHOT", new Column("SENDING_EMAIL_STATE", DbType.Int16, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_PAYMENT_DOC_SNAPSHOT", "SENDING_EMAIL_STATE");
        }
    }
}