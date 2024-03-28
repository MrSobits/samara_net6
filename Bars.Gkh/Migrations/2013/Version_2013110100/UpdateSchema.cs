namespace Bars.Gkh.Migrations.Version_2013110100
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013110100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013103002.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_OBJ_PROTOCOL_MT", new Column("COUNCIL_RESULT", DbType.Int32, defaultValue: 10));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_OBJ_PROTOCOL_MT", "COUNCIL_RESULT");
        }
    }
}