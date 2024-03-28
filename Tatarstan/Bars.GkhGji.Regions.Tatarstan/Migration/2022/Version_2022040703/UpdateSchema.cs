namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022040703
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("202204073")]
    [MigrationDependsOn(typeof(Version_2022040702.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName preventiveActionTaskTable = new SchemaQualifiedObjectName
        {
            Name = "GJI_DOCUMENT_PREVENTIVE_ACTION_TASK"
        };
        
        private SchemaQualifiedObjectName warningDocTable = new SchemaQualifiedObjectName
        {
            Name = "GJI_WARNING_DOC"
        };
        
        public override void Up()
        {
            this.Database.AddColumn(this.preventiveActionTaskTable, "ACTION_END_DATE", DbType.Date);
            
            this.Database.AddColumn(this.warningDocTable, "ACTION_START_DATE", DbType.Date);
            this.Database.AddColumn(this.warningDocTable, "ACTION_END_DATE", DbType.Date);
        }

        public override void Down()
        {
            this.Database.RemoveColumn(this.preventiveActionTaskTable, "ACTION_END_DATE");

            this.Database.RemoveColumn(this.warningDocTable, "ACTION_START_DATE");
            this.Database.RemoveColumn(this.warningDocTable, "ACTION_END_DATE");
        }
    }
}