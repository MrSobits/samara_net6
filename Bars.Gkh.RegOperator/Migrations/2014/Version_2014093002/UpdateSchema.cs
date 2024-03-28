namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014093002
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014093002")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014093001.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_MONEY_LOCK", "LOCK_DATE", DbType.DateTime);
            Database.AddColumn("REGOP_MONEY_LOCK", "UNLOCK_DATE", DbType.DateTime);
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_MONEY_LOCK", "LOCK_DATE");
            Database.RemoveColumn("REGOP_MONEY_LOCK", "UNLOCK_DATE");
        }
    }
}
