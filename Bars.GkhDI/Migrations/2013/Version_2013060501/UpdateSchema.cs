namespace Bars.GkhDi.Migrations.Version_2013060501
{
    using global::Bars.B4.Modules.Ecm7.Framework;
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013060501")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2013060500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddUniqueConstraint("UNQ_PERC_SERV_ID", "DI_PERC_SERVICE", "SERVICE_ID");
        }

        public override void Down()
        {
            Database.RemoveConstraint("DI_PERC_SERVICE", "UNQ_PERC_SERV_ID");
        }
    }
}