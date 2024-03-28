namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2017121600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2017121600")]
    [MigrationDependsOn(typeof(Version_2017121500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GJI_CH_GIS_GMP", new Column("CHARGE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 1));
            this.Database.AddColumn("GJI_CH_GIS_GMP", new Column("PACKET_ID", DbType.String, ColumnProperty.None));

        }
        public override void Down()
        {
            this.Database.RemoveColumn("GJI_CH_GIS_GMP", "PACKET_ID");
            this.Database.RemoveColumn("GJI_CH_GIS_GMP", "CHARGE_TYPE");
        }
    }
}