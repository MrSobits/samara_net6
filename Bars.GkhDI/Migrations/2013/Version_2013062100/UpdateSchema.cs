namespace Bars.GkhDi.Migrations.Version_2013062100
{
    using global::Bars.B4.Modules.Ecm7.Framework;
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013062100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2013060600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddUniqueConstraint("UNQ_DISINFO_DI_DOCS", "DI_DISINFO_DOCUMENTS", "DISINFO_ID");
            Database.AddUniqueConstraint("UNQ_DISINFO_DI_DOCRO", "DI_DISINFO_DOC_RO", "DISINFO_RO_ID");
        }

        public override void Down()
        {
            Database.RemoveConstraint("DI_DISINFO_DOCUMENTS", "UNQ_DISINFO_DI_DOCS");
            Database.RemoveConstraint("DI_DISINFO_DOC_RO", "UNQ_DISINFO_DI_DOCRO");
        }
    }
}