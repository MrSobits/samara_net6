namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014092397
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014092397")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014092396.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveTable("REGOP_WALLET_OPERATION");
            Database.AddColumn("REGOP_TRANSFER", new Column("PAYMENT_DATE", DbType.DateTime, ColumnProperty.NotNull));
            Database.AddColumn("REGOP_TRANSFER", new Column("OPERATION_DATE", DbType.DateTime, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            Database.AddEntityTable("REGOP_WALLET_OPERATION");
            Database.RemoveColumn("REGOP_TRANSFER", "OPERATION_DATE");
            Database.RemoveColumn("REGOP_TRANSFER", "PAYMENT_DATE");
        }
    }
}
