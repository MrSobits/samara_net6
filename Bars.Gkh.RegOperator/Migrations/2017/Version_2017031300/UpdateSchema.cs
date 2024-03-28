namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017031300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// 2017031300
    /// </summary>
    [Migration("2017031300")]
    [MigrationDependsOn(typeof(Version_2017021000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddJoinedSubclassTable("REGOP_PAYMENT_IMPORT_SOURCE","REGOP_PAYMENT_OPERATION_BASE","REGOP_PAYMENT_IMPORT_SOURCE_ID");

            this.Database.AddEntityTable("REGOP_PAYMENTS_IMPORT",
                new Column("PAYMENT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DECISION_PAYMENT", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("TARIFF_PAYMENT", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("PENALTY_PAYMENT", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("PAYMENT_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new Column("REGISTRY_NUM", DbType.String),
                new Column("REGISTRY_DATE", DbType.DateTime),
                new RefColumn("ACCOUNT_ID", ColumnProperty.NotNull, "ACCOUNT_PAYMENT_ID", "REGOP_PERS_ACC", "ID"),
                new RefColumn("PAYMENT_OP_ID", ColumnProperty.NotNull, "PAYMENT_IMPORT_OP_ID", "REGOP_PAYMENT_OPERATION_BASE", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("REGOP_PAYMENT_IMPORT_SOURCE");
            this.Database.RemoveTable("REGOP_PAYMENTS_IMPORT");
        }
    }
}
