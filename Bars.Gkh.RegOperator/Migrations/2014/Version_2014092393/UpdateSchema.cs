namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014092393
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014092393")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014092392.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_WALLET_OPERATION", new Column("DIRECTION", DbType.Int16, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_WALLET_OPERATION", "DIRECTION");
        }
    }
}
