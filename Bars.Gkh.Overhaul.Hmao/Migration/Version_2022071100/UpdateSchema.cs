namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2022071100
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2022071100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2021120200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_SUBSIDY_REC", new Column("SALDO_BALLANCE", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_REC_VERSION", new Column("SALDO_BALLANCE", DbType.Decimal, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_SUBSIDY_REC_VERSION", "SALDO_BALLANCE");
            Database.RemoveColumn("OVRHL_SUBSIDY_REC", "SALDO_BALLANCE");
        }
    }
}