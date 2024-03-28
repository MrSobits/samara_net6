namespace Bars.Gkh.Migrations._2015.Version_2015090701
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015090701")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migration.Version_2015090201.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        private static readonly string[] TableNames =
            {
                "GKH_MORG_CONTRACT_OWNERS",
                "GKH_MORG_CONTRACT_JSKTSJ",
                "GKH_MORG_JSKTSJ_CONTRACT",
                "GKH_OBJ_DIRECT_MANAG_CNRT"
            };

        public override void Up()
        {
            foreach (var tableName in TableNames)
            {
                DropColumns(tableName);
            }
        }

        public override void Down()
        {
            foreach (var tableName in TableNames)
            {
                CreateColumns(tableName);
            }
        }

        private void CreateColumns(string tableName)
        {
            Database.AddColumn(tableName, new Column("OBJECT_VERSION", DbType.Int64, ColumnProperty.NotNull));
            Database.AddColumn(tableName, new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull));
            Database.AddColumn(tableName, new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull));
        }

        private void DropColumns(string tableName)
        {
            Database.RemoveColumn(tableName, "OBJECT_VERSION");
            Database.RemoveColumn(tableName, "OBJECT_CREATE_DATE");
            Database.RemoveColumn(tableName, "OBJECT_EDIT_DATE");
        }
    }
}