namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2016110300
{
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2016110300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2015101300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Up
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("OVRHL_PRG_VERSION", new RefColumn("PARENT_VERSION_ID","FK_PARENT_PRG_VERSION", "OVRHL_PRG_VERSION","ID"));
        }

        /// <summary>
        /// Down
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("OVRHL_PRG_VERSION", "PARENT_VERSION_ID");
        }
    }
}