namespace Bars.GkhCr.Migrations.Version_2013100800
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013100800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2013100300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddIndex("IND_CR_QUAL_MEM_ROLE", false, "CR_DICT_QUAL_MEMBER", "ROLE_ID");
            Database.AddForeignKey("FK_CR_QUAL_MEM_ROLE", "CR_DICT_QUAL_MEMBER", "ROLE_ID", "B4_ROLE", "ID");
        }

        public override void Down()
        {
            Database.RemoveConstraint("CR_DICT_QUAL_MEMBER", "FK_CR_QUAL_MEM_ROLE");
            Database.RemoveIndex("IND_CR_QUAL_MEM_ROLE", "CR_DICT_QUAL_MEMBER");
        }
    }
}