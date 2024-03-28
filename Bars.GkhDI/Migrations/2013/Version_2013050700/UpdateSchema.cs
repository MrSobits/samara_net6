namespace Bars.GkhDi.Migrations.Version_2013050700
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013050700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2013041900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // Поменял дублирующие названия
            Database.RemoveColumn("DI_BASE_SERVICE", "DATE_START");
            Database.AddColumn("DI_BASE_SERVICE", new Column("DATE_START_TARIFF", DbType.Date));
        }

        public override void Down()
        {
            // не нужно
        }
    }
}