namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2019072300
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2019072300")]
    [MigrationDependsOn(typeof(Version_2019072000.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_CH_GIS_ERP", new Column("GOALS", DbType.String));          
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_GIS_ERP", "GOALS");          
        }
    }
}


