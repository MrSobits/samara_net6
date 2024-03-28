namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017042000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция RegOperator 2017042000
    /// </summary>
    [Migration("2017042000")]
    [MigrationDependsOn(typeof(Version_2017041700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddJoinedSubclassTable(
                "REGOP_SPLIT_ACC_SOURCE", 
                "REGOP_CHARGE_OPERATION_BASE", 
                "SPLIT_ACC_SOURCE_ID",
                new RefColumn("ACCOUNT_ID", ColumnProperty.NotNull, "SPLIT_ACC_SOURCE_ACC_ID", "REGOP_PERS_ACC", "ID"));

            this.Database.AddEntityTable(
                "REGOP_SPLIT_ACC_DETAIL",
                new Column("AMOUNT", DbType.Decimal, ColumnProperty.NotNull),
                new Column("WALLET_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new RefColumn("ACCOUNT_ID", "SPLIT_ACC_DETAIL_ACC_ID", "REGOP_PERS_ACC", "ID"),
                new RefColumn("CHARGE_OP_ID", "SPLIT_ACC_DETAIL_OP_ID", "REGOP_CHARGE_OPERATION_BASE", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("REGOP_SPLIT_ACC_DETAIL");
            this.Database.RemoveTable("REGOP_SPLIT_ACC_SOURCE");
        }
    }
}