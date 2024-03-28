namespace Bars.GkhGji.Migrations._2020.Version_2020070800
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020070800")]
    [MigrationDependsOn(typeof(Version_2020070700.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
        

      
            Database.AddColumn("GJI_PRESCRIPTION_OFFICIAL_REPORT", new Column("EXTENSION_DATE", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("GJI_PRESCRIPTION_OFFICIAL_REPORT", new Column("REPORT_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 10));
       
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PRESCRIPTION_OFFICIAL_REPORT", "REPORT_TYPE");
            Database.RemoveColumn("GJI_PRESCRIPTION_OFFICIAL_REPORT", "EXTENSION_DATE");
        
        }
    }
}