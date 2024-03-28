using System.Data;

namespace Bars.Gkh.Migrations._2016.Version_2016060600
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [Migration("2016060600")]
    [MigrationDependsOn(typeof(Version_2016050500.UpdateSchema))]

    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.ChangeColumn("GKH_MANORG_LIC_REQUEST", new Column("REASON_OFFERS", DbType.String, 10000));
        }

        public override void Down()
        {
            this.Database.ChangeColumn("GKH_MANORG_LIC_REQUEST", new Column("REASON_OFFERS", DbType.String, 1000));
        }
    }
}
