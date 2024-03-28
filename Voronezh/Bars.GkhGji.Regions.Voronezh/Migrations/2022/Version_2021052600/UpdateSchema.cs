namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2021052600
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2021052600")]
    [MigrationDependsOn(typeof(Version_2021042100.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_CH_LICENSE_REISSUANCE", new Column("DECLINE_REASON", DbType.String, ColumnProperty.None, 4000));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_LICENSE_REISSUANCE", "DECLINE_REASON");
        }


    }
}
