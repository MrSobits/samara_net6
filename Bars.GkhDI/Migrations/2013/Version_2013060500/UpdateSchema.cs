namespace Bars.GkhDi.Migrations.Version_2013060500
{
    using global::Bars.B4.Modules.Ecm7.Framework;
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013060500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2013052900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddUniqueConstraint("UNQ_DISINFO_FINACT", "DI_DISINFO_FIN_ACTIVITY", "DISINFO_ID");
            Database.AddUniqueConstraint("UNQ_DISINFO_FIN_DOC", "DI_DISINFO_FINACT_DOCS", "DISINFO_ID");
        }

        public override void Down()
        {
            Database.RemoveConstraint("DI_DISINFO_FIN_ACTIVITY", "UNQ_DISINFO_FINACT");
            Database.RemoveConstraint("DI_DISINFO_FINACT_DOCS", "UNQ_DISINFO_FIN_DOC");
        }
    }
}