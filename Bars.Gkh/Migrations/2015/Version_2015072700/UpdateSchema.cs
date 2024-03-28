namespace Bars.Gkh.Migrations.Version_2015072700
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015072702")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2015070900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_REALITY_OBJECT", new Column("NONRES_PREMISES", DbType.Int32));
            Database.AddColumn("GKH_REALITY_OBJECT",
                new Column("AREA_NL_PREMISES", new ColumnType(DbType.Decimal, 5, 2)));
        }

        public override void Down()
        {
        }
    }
}
