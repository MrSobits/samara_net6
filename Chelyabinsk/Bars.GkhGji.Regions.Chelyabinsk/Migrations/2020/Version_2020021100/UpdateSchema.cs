namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2020021100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2020021100")]
    [MigrationDependsOn(typeof(Version_2020010900.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GJI_CH_GIS_GMP", new Column("DOC_NUM_DATE", DbType.String, ColumnProperty.None));
        }
        public override void Down()
        {
            this.Database.RemoveColumn("GJI_CH_GIS_GMP", "DOC_NUM_DATE");
        }
    }
}


