namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022041501
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Utils;

    [Migration("2022041501")]
    [MigrationDependsOn(typeof(Version_2022041500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName preventiveActionTable = new SchemaQualifiedObjectName
        {
            Name = "GJI_DOCUMENT_PREVENTIVE_ACTION"
        };

        private Column[] columns =
        {
            new Column("ERKNM_REGISTRATION_NUMBER", DbType.String, 30),
            new Column("ERKNM_REGISTRATION_DATE", DbType.Date)
        };
        
        public override void Up()
        {
            this.columns.ForEach(column => this.Database.AddColumn(this.preventiveActionTable, column));
        }

        public override void Down()
        {
            this.columns.ForEach(column => this.Database.RemoveColumn(this.preventiveActionTable, column.Name));
        }
    }
}
