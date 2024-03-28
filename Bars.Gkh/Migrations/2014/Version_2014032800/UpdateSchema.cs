namespace Bars.Gkh.Migrations.Version_2014032800
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014032800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014032601.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddIndex("IND_EN_LOG_DATES", false, "GKH_ENTITY_LOG_LIGHT", new[] { "CDATE_APPLIED", "DATE_ACTUAL" });
        }

        public override void Down()
        {
            Database.RemoveIndex("IND_EN_LOG_DATES", "GKH_ENTITY_LOG_LIGHT");
        }
    }
}