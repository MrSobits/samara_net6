namespace Bars.Gkh.RegOperator.Migrations._2023.Version_2023041900
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2023041900")]

    [MigrationDependsOn(typeof(_2023.Version_2023031400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("REGOP_LAWSUIT_REFERENCE_CALCULATION", new Column("TARIF_DEBTPAY", DbType.Decimal, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_LAWSUIT_REFERENCE_CALCULATION", "TARIF_DEBTPAY");
        }
    }
}
