namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022041400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    using NHibernate.Util;

    [Migration("2022041400")]
    [MigrationDependsOn(typeof(Version_2022041102.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName warningInspectionTable = new SchemaQualifiedObjectName {Name = "GJI_WARNING_INSPECTION"};
        private SchemaQualifiedObjectName taskActionIsolatedTable = new SchemaQualifiedObjectName {Name = "GJI_TASK_ACTIONISOLATED"};
        
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn(this.warningInspectionTable, "INN", DbType.String, 12);
            
            this.Database.ChangeColumn(this.taskActionIsolatedTable, new Column("INN", DbType.String, 12));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(this.warningInspectionTable, "INN");
            
            this.Database.ChangeColumn(this.taskActionIsolatedTable, new Column("INN", DbType.String, 255));
        }
    }
}