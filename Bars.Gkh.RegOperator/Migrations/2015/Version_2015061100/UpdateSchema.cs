namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015061100
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015061100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015060900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("regop_pers_paydoc_snap", new Column("base_tariff_sum", DbType.Decimal, ColumnProperty.NotNull, 0m));
            Database.AddColumn("regop_pers_paydoc_snap", new Column("dec_tariff_sum", DbType.Decimal, ColumnProperty.NotNull, 0m));
            Database.AddColumn("regop_pers_paydoc_snap", new Column("penalty_sum", DbType.Decimal, ColumnProperty.NotNull, 0m));
        }

        public override void Down()
        {
            Database.RemoveColumn("regop_pers_paydoc_snap", "base_tariff_sum");
            Database.RemoveColumn("regop_pers_paydoc_snap", "dec_tariff_sum");
            Database.RemoveColumn("regop_pers_paydoc_snap", "penalty_sum");
        }
    }
}
