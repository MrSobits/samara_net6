namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013092500
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013092500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013092401.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // перенесли на RegOperator
            //Database.AddEntityTable("OVRHL_REG_OPERATOR",
            //    new RefColumn("CONTRAGENT_ID", ColumnProperty.NotNull, "OVRHL_REG_OPER_CNTR", "GKH_CONTRAGENT", "ID"));

            //Database.AddEntityTable("OVRHL_REG_OPERATOR_MU",
            //    new RefColumn("REG_OPERATOR_ID", ColumnProperty.NotNull, "OVRHL_REG_OPERATOR_MU_OPER", "OVRHL_REG_OPERATOR", "ID"),
            //    new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "OVRHL_REG_OPERATOR_MU_MU", "GKH_DICT_MUNICIPALITY", "ID"));
        }

        public override void Down()
        {
            //Database.RemoveEntityTable("OVRHL_REG_OPERATOR_MU");
            //Database.RemoveEntityTable("OVRHL_REG_OPERATOR");
        }
    }
}