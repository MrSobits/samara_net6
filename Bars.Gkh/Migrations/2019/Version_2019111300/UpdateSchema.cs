namespace Bars.Gkh.Migrations._2019.Version_2019111300
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019111300")]
    [MigrationDependsOn(typeof(Version_2019110500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("FLATTENED_CLAIM_WORK", new Column("ZVSPPenaltyAmmount", DbType.Decimal, ColumnProperty.None));
            Database.AddColumn("FLATTENED_CLAIM_WORK", new Column("LawsuitDeterminationMotionless", DbType.Boolean, false));
            Database.AddColumn("FLATTENED_CLAIM_WORK", new Column("LawsuitDeterminationMotionlessDate", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("FLATTENED_CLAIM_WORK", new Column("LawsuitDeterminationDenail", DbType.Boolean, false));
            Database.AddColumn("FLATTENED_CLAIM_WORK", new Column("LawsuitDeterminationDenailDate", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("FLATTENED_CLAIM_WORK", new Column("LawsuitDeterminationJurDirected", DbType.Boolean, false));
            Database.AddColumn("FLATTENED_CLAIM_WORK", new Column("LawsuitDeterminationJurDirectedDate", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("FLATTENED_CLAIM_WORK", new Column("LawsuitDeterminationReturn", DbType.Boolean, false));
            Database.AddColumn("FLATTENED_CLAIM_WORK", new Column("LawsuitDeterminationReturnDate", DbType.DateTime, ColumnProperty.None));
        }

        public override void Down()
        {
            Database.RemoveColumn("FLATTENED_CLAIM_WORK", "LawsuitDeterminationReturnDate");
            Database.RemoveColumn("FLATTENED_CLAIM_WORK", "LawsuitDeterminationReturn");
            Database.RemoveColumn("FLATTENED_CLAIM_WORK", "LawsuitDeterminationJurDirectedDate");
            Database.RemoveColumn("FLATTENED_CLAIM_WORK", "LawsuitDeterminationJurDirected");
            Database.RemoveColumn("FLATTENED_CLAIM_WORK", "LawsuitDeterminationDenailDate");
            Database.RemoveColumn("FLATTENED_CLAIM_WORK", "LawsuitDeterminationDenail");
            Database.RemoveColumn("FLATTENED_CLAIM_WORK", "LawsuitDeterminationMotionlessDate");
            Database.RemoveColumn("FLATTENED_CLAIM_WORK", "LawsuitDeterminationMotionless");
            Database.RemoveColumn("FLATTENED_CLAIM_WORK", "ZVSPPenaltyAmmount");
        }
    }
}