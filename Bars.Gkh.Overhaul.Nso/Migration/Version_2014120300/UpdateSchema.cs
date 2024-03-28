namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2014120300
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014120300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2014100500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddPersistentObjectTable(
                "OVRHL_PRIOR_PAR_ADDITION",
                new Column("CODE", DbType.String, 100),
                new Column("FACTOR_VALUE", DbType.Decimal),
                new Column("ADDITION_FACTOR", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("FINAL_VALUE", DbType.Int32, 4, ColumnProperty.NotNull, 10));
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_PRIOR_PAR_ADDITION");
        }
    }
}