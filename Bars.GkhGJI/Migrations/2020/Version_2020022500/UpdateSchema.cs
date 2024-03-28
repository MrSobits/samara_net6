namespace Bars.GkhGji.Migrations._2020.Version_2020022500
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020022500")]
    [MigrationDependsOn(typeof(Version_2020022100.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
          
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("MUNICIPALITY", DbType.String, 100));
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("MUNICIPALITY_ID", DbType.Int64, ColumnProperty.None));        
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "MUNICIPALITY_ID");
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "MUNICIPALITY");         
        }
    }
}