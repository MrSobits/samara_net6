namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2019012900
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2019012900")]
    [MigrationDependsOn(typeof(Version_2019012500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
         
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("PAY_KBK", DbType.String, ColumnProperty.None));
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("PAY_START_DATE", DbType.DateTime,ColumnProperty.None));
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("PAY_END_DATE", DbType.DateTime, ColumnProperty.None));


        }
    

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_GIS_GMP", "PAY_END_DATE");
            Database.RemoveColumn("GJI_CH_GIS_GMP", "PAY_START_DATE");
            Database.RemoveColumn("GJI_CH_GIS_GMP", "PAY_KBK");
        }
    }
}