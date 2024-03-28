namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014012503
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014012503")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014012502.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_UNACCEPT_CHARGE", new Column("TARIFF_OVERPLUS", DbType.Decimal, ColumnProperty.NotNull));
            Database.AddColumn("REGOP_UNACCEPT_CHARGE", new Column("ACCEPTED", DbType.Boolean, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_UNACCEPT_CHARGE", "TARIFF_OVERPLUS");
            Database.RemoveColumn("REGOP_UNACCEPT_CHARGE", "ACCEPTED");
        }
    }
}
