namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013112100
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013112100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013111200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_REAL_EST_TYPE_RATE", new Column("YEAR", DbType.Int32, (object) 2014));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_REAL_EST_TYPE_RATE", "YEAR");
        }
    }
}