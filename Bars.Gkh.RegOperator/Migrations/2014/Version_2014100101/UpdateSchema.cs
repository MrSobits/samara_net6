namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014100101
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014100101")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014100100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("REGOP_MONEY_LOCK",
                new RefColumn("CANCEL_OPERATION_ID", ColumnProperty.Null, "MLOCK_CANCEL_OP", "REGOP_MONEY_OPERATION", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_MONEY_LOCK", "CANCEL_OPERATION_ID");
        }
    }
}
