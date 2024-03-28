namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2014012700
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014012700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2014012500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_PR_DEC_SPEC_ACC", new Column("INN", DbType.String, 50));
            Database.AddColumn("OVRHL_PR_DEC_SPEC_ACC", new Column("KPP", DbType.String, 50));
            Database.AddColumn("OVRHL_PR_DEC_SPEC_ACC", new Column("OGRN", DbType.String, 50));
            Database.AddColumn("OVRHL_PR_DEC_SPEC_ACC", new Column("OKPO", DbType.String, 50));
            Database.AddColumn("OVRHL_PR_DEC_SPEC_ACC", new Column("BIK", DbType.String, 50));
            Database.AddColumn("OVRHL_PR_DEC_SPEC_ACC", new Column("CORR_ACCOUNT", DbType.String, 50));

            Database.AddRefColumn("OVRHL_PR_DEC_SPEC_ACC", new RefColumn("FIAS_MAIL_ADDRESS_ID", "OVRHL_SPECACCDEC_MAILADR", "B4_FIAS_ADDRESS", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_PR_DEC_SPEC_ACC", "INN");
            Database.RemoveColumn("OVRHL_PR_DEC_SPEC_ACC", "KPP");
            Database.RemoveColumn("OVRHL_PR_DEC_SPEC_ACC", "OGRN");
            Database.RemoveColumn("OVRHL_PR_DEC_SPEC_ACC", "OKPO");
            Database.RemoveColumn("OVRHL_PR_DEC_SPEC_ACC", "BIK");
            Database.RemoveColumn("OVRHL_PR_DEC_SPEC_ACC", "CORR_ACCOUNT");

            Database.RemoveColumn("OVRHL_PR_DEC_SPEC_ACC", "FIAS_MAIL_ADDRESS_ID");
        }
    }
}