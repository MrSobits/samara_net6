namespace Bars.GkhGji.Migrations._2014.Version_2014112500
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014112500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations._2014.Version_2014111400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.TableExists("GJI_DICT_VIOLATION_NORMATIVEDOCITEM"))
            {
                Database.RenameTable("GJI_DICT_VIOLATION_NORMATIVEDOCITEM", "GJI_DICT_VIOL_NORMDITEM");
            }
        }

        public override void Down()
        {
            if (Database.TableExists("GJI_DICT_VIOL_NORMDITEM"))
            {
                Database.RenameTable("GJI_DICT_VIOL_NORMDITEM", "GJI_DICT_VIOLATION_NORMATIVEDOCITEM");
            }
        }
    }
}
