namespace Bars.Gkh.Migrations.Version_2014020401
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014020401")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014020400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_REALITY_OBJECT", new Column("STL_MUNICIPALITY_ID", DbType.Int64));
            Database.AddIndex("IND_GKH_RO_STL", false, "GKH_REALITY_OBJECT", "STL_MUNICIPALITY_ID");
            Database.AddForeignKey("FK_GKH_REALITY_OBJECT_STL", "GKH_REALITY_OBJECT", "STL_MUNICIPALITY_ID", "GKH_DICT_MUNICIPALITY", "ID");
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_REALITY_OBJECT", "STL_MUNICIPALITY_ID");
        }
    }
}