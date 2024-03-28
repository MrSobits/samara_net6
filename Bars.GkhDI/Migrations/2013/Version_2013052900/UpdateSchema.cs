namespace Bars.GkhDi.Migrations.Version_2013052900
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013052900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2013052200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // Добавил колонку дата учета в периоде
            Database.AddColumn("DI_DICT_PERIOD", new Column("DATE_ACCOUNTING", DbType.Date));
        }

        public override void Down()
        {
            Database.RemoveColumn("DI_DICT_PERIOD", "DATE_ACCOUNTING");
        }
    }
}