namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015051900
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015051900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015051800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_PAYMENT_DOC_INFO", new Column("FUND_FORM_TYPE", DbType.Int32));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PAYMENT_DOC_INFO", "FUND_FORM_TYPE");
        }
    }
}
