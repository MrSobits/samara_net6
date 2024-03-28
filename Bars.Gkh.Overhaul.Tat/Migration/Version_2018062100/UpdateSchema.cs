namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2018062100
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [Migration("2018062100")]
    [MigrationDependsOn(typeof(Version_2018052500.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016111000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("OVRHL_PROP_OWN_PROTOCOLS", "NPA_NAME", DbType.String);
            this.Database.AddColumn("OVRHL_PROP_OWN_PROTOCOLS", "NPA_DATE", DbType.DateTime);
            this.Database.AddColumn("OVRHL_PROP_OWN_PROTOCOLS", "NPA_NUMBER", DbType.String);
            this.Database.AddColumn("OVRHL_PROP_OWN_PROTOCOLS", "NPA_STATUS", DbType.Int32);
            this.Database.AddColumn("OVRHL_PROP_OWN_PROTOCOLS", "NPA_CANCELLATION_REASON", DbType.String);

            this.Database.AddRefColumn("OVRHL_PROP_OWN_PROTOCOLS", new RefColumn("TYPE_INFORMATION_NPA_ID", "OVRHL_OWN_PROTOCOLS_DECISION_TYPE_INFORMATION", "GKH_DICT_TYPE_INFORMATION_NPA", "ID"));
            this.Database.AddRefColumn("OVRHL_PROP_OWN_PROTOCOLS", new RefColumn("TYPE_NPA_ID", "OVRHL_OWN_PROTOCOLS_TYPE_NPA", "GKH_DICT_TYPE_NPA", "ID"));
            this.Database.AddRefColumn("OVRHL_PROP_OWN_PROTOCOLS", new RefColumn("TYPE_NORMATIVE_ACT_ID", "OVRHL_OWN_PROTOCOLS_TYPE_NORMATIVE_ACT", "GKH_DICT_TYPE_NORMATIVE_ACT", "ID"));
            this.Database.AddRefColumn("OVRHL_PROP_OWN_PROTOCOLS", new RefColumn("NPA_CONTRAGENT_ID", "OVRHL_OWN_PROTOCOLS_NPA_CONTRAGENT", "GKH_CONTRAGENT", "ID"));
            this.Database.AddRefColumn("OVRHL_PROP_OWN_PROTOCOLS", new RefColumn("NPA_FILE_ID", "OVRHL_OWN_PROTOCOLS_NPA_FILE", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "NPA_NAME");
            this.Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "NPA_DATE");
            this.Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "NPA_NUMBER");
            this.Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "NPA_STATUS");
            this.Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "NPA_CANCELLATION_REASON");
            this.Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "TYPE_INFORMATION_NPA_ID");
            this.Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "TYPE_NPA_ID");
            this.Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "TYPE_NORMATIVE_ACT_ID");
            this.Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "NPA_CONTRAGENT_ID");
            this.Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "NPA_FILE_ID");
        }
    }
}