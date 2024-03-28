namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022040701
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2022040701")]
    [MigrationDependsOn(typeof(Version_2022040700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string ControlTypeTableName = "GJI_DICT_CONTROL_TYPES";
        
        public override void Up()
        {
            this.Database.RenameColumn(ControlTypeTableName, "ERVK_IDENTIFIER", "ERVK_ID");
            this.Database.RemoveColumn(ControlTypeTableName, "ERVK_VERSION");
        }

        public override void Down()
        {
            this.Database.RenameColumn(ControlTypeTableName, "ERVK_ID", "ERVK_IDENTIFIER");
            this.Database.AddColumn(ControlTypeTableName, 
                new Column("ERVK_VERSION", DbType.String.WithSize(36)));
        }
    }
}