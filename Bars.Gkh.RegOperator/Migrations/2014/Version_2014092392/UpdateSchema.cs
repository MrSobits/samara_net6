namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014092392
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014092392")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014092391.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RenameTable("REG_OP_SUSPEN_ACCOUNT", "REGOP_SUSPENSE_ACCOUNT");
            Database.AddColumn("REGOP_SUSPENSE_ACCOUNT", "DCODE", DbType.String, 50);
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_SUSPENSE_ACCOUNT", "DCODE");
            Database.RenameTable("REGOP_SUSPENSE_ACCOUNT", "REG_OP_SUSPEN_ACCOUNT");
        }
    }
}
