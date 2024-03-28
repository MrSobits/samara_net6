namespace Bars.Gkh.Migrations._2017.Version_2017101500
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017101500")]
    [MigrationDependsOn(typeof(Version_2017091100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn("CLW_PRETENSION", "DEBT_BASE_TARIFF_SUM", DbType.Decimal, ColumnProperty.NotNull, 0);
            this.Database.AddColumn("CLW_PRETENSION", "DEBT_DECISION_TARIFF_SUM", DbType.Decimal, ColumnProperty.NotNull, 0);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("CLW_PRETENSION", "DEBT_BASE_TARIFF_SUM");
            this.Database.RemoveColumn("CLW_PRETENSION", "DEBT_DECISION_TARIFF_SUM");
        }
    }
}