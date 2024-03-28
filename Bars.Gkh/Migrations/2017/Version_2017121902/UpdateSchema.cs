namespace Bars.Gkh.Migrations._2017.Version_2017121902
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017121902")]
    [MigrationDependsOn(typeof(Version_2017071800.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017082401.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017100600.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017111400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            if (this.Database.TableExists("GKH_ENTITY_HISTORY_INFO"))
            {
                this.Database.AddColumn("GKH_ENTITY_HISTORY_INFO", new Column("GROUP_TYPE", DbType.Int32));
                this.Database.AddColumn("GKH_ENTITY_HISTORY_INFO", new Column("ENTITY_NAME", DbType.String));
                this.Database.AddColumn("GKH_ENTITY_HISTORY_INFO", new Column("PARENT_ENTITY_NAME", DbType.String));

                var sql = @"
WITH names as (
SELECT id,
    CASE 
        WHEN entity_id in (SELECT id FROM DEC_GOV_DECISION where RO_ID = parent_entity_id) THEN 'Bars.Gkh.Decisions.Nso.Entities.Decisions.GovDecision'
        ELSE 'Bars.Gkh.Decisions.Nso.Entities.RealityObjectDecisionProtocol'
    END as entity_name
FROM gkh_entity_history_info
)
UPDATE GKH_ENTITY_HISTORY_INFO i
SET entity_name = names.entity_name, parent_entity_name = 'Bars.Gkh.Entities.RealityObject', group_type = 1 -- EntityHistoryType.DecisionProtocol
FROM names
WHERE i.id = names.id;
";
                this.Database.AddIndex("IND_GKH_ENTITY_HISTORY_INFO_GROUP", false, "GKH_ENTITY_HISTORY_INFO",
                    "GROUP_TYPE", "PARENT_ENTITY_ID", "ENTITY_ID");

                this.Database.ExecuteNonQuery(sql);
                this.Database.ChangeColumnNotNullable("GKH_ENTITY_HISTORY_INFO", "GROUP_TYPE", true);
                this.Database.ChangeColumnNotNullable("GKH_ENTITY_HISTORY_INFO", "ENTITY_NAME", true);
                
            }
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GKH_ENTITY_HISTORY_INFO", "ENTITY_NAME");
            this.Database.RemoveColumn("GKH_ENTITY_HISTORY_INFO", "PARENT_ENTITY_NAME");
        }
    }
}