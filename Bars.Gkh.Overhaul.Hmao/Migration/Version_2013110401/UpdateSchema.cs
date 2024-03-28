namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013110401
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013110401")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013110400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_SUBSIDY_REC_VERSION", new Column("BALANCE_AFTER_CR", DbType.Decimal, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_SUBSIDY_REC_VERSION", "BALANCE_AFTER_CR");
        }
    }
}