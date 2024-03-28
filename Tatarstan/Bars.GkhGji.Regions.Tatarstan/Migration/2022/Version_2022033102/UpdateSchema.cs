namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022033102
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.NumberValidation;

    [Migration("2022033102")]
    [MigrationDependsOn(typeof(Version_2022033101.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private string DocNumValidRuleTable => "GJI_DOCNUM_VALID_RULE";
        private string TatNumRuleId => new BaseTatValidationRule().Id;

        public override void Up()
        {
            this.Database.ExecuteNonQuery($@"
                INSERT INTO {this.DocNumValidRuleTable} (object_version, object_create_date, object_edit_date, rule_id, type_document_gji)
                SELECT 0, now(), now(), '{this.TatNumRuleId}', {(int)TypeDocumentGji.Decision}
                WHERE NOT EXISTS (SELECT 1 FROM {this.DocNumValidRuleTable} dvl
	                WHERE dvl.rule_id = '{this.TatNumRuleId}' AND dvl.type_document_gji = {(int)TypeDocumentGji.Decision});");
        }

        public override void Down()
        {
            this.Database.ExecuteNonQuery($@"
                DELETE FROM {this.DocNumValidRuleTable} 
                WHERE rule_id = '{this.TatNumRuleId}' AND type_document_gji = {(int)TypeDocumentGji.Decision};");
        }
    }
}