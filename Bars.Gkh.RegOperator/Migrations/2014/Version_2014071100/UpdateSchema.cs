namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014071100
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014071100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014071001.UpdateSchema))]
    public class UpdateSchema: global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddIndex("IND_BPA_ACC_NUM", false, "REGOP_PERS_ACC", new[] { "ACC_NUM" });
        }

        public override void Down()
        {
            Database.RemoveIndex("IND_BPA_ACC_NUM", "REGOP_PERS_ACC");
        }
    }
}
