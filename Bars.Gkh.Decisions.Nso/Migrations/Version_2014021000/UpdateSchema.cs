namespace Bars.Gkh.Decisions.Nso.Migrations.Version_2014021000
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Decisions.Nso.Migrations.Version_2014020900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("DEC_MIN_FUND_AMOUNT", new Column("DEFAULT_VALUE", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("DEC_MONTHLY_FEE", new Column("DEFAULT_VALUE", DbType.Decimal, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveColumn("DEC_MONTHLY_FEE", "DEFAULT_VALUE");
            Database.RemoveColumn("DEC_MIN_FUND_AMOUNT", "DEFAULT_VALUE");
        }
    }
}