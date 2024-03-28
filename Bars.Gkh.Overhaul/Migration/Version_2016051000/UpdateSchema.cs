namespace Bars.Gkh.Overhaul.Migration.Version_2016051000
{
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2016051000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Migration.Version_2016041500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.RemoveColumn("OVRHL_DICT_WORK_PRICE", "SETTLEMENT_ID");
        }

        public override void Down()
        {
            this.Database.AddRefColumn("OVRHL_DICT_WORK_PRICE", new RefColumn("SETTLEMENT_ID", "MUNICIPALITY_ID", "GKH_DICT_MUNICIPALITY", "ID"));
        }
    }
}
