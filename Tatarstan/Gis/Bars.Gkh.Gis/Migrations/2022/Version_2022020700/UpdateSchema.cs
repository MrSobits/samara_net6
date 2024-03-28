namespace Bars.Gkh.Gis.Migrations._2022.Version_2022020700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [MigrationDependsOn(typeof(_2021.Version_2021100100.UpdateSchema))]
    [Migration("2022020700")]
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName table = new SchemaQualifiedObjectName { Name = "GIS_LOADED_FILE_REGISTER"};
        
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.RemoveColumn(table, "SUPPLIER");
            this.Database.AddColumn(table, "SUPPLIER_NAME", DbType.String);
        }
    }
}