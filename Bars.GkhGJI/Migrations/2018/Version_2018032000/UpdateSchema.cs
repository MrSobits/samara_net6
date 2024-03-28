namespace Bars.GkhGji.Migrations._2018.Version_2018032000
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    
    [Migration("2018032000")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2018.Version_2018020100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GJI_PROTOCOL", new Column("UIN", DbType.String));
          

        }
        public override void Down()
        {
            this.Database.RemoveColumn("GJI_PROTOCOL", "UIN");
           
        }
    }
}