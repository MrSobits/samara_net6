namespace Bars.Gkh.Gasu.Migration.Version_1
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GASU_INDICATOR",
                new Column("NAME", DbType.String, 250),
                new Column("CODE", DbType.String, 100),
                new Column("PERIODICITY", DbType.Int16, ColumnProperty.NotNull, 10),
                new Column("EBIR_MODULE", DbType.Int16, ColumnProperty.NotNull, 10),
                new RefColumn("UNIT_MEASURE_ID", "GASU_INDICATOR_UM", "GKH_DICT_UNITMEASURE", "ID"));

            Database.AddEntityTable(
                "GASU_INDICATOR_VALUE",
                new Column("VALUE", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("MONTH", DbType.Int16, ColumnProperty.NotNull, 1),
                new Column("YEAR", DbType.Int16, ColumnProperty.NotNull, 1),
                new RefColumn("MU_ID", ColumnProperty.NotNull, "GASU_IND_VALUE_MU", "GKH_DICT_MUNICIPALITY", "ID"),
                new RefColumn("INDICATOR_ID", ColumnProperty.NotNull, "GASU_IND_VALUE_IND", "GASU_INDICATOR", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GASU_INDICATOR_VALUE");
            Database.RemoveTable("GASU_INDICATOR");
        }
    }
}