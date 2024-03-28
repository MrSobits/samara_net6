namespace Bars.Gkh.Migrations._2020.Version_2020112000
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2020112000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2020111700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_CS_COEFFICIENT",
                new Column("NAME", DbType.String, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 20, ColumnProperty.NotNull),
                new Column("UNIT_MEASURE", DbType.String, 50),
                new Column("VALUE", DbType.Decimal, ColumnProperty.None),
                new Column("DATE_FROM", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DATE_TO", DbType.DateTime, ColumnProperty.None),
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.None, "GKH_CS_COEFFICIENT_MUNICIPALITY_ID", "GKH_DICT_MUNICIPALITY", "ID"),
                new RefColumn("CATEGORY_ID", ColumnProperty.None, "GKH_CS_COEFFICIENT_CATEGORY_ID", "GKH_CS_CATEGORY", "ID"));
            Database.AddIndex("IND_GKH_CS_COEFFICIENT_CODE", false, "GKH_CS_COEFFICIENT", "CODE");

            Database.AddColumn("GKH_CS_CALCULATION", new Column("CALC_DATE", DbType.DateTime, ColumnProperty.None));



        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_CS_CALCULATION", "CALC_DATE");
            Database.RemoveTable("GKH_CS_COEFFICIENT");
        }
    }
}