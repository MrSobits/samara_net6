namespace Bars.GkhDi.Regions.Tatarstan.Migrations.Version_2014022000
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014022000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Regions.Tatarstan.Migrations.Version_2013120600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("DI_TARIFF_FCONSUMERS", new Column("DATE_END", DbType.Date));
            Database.AddColumn("DI_TARIFF_FRSO", new Column("DATE_END", DbType.Date));
        }

        public override void Down()
        {
            Database.RemoveColumn("DI_TARIFF_FCONSUMERS", "DATE_END");
            Database.RemoveColumn("DI_TARIFF_FRSO", "DATE_END");
        }
    }
}