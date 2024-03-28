namespace Bars.Gkh.Decisions.Nso.Migrations.Version_2014040100
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014040100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Decisions.Nso.Migrations.Version_2014032000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("REGOP_DEC_NOTIF", new RefColumn("STATE_ID", ColumnProperty.Null, "REGOP_DECNOTIF_STATE", "B4_STATE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_DEC_NOTIF", "STATE_ID");
        }
    }
}