namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014031801
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014031801")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014031800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_RO_PAYMENT_ACCOUNT", new Column("ACC_NUM", DbType.String, 20, ColumnProperty.Null));
            Database.AddColumn("REGOP_RO_SUPP_ACC", new Column("ACC_NUM", DbType.String, 20, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "ACC_NUM");
            Database.RemoveColumn("REGOP_RO_SUPP_ACC", "ACC_NUM");
        }
    }
}
