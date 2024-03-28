namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2020102800
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020102800")]
    [MigrationDependsOn(typeof(Version_2020071400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddColumn("GJI_CH_SMEV_EGRN", new RefColumn("RO_ID", ColumnProperty.None, "GJI_CH_SMEV_EGRN_RO_ID", "GKH_REALITY_OBJECT", "ID"));
            Database.AddColumn("GJI_CH_SMEV_EGRN", new RefColumn("ROOM_ID", ColumnProperty.None, "GJI_CH_SMEV_EGRN_ROOM_ID", "GKH_ROOM", "ID"));
            Database.AddColumn("GJI_CH_SMEV_EGRN", new Column("REQUEST_DATA_TYPE", System.Data.DbType.Int32, ColumnProperty.None));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_SMEV_EGRN", "REQUEST_DATA_TYPE");
            Database.RemoveColumn("GJI_CH_SMEV_EGRN", "ROOM_ID");
            Database.RemoveColumn("GJI_CH_SMEV_EGRN", "RO_ID");
        }
    }
}