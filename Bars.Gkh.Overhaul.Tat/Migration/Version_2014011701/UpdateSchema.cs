namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2014011701
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014011701")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2014011700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_PROP_OWN_PROTOCOLS", new Column("TYPE_PROTOCOL", DbType.Int32, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "TYPE_PROTOCOL");
        }
    }
}