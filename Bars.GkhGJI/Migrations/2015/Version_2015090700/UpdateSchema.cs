namespace Bars.GkhGji.Migrations._2015.Version_2015090700
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015090700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations._2015.Version_2015082400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        private static readonly string[] TableNames =
            {
                "GJI_ACTCHECK",
                "GJI_ACTREMOVAL",
                "GJI_ACTSURVEY",
                "GJI_DISPOSAL",
                "GJI_PRESCRIPTION",
                "GJI_PRESENTATION",
                "GJI_PROTOCOL",
                "GJI_PROTOCOL_MVD",
                "GJI_PROTOCOLMHC",
                "GJI_RESOLPROS",
                "GJI_RESOLUTION",
                "GJI_INSPECTION_ACTIVITY",
                "GJI_INSPECTION_BASEDEF",
                "GJI_INSPECTION_DISPHEAD",
                "GJI_INSPECTION_HEATSEASON",
                "GJI_INSPECTION_INSCHECK",
                "GJI_INSPECTION_JURPERSON",
                "GJI_INSPECTION_PLANACTION",
                "GJI_INSPECTION_PROSCLAIM",
                "GJI_INSPECTION_PROTMHC",
                "GJI_INSPECTION_PROTMVD",
                "GJI_INSPECTION_RESOLPROS",
                "GJI_INSPECTION_STATEMENT",
                "GJI_ACTCHECK_VIOLAT",
                "GJI_ACTREMOVAL_VIOLAT",
                "GJI_DISPOSAL_VIOLAT",
                "GJI_PRESCRIPTION_VIOLAT",
                "GJI_PROTOCOL_VIOLAT"
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