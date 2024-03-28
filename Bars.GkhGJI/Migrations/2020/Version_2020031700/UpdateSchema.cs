namespace Bars.GkhGji.Migrations._2020.Version_2020031700
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020031700")]
    [MigrationDependsOn(typeof(Version_2020030300.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {       
            Database.AddColumn("GJI_DISPOSAL", new Column("ERPID", DbType.String, 250));
        }

        public override void Down()
        {           
            Database.RemoveColumn("GJI_DISPOSAL", "ERPID");           
        }
    }
}