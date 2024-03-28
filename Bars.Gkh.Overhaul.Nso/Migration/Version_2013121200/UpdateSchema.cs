namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013121200
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013121200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013121101.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // перенесен в модуль overhaul
            //if (Database.For<PostgreSQLDialect>().ColumnExists("OVRHL_ACCOUNT", "NUMBER"))
            //{
            //    Database.RenameColumn("OVRHL_ACCOUNT", "NUMBER", "ACC_NUMBER");
            //}
        }

        public override void Down()
        {

        }
    }
}