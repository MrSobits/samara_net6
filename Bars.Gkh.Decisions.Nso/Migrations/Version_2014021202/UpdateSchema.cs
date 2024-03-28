namespace Bars.Gkh.Decisions.Nso.Migrations.Version_2014021202
{
    using Bars.Gkh.Utils;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021202")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Decisions.Nso.Migrations.Version_2014021201.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AlterColumnSetNullable("DEC_ULTIMATE_DECISION", "PROTOCOL_ID", true);
        }

        public override void Down()
        {

        }
    }
}