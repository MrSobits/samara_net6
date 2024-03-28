namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013122000
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013122000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013121900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // перенесли в модуль RegOperator
            // Database.AddColumn("OVRHL_REG_OP_PERS_ACC_MU", new Column("ACC_NUMBER", DbType.String, 50));
        }

        public override void Down()
        {
            // Database.RemoveColumn("OVRHL_REG_OP_PERS_ACC_MU", "ACC_NUMBER");
        }
    }
}