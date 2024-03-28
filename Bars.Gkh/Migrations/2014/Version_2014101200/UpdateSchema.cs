namespace Bars.Gkh.Migrations.Version_2014101200
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014101200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014093000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // Закментировал потому что в более поздних миграциях уже вьюъи с новыми колонками и снуля ненакатываются
            //ViewManager.Create(Database, "Gkh");

            //ViewManager.CreateAll(Database);
        }

        public override void Down()
        {
        }
    }
}