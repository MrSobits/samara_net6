namespace Bars.Gkh.Migrations._2019.Version_2019121800
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2019121800")]
    
    [MigrationDependsOn(typeof(Version_2019121600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            Database.AddColumn("FLATTENED_CLAIM_WORK", new Column("LawsuitResultConsideration", DbType.Int16));
            Database.AddColumn("FLATTENED_CLAIM_WORK", new Column("InstallmentPlan", DbType.String, 255));
            Database.AddColumn("FLATTENED_CLAIM_WORK", new Column("LawsuitDistanceDecisionCancel", DbType.Boolean));
            Database.AddColumn("FLATTENED_CLAIM_WORK", new Column("LawsuitDocType", DbType.Int16));
            Database.AddColumn("FLATTENED_CLAIM_WORK", new Column("DebtorPenaltyAmount", DbType.Decimal));
            Database.AddColumn("FLATTENED_CLAIM_WORK", new Column("LawsuitDeterminationMotionFix", DbType.Date));
            Database.RemoveColumn("FLATTENED_CLAIM_WORK", "FkrConsiderationResult");
        }

        public override void Down()
        {
            Database.RemoveColumn("FLATTENED_CLAIM_WORK", "LawsuitResultConsideration");
            Database.RemoveColumn("FLATTENED_CLAIM_WORK", "InstallmentPlan");
            Database.RemoveColumn("FLATTENED_CLAIM_WORK", "LawsuitDistanceDecisionCancel");
            Database.RemoveColumn("FLATTENED_CLAIM_WORK", "LawsuitDocType");
            Database.RemoveColumn("FLATTENED_CLAIM_WORK", "DebtorPenaltyAmount");
            Database.RemoveColumn("FLATTENED_CLAIM_WORK", "LawsuitDeterminationMotionFix");
            Database.AddColumn("FLATTENED_CLAIM_WORK", new Column("FkrConsiderationResult", DbType.String, ColumnProperty.Null));
            
        }
    }
}