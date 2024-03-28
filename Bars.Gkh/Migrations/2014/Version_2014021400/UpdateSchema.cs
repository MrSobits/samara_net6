using System.Data;

namespace Bars.Gkh.Migrations.Version_2014021400
{
    using global::Bars.B4.Modules.Ecm7.Framework;
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014021300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("GKH_DICT_INSPECTOR", new Column("POSITION", DbType.String, 300));
        }

        public override void Down()
        {
            Database.ChangeColumn("GKH_DICT_INSPECTOR", new Column("POSITION", DbType.String, 100));
        }
    }
}