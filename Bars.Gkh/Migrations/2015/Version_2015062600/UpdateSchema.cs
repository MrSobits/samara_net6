namespace Bars.Gkh.Migration.Version_2015062600
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015062600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015062400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_ENTITY_LOG_LIGHT", "REASON", DbType.String, 500);
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_ENTITY_LOG_LIGHT", "REASON");
        }
    }
}