namespace Bars.Gkh.Migrations.Version_2013122602
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013122602")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013122601.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("GKH_DICT_ZONAINSP", "INDEX_OF_GJI");
            Database.AddColumn("GKH_DICT_ZONAINSP", new Column("INDEX_OF_GJI", DbType.String, 300));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_ZONAINSP", "INDEX_OF_GJI");
            Database.AddColumn("GKH_DICT_ZONAINSP", new Column("INDEX_OF_GJI", DbType.Int32));
        }
    }
}