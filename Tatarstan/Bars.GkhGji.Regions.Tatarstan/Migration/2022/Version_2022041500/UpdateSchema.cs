namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022041500
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022041500")]
    [MigrationDependsOn(typeof(Version_2022041401.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName table = new SchemaQualifiedObjectName { Name = "GJI_DICT_INSPECTION_BASE_TYPE_DOCUMENT_TYPE" };
        
        public override void Up()
        {
            this.Database.RemoveTable(this.table);
        }

        public override void Down()
        {
            if (!this.Database.TableExists(this.table))
            {
                this.Database.AddPersistentObjectTable(
                    this.table.Name,
                    new RefColumn(
                        "INSPECTION_BASE_TYPE_ID",
                        ColumnProperty.NotNull,
                        "GJI_DICT_INSPECTION_BASE_TYPE_DOCUMENT_TYPE_INSPECTION_BASE_TYPE",
                        "GJI_DICT_INSPECTION_BASE_TYPE",
                        "ID"
                    ),
                    new Column("DOCUMENT_TYPE", DbType.Int32, ColumnProperty.NotNull)
                );
            }
        }
    }
}