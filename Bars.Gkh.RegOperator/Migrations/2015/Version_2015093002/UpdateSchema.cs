namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015093002
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Gkh.Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015093002")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015093001.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AlterColumnSetNullable("REGOP_INDIVIDUAL_ACC_OWN", "ID_NUM", true);

            Database.AlterColumnSetNullable("REGOP_INDIVIDUAL_ACC_OWN", "ID_SERIAL", true);
        }

        public override void Down()
        {
            Database.ExecuteNonQuery("UPDATE REGOP_INDIVIDUAL_ACC_OWN SET ID_NUM = '' WHERE ID_NUM is NULL");
            Database.AlterColumnSetNullable("REGOP_INDIVIDUAL_ACC_OWN", "ID_NUM", false);

            Database.ExecuteNonQuery("UPDATE REGOP_INDIVIDUAL_ACC_OWN SET ID_SERIAL = '' WHERE ID_SERIAL is NULL");
            Database.AlterColumnSetNullable("REGOP_INDIVIDUAL_ACC_OWN", "ID_SERIAL", false);
        }
    }
}
