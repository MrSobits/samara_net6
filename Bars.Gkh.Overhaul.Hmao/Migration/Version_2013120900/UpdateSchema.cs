namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013120900
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013120900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013120700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // перенесен в Overhaul
            //Database.AddColumn("OVRHL_CREDIT_ORG", new Column("OGRN", DbType.String, 250));
            //Database.AddColumn("OVRHL_CREDIT_ORG", new Column("MAILING_ADDRESS", DbType.String, 500));
            //Database.AddRefColumn("OVRHL_CREDIT_ORG", new RefColumn("FIAS_MAIL_ADDRESS_ID", "OV_CR_ORG_FIAS_MAIL", "B4_FIAS_ADDRESS", "ID"));

        }

        public override void Down()
        {
        //    Database.RemoveRefColumn("OVRHL_CREDIT_ORG", "FIAS_MAIL_ADDRESS_ID");
        //    Database.RemoveColumn("OVRHL_CREDIT_ORG", "MAILING_ADDRESS");
        //    Database.RemoveColumn("OVRHL_CREDIT_ORG", "OGRN");
        }
    }
}