namespace Bars.GisIntegration.Base.Map.OrgRegistry
{
    using Bars.GisIntegration.Base.Entities.OrgRegistry;
    using Bars.GisIntegration.Base.Map;

    /// <summary>
    /// Маппинг сущности Bars.Gkh.Ris.Entities.OrgRegistry.RisSubsidiary
    /// </summary>
    public class RisSubsidiaryMap : BaseRisEntityMap<RisSubsidiary>
    {
        /// <summary>
        /// Конструктор маппинга
        /// </summary>
        public RisSubsidiaryMap()
            : base("Bars.Gkh.Ris.Entities.OrgRegistry.RisSubsidiary", "RIS_SUBSIDARY")
        {
        }

        /// <summary>
        /// Инициализация маппинга
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.FullName, "Полное наименование").Column("FULLNAME").Length(300);
            this.Property(x => x.ShortName, "Сокращенное наименование").Column("SHORTNAME").Length(300);
            this.Property(x => x.Ogrn, "ОГРН").Column("OGRN").Length(250);
            this.Property(x => x.Inn, "ИНН").Column("INN").Length(20);
            this.Property(x => x.Kpp, "КПП").Column("KPP").Length(20);
            this.Property(x => x.Okopf, "ОКОПФ").Column("OKOPF").Length(50);
            this.Property(x => x.Address, "Адрес регистрации").Column("ADDRESS").Length(500);
            this.Property(x => x.FiasHouseGuid, "Глобальный уникальный идентификатор дома по ФИАС").Column("FIASHOUSEGUID").Length(1000);
            this.Property(x => x.ActivityEndDate, "Дата прекращения деятельности").Column("ACTIVITYENDDATE");
            this.Property(x => x.SourceName, "Источник информации - Наименование").Column("SOURCENAME").Length(255);
            this.Property(x => x.SourceDate, "Источник информации - дата - от").Column("SOURCEDATE");

            this.Reference(x => x.Parent, "Головная организация").Column("PARENT_ID").Fetch();
        }
    }
}
