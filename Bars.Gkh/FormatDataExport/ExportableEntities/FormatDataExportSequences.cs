namespace Bars.Gkh.FormatDataExport.ExportableEntities
{
    using System;

    /// <summary>
    /// Содержит названия секценций для комбинированных сущностей
    /// </summary>
    public static class FormatDataExportSequences
    {
        public const string ContragentExportId = "GKH_CONTRAGENT_EXPORT_ID_SEQ";
        public const string DictUslugaExportId = "GKH_SERVICE_EXPORT_ID_SEQ";
        public const string OplataPackExportId = "REGOP_BANK_DOC_EXPORT_ID_SEQ";
        public const string ProtocolossExportId = "GKH_DECISION_PROTOCOL_EXPORT_ID_SEQ";
        public const string OuExportId = "GKH_OU_EXPORT_ID_SEQ";
        public const string PkrExportId = "GKH_PKR_EXPORT_ID_SEQ";
        public const string WorkKprTypeExportId = "GKH_WORKKPRTYPE_EXPORT_ID_SEQ";

        [Obsolete]
        public const string ContragentRschetExportId = "GKH_CONTRACT_EXPORT_ID_SEQ";

        [Obsolete]
        public const string PkrDomExportId = "GKH_PKRDOM_EXPORT_ID_SEQ";

        [Obsolete]
        public const string PayDogovExprtId = "GKH_PAYDOGOV_EXPORT_ID_SEQ";
    }
}