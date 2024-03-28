namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014032100
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014032100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014031900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_SUSPACC_ACTPAYMENT",
                new RefColumn("ACTPAY_ID", ColumnProperty.NotNull, "REGOP_SUSPACTACTPAY_ACT", "CR_OBJ_PER_ACT_PAYMENT", "ID"),
                new RefColumn("SUSPACC_ID", ColumnProperty.NotNull, "REGOP_SUSPACTACTPAY_SUSP", "REG_OP_SUSPEN_ACCOUNT", "ID")
                );
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_SUSPACC_ACTPAYMENT");
        }
    }
}
