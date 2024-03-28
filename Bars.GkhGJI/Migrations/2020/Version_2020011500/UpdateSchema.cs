namespace Bars.GkhGji.Migrations._2020.Version_2020011500
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020011500")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2019.Version_2019121600.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GJI_APPEAL_CITIZENS", new RefColumn("ORDER_CONTRAGENT_ID", "FK_APPEAL_CONTRAGENT", "GKH_CONTRAGENT", "ID"));
        }

        public override void Down()
        {          
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "ORDER_CONTRAGENT_ID");
        }
    }
}