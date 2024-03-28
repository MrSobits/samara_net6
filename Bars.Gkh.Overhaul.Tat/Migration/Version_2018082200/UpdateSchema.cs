namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2018082200
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [Migration("2018082200")]
    [MigrationDependsOn(typeof(Version_2018052500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("OVRHL_PROP_OWN_PROTOCOLS", "VOTING_START_TIME", DbType.DateTime);
        }

        public override void Down()
        {
            this.Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "VOTING_START_TIME");
        }
    }
}