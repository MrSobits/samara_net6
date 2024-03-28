namespace Bars.Gkh.Diagnostic.Migrations.Version_1
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_COLLECTED_DIAGNOSTIC_RESULT", new Column("SUCCESS", DbType.Boolean));

            Database.AddEntityTable(
                "GKH_DIAGNOSTIC_RESULT",
                new Column("SUCCESS", DbType.Boolean),
                new Column("MESSAGE", DbType.Binary),
                new RefColumn(
                    "COLLECTED_DIAGNOSTIC_ID",
                    "FK_COLLECTED_DIAGNOSTIC_DIAGNOSTIC_RES",
                    "GKH_COLLECTED_DIAGNOSTIC_RESULT",
                    "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_DIAGNOSTIC_RESULT");

            Database.RemoveTable("GKH_COLLECTED_DIAGNOSTIC_RESULT");
        }
    }
}