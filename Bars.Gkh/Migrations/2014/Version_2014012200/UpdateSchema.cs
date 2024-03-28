namespace Bars.Gkh.Migrations.Version_2014012200
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014012200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_201401600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_ZONAINSP", new Column("OKTMO", DbType.String, 30));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_ZONAINSP", "OKTMO");
        }
    }
}