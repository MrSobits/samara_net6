namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014082000
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014082000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014081900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_IMPORTED_PAYMENT", new Column("PAYMENT_ID", DbType.Int64));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_IMPORTED_PAYMENT", "PAYMENT_ID");
        }
    }
}
