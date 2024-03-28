using Bars.B4.Modules.Ecm7.Framework;

using System.Data;

namespace Bars.Gkh.Migrations._2018.Version_2018100400
{
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2018100400")]
    [MigrationDependsOn(typeof(Version_2018091700.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        private string tableName = "FLATTENED_CLAIM_WORK";
        public override void Up()
        {
            this.Database.AddEntityTable(
                this.tableName,
                new Column("Num", DbType.String, ColumnProperty.Null),
                new Column("DebtorFullname", DbType.String, ColumnProperty.Null),
                new Column("DebtorRoomAddress", DbType.String, ColumnProperty.Null),
                new Column("DebtorRoomType", DbType.String, ColumnProperty.Null),
                new Column("DebtorRoomNumber", DbType.String, ColumnProperty.Null),
                new Column("DebtorDebtPeriod", DbType.String, ColumnProperty.Null),
                new Column("DebtorDebtAmount", DbType.Decimal, ColumnProperty.Null),
                new Column("DebtorDutyAmount", DbType.Decimal, ColumnProperty.Null),
                new Column("DebtorDebtPaymentDate", DbType.DateTime, ColumnProperty.Null),
                new Column("DebtorDutyPaymentAssignment", DbType.String, ColumnProperty.Null),
                new Column("DebtorClaimDeliveryDate", DbType.DateTime, ColumnProperty.Null),
                new Column("DebtorPaymentsAfterCourtOrder", DbType.Decimal, ColumnProperty.Null),
                new Column("DebtorJurInstType", DbType.String, ColumnProperty.Null),
                new Column("DebtorJurInstName", DbType.String, ColumnProperty.Null),
                new Column("CourtClaimNum", DbType.String, ColumnProperty.Null),
                new Column("CourtClaimDate", DbType.DateTime, ColumnProperty.Null),
                new Column("CourtClaimConsiderationResult", DbType.String, ColumnProperty.Null),
                new Column("CourtClaimCancellationDate", DbType.DateTime, ColumnProperty.Null),
                new Column("CourtClaimRospName", DbType.String, ColumnProperty.Null),
                new Column("CourtClaimRospDate", DbType.DateTime, ColumnProperty.Null),
                new Column("CourtClaimEnforcementProcNum", DbType.String, ColumnProperty.Null),
                new Column("CourtClaimEnforcementProcDate", DbType.DateTime, ColumnProperty.Null),
                new Column("CourtClaimPaymentAssignmentNum", DbType.String, ColumnProperty.Null),
                new Column("CourtClaimPaymentAssignmentDate", DbType.DateTime, ColumnProperty.Null),
                new Column("CourtClaimRospDebtExact", DbType.Decimal, ColumnProperty.Null),
                new Column("CourtClaimRospDutyExact", DbType.Decimal, ColumnProperty.Null),
                new Column("CourtClaimEnforcementProcActEndNum", DbType.String, ColumnProperty.Null),
                new Column("CourtClaimDeterminationTurnDate", DbType.DateTime, ColumnProperty.Null),
                new Column("FkrRospName", DbType.String, ColumnProperty.Null),
                new Column("FkrEnforcementProcDecisionNum", DbType.String, ColumnProperty.Null),
                new Column("FkrEnforcementProcDate", DbType.DateTime, ColumnProperty.Null),
                new Column("FkrPaymentAssignementNum", DbType.String, ColumnProperty.Null),
                new Column("FkrPaymentAssignmentDate", DbType.DateTime, ColumnProperty.Null),
                new Column("FkrDebtExact", DbType.Decimal, ColumnProperty.Null),
                new Column("FkrDutyExact", DbType.Decimal, ColumnProperty.Null),
                new Column("FkrEnforcementProcActEndNum", DbType.String, ColumnProperty.Null),
                new Column("LawsuitCourtDeliveryDate", DbType.DateTime, ColumnProperty.Null),
                new Column("LawsuitDocNum", DbType.String, ColumnProperty.Null),
                new Column("LawsuitConsiderationDate", DbType.DateTime, ColumnProperty.Null),
                new Column("FkrConsiderationResult", DbType.String, ColumnProperty.Null),
                new Column("LawsuitDebtExact", DbType.Decimal, ColumnProperty.Null),
                new Column("LawsuitDutyExact", DbType.Decimal, ColumnProperty.Null),
                new Column("ListListNum", DbType.String, ColumnProperty.Null),
                new Column("ListListRopsDate", DbType.DateTime, ColumnProperty.Null),
                new Column("ListRospName", DbType.String, ColumnProperty.Null),
                new Column("ListRospDate", DbType.DateTime, ColumnProperty.Null),
                new Column("ListEnfProcDecisionNum", DbType.String, ColumnProperty.Null),
                new Column("ListEnfProcDate", DbType.DateTime, ColumnProperty.Null),
                new Column("ListPaymentAssignmentNum", DbType.String, ColumnProperty.Null),
                new Column("ListPaymentAssignmentDate", DbType.DateTime, ColumnProperty.Null),
                new Column("ListRospDebtExacted", DbType.Decimal, ColumnProperty.Null),
                new Column("ListRospDutyExacted", DbType.Decimal, ColumnProperty.Null),
                new Column("ListEnfProcActEndNum", DbType.String, ColumnProperty.Null),
                new Column("Note", DbType.String, ColumnProperty.Null),
                new Column("RloiId", DbType.Int32, ColumnProperty.Null),
                new Column("Archived", DbType.Boolean, ColumnProperty.NotNull,false));
        }

        public override void Down()
        {
            this.Database.RemoveTable(this.tableName);
        }
    }
}