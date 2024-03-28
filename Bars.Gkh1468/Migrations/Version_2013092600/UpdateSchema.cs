namespace Bars.Gkh1468.Migrations.Version_2013092600
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013092600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh1468.Migrations.Version_2013092400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("GKH_PSTRUCT_META_ATTR", "ORDER_NUM");
            Database.RemoveColumn("GKH_PSTRUCT_PART", "ORDER_NUM");
            Database.AddColumn("GKH_PSTRUCT_META_ATTR",
                new Column("ORDER_NUM", DbType.Int32, ColumnProperty.Null, defaultValue: 0));
            Database.AddColumn("GKH_PSTRUCT_PART",
                new Column("ORDER_NUM", DbType.Int32, ColumnProperty.Null, defaultValue: 0));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_PSTRUCT_META_ATTR", "ORDER_NUM");
            Database.RemoveColumn("GKH_PSTRUCT_PART", "ORDER_NUM");
            Database.AddColumn("GKH_PSTRUCT_META_ATTR",
                new Column("ORDER_NUM", DbType.String, ColumnProperty.Null));
            Database.AddColumn("GKH_PSTRUCT_PART",
                new Column("ORDER_NUM", DbType.String, ColumnProperty.Null));
        }
    }
}