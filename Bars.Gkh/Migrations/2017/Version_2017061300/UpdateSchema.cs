namespace Bars.Gkh.Migrations._2017.Version_2017061300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017061300")]
    [MigrationDependsOn(typeof(Version_2017053100.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ChangeColumn("GKH_MUNICIPALITY_FIAS_OKTMO", new Column("OKTMO", DbType.String, 20));
 }

        public override void Down()
        {
            this.Database.ChangeColumn("GKH_MUNICIPALITY_FIAS_OKTMO", new Column("OKTMO", DbType.Int64));
        }
    }
}
