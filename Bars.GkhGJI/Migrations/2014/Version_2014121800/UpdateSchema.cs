using System.Data;

namespace Bars.GkhGji.Migrations._2014.Version_2014121800
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014121800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations._2014.Version_2014121700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DICT_VIOLATION", new Column("NPD_NAME", DbType.String, 2000));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DICT_VIOLATION", "NPD_NAME");
        }
    }
}