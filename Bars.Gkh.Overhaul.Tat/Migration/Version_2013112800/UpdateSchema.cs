namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013112800
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013112800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013112700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_SM_RECORD_VERSION", new Column("CORR_NEED_FINANCE", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SM_RECORD_VERSION", new Column("CORR_DEFICIT", DbType.Decimal, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_SM_RECORD_VERSION", "CORR_NEED_FINANCE");
            Database.RemoveColumn("OVRHL_SM_RECORD_VERSION", "CORR_DEFICIT");
        }
    }
}