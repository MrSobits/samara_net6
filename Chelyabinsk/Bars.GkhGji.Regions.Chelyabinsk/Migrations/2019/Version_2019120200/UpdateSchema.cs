namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2019120200
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2019120200")]
    [MigrationDependsOn(typeof(Version_2019102900.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.ChangeColumnNotNullable("GJI_SMEV_HISTORY", "REQUEST_ID", false);
            Database.ChangeColumn("GJI_SMEV_HISTORY", new Column("STATUS", DbType.String, 1000, ColumnProperty.Null));
        }

        public override void Down()
        {
        }
    }
}


