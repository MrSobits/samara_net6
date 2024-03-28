namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013121600
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013121600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013121300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // перенесли на RegOperator
            //Database.RemoveForeignKey("OVRHL_REG_OP_CALC_ACC", "FK_OV_REG_OP_CA_REG_OP");
            //Database.AddForeignKey("FK_OV_REG_OP_CA_REG_OP", "OVRHL_REG_OP_CALC_ACC", "REG_OP_ID", "OVRHL_REG_OPERATOR", "ID");

            //Database.ChangeColumn("OVRHL_REG_OP_CALC_ACC", new Column("ACC_NUMBER", DbType.String, 50));
        }

        public override void Down()
        {
        }
    }
}