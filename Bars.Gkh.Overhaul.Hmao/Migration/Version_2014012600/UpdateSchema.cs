namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2014012600
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014012600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2014012100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_SUBSIDY_REC", new Column("DATE_CALC_OWN", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_SUBSIDY_REC", "DATE_CALC_OWN");
        }
    }
}