namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2019012500
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2019012500")]
    [MigrationDependsOn(typeof(Version_2019011700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
         
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("GIS_GMP_PAYMENTS_TYPE", DbType.Int32, 4, ColumnProperty.None, 1));

            Database.AddEntityTable(
            "GJI_CH_GIS_GMP_PAYMENTS",
            new Column("AMOUNT", DbType.Decimal, ColumnProperty.NotNull),
            new Column("KBK", DbType.String),
            new Column("OKTMO", DbType.String),
            new Column("PAYMENT_DATE", DbType.DateTime, ColumnProperty.None),
            new Column("PAYMENT_ID", DbType.String),
            new Column("PRPOSE", DbType.String),
            new Column("UIN", DbType.String),
            new RefColumn("GIS_GMP_ID", ColumnProperty.None, "PAYM_GJI_CH_GIS_GMP_ID", "GJI_CH_GIS_GMP", "ID"),
            new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "PAYM_GJI_CH_GIS_GMP_FILE_INFO_ID", "B4_FILE_INFO", "ID"));
        }
    

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_GIS_GMP_PAYMENTS");
            Database.RemoveColumn("GJI_CH_GIS_GMP", "GIS_GMP_PAYMENTS_TYPE");
        }
    }
}