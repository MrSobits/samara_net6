namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2014050500
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014050500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2014043000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_PRIOR_PARAM_MULTI", new Column("STORED_VALUES", DbType.String, 10000));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_PRIOR_PARAM_MULTI", "STORED_VALUES");
        }
    }
}