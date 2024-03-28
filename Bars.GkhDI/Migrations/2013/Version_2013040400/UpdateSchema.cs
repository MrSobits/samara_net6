namespace Bars.GkhDi.Migrations.Version_2013040400
{
    using global::Bars.B4.Modules.Ecm7.Framework;
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013040400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2013040300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddIndex("IND_DI_DICT_TEMPL_CODE", false, "DI_DICT_TEMPL_SERVICE", "CODE");
            Database.AddIndex("IND_DI_PERC_CALC_CODE", false, "DI_PERC_CALC", "CODE");
            Database.AddIndex("IND_DI_PERC_CALC_DATE", false, "DI_PERC_CALC", "CALC_DATE");
        }

        public override void Down()
        {
            Database.RemoveIndex("IND_DI_PERC_CALC_DATE", "DI_PERC_CALC");
            Database.RemoveIndex("IND_DI_PERC_CALC_CODE", "DI_PERC_CALC");
            Database.RemoveIndex("IND_DI_DICT_TEMPL_CODE", "DI_DICT_TEMPL_SERVICE");
        }
    }
}