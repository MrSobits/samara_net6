namespace Bars.Gkh.Migrations.Version_2013100801
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013100801")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013100800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddIndex("IND_GKH_CONST_EL_GROUP", false, "GKH_DICT_CONST_ELEMENT", "GROUP_ID");
            Database.AddForeignKey("FK_GKH_CONST_EL_GROUP", "GKH_DICT_CONST_ELEMENT", "GROUP_ID", "GKH_DICT_CONEL_GROUP", "ID");

            Database.AddIndex("IND_OPER_CONTRAGENT", false, "GKH_OPERATOR", "CONTRAGENT_ID");
            Database.AddForeignKey("FK_OPER_CONTRAGENT", "GKH_OPERATOR", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
        }

        public override void Down()
        {
            Database.RemoveConstraint("GKH_CONTRAGENT", "FK_OPER_CONTRAGENT");
            Database.RemoveConstraint("GKH_DICT_CONST_ELEMENT", "FK_GKH_CONST_EL_GROUP");

            Database.RemoveIndex("IND_OPER_CONTRAGENT", "GKH_OPERATOR");
            Database.RemoveIndex("IND_GKH_CONST_EL_GROUP", "GKH_DICT_CONST_ELEMENT");
        }
    }
}