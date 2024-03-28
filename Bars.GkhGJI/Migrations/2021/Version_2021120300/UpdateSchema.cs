namespace Bars.GkhGji.Migrations._2021.Version_2021120300
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2021120300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2021102700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PROTOCOL", new Column("TYPE_ADDRESS", DbType.Int16, ColumnProperty.NotNull, (int)Enums.TypeAddress.Place));
            Database.AddColumn("GJI_PROTOCOL", new Column("PLACE_OFFENSE", DbType.Int16, ColumnProperty.NotNull, (int)Enums.PlaceOffense.AddressUr));
            Database.AddRefColumn("GJI_PROTOCOL", new RefColumn("FIAS_PLACE_ADDRESS", "GJI_PROT_FIAS_PLACE_ADDR_ID", "B4_FIAS_ADDRESS", "ID"));
            Database.AddRefColumn("GJI_PROTOCOL", new RefColumn("JUD_SECTOR", "GJI_PROT_JUD_SECTOR_ID", "CLW_JUR_INSTITUTION", "ID"));
            Database.AddColumn("GJI_PROTOCOL", "ADDRESS_PLACE", DbType.String, 1500);
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL", "ADDRESS_PLACE");
            Database.RemoveColumn("GJI_PROTOCOL", "JUD_SECTOR");
            Database.RemoveColumn("GJI_PROTOCOL", "FIAS_PLACE_ADDRESS");
            Database.RemoveColumn("GJI_PROTOCOL", "PLACE_OFFENSE");
            Database.RemoveColumn("GJI_PROTOCOL", "TYPE_ADDRESS");
        }
    }
}