namespace Bars.Gkh.Migrations.Version_2014013000
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014013000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014012400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_MUNICIPALITY",  new Column("TYPE_MO", DbType.Int32, 4, ColumnProperty.NotNull, 30));
            Database.AddColumn("GKH_DICT_MUNICIPALITY", new Column("PARENT_MO_ID", DbType.Int64, 22));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_MUNICIPALITY", "TYPE_MO");
            Database.RemoveColumn("GKH_DICT_MUNICIPALITY", "PARENT_MO_ID");
        }
    }
}