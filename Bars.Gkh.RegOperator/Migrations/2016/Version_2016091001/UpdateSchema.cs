namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016091001
{
    using System.Data;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using Bars.B4.Modules.Ecm7.Framework;
    using B4.Utils;

    /// <summary>
    /// Миграция RegOperator 2016091001
    /// </summary>
    [Migration("2016091001")]
    [MigrationDependsOn(typeof(Version_2016090100.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016090500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("REGOP_SALDO_CHANGE_EXPORT",
                new Column("FILE_NAME", DbType.String, 30, ColumnProperty.NotNull),
                new Column("IMPORTED", DbType.Boolean, ColumnProperty.NotNull),
                new RefColumn("PERIOD_ID", ColumnProperty.NotNull, "SALDO_CHANGE_PERIOD_ID", "REGOP_PERIOD", "ID"));

            this.Database.AddEntityTable("REGOP_SALDO_CHANGE_EXPORT_PA",
                new Column("SALDO_BASE_BEFORE", DbType.Decimal, ColumnProperty.NotNull),
                new Column("SALDO_DEC_BEFORE", DbType.Decimal, ColumnProperty.NotNull),
                new Column("SALDO_PENALTY_BEFORE", DbType.Decimal, ColumnProperty.NotNull),
                new RefColumn("CHANGE_ID", ColumnProperty.NotNull, "EXPORTED_CHANGE_ID", "REGOP_SALDO_CHANGE_EXPORT", "ID"),
                new RefColumn("ACC_ID", ColumnProperty.NotNull, "EXPORTED_ACCOUNT_ID", "REGOP_PERS_ACC", "ID"));

            this.Database.AddTable("REGOP_SALDO_CHAGE_SOURCE",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new RefColumn("EXPORT_SALDO_ID", ColumnProperty.Null, "REGOP_SALDO_CHAGE_SOURCE_SAID", "REGOP_CHARGE_OPERATION_BASE", "ID"),
                new RefColumn("PERIOD_ID", ColumnProperty.NotNull, "REGOP_SALDO_CHAGE_SOURCE_PER_ID", "REGOP_PERIOD", "ID"));

            this.Database.AddForeignKey("FK_REGOP_SALDO_CHAGE_SOURCE", "REGOP_SALDO_CHAGE_SOURCE", "ID", "REGOP_CHARGE_OPERATION_BASE", "ID");

            this.Database.AddEntityTable("REGOP_SALDO_CHANGE_DETAIL",
                new Column("CHANGE_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new Column("OLD_VALUE", DbType.Decimal, ColumnProperty.NotNull),
                new Column("NEW_VALUE", DbType.Decimal, ColumnProperty.NotNull),
                new RefColumn("CHARGE_OP_ID", ColumnProperty.NotNull, "SALDO_CHANGE_CHARGE_OP_ID", "REGOP_CHARGE_OPERATION_BASE", "ID"),
                new RefColumn("ACC_ID", ColumnProperty.NotNull, "SALDO_CHANGE_CHARGE_ACC_ID", "REGOP_PERS_ACC", "ID"));
        }
        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("REGOP_SALDO_CHANGE_DETAIL");
            this.Database.RemoveTable("REGOP_SALDO_CHAGE_SOURCE");
            this.Database.RemoveTable("REGOP_SALDO_CHANGE_EXPORT_PA");
            this.Database.RemoveTable("REGOP_SALDO_CHANGE_EXPORT");
        }
    }
}
