namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2014030500
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014030500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2014020700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // перенесли на RegOperator
            //Database.AddColumn("OVRHL_REG_OP_CALC_ACC", new Column("IS_SPECIAL", DbType.Boolean, false));
        }

        public override void Down()
        {
            //Database.RemoveColumn("OVRHL_REG_OP_CALC_ACC", "IS_SPECIAL");
        }
    }
}