namespace Bars.GkhDi.Migrations.Version_2013041200
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013041200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2013041001.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("DI_BASE_SERVICE", new Column("TARIFF", DbType.Decimal));
            Database.AddColumn("DI_BASE_SERVICE", new Column("TARIFF_SET_FOR", DbType.Int32, 4, ColumnProperty.NotNull, 30));
            Database.AddColumn("DI_BASE_SERVICE", new Column("DATE_START", DbType.Date));
        }

        public override void Down()
        {
            Database.RemoveColumn("DI_BASE_SERVICE", "TARIFF");
            Database.RemoveColumn("DI_BASE_SERVICE", "TARIFF_SET_FOR");
            Database.RemoveColumn("DI_BASE_SERVICE", "DATE_START");
        }
    }
}