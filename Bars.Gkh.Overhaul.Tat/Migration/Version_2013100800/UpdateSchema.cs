namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013100800
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013100800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013100701.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddIndex("IND_PRG_ST2", false, "OVRHL_RO_STRUCT_EL_IN_PRG", "STAGE2_ID");
            Database.AddForeignKey("FK_PRG_ST2", "OVRHL_RO_STRUCT_EL_IN_PRG", "STAGE2_ID", "OVRHL_RO_STRUCT_EL_IN_PRG_2", "ID");
        }

        public override void Down()
        {
            Database.RemoveConstraint("OVRHL_RO_STRUCT_EL_IN_PRG", "FK_PRG_ST2");
            Database.RemoveIndex("IND_PRG_ST2", "OVRHL_RO_STRUCT_EL_IN_PRG_2");
        }
    }
}