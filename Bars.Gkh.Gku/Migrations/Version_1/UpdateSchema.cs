namespace Bars.Gkh.Gku.Migrations.Version_1
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
                "GKH_GKU_TARIFF",
                new Column("SERVICE", DbType.String, ColumnProperty.NotNull),
                new Column("SERVICE_KIND", DbType.Int16, ColumnProperty.Null),
                new Column("TARIF_VALUE", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("NORM_VALUE", DbType.Decimal, ColumnProperty.NotNull, 0),
                new RefColumn("CONTRACTOR_ID", ColumnProperty.NotNull, "GKH_GKU_TAR_CTR_ID", "GKH_CONTRAGENT", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_GKU_TARIFF");
        }
    }
}