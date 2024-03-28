namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014102701
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014102701")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014102700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_PAYMENT_PENALTIES", new Column("DECISION_TYPE", DbType.Int32, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PAYMENT_PENALTIES", "DECISION_TYPE");
        }
    }
}
