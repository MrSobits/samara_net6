namespace Bars.Gkh.Gis.Migrations._2014.Version_2014112001
{
    using System.Data;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014112001")][global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2014.Version_2014112000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("HOUSESERVICEREGISTER", "CHARGE", DbType.Double);
            this.Database.AddColumn("HOUSESERVICEREGISTER", "PAYMENT", DbType.Double);
            this.Database.AddColumn("HOUSESERVICEREGISTER", "MANORGS", DbType.String, 500);
        }

        public override void Down()
        {
            this.Database.RemoveColumn("HOUSESERVICEREGISTER", "CHARGE");
            this.Database.RemoveColumn("HOUSESERVICEREGISTER", "PAYMENT");
            this.Database.RemoveColumn("HOUSESERVICEREGISTER", "MANORGS");
        }
    }
}