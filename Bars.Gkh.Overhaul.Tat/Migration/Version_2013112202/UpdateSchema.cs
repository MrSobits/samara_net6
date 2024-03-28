namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013112202
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013112202")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013112201.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("OVRHL_PRIOR_PARAM_QUANT",
                new Column("CODE", DbType.String, 100, ColumnProperty.NotNull),
                new Column("MAX_VALUE", DbType.String, 100),
                new Column("MIN_VALUE", DbType.String, 100),
                new Column("POINT", DbType.Int32, (object)0));

            Database.AddEntityTable("OVRHL_PRIOR_PARAM_QUALITY",
                new Column("CODE", DbType.String, 100, ColumnProperty.NotNull),
                new Column("TYPE_PRESENCE", DbType.Int16, (object) 0),
                new Column("POINT", DbType.Int32, (object) 0));
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_PRIOR_PARAM_QUANT");

            Database.RemoveTable("OVRHL_PRIOR_PARAM_QUALITY");
        }
    }
}