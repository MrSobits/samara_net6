namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015060801
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015060801")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015060800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (!Database.ColumnExists("REGOP_CASHPAYMENT_CENTER", "SHOW_PERSONAL_DATA"))
            {
                Database.AddColumn("REGOP_CASHPAYMENT_CENTER", new Column("SHOW_PERSONAL_DATA", DbType.Boolean, ColumnProperty.NotNull, true));
            }
        }

        public override void Down()
        {
            if (Database.ColumnExists("REGOP_CASHPAYMENT_CENTER", "SHOW_PERSONAL_DATA"))
            {
                Database.RemoveColumn("REGOP_CASHPAYMENT_CENTER", "SHOW_PERSONAL_DATA");
            }
        }
    }
}
