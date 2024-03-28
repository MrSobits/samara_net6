namespace Bars.GkhDi.Migrations.Version_2013062700
{
    using global::Bars.B4.Modules.Ecm7.Framework;
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013062700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2013062100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddUniqueConstraint("UNQ_DISINFO_DI_FA_RO", "DI_DISINFO_FINACT_REALOBJ", "DISINFO_ID", "REALITY_OBJ_ID");
            // Database.AddIndex("IND_TEMPL_SERV_CODE", false, "DI_DICT_TEMPL_SERVICE", "CODE");
        }

        public override void Down()
        {
            Database.RemoveConstraint("DI_DISINFO_FINACT_REALOBJ", "UNQ_DISINFO_DI_FA_RO");
            Database.RemoveIndex("IND_TEMPL_SERV_CODE", "DI_DICT_TEMPL_SERVICE");
        }
    }
}