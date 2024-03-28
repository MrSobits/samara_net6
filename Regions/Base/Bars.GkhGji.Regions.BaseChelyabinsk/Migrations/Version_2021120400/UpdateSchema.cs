namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2021120400
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;
    using Bars.GkhGji.Enums;

    [Migration("2021120400")]
    [MigrationDependsOn(typeof(Version_2021113000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PROTOCOL197", new Column("TYPE_ADDRESS", DbType.Int16, ColumnProperty.NotNull, (int)TypeAddress.Place));
            Database.AddColumn("GJI_PROTOCOL197", new Column("PLACE_OFFENSE", DbType.Int16, ColumnProperty.NotNull, (int)PlaceOffense.AddressUr));
            Database.AddRefColumn("GJI_PROTOCOL197", new RefColumn("FIAS_PLACE_ADDRESS", "GJI_PROT_FIAS_PLACE_ADDR197_ID", "B4_FIAS_ADDRESS", "ID"));
            Database.AddRefColumn("GJI_PROTOCOL197", new RefColumn("JUD_SECTOR", "GJI_PROT_JUD_SECTOR197_ID", "CLW_JUR_INSTITUTION", "ID"));
            Database.AddColumn("GJI_PROTOCOL197", "ADDRESS_PLACE", DbType.String, 1500);
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL197", "ADDRESS_PLACE");
            Database.RemoveColumn("GJI_PROTOCOL197", "JUD_SECTOR");
            Database.RemoveColumn("GJI_PROTOCOL197", "FIAS_PLACE_ADDRESS");
            Database.RemoveColumn("GJI_PROTOCOL197", "PLACE_OFFENSE");
            Database.RemoveColumn("GJI_PROTOCOL197", "TYPE_ADDRESS");
        }

    }
}