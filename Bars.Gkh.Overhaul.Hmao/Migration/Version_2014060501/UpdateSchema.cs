namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2014060501
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014060501")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2014060500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_VERSION_REC", new Column("CHANGES", DbType.String, 50));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_VERSION_REC", "CHANGES");
        }
    }
}