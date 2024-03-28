namespace Bars.Gkh.Decisions.Nso.Migrations.Version_2023123102
{
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    using global::Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2023123102")]
    [MigrationDependsOn(typeof(Version_2023123101.UpdateSchema))]
    //Является Version_2018082200 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GKH_OBJ_D_PROTOCOL", "VOTING_START_TIME", DbType.DateTime);
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GKH_OBJ_D_PROTOCOL", "VOTING_START_TIME");
        }
    }
}