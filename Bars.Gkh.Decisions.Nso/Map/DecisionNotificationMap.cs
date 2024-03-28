namespace Bars.Gkh.Decisions.Nso.Map
{
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Map;

    /// <summary>Маппинг для "Bars.Gkh.Decisions.Nso.Entities.DecisionNotification"</summary>
    public class DecisionNotificationMap : BaseImportableEntityMap<DecisionNotification>
    {
        public DecisionNotificationMap() :
                base("Bars.Gkh.Decisions.Nso.Entities.DecisionNotification", "REGOP_DEC_NOTIF")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Number, "Number").Column("NOTIF_NUMBER");
            this.Property(x => x.Date, "Date").Column("NOTIF_DATE");
            this.Property(x => x.AccountNum, "AccountNum").Column("ACCOUNT_NUM");
            this.Property(x => x.OpenDate, "OpenDate").Column("OPEN_DATE");
            this.Property(x => x.CloseDate, "CloseDate").Column("CLOSE_DATE");
            this.Property(x => x.IncomeNum, "IncomeNum").Column("INCOME_NUM");
            this.Property(x => x.RegistrationDate, "RegistrationDate").Column("REG_DATE");
            this.Property(x => x.OriginalIncome, "OriginalIncome").Column("ORIG_INCOME");
            this.Property(x => x.CopyIncome, "CopyIncome").Column("COPY_INCOME");
            this.Property(x => x.CopyProtocolIncome, "CopyProtocolIncome").Column("COPY_PROTO_INCOME");
            this.Property(x => x.GisGkhAttachmentGuid, "ГИС ЖКХ GUID вложения").Column("GIS_GKH_ATTACHMENT_GUID").Length(36);
            this.Reference(x => x.Document, "Document").Column("DOC_FILE_ID");
            this.Reference(x => x.ProtocolFile, "ProtocolFile").Column("PROTO_FILE_ID");
            this.Reference(x => x.BankDoc, "BankDoc").Column("BANKDOC_FILE_ID");
            this.Reference(x => x.Protocol, "Protocol").Column("RO_DEC_PROTO_ID");
            this.Reference(x => x.State, "State").Column("STATE_ID");
        }
    }
}
