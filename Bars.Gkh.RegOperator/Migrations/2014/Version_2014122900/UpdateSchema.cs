namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014122900
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Gkh.Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014122900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014122000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_FS_IMPORT_INFO", new Column("DELIMITER", DbType.String, 10, ColumnProperty.Null));
            Database.AddColumn("REGOP_FS_IMPORT_MAP_ITEM", new Column("USE_FILENAME", DbType.Boolean, ColumnProperty.Null));
            Database.AddColumn("REGOP_FS_IMPORT_MAP_ITEM", new Column("FORMAT", DbType.String, 100, ColumnProperty.Null));

            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery("update REGOP_FS_IMPORT_MAP_ITEM set USE_FILENAME = false where USE_FILENAME is null");
            }
            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery("update REGOP_FS_IMPORT_MAP_ITEM set USE_FILENAME = 'f' where USE_FILENAME is null");
            }

            Database.AlterColumnSetNullable("REGOP_FS_IMPORT_MAP_ITEM", "USE_FILENAME", false);
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_FS_IMPORT_MAP_ITEM", "USE_FILENAME");
            Database.RemoveColumn("REGOP_FS_IMPORT_MAP_ITEM", "FORMAT");
            Database.RemoveColumn("REGOP_FS_IMPORT_INFO", "DELIMITER");
        }
    }
}
