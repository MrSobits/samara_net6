namespace Bars.Gkh.Migrations._2017.Version_2017052500
{
    using System.Data;

    using B4.Modules.Ecm7.Framework;

    [Migration("2017052500")]
    [MigrationDependsOn(typeof(Version_2017041700.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ChangeColumn("GKH_DICT_MUNICIPALITY", new Column("OKTMO", DbType.String, 30));
 }

        public override void Down()
        {
            this.Database.ChangeColumn("GKH_DICT_MUNICIPALITY", new Column("OKTMO", DbType.Int64));
        }
    }
}
