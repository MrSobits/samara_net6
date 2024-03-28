namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2018052500
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2018052500")]
    [MigrationDependsOn(typeof(Version_2017052300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("OVRHL_PROP_OWN_PROTOCOLS", "FORM_VOTING", DbType.Int32);
            this.Database.AddColumn("OVRHL_PROP_OWN_PROTOCOLS", "END_DATE_DECISION", DbType.DateTime);
            this.Database.AddColumn("OVRHL_PROP_OWN_PROTOCOLS", "PLACE_DECISION", DbType.String);
            this.Database.AddColumn("OVRHL_PROP_OWN_PROTOCOLS", "PLACE_MEETING", DbType.String);
            this.Database.AddColumn("OVRHL_PROP_OWN_PROTOCOLS", "DATE_MEETING", DbType.DateTime);
            this.Database.AddColumn("OVRHL_PROP_OWN_PROTOCOLS", "TIME_MEETING", DbType.DateTime);
            this.Database.AddColumn("OVRHL_PROP_OWN_PROTOCOLS", "VOTING_START_DATE", DbType.DateTime);
            this.Database.AddColumn("OVRHL_PROP_OWN_PROTOCOLS", "VOTING_END_DATE", DbType.DateTime);
            this.Database.AddColumn("OVRHL_PROP_OWN_PROTOCOLS", "VOTING_END_TIME", DbType.DateTime);
            this.Database.AddColumn("OVRHL_PROP_OWN_PROTOCOLS", "ORDER_TAKING_DEC_OWNERS", DbType.String);
            this.Database.AddColumn("OVRHL_PROP_OWN_PROTOCOLS", "ORDER_ACQUAINTANCE_INFO", DbType.String);
            this.Database.AddColumn("OVRHL_PROP_OWN_PROTOCOLS", "ANNUAL_MEETING", DbType.Int32);
            this.Database.AddColumn("OVRHL_PROP_OWN_PROTOCOLS", "LEGALITY_MEETING", DbType.Int32);
            this.Database.AddColumn("OVRHL_PROP_OWN_PROTOCOLS", "VOTING_STATUS", DbType.Int32);
        }

        public override void Down()
        {
            this.Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "FORM_VOTING");
            this.Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "END_DATE_DECISION");
            this.Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "PLACE_DECISION");
            this.Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "PLACE_MEETING");
            this.Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "DATE_MEETING");
            this.Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "TIME_MEETING");
            this.Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "VOTING_START_DATE");
            this.Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "VOTING_END_DATE");
            this.Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "VOTING_END_TIME");
            this.Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "ORDER_TAKING_DEC_OWNERS");
            this.Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "ORDER_ACQUAINTANCE_INFO");
            this.Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "ANNUAL_MEETING");
            this.Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "LEGALITY_MEETING");
            this.Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "VOTING_STATUS");
        }
    }
}