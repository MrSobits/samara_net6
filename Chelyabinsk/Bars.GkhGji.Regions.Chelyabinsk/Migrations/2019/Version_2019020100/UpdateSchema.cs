namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2019020100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2019020100")]
    [MigrationDependsOn(typeof(Version_2019012900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
         
            Database.AddColumn("GJI_CH_GIS_GMP_PAYMENTS", new Column("RECONSILE", DbType.Int32, 4, ColumnProperty.NotNull, 20));
          


        }
    

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_GIS_GMP_PAYMENTS", "RECONSILE");
           
        }
    }
}