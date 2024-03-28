namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014100201
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014100201")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014100200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_MONEY_OPERATION", new Column("OPERATION_DATE", DbType.DateTime));
            Database.AddColumn("REGOP_MONEY_OPERATION", new Column("AMOUNT", DbType.Decimal, ColumnProperty.NotNull, 0M));
            Database.AddColumn("REGOP_MONEY_OPERATION", new Column("REASON", DbType.String, 1000));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_MONEY_OPERATION", "OPERATION_DATE");
            Database.RemoveColumn("REGOP_MONEY_OPERATION", "AMOUNT");
            Database.RemoveColumn("REGOP_MONEY_OPERATION", "REASON");
 
        }
    }
}
