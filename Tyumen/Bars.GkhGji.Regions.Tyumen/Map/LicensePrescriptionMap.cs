
namespace Bars.GkhGji.Regions.Tyumen.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tyumen.Entities;
    using Bars.GkhGji.Regions.Tyumen.Entities.Suggestion;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tyumen.Entities.Suggestion.ApplicantNotification"</summary>
    public class LicensePrescriptionMap : BaseEntityMap<LicensePrescription>
    {
        
        public LicensePrescriptionMap() : 
                base("Bars.GkhGji.Regions.Tyumen.Entities.LicensePrescription", "GKH_LIC_PRESCRIPTION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Code").Column("CODE");
            Property(x => x.ActualDate, "Дата вступления в силу").Column("ACTUAL_DATE");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE").NotNull();
            Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUMBER").NotNull();
            Property(x => x.Penalty, "Сумма штрафа").Column("PENALTY");
            Property(x => x.YesNoNotSet, "Оспаривание").Column("IS_CANCELED");
            Reference(x => x.ArticleLawGji, "Статья закона").Column("ART_LAW_ID").NotNull();
            Reference(x => x.SanctionGji, "Санкция").Column("SANCTION_ID").NotNull();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").NotNull();
            Reference(x => x.MorgContractRO, "Управление домом").Column("MC_ID").NotNull();
        }
    }
}
