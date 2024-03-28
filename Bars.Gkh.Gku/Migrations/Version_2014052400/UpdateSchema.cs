namespace Bars.Gkh.Gku.Migrations.Version_2014052400
{
    using System;
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014052400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Gku.Migrations.Version_1.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RenameColumn("GKH_GKU_TARIFF", "TARIF_VALUE", "TARIF_RSO");
            Database.RemoveColumn("GKH_GKU_TARIFF", "CONTRACTOR_ID");
            Database.RemoveColumn("GKH_GKU_TARIFF", "SERVICE");

            Database.AddRefColumn("GKH_GKU_TARIFF", new RefColumn("SERVICE_ID", "GKH_GKU_TARIFF_SRV", "GKH_PUBLIC_SERV", "ID"));
            Database.AddRefColumn("GKH_GKU_TARIFF", new RefColumn("MANORG_ID", "GKH_GKU_TARIFF_MO", "GKH_MANAGING_ORGANIZATION", "ID"));
            Database.AddRefColumn("GKH_GKU_TARIFF", new RefColumn("RESORG_ID", "GKH_GKU_TARIFF_RSO", "GKH_SUPPLY_RESORG", "ID"));

            Database.AddColumn("GKH_GKU_TARIFF", "DATE_START", DbType.Date);
            Database.AddColumn("GKH_GKU_TARIFF", "DATE_END", DbType.Date);

            Database.AddColumn("GKH_GKU_TARIFF", "PURCHASE_PRICE", DbType.String, 100);
            Database.AddColumn("GKH_GKU_TARIFF", "PURCHASE_VOLUME", DbType.Decimal, ColumnProperty.NotNull, 0m);

            Database.AddColumn("GKH_GKU_TARIFF", "TARIF_MO", DbType.Decimal, ColumnProperty.NotNull, 0m);

            Database.AddColumn("GKH_GKU_TARIFF", "NORMATIVE_ACT_INFO", DbType.String, 500);
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_GKU_TARIFF", "NORMATIVE_ACT_INFO");

            Database.RemoveColumn("GKH_GKU_TARIFF", "TARIF_MO");

            Database.RemoveColumn("GKH_GKU_TARIFF", "PURCHASE_VOLUME");
            Database.RemoveColumn("GKH_GKU_TARIFF", "PURCHASE_PRICE");

            Database.RemoveColumn("GKH_GKU_TARIFF", "DATE_END");
            Database.RemoveColumn("GKH_GKU_TARIFF", "DATE_START");

            Database.RemoveColumn("GKH_GKU_TARIFF", "RESORG_ID");
            Database.RemoveColumn("GKH_GKU_TARIFF", "MANORG_ID");
            Database.RemoveColumn("GKH_GKU_TARIFF", "SERVICE_ID");

            Database.AddColumn("GKH_GKU_TARIFF", new Column("SERVICE", DbType.String));
            Database.AddRefColumn("GKH_GKU_TARIFF", new RefColumn("CONTRACTOR_ID", "GKH_GKU_TARIFF_CNTR", "GKH_CONTRAGENT", "ID"));
            Database.RenameColumn("GKH_GKU_TARIFF", "TARIF_RSO", "TARIF_VALUE");
        }
    }
}