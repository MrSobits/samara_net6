namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2014051200
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014051200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2014050500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ExecuteNonQuery("UPDATE OVRHL_PRG_VERSION SET ACTUALIZE_DATE = OBJECT_CREATE_DATE");
        }

        public override void Down()
        {
            
        }
    }
}