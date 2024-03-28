namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2014011400
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014011400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013122603.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("OVRHL_PR_DEC_OWNER_ACCOUNT", "OWNER_SURNAME");
            Database.RemoveColumn("OVRHL_PR_DEC_OWNER_ACCOUNT", "OWNER_NAME");
            Database.RemoveColumn("OVRHL_PR_DEC_OWNER_ACCOUNT", "OWNER_PATRONYMIC");
           // Database.RemoveRefColumn("OVRHL_PR_DEC_OWNER_ACCOUNT", "REG_OPERATOR_ID");

            Database.AddRefColumn("OVRHL_PR_DEC_OWNER_ACCOUNT", new RefColumn("CONTRAGENT_ID", "OVRHL_PR_DEC_OWNER_ACC_CTR", "GKH_CONTRAGENT", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_PR_DEC_OWNER_ACCOUNT", "CONTRAGENT_ID");

            //Database.AddRefColumn("OVRHL_PR_DEC_OWNER_ACCOUNT", new RefColumn("REG_OPERATOR_ID", "OVRHL_PR_DEC_OWNER_ACC_REG", "OVRHL_REG_OPERATOR", "ID"));
            Database.AddColumn("OVRHL_PR_DEC_OWNER_ACCOUNT", new Column("OWNER_SURNAME", DbType.String, 255));
            Database.AddColumn("OVRHL_PR_DEC_OWNER_ACCOUNT", new Column("OWNER_NAME", DbType.String, 255));
            Database.AddColumn("OVRHL_PR_DEC_OWNER_ACCOUNT", new Column("OWNER_PATRONYMIC", DbType.String, 255));
        }
    }
}