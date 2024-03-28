namespace Bars.GkhDi.Migrations.Version_2013032700
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013032700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2013032600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // Изменил колонку
            Database.RemoveColumn("DI_DISINFO_FIN_REPAIR_CAT", "PERIOD_SERVICE");
            Database.AddColumn("DI_DISINFO_FIN_REPAIR_CAT", new Column("PERIOD_SERVICE", DbType.Int32));
        }

        public override void Down()
        {
        }
    }
}