namespace Bars.Gkh.Diagnostic.Migrations.Version_2015120200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015120200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Diagnostic.Migrations.Version_1.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("GKH_COLLECTED_DIAGNOSTIC_RESULT", "SUCCESS");
            Database.RemoveColumn("GKH_DIAGNOSTIC_RESULT", "SUCCESS");
            Database.AddColumn("GKH_COLLECTED_DIAGNOSTIC_RESULT", new Column("STATE", DbType.Int32, defaultValue: 30));
            Database.AddColumn("GKH_DIAGNOSTIC_RESULT", new Column("STATE", DbType.Int32, defaultValue: 40));
            Database.AddColumn("GKH_DIAGNOSTIC_RESULT", new Column("Name", DbType.String));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_COLLECTED_DIAGNOSTIC_RESULT", "STATE");
            Database.RemoveColumn("GKH_DIAGNOSTIC_RESULT", "STATE");
            Database.AddColumn("GKH_COLLECTED_DIAGNOSTIC_RESULT", new Column("SUCCESS", DbType.Boolean));
            Database.AddColumn("GKH_DIAGNOSTIC_RESULT", new Column("SUCCESS", DbType.Boolean));
        }
    }
}