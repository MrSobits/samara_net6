namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014092398
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014092398")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014092397.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_MONEY_OPERATION", new Column("IS_CANCELLED", DbType.Boolean, defaultValue: false));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_MONEY_OPERATION", "IS_CANCELLED");
        }
    }
}
