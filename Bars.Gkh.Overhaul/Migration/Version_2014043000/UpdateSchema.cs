namespace Bars.Gkh.Overhaul.Migration.Version_2014043000
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014043000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Migration.Version_2014042300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_CREDIT_ORG", new Column("IS_MAILADDR_OUT", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddColumn("OVRHL_CREDIT_ORG", new Column("MAILADDR_OUT_SUBJ", DbType.String, 500));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_CREDIT_ORG", "IS_MAILADDR_OUT");
            Database.RemoveColumn("OVRHL_CREDIT_ORG", "MAILADDR_OUT_SUBJ");
        }
    }
}