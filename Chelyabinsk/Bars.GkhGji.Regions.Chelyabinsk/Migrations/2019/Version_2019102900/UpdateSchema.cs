namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2019102900
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2019102900")]
    [MigrationDependsOn(typeof(Version_2019080500.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GJI_SMEV_HISTORY",
               new Column("REQUEST_ID", DbType.Int64, ColumnProperty.NotNull),
               new Column("ACTION_CODE", DbType.String, ColumnProperty.NotNull),
               new Column("REQUEST_TYPE", DbType.Int32, ColumnProperty.NotNull),
               new Column("STATUS", DbType.String, ColumnProperty.None),
               new Column("GUID", DbType.String, 36, ColumnProperty.None),
               new Column("UNIQ_ID", DbType.String, 36, ColumnProperty.NotNull),
               new Column("INNER_ID", DbType.String, 36, ColumnProperty.None),
               new Column("EXT_ACTION_ID", DbType.String, 36, ColumnProperty.None),
               new Column("SOC_ID", DbType.String, 36, ColumnProperty.NotNull));
        }

        public override void Down()
        {
           
            Database.RemoveTable("GJI_SMEV_HISTORY");
        }
    }
}


