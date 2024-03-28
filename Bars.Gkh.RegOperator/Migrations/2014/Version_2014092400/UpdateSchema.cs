namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014092400
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014092400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014092399.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_MONEY_OPERATION", new RefColumn("CANCELED_OP_ID", ColumnProperty.Null, "OP_CANCEL_OP_ID", "REGOP_MONEY_OPERATION", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_MONEY_OPERATION", "CANCELED_OP_ID");
        }
    }
}
