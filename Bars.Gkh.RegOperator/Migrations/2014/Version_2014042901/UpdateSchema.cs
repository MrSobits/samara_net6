namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014042901
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014042901")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014042900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_SUSPACC_RENTPAY", 
                new RefColumn("SUSPACC_ID", ColumnProperty.NotNull, "REGOP_SUSPACC_RENTPAY_SA", "REG_OP_SUSPEN_ACCOUNT", "ID"),
                new RefColumn("PAYMENT_ID", ColumnProperty.NotNull, "REGOP_SUSPACC_RENTPAY_RP", "REGOP_RENT_PAYMENT_IN", "ID"));

            Database.AddEntityTable("REGOP_SUSPACC_ACCFUNDS",
                new RefColumn("SUSPACC_ID", ColumnProperty.NotNull, "REGOP_SUSPACC_ACCFUNDS_SA", "REG_OP_SUSPEN_ACCOUNT", "ID"),
                new RefColumn("ACCUMFUNDS_ID", ColumnProperty.NotNull, "REGOP_SUSPACC_ACCFUNDS_AF", "REGOP_ACCUMULATED_FUNDS", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_SUSPACC_RENTPAY");

            Database.RemoveTable("REGOP_SUSPACC_ACCFUNDS");
        }
    }
}
