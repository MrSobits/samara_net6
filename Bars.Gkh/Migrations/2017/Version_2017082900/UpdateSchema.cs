namespace Bars.Gkh.Migrations._2017.Version_2017082900
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017082900")]
    [MigrationDependsOn(typeof(Version_2017082400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ChangeColumn("GKH_CONTRAGENT", new Column("OKOGU", DbType.String));
            this.Database.ChangeColumn("GKH_CONTRAGENT", new Column("OKFS", DbType.String));
        }

        public override void Down()
        {
            //не требуется
        }
    }
}