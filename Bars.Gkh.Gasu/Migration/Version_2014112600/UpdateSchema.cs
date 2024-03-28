namespace Bars.Gkh.Gasu.Migration.Version_2014112600
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014112600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Gasu.Migration.Version_1.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GASU_INDICATOR_VALUE", new Column("PERIOD_START", DbType.DateTime, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            Database.RemoveColumn("GASU_INDICATOR_VALUE", "PERIOD_START");
        }
    }
}