namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014093000
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014093000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014092901.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_MONEY_LOCK",
                new Column("AMOUNT", DbType.Decimal, ColumnProperty.NotNull),
                new Column("IS_ACTIVE", DbType.Boolean, true),
                new Column("LOCK_GUID", DbType.String, 40, ColumnProperty.NotNull),
                new Column("TARGET_GUID", DbType.String, 40, ColumnProperty.Null),
                new RefColumn("WALLET_ID", ColumnProperty.NotNull, "MONEY_LOCK_WALLET", "REGOP_WALLET", "ID"),
                new RefColumn("OPERATION_ID", ColumnProperty.NotNull, "MONEY_LOCK_OPERATION", "REGOP_MONEY_OPERATION", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_MONEY_LOCK");
        }
    }
}
