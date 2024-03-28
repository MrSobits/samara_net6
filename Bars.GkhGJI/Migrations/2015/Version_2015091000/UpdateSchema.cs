namespace Bars.GkhGji.Migration.Version_2015091000
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015091000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations._2015.Version_2015090700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DICT_PLANJURPERSON", "DATE_APPROVAL", DbType.DateTime);
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DICT_PLANJURPERSON", "DATE_APPROVAL");
        }
    }
}