namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014091000
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014091000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014082800.UpdateSchema))]
    public class UpdateSchema: global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveIndex("IND_BPA_ACC_NUM", "REGOP_PERS_ACC");
            Database.AddIndex("IND_BPA_ACC_NUM", true, "REGOP_PERS_ACC", new[] { "ACC_NUM" });
        }

        public override void Down()
        {
            Database.RemoveIndex("IND_BPA_ACC_NUM", "REGOP_PERS_ACC");
            Database.AddIndex("IND_BPA_ACC_NUM", false, "REGOP_PERS_ACC", new[] { "ACC_NUM" });
        }
    }
}
