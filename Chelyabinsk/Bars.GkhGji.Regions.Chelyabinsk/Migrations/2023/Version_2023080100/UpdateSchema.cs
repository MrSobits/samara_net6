namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2023080100
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023080100")]
    [MigrationDependsOn(typeof(Version_2023072400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddEntityTable(
              "GJI_CH_SMEV_DO",
              new Column("ANSWER", DbType.String, 500),
              new Column("REQUEST_TEXT", DbType.String, 10000),
              new Column("REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
              new Column("REQUEST_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
              new Column("MESSAGE_ID", DbType.String, 500),
            new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_CH_SMEV_DO_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"));

            Database.AddEntityTable(
                "GJI_CH_SMEV_DO_FILE",
                new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
                new RefColumn("SMEV_DO_ID", ColumnProperty.None, "GJI_CH_DO_FILE_DO_ID", "GJI_CH_SMEV_DO", "ID"),
                new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "GJI_CH_DO_FILE_INFO_ID", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_SMEV_DO");
            Database.RemoveTable("GJI_CH_SMEV_DO_FILE");
        }
    }
}