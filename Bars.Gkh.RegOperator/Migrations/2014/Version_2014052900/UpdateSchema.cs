namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014052900
{
    [B4.Modules.Ecm7.Framework.Migration("2014052900")]
    [B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2014052700.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddForeignKey("FK_LEGAL_ACC_CONTR", "REGOP_LEGAL_ACC_OWN", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            Database.AddIndex("IND_LEGAL_ACC_CONTR", false, "REGOP_LEGAL_ACC_OWN", "CONTRAGENT_ID");
        }

        public override void Down()
        {
            Database.RemoveConstraint("REGOP_LEGAL_ACC_OWN", "FK_LEGAL_ACC_CONTR");
            Database.RemoveIndex("IND_LEGAL_ACC_CONTR", "REGOP_LEGAL_ACC_OWN");
        }
    }
}
