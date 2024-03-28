namespace Bars.Gkh.Migrations._2015.Version_2015092900
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015092900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015092800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("GKH_DICT_BUILDING_FEATURE", new Column("NAME", DbType.String, 1000));
        }

        public override void Down()
        {

        }
    }
}