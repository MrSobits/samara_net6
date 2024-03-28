namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013121000
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013121000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013120900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("OVRHL_PRIOR_PARAM_MULTI",
                new Column("CODE", DbType.String, 100, ColumnProperty.NotNull),
                new Column("VALUE", DbType.String, 300, ColumnProperty.NotNull),
                new Column("POINT", DbType.Decimal, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_PRIOR_PARAM_MULTI");
        }
    }
}