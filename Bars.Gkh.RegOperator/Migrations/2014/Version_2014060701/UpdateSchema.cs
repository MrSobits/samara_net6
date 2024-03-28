namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014060701
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014060701")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014060700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_PAYMENT_DOC_INFO", new Column("IS_FOR_REGION", DbType.Boolean, false));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PAYMENT_DOC_INFO", "IS_FOR_REGION");
        }
    }
}
