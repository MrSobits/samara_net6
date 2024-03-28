namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014071500
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014071500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014071100.UpdateSchema))]
    public class UpdateSchema: global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (!Database.ConstraintExists("REGOP_PERS_ACC_CALC_PARAM", "FK_ROP_P_ACC_C_P_LE"))
            {
                //удаление записей параметров,  которые ссылаются на несуществующие записи логов
                Database.ExecuteNonQuery(@"delete from REGOP_PERS_ACC_CALC_PARAM cp where not exists(select id from gkh_entity_log_light ll where ll.id = cp.logged_entity_id)");

                Database.AddForeignKey("FK_ROP_P_ACC_C_P_LE", "REGOP_PERS_ACC_CALC_PARAM", "LOGGED_ENTITY_ID", "GKH_ENTITY_LOG_LIGHT", "ID");
            }
        }

        public override void Down()
        {
        }
    }
}
