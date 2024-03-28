namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2022031700
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2022031700")]
    [MigrationDependsOn(typeof(Version_2021122100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
               "GJI_CH_SMEV_ERUL_LICNUMBER",
                    new Column("MESSAGE_ID", DbType.String, 500),
                    new Column("REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
                    new Column("REQUEST_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                    new Column("ANSWER", DbType.String, 500),
                    new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_ERUL_LICNUMBER_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"),
                    new RefColumn("LICENSE_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_ERUL_LICNUMBER_LIC_ID", "GKH_MANORG_LICENSE", "ID"));



            Database.AddEntityTable(
                 "GJI_CH_SMEV_ERULLICNUMBER_FILE",
                 new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
                 new RefColumn("SMEV_ERUL_ID", ColumnProperty.None, "GJI_CH_SMEV_ERUL_ERUL_ID", "GJI_CH_SMEV_ERUL_LICNUMBER", "ID"),
                 new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_ERUL_FILE_INFO_ID", "B4_FILE_INFO", "ID"));


        }
                       
        public override void Down()
        {
            Database.RemoveTable("GJI_CH_SMEV_ERULLICNUMBER_FILE");
            Database.RemoveTable("GJI_CH_SMEV_ERUL_LICNUMBER");
        }

    }
}