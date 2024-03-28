namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2014100500
{
    
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014100500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2014100100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ExecuteNonQuery("UPDATE cr_obj_type_work SET IS_DPKR_CREATED = true WHERE id IN (SELECT type_work_cr_id FROM ovrhl_type_work_cr_st1)");
        }

        public override void Down()
        {
            
        }
    }
}