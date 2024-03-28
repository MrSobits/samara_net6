namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2014041700
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014041700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2014040900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("OVRHL_PUBLISH_PRG_REC", new Column("COMMON_ESTATE_OBJECT", DbType.String, 2000));
        }

        public override void Down()
        {

        }
    }
}