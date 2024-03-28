namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014020901
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014020901")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014020900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REG_OP_SUSPEN_ACCOUNT",
                new Column("DATE_RECEIPT", DbType.DateTime, ColumnProperty.Null),
                new Column("SUSPEN_ACCOUNT_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new Column("ACCOUNT_BENEFICIARY", DbType.String, ColumnProperty.Null),
                new Column("SUM", DbType.Decimal, ColumnProperty.NotNull),
                new Column("DETAILS_OF_PAYMENT", DbType.String, ColumnProperty.Null),
                new Column("SUSPEN_ACCOUNT_STATUS", DbType.Int32, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            Database.RemoveTable("REG_OP_SUSPEN_ACCOUNT");
        }
    }
}
