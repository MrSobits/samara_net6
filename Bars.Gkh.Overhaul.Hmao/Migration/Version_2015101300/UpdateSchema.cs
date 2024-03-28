namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2015101300
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015101300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2015091400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_ACTUALIZE_LOG", new Column("PROGRAM_CR_NAME", DbType.String, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_ACTUALIZE_LOG", "PROGRAM_CR_NAME");
        }
    }
}