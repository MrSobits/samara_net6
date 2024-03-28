namespace Bars.Gkh.Migrations.Version_2013041800
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013041800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013041001.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_INSTRUCTIONS", new Column("CODE", DbType.String, 300));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_INSTRUCTIONS", "CODE");
        }
    }
}