namespace Bars.GkhCr.Migrations.Version_2013042500
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013042500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2013041200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveIndex("IND_CR_OBJ_EST_CALC_RES", "CR_OBJ_ESTIMATE_CALC");
            Database.AddIndex("IND_CR_OBJ_EST_CALC_RES_NAME", false, "CR_OBJ_ESTIMATE_CALC", "RES_STAT_DOC_NAME");
            Database.AddIndex("IND_CR_OBJ_EST_CALC_RES_ID", false, "CR_OBJ_ESTIMATE_CALC", "FILE_RES_STATMENT_ID");

            Database.RemoveIndex("IND_CR_OBJ_EST_CALC_EST", "CR_OBJ_ESTIMATE_CALC");
            Database.AddIndex("IND_CR_OBJ_EST_CALC_EST_NAME", false, "CR_OBJ_ESTIMATE_CALC", "ESTIMATE_DOC_NAME");
            Database.AddIndex("IND_CR_OBJ_EST_CALC_EST_ID", false, "CR_OBJ_ESTIMATE_CALC", "FILE_ESTIMATE_ID");

            Database.RemoveIndex("IND_CR_OBJ_EST_CALC_EF", "CR_OBJ_ESTIMATE_CALC");
            Database.AddIndex("IND_CR_OBJ_EST_CALC_EF_NAME", false, "CR_OBJ_ESTIMATE_CALC", "ESTIMATE_FILE_DOC_NAME");
            Database.AddIndex("IND_CR_OBJ_EST_CALC_EF_ID", false, "CR_OBJ_ESTIMATE_CALC", "FILE_ESTIMATE_FILE_ID");
        }

        public override void Down()
        {
        }
    }
}