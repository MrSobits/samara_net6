namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013102802
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013102802")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013102801.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("OVRHL_SUBSIDY_MU_REC", "FINANCE_NEED_AFTER");

            Database.AddColumn("OVRHL_SM_RECORD_VERSION", "FINANCE_NEED_AFTER", DbType.Decimal, ColumnProperty.NotNull, 0);
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_SM_RECORD_VERSION", "FINANCE_NEED_AFTER");

            Database.AddColumn("OVRHL_SUBSIDY_MU_REC", "FINANCE_NEED_AFTER", DbType.Decimal, ColumnProperty.NotNull, 0);
        }
    }
}