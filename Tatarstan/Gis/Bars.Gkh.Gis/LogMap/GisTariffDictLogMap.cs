namespace Bars.Gkh.Gis.LogMap
{
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Utils;
    using Bars.Gkh.Gis.Entities.Dict;

    /// <summary>
    /// Регистрация изменений полей сущности <see cref="GisTariffDict"/>
    /// </summary>
    /// <remarks>
    /// Этот файл сгенерирован автоматичеески
    /// 13.10.2017 19:18:51
    /// УКАЖИТЕ ПОЛЯ ОТОБРАЖЕНИЯ ДЛЯ ССЫЛОЧНЫХ ТИПОВ
    /// </remarks>
    public class GisTariffDictLogMap : AuditLogMap<GisTariffDict>
    {
        public GisTariffDictLogMap()
        {
            this.Name("GisTariffDict");
            this.Description(x => "Справочник тарифы ГИС ЖКХ");

            // bool?
            this.MapProperty(x => x.IsNdsInclude, "IsNdsInclude", "Включая НДС");
            this.MapProperty(x => x.IsSocialNorm, "IsSocialNorm", "В пределах социальной нормы");
            this.MapProperty(x => x.IsMeterExists, "IsMeterExists", "Наличие прибора учета");
            this.MapProperty(x => x.IsElectricStoveExists, "IsElectricStoveExists", "Наличие электрической плиты");

            // ConsumerByElectricEnergyType?
            this.MapProperty(x => x.ConsumerByElectricEnergyType, "ConsumerByElectricEnergyType", "Тип потребителя по электроэнергии", x => x?.GetDisplayName());

            // ConsumerType?
            this.MapProperty(x => x.ConsumerType, "ConsumerType", "Вид потребителя", x => x?.GetDisplayName());

            // Contragent
            this.MapProperty(x => x.Contragent.Name, "Contragent", "Поставщик");

            // DateTime
            this.MapProperty(x => x.StartDate, "StartDate", "Дата начала периода");
            this.MapProperty(x => x.EndDate, "EndDate", "Дата окончания периода");

            // DateTime?
            this.MapProperty(x => x.EiasUploadDate, "EaisUploadDate", "Дата загрузки в ЕАИС");
            this.MapProperty(x => x.EiasEditDate, "EaisEditDate", "Дата последнего изменения в ЕАИС");

            // decimal?
            this.MapProperty(x => x.TariffValue, "TariffValue", "Значение тарифа", x => x?.ToString("F2"));
            this.MapProperty(x => x.TariffValue1, "TariffValue1", "Значение тарифа 1", x => x?.ToString("F2"));
            this.MapProperty(x => x.TariffValue2, "TariffValue2", "Значение тарифа 2", x => x?.ToString("F2"));
            this.MapProperty(x => x.TariffValue3, "TariffValue3", "Значение тарифа 3", x => x?.ToString("F2"));

            // GisTariffKind
            this.MapProperty(x => x.TariffKind, "TariffKind", "Вид тарифа", x => x.GetDisplayName());

            // int?
            this.MapProperty(x => x.ZoneCount, "ZoneCount", "Количество зон");
            this.MapProperty(x => x.Floor, "Floor", "Этаж");

            // Municipality
            this.MapProperty(x => x.Municipality.Name, "Municipality", "Муниципальный район");

            // ServiceDictionary
            this.MapProperty(x => x.Service.Name, "Service", "Услуга");

            // SettelmentType?
            this.MapProperty(x => x.SettelmentType, "SettelmentType", "Вид населенногоо пункта", x => x?.GetDisplayName());

            // string
            this.MapProperty(x => x.ActivityKind, "ActivityKind", "Вид деятельности поставщика");
            this.MapProperty(x => x.ContragentName, "ContragentName", "Наименование контрагента в базовом периоде");
            this.MapProperty(x => x.RegulatedPeriodAttribute, "RegulatedPeriodAttribute", "Дополнительный признак организации в регулируемом периоде");
            this.MapProperty(x => x.BasePeriodAttribute, "BasePeriodAttribute", "Дополнительный признак организации в базовом периоде");
        }
    }
}
