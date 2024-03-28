namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013101001
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013101001")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013101000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddUniqueConstraint("UNQ_LONGTERM_PR_REALITY_OBJECT", "OVRHL_LONGTERM_PR_OBJECT", "REALITY_OBJ_ID");
        }

        public override void Down()
        {
            Database.RemoveConstraint("OVRHL_LONGTERM_PR_OBJECT", "UNQ_LONGTERM_PR_REALITY_OBJECT");
        }
    }
}