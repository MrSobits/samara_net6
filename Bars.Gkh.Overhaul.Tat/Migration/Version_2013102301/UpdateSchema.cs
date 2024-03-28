namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013102301
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013102301")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013102300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "OVRHL_PRG_VERSION",
                new Column("NAME", DbType.String, ColumnProperty.NotNull),
                new Column("VERSION_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new RefColumn("MU_ID", ColumnProperty.NotNull, "OVRHL_PRG_VER_MU", "GKH_DICT_MUNICIPALITY", "ID"));

            Database.AddEntityTable(
                "OVRHL_VERSION_PRM",
                new RefColumn("VERSION_ID", ColumnProperty.Null, "OVRHL_VER_PRM_VER", "OVRHL_PRG_VERSION", "ID"),
                new Column("CODE", DbType.String, 300, ColumnProperty.NotNull),
                new Column("WEIGHT", DbType.Int32, ColumnProperty.NotNull),
                new RefColumn("MU_ID", ColumnProperty.Null, "OVRHL_VER_PRM_MU", "GKH_DICT_MUNICIPALITY", "ID"));

            Database.AddEntityTable(
               "OVRHL_VERSION_REC",
               new RefColumn("VERSION_ID", ColumnProperty.Null, "OVRHL_VER_REC_VER", "OVRHL_PRG_VERSION", "ID"),
               new RefColumn("RO_ID", ColumnProperty.NotNull, "OVRHL_VER_REC_RO", "GKH_REALITY_OBJECT", "ID"),
               new Column("YEAR", DbType.Int64, ColumnProperty.NotNull, 0),
               new Column("CEO_STRING", DbType.String, 4000, ColumnProperty.Null),
               new Column("SUM", DbType.Decimal, ColumnProperty.NotNull, 0),
               new Column("POINT", DbType.Decimal, ColumnProperty.NotNull, 0m),
               new Column("INDEX_NUM", DbType.Int64, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_VERSION_REC");
            Database.RemoveTable("OVRHL_VERSION_PRM");
            Database.RemoveTable("OVRHL_PRG_VERSION");
        }
    }
}