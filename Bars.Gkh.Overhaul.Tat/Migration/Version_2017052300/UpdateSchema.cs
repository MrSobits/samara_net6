namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2017052300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2017052300")]
    [MigrationDependsOn(typeof(Version_2013101000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("OVRHL_PROP_OWN_PROTOCOLS", "LOAN_AMOUNT", DbType.Decimal);
            this.Database.AddRefColumn("OVRHL_PROP_OWN_PROTOCOLS", 
                new RefColumn("BORROWER_CONTRAGENT_ID", "OVRHL_PROP_OWN_PROTOCOLS_BORROWER", "GKH_CONTRAGENT", "ID"));
            this.Database.AddRefColumn("OVRHL_PROP_OWN_PROTOCOLS",
                new RefColumn("LENDER_CONTRAGENT_ID", "OVRHL_PROP_OWN_PROTOCOLS_LENDER", "GKH_CONTRAGENT", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "LOAN_AMOUNT");
            this.Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "BORROWER_CONTRAGENT_ID");
            this.Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "LENDER_CONTRAGENT_ID");
        }
    }
}