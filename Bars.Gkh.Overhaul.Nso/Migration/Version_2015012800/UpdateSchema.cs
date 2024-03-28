namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2015012800
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015012800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2014121900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_PUBLISH_PRG", new Column("PUBLISH_DATE", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_PUBLISH_PRG", "PUBLISH_DATE");
        }
    }
}