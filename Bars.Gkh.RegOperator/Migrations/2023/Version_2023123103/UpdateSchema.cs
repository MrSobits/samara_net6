namespace Bars.Gkh.RegOperator.Migrations._2023.Version_2023123103
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023123103")]

    [MigrationDependsOn(typeof(Version_2023123102.UpdateSchema))]
    // Является Version_2020072400 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            //this.Database.AddEntityTable("REGOP_LAWSUIT_REFERENCE_CALCULATION",
            //  new Column("ACC_NUM", DbType.String, ColumnProperty.NotNull),
            //  new Column("AREA_SHARE", DbType.Decimal, ColumnProperty.NotNull),
            //  new Column("BASE_TARIF", DbType.Decimal, ColumnProperty.NotNull),
            //  new Column("PERIOD_ID", DbType.Int64, ColumnProperty.NotNull),
            //  new Column("ACCOUNT_ID", DbType.Int64, ColumnProperty.NotNull),
            //  new Column("ROOM_AREA", DbType.Decimal, ColumnProperty.NotNull),
            //  new Column("TARIF_DEBT", DbType.Decimal, ColumnProperty.NotNull),
            //  new Column("TARIF_CHARGED", DbType.Decimal, ColumnProperty.NotNull),
            //  new Column("TARIF_PAYMENTS", DbType.Decimal, ColumnProperty.NotNull),
            //  new Column("PAYMENT_DATE", DbType.String, ColumnProperty.None),
            //  new Column("DESCRIPTION", DbType.String, ColumnProperty.None),
            //  new RefColumn("LAWSUIT_ID", ColumnProperty.NotNull, "LAWSUIT_REFERENCE_CALCULATION_LAWSUIT", "CLW_LAWSUIT", "ID")
            //);
        }
        public override void Down()
        {
            //this.Database.RemoveTable("REGOP_LAWSUIT_REFERENCE_CALCULATION");
        }
    }
}
