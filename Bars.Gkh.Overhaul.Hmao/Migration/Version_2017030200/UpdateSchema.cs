namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2017030200
{
    /// <summary>
    /// Миграция 2017030200
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2017030200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2016110300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Up
        /// </summary>
        public override void Up()
        {
            this.Database.AddForeignKey("FK_OVRHL_PRG_VERSION_PAPENT_ID", "OVRHL_PRG_VERSION", "PARENT_VERSION_ID", "OVRHL_PRG_VERSION", "ID");
        }

        /// <summary>
        /// Down
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveConstraint("OVRHL_PRG_VERSION", "FK_OVRHL_PRG_VERSION_PAPENT_ID");
        }
    }
}