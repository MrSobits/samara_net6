namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2019072900
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2019072900")]
    [MigrationDependsOn(typeof(Version_2019072400.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_CH_GIS_ERP", new Column("KND_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 10));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_GIS_ERP", "KND_TYPE");          
        }
    }
}


