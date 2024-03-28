namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015032001
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015032001")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015032000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_PAY_PEN_EXC_PA", 
                new RefColumn("PAY_PENALTIES_ID", "REG_PAY_PEN_PAY_PEN", "REGOP_PAYMENT_PENALTIES", "ID"),
                new RefColumn("PERS_ACC_ID", "REG_PAY_PEN_PERS_ACC", "REGOP_PERS_ACC", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_PAY_PEN_EXC_PA");
        }
    }
}
