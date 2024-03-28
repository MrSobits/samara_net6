namespace Bars.Gkh.Migrations.Version_2013120900
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013120900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013120600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_CAPITAL_GROUP", new Column("CODE", DbType.String, 300));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_CAPITAL_GROUP", "CODE");
        }
    }
}