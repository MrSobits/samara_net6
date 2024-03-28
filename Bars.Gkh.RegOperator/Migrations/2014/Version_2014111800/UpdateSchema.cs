namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014111800
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014111800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014111000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ExecuteNonQuery("update regop_pers_acc set persacc_serv_type = 10 where persacc_serv_type = 0");
        }

        public override void Down()
        {
        }
    }
}
