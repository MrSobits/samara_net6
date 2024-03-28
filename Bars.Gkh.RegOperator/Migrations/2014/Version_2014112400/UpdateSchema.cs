using System.Data;
using global::Bars.B4.Modules.Ecm7.Framework;

namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014112400
{
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014112400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014112100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_FS_IMPORT_MAP_ITEM", new Column("REGEX_VAL", DbType.String, 500, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_FS_IMPORT_MAP_ITEM", "REGEX_VAL");
        }
    }
}
