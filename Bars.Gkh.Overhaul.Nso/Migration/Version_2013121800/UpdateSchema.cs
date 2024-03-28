namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013121800
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013121800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013121702.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_ACCOUNT_OPERATION", new Column("OPER_NUMBER", DbType.String, 50));
            Database.ChangeColumn("OVRHL_ACCOUNT_OPERATION", new Column("PURPOSE", DbType.String, 500));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_ACCOUNT_OPERATION", "OPER_NUMBER");
        }
    }
}