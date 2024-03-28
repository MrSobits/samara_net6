namespace Bars.Gkh.Migrations._2020.Version_2020111100
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2020111100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2020111000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_CS_TARIF",
                new Column("NAME", DbType.String, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 20, ColumnProperty.NotNull),
                new Column("UNIT_MEASURE", DbType.String, 50),
                new Column("VALUE", DbType.Decimal, ColumnProperty.None),
                new Column("DATE_FROM", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DATE_TO", DbType.DateTime, ColumnProperty.None),
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.None, "GKH_CS_TARIF_MUNICIPALITY_ID", "GKH_DICT_MUNICIPALITY", "ID"));
            Database.AddIndex("IND_GKH_CS_TARIF_CODE", false, "GKH_CS_TARIF", "CODE");

            Database.AddEntityTable(
             "GKH_DICT_TYPE_VIDEO_OVERWATCH",
             new Column("NAME", DbType.String, 50),
             new Column("CODE", DbType.String, 300),
             new Column("DESCRIPTION", DbType.String, 300));
            Database.AddIndex("IND_GKH_VIDEO_OVERWATCH_NAME", false, "GKH_DICT_TYPE_VIDEO_OVERWATCH", "NAME");
            Database.AddIndex("IND_GKH_VIDEO_OVERWATCH_CODE", false, "GKH_DICT_TYPE_VIDEO_OVERWATCH", "CODE");

            Database.AddEntityTable("GKH_REALITY_VIDEO_OVERWATCH",             
               new Column("DATE_FROM", DbType.DateTime, ColumnProperty.NotNull),
               new Column("DATE_TO", DbType.DateTime, ColumnProperty.None),
               new RefColumn("RO_ID", ColumnProperty.NotNull, "GKH_VIDEO_OVERWATCH_RO_ID", "GKH_REALITY_OBJECT", "ID"),
               new RefColumn("CONTRAGENT_ID", ColumnProperty.NotNull, "GKH_VIDEO_OVERWATCH_CONTRAGENT_ID", "GKH_CONTRAGENT", "ID"));

            Database.AddEntityTable("GKH_RO_OVERWATCH_CGNT_TYPE",
             new RefColumn("ROOC_ID", ColumnProperty.NotNull, "GKH_RO_OVERWATCH_CGNT_TYPE_ROOC", "GKH_REALITY_VIDEO_OVERWATCH", "ID"),
             new RefColumn("TYPE_ID", ColumnProperty.NotNull, "GKH_RO_OVERWATCH_CGNT_TYPE_OWTYPE", "GKH_DICT_TYPE_VIDEO_OVERWATCH", "ID"));

        }

        public override void Down()
        {
            Database.RemoveTable("GKH_RO_OVERWATCH_CGNT_TYPE");
            Database.RemoveTable("GKH_REALITY_VIDEO_OVERWATCH");
            Database.RemoveTable("GKH_DICT_TYPE_VIDEO_OVERWATCH");
            Database.RemoveTable("GKH_CS_TARIF");
        }
    }
}