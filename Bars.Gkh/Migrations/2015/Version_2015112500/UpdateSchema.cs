namespace Bars.Gkh.Migrations._2015.Version_2015112500
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015112500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015112400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_CIT_SUG_COMMENT", new Column("HAS_ANSWER", DbType.Boolean, false));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_CIT_SUG_COMMENT", "HAS_ANSWER");
        }
    }
}
