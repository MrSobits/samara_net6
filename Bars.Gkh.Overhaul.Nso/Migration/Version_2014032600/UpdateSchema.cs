namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2014032600
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014032600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2014031700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // перенесли на RegOperator
            //Database.RemoveRefColumn("OVRHL_REG_OP_CALC_ACC", "CREDIT_ORG_ID");
            //Database.AddRefColumn("OVRHL_REG_OP_CALC_ACC", new RefColumn("CA_BANK_ID", ColumnProperty.Null, "OVRHL_REGOP_CACC_CAB", "GKH_CONTRAGENT_BANK", "ID"));

            //Database.AddEntityTable("REGOP_CALCACC_RO",
            //    new RefColumn("RO_ID", ColumnProperty.NotNull, "REGOP_CALC_ACC_RO_RO", "GKH_REALITY_OBJECT", "ID"),
            //    new RefColumn("REGOP_CALC_ACC_ID", ColumnProperty.NotNull, "REGOP_CALC_ACC_RO_RO", "OVRHL_REG_OP_CALC_ACC", "ID")
            //    );
        }

        public override void Down()
        {
            //Database.RemoveEntityTable("REGOP_CALCACC_RO");

            //Database.RemoveRefColumn("OVRHL_REG_OP_CALC_ACC", "CA_BANK_ID");
            //Database.AddRefColumn("OVRHL_REG_OP_CALC_ACC", new RefColumn("CREDIT_ORG_ID", ColumnProperty.Null, "OV_REG_OP_CA_CR_ORG", "OVRHL_CREDIT_ORG", "ID"));
        }
    }
}