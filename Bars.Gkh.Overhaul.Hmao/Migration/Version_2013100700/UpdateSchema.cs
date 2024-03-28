namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013100700
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013100700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013092501.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("OVRHL_DPKR_PARAMS",
                    new Column("PARAMS", DbType.String, 2000, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_DPKR_PARAMS");
        }
    }
}