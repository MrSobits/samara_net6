namespace Bars.GisIntegration.Smev.Migrations._2022.Version_2022060300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022060300")]
    [MigrationDependsOn(typeof(Version_2022042900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName table = new SchemaQualifiedObjectName
        {
            Name = "STORABLE_SMEV3_RESPONSE"
        };
        
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddPersistentObjectTable(this.table.Name, 
                new Column("REQUEST_GUID", DbType.String.WithSize(36), ColumnProperty.NotNull),
                new Column("RESPONSE", DbType.Binary, ColumnProperty.NotNull));

            this.Database.AddIndex("STORABLE_SMEV3_RESPONSE_REQUEST_GUID_INDX", false, this.table, "REQUEST_GUID");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(table);
        }
    }
}