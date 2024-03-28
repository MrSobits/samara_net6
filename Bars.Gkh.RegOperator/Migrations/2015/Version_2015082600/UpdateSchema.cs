namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015082600
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015082600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015081400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddIndex("ind_pers_acc_owner_type", false, "REGOP_PERS_ACC_OWNER", "OWNER_TYPE");
        }

        public override void Down()
        {
            Database.RemoveIndex("ind_pers_acc_owner_type", "REGOP_PERS_ACC_OWNER");
        }
    }
}
