namespace Bars.Gkh.Decisions.Nso.Migrations.Version_2023123100
{
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    using global::Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2023123100")]
    [MigrationDependsOn(typeof(Version_2019082100.UpdateSchema))]
    //Является Version_2018052400 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GKH_OBJ_D_PROTOCOL", "FORM_VOTING", DbType.Int32);
            this.Database.AddColumn("GKH_OBJ_D_PROTOCOL", "END_DATE_DECISION", DbType.DateTime);
            this.Database.AddColumn("GKH_OBJ_D_PROTOCOL", "PLACE_DECISION", DbType.String);
            this.Database.AddColumn("GKH_OBJ_D_PROTOCOL", "PLACE_MEETING", DbType.String);
            this.Database.AddColumn("GKH_OBJ_D_PROTOCOL", "DATE_MEETING", DbType.DateTime);
            this.Database.AddColumn("GKH_OBJ_D_PROTOCOL", "TIME_MEETING", DbType.DateTime);
            this.Database.AddColumn("GKH_OBJ_D_PROTOCOL", "VOTING_START_DATE", DbType.DateTime);
            this.Database.AddColumn("GKH_OBJ_D_PROTOCOL", "VOTING_END_DATE", DbType.DateTime);
            this.Database.AddColumn("GKH_OBJ_D_PROTOCOL", "VOTING_END_TIME", DbType.DateTime);
            this.Database.AddColumn("GKH_OBJ_D_PROTOCOL", "ORDER_TAKING_DEC_OWNERS", DbType.String);
            this.Database.AddColumn("GKH_OBJ_D_PROTOCOL", "ORDER_ACQUAINTANCE_INFO", DbType.String);
            this.Database.AddColumn("GKH_OBJ_D_PROTOCOL", "ANNUAL_MEETING", DbType.Int32);
            this.Database.AddColumn("GKH_OBJ_D_PROTOCOL", "LEGALITY_MEETING", DbType.Int32);
            this.Database.AddColumn("GKH_OBJ_D_PROTOCOL", "VOTING_STATUS", DbType.Int32);
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GKH_OBJ_D_PROTOCOL", "FORM_VOTING");
            this.Database.RemoveColumn("GKH_OBJ_D_PROTOCOL", "END_DATE_DECISION");
            this.Database.RemoveColumn("GKH_OBJ_D_PROTOCOL", "PLACE_DECISION");
            this.Database.RemoveColumn("GKH_OBJ_D_PROTOCOL", "PLACE_MEETING");
            this.Database.RemoveColumn("GKH_OBJ_D_PROTOCOL", "DATE_MEETING");
            this.Database.RemoveColumn("GKH_OBJ_D_PROTOCOL", "TIME_MEETING");
            this.Database.RemoveColumn("GKH_OBJ_D_PROTOCOL", "VOTING_START_DATE");
            this.Database.RemoveColumn("GKH_OBJ_D_PROTOCOL", "VOTING_END_DATE");
            this.Database.RemoveColumn("GKH_OBJ_D_PROTOCOL", "VOTING_END_TIME");
            this.Database.RemoveColumn("GKH_OBJ_D_PROTOCOL", "ORDER_TAKING_DEC_OWNERS");
            this.Database.RemoveColumn("GKH_OBJ_D_PROTOCOL", "ORDER_ACQUAINTANCE_INFO");
            this.Database.RemoveColumn("GKH_OBJ_D_PROTOCOL", "ANNUAL_MEETING");
            this.Database.RemoveColumn("GKH_OBJ_D_PROTOCOL", "LEGALITY_MEETING");
            this.Database.RemoveColumn("GKH_OBJ_D_PROTOCOL", "VOTING_STATUS");
        }
    }
}