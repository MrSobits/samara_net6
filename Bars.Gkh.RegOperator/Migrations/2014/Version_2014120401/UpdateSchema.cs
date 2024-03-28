namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014120401
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014120401")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014120400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("RF_TRANSFER_CTR", new Column("PAYMENT_DATE", DbType.Date));
        }

        public override void Down()
        {
            Database.RemoveColumn("RF_TRANSFER_CTR", "PAYMENT_DATE");
        }
    }
}
