namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022040600
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.GkhGji.Regions.Tatarstan.Rules;

    [Migration("2022040600")]
    [MigrationDependsOn(typeof(Version_2022031800.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2022040100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private string KindCheckRuleReplaceTable => "gji_kind_check_rule";

        private List<Tuple<string, string>> ruleCodes = new List<Tuple<string, string>>
        {
            Tuple.Create("DispPrescrCode1279Rule", nameof(DecisionPrescriptionCode1279Rule)),
            Tuple.Create("DispPrescrCode34Rule", nameof(DecisionPrescriptionCode34Rule)),
            Tuple.Create("DispPrescrCode5", nameof(DecisionPrescriptionCode5Rule)),
            Tuple.Create("DispPrescrCode6", nameof(DecisionPrescriptionCode6Rule)),
            Tuple.Create("DispPrescrCode8", nameof(DecisionPrescriptionCode8Rule))
        };

        public override void Up()
        {
            this.ruleCodes.ForEach(x =>
            {
                // REPLACE(gkcr.name, 'аспоряж', 'еш') - Подменить распоряжение на решение
                this.Database.ExecuteNonQuery($@"
                    INSERT INTO {this.KindCheckRuleReplaceTable} (object_version, object_create_date, object_edit_date, name, code, rule_code)
                    SELECT 0, now(), now(), REPLACE(gkcr.name, 'аспоряж', 'еш'), gkcr.code, '{x.Item2}'
                    FROM {this.KindCheckRuleReplaceTable} gkcr
                    WHERE gkcr.rule_code = '{x.Item1}'
	                    AND NOT EXISTS (SELECT 1 FROM {this.KindCheckRuleReplaceTable} WHERE rule_code = '{x.Item2}');");
            });
        }

        public override void Down()
        {
            this.Database.ExecuteNonQuery($@"
                DELETE FROM {this.KindCheckRuleReplaceTable} WHERE rule_code IN 
                    ('{string.Join("', '", this.ruleCodes.Select(x => x.Item2))}');");
        }
    }
}