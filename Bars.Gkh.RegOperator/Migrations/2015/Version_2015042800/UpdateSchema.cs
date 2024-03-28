namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015042800
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015042800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015042000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //не удалять миграцию
            /*ViewManager.Drop(Database, "Regop");

            ViewManager.Create(Database, "Regop");*/
        }

        public override void Down()
        {
            //ViewManager.Drop(Database, "Regop");
        }
    }
}
