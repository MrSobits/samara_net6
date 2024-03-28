namespace Bars.Gkh.Ris.Migrations.Version_2016081100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2016081100")]
    [MigrationDependsOn(typeof(Version_2016080300.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016021700.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016031400.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016032900.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016051500.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016051700.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016051901.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016052300.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016052800.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016060300.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016070101.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            //this.Database.RemoveColumn("RIS_WORKLIST_ITEM", "SERVICETYPE_ID");
            //this.Database.RemoveTable("RIS_SERVICE_TYPE");

            //this.Database.AddColumn("RIS_WORKLIST_ITEM", new Column("WORK_ITEM_CODE", DbType.String, 10));
            //this.Database.AddColumn("RIS_WORKLIST_ITEM", new Column("WORK_ITEM_GUID", DbType.String, 50));
        }

        public override void Down()
        {
            //this.Database.RemoveColumn("RIS_WORKLIST_ITEM", "WORK_ITEM_GUID");
            //this.Database.RemoveColumn("RIS_WORKLIST_ITEM", "WORK_ITEM_CODE");

            //this.Database.AddRisTable("RIS_SERVICE_TYPE", new RefColumn("GISDICTREF_ID", "RIS_ST_DICTREF", "RIS_INTEGR_REF_DICT", "ID"));

            //this.Database.AddRefColumn("RIS_WORKLIST_ITEM", new RefColumn("SERVICETYPE_ID", "RIS_WORKITEM_SRVT", "RIS_SERVICE_TYPE", "ID"));
        }
    }
}