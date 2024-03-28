namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014032701
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014032701")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014032700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REG_OP_SUSPEN_ACCOUNT", new Column("MONEY_DIRECTION", DbType.Int16, ColumnProperty.NotNull, 0m));
        }

        public override void Down()
        {
            Database.RemoveColumn("REG_OP_SUSPEN_ACCOUNT", "MONEY_DIRECTION");
        }
    }
}
