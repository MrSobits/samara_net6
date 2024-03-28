namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;


    /// <summary>Маппинг для "Источник поступления заявки"</summary>
    public class MKDLicRequestSourceMap : BaseEntityMap<MKDLicRequestSource>
    {
        
        public MKDLicRequestSourceMap() : 
                base("Источник поступления заявки", "GJI_MKD_LIC_REQUEST_SOURCES")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.RevenueDate, "Дата поступления").Column("REVENUE_DATE");
            Property(x => x.SSTUDate, "Дата ССТУ").Column("SSTU_DATE");
            Property(x => x.RevenueSourceNumber, "Исх. № источника поступления").Column("REVENUE_SOURCE_NUMBER").Length(50);
            Reference(x => x.RevenueSource, "Источник поступления").Column("REVENUE_SOURCE_ID").Fetch();
            Reference(x => x.RevenueForm, "Форма поступления").Column("REVENUE_FORM_ID").Fetch();
            Reference(x => x.MKDLicRequest, "Заявка").Column("MKD_LIC_REQUEST_ID").NotNull().Fetch();
        }
    }
}
