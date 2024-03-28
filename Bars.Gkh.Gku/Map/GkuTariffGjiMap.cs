/// <mapping-converter-backup>
/// namespace Bars.Gkh.Gku.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class GkuTariffGjiMap : BaseEntityMap<GkuTariffGji>
///     {
///         public GkuTariffGjiMap()
///             : base("GKH_GKU_TARIFF")
///         {
///             Map(x => x.ServiceKind, "SERVICE_KIND");
///             Map(x => x.TarifRso, "TARIF_RSO", true, 0m);
///             Map(x => x.TarifMo, "TARIF_MO", true, 0m);
///             Map(x => x.NormativeValue, "NORM_VALUE");
/// 
///             Map(x => x.DateEnd, "DATE_END");
///             Map(x => x.DateStart, "DATE_START");
///             Map(x => x.PurchasePrice, "PURCHASE_PRICE", false, 100);
///             Map(x => x.PurchaseVolume, "PURCHASE_VOLUME", true, 0m);
///             Map(x => x.NormativeActInfo, "NORMATIVE_ACT_INFO", false, 500);
/// 
///             References(x => x.Service, "SERVICE_ID", ReferenceMapConfig.Fetch);
///             References(x => x.ManOrg, "MANORG_ID", ReferenceMapConfig.Fetch);
///             References(x => x.ResourceOrg, "RESORG_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Gku.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Gku.Entities;
    using System;
    
    
    /// <summary>Маппинг для "Тариф ЖКУ"</summary>
    public class GkuTariffGjiMap : BaseEntityMap<GkuTariffGji>
    {
        
        public GkuTariffGjiMap() : 
                base("Тариф ЖКУ", "GKH_GKU_TARIFF")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Service, "Коммунальная услуга").Column("SERVICE_ID").Fetch();
            Reference(x => x.ResourceOrg, "Поставщик коммунальных услуг").Column("RESORG_ID").Fetch();
            Reference(x => x.ManOrg, "Управляющая организация").Column("MANORG_ID").Fetch();
            Property(x => x.ServiceKind, "Вид услуги").Column("SERVICE_KIND");
            Property(x => x.DateStart, "Дата начала действия тарифа").Column("DATE_START");
            Property(x => x.DateEnd, "Дата окончания действия тарифа").Column("DATE_END");
            Property(x => x.PurchasePrice, "Закупочная стоимость коммунального ресурса").Column("PURCHASE_PRICE").Length(100);
            Property(x => x.PurchaseVolume, "Объем закупаемых ресурсов").Column("PURCHASE_VOLUME").DefaultValue(0m).NotNull();
            Property(x => x.TarifRso, "Тариф РСО").Column("TARIF_RSO").DefaultValue(0m).NotNull();
            Property(x => x.TarifMo, "Тариф УО").Column("TARIF_MO").DefaultValue(0m).NotNull();
            Property(x => x.NormativeActInfo, "Реквизиты нормативного акта, устанавливающего тариф").Column("NORMATIVE_ACT_INFO").Length(500);
            Property(x => x.NormativeValue, "Норматив").Column("NORM_VALUE");
        }
    }
}
