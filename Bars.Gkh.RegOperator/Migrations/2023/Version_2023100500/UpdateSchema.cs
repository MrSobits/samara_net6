namespace Bars.Gkh.RegOperator.Migrations._2023.Version_2023100500
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2023100500")]

    [MigrationDependsOn(typeof(Version_2023100200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("EMAIL_NEWSLETTER", new Column("SEND_DATE", DbType.DateTime));
        }
        public override void Down()
        {
            this.Database.RemoveColumn("EMAIL_NEWSLETTER", "SEND_DATE");
        }      
    }
}
