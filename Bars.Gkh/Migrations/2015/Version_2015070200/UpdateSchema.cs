namespace Bars.Gkh.Migrations._2015.Version_2015070200
{
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015070200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migration.Version_2015062600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ExecuteNonQuery(" UPDATE GKH_DICT_WORK_CUR_REPAIR SET NAME=DEFAULT WHERE NAME IS NULL ");
            Database.ExecuteNonQuery(" UPDATE GKH_DICT_WORK_CUR_REPAIR SET CODE=DEFAULT WHERE CODE IS NULL ");

            Database.AlterColumnSetNullable("GKH_DICT_WORK_CUR_REPAIR", "NAME", false);

            Database.AlterColumnSetNullable("GKH_DICT_WORK_CUR_REPAIR", "CODE", false);

        }

        public override void Down()
        {
            Database.AlterColumnSetNullable("GKH_DICT_WORK_CUR_REPAIR", "NAME", true);

            Database.AlterColumnSetNullable("GKH_DICT_WORK_CUR_REPAIR", "CODE", true);

        }
    }
}
