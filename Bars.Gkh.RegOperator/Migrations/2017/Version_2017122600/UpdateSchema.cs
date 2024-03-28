namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017122600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2017122600")]
    [MigrationDependsOn(typeof(Version_2017121901.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddEntityTable("REGOP_PERS_ACC_CALC_DEBT",
                new RefColumn("ACCOUNT_ID", ColumnProperty.NotNull, "REGOP_CALC_DEBT_PERS_ACC", "REGOP_PERS_ACC", "ID"),
                new RefColumn("PREV_OWNER_ID", ColumnProperty.NotNull, "REGOP_CALC_DEBT_OWNER", "REGOP_PERS_ACC_OWNER", "ID"),
                new FileColumn("DOCUMENT_ID", ColumnProperty.None, "REGOP_CALC_DEBT_DOCUMENT"),
                new Column("START_DATE", DbType.DateTime),
                new Column("END_DATE", DbType.DateTime),
                new Column("AGREEMENT_NUMBER", DbType.String)
            );

            this.Database.AddEntityTable("REGOP_CALC_DEBT_DETAIL",
                new RefColumn("CALC_DEBT_ID", ColumnProperty.NotNull, "REGOP_CALC_DEBT_DETAIL_DEBT", "REGOP_PERS_ACC_CALC_DEBT", "ID"),
                new RefColumn("OWNER_ID", ColumnProperty.NotNull, "REGOP_CALC_DEBT_DTL_OWNER", "REGOP_PERS_ACC_OWNER", "ID"),
                new Column("TYPE", DbType.Int32),
                new Column("CHARGE_BASE_TARIFF", DbType.Decimal),
                new Column("CHARGE_DEC_TARIFF", DbType.Decimal),
                new Column("CHARGE_PENALTY", DbType.Decimal),
                new Column("DISTR_DEBT_BASE_TARIFF", DbType.Decimal),
                new Column("DISTR_DEBT_DEC_TARIFF", DbType.Decimal),
                new Column("DISTR_DEBT_PENALTY", DbType.Decimal),
                new Column("DISTR_PAY_BASE_TARIFF", DbType.Decimal),
                new Column("DISTR_PAY_DEC_TARIFF", DbType.Decimal),
                new Column("DISTR_PAY_PENALTY", DbType.Decimal),
                new Column("PAYMENT_BASE_TARIFF", DbType.Decimal),
                new Column("PAYMENT_DEC_TARIFF", DbType.Decimal),
                new Column("PAYMENT_PENALTY", DbType.Decimal),
                new Column("SALDO_OUT_BASE_TARIFF", DbType.Decimal),
                new Column("SALDO_OUT_DEC_TARIFF", DbType.Decimal),
                new Column("SALDO_OUT_PENALTY", DbType.Decimal)
            );
        }

        public override void Down()
        {
            this.Database.RemoveTable("REGOP_CALC_DEBT_DETAIL");
            this.Database.RemoveTable("REGOP_PERS_ACC_CALC_DEBT");
        }
    }
}