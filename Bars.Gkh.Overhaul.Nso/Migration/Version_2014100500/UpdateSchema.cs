namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2014100500
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014100500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2014060600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery("UPDATE cr_obj_type_work SET IS_DPKR_CREATED = true WHERE id IN (SELECT type_work_cr_id FROM ovrhl_type_work_cr_st1)");
            }

            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery("UPDATE cr_obj_type_work SET IS_DPKR_CREATED = 1 WHERE id IN (SELECT type_work_cr_id FROM ovrhl_type_work_cr_st1)");
            }
        }

        public override void Down()
        {
            
        }
    }
}