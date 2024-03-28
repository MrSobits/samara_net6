namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022042100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Utils;

    [Migration("2022042100")]
    [MigrationDependsOn(typeof(Version_2022042001.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private Column actCheckColumn = new Column("ERKNM_GUID", DbType.String.WithSize(36));

        private SchemaQualifiedObjectName actCheckTable = new SchemaQualifiedObjectName { Name = "GJI_ACTCHECK_ACTION" };

        private Column[] decisionColumns =
        {
            new Column("ORGANIZATION_ERKNM_GUID", DbType.String.WithSize(36)),
            new Column("PLACE_ERKNM_GUID", DbType.String.WithSize(36))
        };

        private SchemaQualifiedObjectName decisionTable = new SchemaQualifiedObjectName { Name = "GJI_DECISION" };
        
        /// <inheritdoc />
        public override void Up()
        {
            this.decisionColumns.ForEach(column => this.Database.AddColumn(this.decisionTable, column));
            this.Database.AddColumn(this.actCheckTable, this.actCheckColumn);
        }

        /// <inheritdoc />
        public override void Down()
        {  
            this.decisionColumns.ForEach(column => this.Database.RemoveColumn(this.decisionTable, column.Name));
            this.Database.RemoveColumn(this.actCheckTable, this.actCheckColumn.Name);
        }
    }
}