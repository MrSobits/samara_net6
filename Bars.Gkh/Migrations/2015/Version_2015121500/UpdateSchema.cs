namespace Bars.Gkh.Migrations._2015.Version_2015121500
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015121500")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_CIT_SUG", new Column("TEST_SUGG", DbType.Boolean, false));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_CIT_SUG", "TEST_SUGG");
        }
    }
}
