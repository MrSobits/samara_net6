namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014120100
{
    using System.Data;
    using Bars.Gkh.Utils;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014120100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014112900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("REGOP_FS_IMPORT_MAP_ITEM", new Column("PROPERTY_NAME", DbType.String, 100));
            Database.AlterColumnSetNullable("REGOP_FS_IMPORT_MAP_ITEM", "PROPERTY_NAME", false);
        }

        public override void Down()
        {
        }
    }
}
