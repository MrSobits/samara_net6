namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022040704
{
	using System.Data;

	using Bars.B4.Modules.Ecm7.Framework;
	using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

	[Migration("2022040704")]
    [MigrationDependsOn(typeof(Version_2022040703.UpdateSchema))]
    public class UpdateSchema : Migration
    {
	    private readonly SchemaQualifiedObjectName InspectionBaseTypeKindCheckTable = new SchemaQualifiedObjectName
	    {
		    Name = "GJI_DICT_INSPECTION_BASE_TYPE_KIND_CHECK"
	    };
	    
	    private readonly SchemaQualifiedObjectName InspectionBaseTypeDocumentTypeTable = new SchemaQualifiedObjectName
	    {
		    Name = "GJI_DICT_INSPECTION_BASE_TYPE_DOCUMENT_TYPE"
	    };
        
        public override void Up()
        {
	        if (!this.Database.TableExists(InspectionBaseTypeKindCheckTable))
	        {
		        this.Database.AddPersistentObjectTable(
			        this.InspectionBaseTypeKindCheckTable.Name,
			        new RefColumn(
				        "INSPECTION_BASE_TYPE_ID",
				        ColumnProperty.NotNull,
				        "GJI_DICT_INSPECTION_BASE_TYPE_KIND_CHECK_INSPECTION_BASE_TYPE",
				        "GJI_DICT_INSPECTION_BASE_TYPE",
				        "ID"
			        ),
			        new RefColumn(
				        "KIND_CHECK_ID",
				        ColumnProperty.NotNull,
				        "GJI_DICT_INSPECTION_BASE_TYPE_KIND_CHECK_KIND_CHECK",
				        "GJI_DICT_KIND_CHECK",
				        "ID"
			        )
		        );
	        }

	        if (!this.Database.TableExists(InspectionBaseTypeDocumentTypeTable))
	        {
		        this.Database.AddPersistentObjectTable(
			        this.InspectionBaseTypeDocumentTypeTable.Name,
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

        public override void Down()
        {
	        this.Database.RemoveTable(this.InspectionBaseTypeKindCheckTable);
	        this.Database.RemoveTable(this.InspectionBaseTypeDocumentTypeTable);
        }
    }
}