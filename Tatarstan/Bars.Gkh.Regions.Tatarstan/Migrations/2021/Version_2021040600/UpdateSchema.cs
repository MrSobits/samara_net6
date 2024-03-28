namespace Bars.Gkh.Regions.Tatarstan.Migrations._2021.Version_2021040600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [MigrationDependsOn(typeof(_2021.Version_2021040100.UpdateSchema))]
    [Migration("2021040600")]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            var query = @"UPDATE gkh_obj_intercom int
                          SET(intercom_installation_date) =
                          (SELECT installation_date from gkh_obj_intercom int1
                          WHERE int.id = int1.id);

                          UPDATE gkh_obj_intercom
                          set archive_access = 30
                          where archive_access = 0 or archive_access is not distinct from null;

                          UPDATE gkh_obj_intercom
                          set recording = 30 
                          where recording = 0 or recording is not distinct from null;";

            this.Database.AddColumn("GKH_OBJ_INTERCOM", new Column("HAS_NOT_TARIFF", DbType.Boolean, false));
            this.Database.AddColumn("GKH_OBJ_INTERCOM", new Column("ANALOG_INTERCOM_COUNT", DbType.Int32));
            this.Database.AddColumn("GKH_OBJ_INTERCOM", new Column("IP_INTERCOM_COUNT", DbType.Int32));
            this.Database.AddColumn("GKH_OBJ_INTERCOM", new Column("ENTRANCE_COUNT", DbType.Int32));
            this.Database.AddColumn("GKH_OBJ_INTERCOM", new Column("INTERCOM_INSTALLATION_DATE", DbType.DateTime));
            this.Database.RemoveColumn("GKH_OBJ_INTERCOM", "INTERCOM_TYPE");
            this.Database.ExecuteQuery(query);
        }

        public override void Down()
        {
            this.Database.AddColumn("GKH_OBJ_INTERCOM", new Column("INTERCOM_TYPE", DbType.Int32, defaultValue: 1));
            this.Database.RemoveColumn("GKH_OBJ_INTERCOM", "HAS_NOT_TARIFF");
            this.Database.RemoveColumn("GKH_OBJ_INTERCOM", "ANALOG_INTERCOM_COUNT");
            this.Database.RemoveColumn("GKH_OBJ_INTERCOM", "IP_INTERCOM_COUNT");
            this.Database.RemoveColumn("GKH_OBJ_INTERCOM", "ENTRANCE_COUNT");
            this.Database.RemoveColumn("GKH_OBJ_INTERCOM", "INTERCOM_INSTALLATION_DATE");
        }
    }
}
