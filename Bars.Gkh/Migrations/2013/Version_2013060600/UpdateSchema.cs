namespace Bars.Gkh.Migrations.Version_2013060600
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013060600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013052100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_ZONAINSP", new Column("BLANK_NAME", DbType.String, 300));
            Database.AddColumn("GKH_DICT_ZONAINSP", new Column("BLANK_NAME_SECOND", DbType.String, 300));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_ZONAINSP", "BLANK_NAME");
            Database.RemoveColumn("GKH_DICT_ZONAINSP", "BLANK_NAME_SECOND");
        }
    }
}