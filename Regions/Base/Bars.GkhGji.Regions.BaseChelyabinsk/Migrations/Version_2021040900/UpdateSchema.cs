namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2021040900
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2021040900")]
    [MigrationDependsOn(typeof(Version_2021040801.UpdateSchema))]
    public class UpdateSchema : Migration
    {     
        public override void Up()
        {
            Database.AddColumn("GJI_PROTOCOL197", new Column("PP_POSITION", DbType.String, 150));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL197", "PP_POSITION");
        }
        
    }
}