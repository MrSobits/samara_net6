namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013101403
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013101403")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013101402.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Database.RemoveForeignKey("OVRHL_REAL_ACCOUNT", "FK_OVRHL_REAL_ACC_REG_OP");

            //Database.AddForeignKey("FK_OVRHL_REAL_ACC_REG_OP", "OVRHL_REAL_ACCOUNT", "REG_OPER_ID", "GKH_CONTRAGENT", "ID");
        }

        public override void Down()
        {
            //Database.RemoveForeignKey("OVRHL_REAL_ACCOUNT", "FK_OVRHL_REAL_ACC_REG_OP");

            //Database.AddForeignKey("FK_OVRHL_REAL_ACC_REG_OP", "OVRHL_REAL_ACCOUNT", "REG_OPER_ID", "OVRHL_REG_OPERATOR", "ID");
        }
    }
}