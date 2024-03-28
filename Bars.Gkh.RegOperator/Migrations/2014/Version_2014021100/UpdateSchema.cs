namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014021100
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014021000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_PERS_ACC_PAYMENT", new Column("PAYMENT_TYPE", DbType.Int16, ColumnProperty.NotNull, 10));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PERS_ACC_PAYMENT", "PAYMENT_TYPE");
        }
    }
}
