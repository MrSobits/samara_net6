namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013121001
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2013121001")]
    [MigrationDependsOn(typeof(Version_2013121000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "OVRHL_PRIOR_PARAM_QUANT",
                new Column("CODE", DbType.String, 100, ColumnProperty.NotNull),
                new Column("MAX_VALUE", DbType.String, 100),
                new Column("MIN_VALUE", DbType.String, 100),
                new Column("POINT", DbType.Int32, (object) 0));

            this.Database.AddEntityTable(
                "OVRHL_PRIOR_PARAM_QUALITY",
                new Column("CODE", DbType.String, 100, ColumnProperty.NotNull),
                new Column("TYPE_PRESENCE", DbType.Int16, (object) 0),
                new Column("POINT", DbType.Int32, (object) 0));
        }

        public override void Down()
        {
            this.Database.RemoveTable("OVRHL_PRIOR_PARAM_QUANT");

            this.Database.RemoveTable("OVRHL_PRIOR_PARAM_QUALITY");
        }
    }
}