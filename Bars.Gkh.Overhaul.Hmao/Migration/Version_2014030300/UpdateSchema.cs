namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2014030300
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014030300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2014021000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                    "OVRHL_PLAN_COLL_INF",
                    new Column("YEAR", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique, 0),
                    new Column("PLAN_OWN_PRC", DbType.Decimal, ColumnProperty.NotNull, 0),
                    new Column("NOT_REDUCE_SIZE_PRC", DbType.Decimal, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            this.Database.RemoveTable("OVRHL_PLAN_COLL_INF");
        }
    }
}