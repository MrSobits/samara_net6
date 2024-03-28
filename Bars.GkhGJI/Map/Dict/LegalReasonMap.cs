namespace Bars.GkhGji.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.Dict;
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.Dict.LegalReason"</summary>
    public class LegalReasonMap : BaseEntityMap<LegalReason>
    {
        /// <summary>
		/// Конструктор
		/// </summary>
        public LegalReasonMap() : 
                base("Bars.GkhGji.Entities.Dict.LegalReason", "GJI_DICT_LEGAL_REASON")
        {
        }
        
		/// <summary>
		/// Замапить
		/// </summary>
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.Name, "Name").Column("NAME").Length(2000).NotNull();
            this.Property(x => x.Code, "Code").Column("CODE").Length(300);
        }
    }
}
