namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022040702
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2022040702")]
    [MigrationDependsOn(typeof(Version_2022040701.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName table = new SchemaQualifiedObjectName
        {
            Name = "GJI_DOCUMENT_PREVENTIVE_ACTION"
        };
        
        public override void Up()
        {
            this.Database.AddRefColumn
            (
                this.table, 
                new RefColumn("CONTROL_TYPE_ID", $"{this.table.Name}_CONTROL_TYPE_ID", "GJI_DICT_CONTROL_TYPES", "ID")
            );
        }

        public override void Down()
        {
            this.Database.RemoveColumn(this.table, "CONTROL_TYPE_ID");
        }
    }
}