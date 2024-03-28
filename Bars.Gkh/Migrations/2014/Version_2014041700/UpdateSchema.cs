namespace Bars.Gkh.Migrations.Version_2014041700
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014041700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014032800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_DICT_FIASOKTMO",
                new RefColumn("MUNICIPALITY_ID", "GKH_DICT_FIASOKTMO", "GKH_DICT_MUNICIPALITY", "ID"),
                new Column("FIAS_GUID", DbType.String, 36));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_DICT_FIASOKTMO");
        }
    }
}