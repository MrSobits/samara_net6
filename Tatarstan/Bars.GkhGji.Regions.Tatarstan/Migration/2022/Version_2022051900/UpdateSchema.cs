namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022051900
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [MigrationDependsOn(typeof(Version_2022051601.UpdateSchema))]
    [Migration("2022051900")]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName decisionTable = new SchemaQualifiedObjectName { Name = "GJI_DECISION" };
        private readonly Column column = new Column("PLACE_ERKNM_GUID", DbType.String.WithSize(36));
        
        public override void Up()
        {
            this.Database.RemoveColumn(decisionTable, column.Name);
        }

        public override void Down()
        {
            this.Database.AddColumn(this.decisionTable, this.column);
        }
    }
}