namespace Bars.Gkh.Ris.Extractors.OrgRegistry
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.OrgRegistry;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Экстрактор сведений об обособленных подразделениях
    /// </summary>
    public class SubsidiaryDataExtractor : BaseDataExtractor<RisSubsidiary, Contragent>
    {
        // Словарь зарегистрированных в ГИС-е организаций. Ключ => GkhId
        private Dictionary<long, RisContragent> risRegisteredContragentsByGkhId;

        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<Contragent> GetExternalEntities(DynamicDictionary parameters)
        {
            var gkhContragentDomain = this.Container.ResolveDomain<Contragent>();

            try
            {
                var registeredContragentsGkhIds = this.risRegisteredContragentsByGkhId
                    .Select(x => x.Key)
                    .ToArray();

                // Cписок id головных организаций
                var gkhParentContragentIds = gkhContragentDomain.GetAll()
                    .Where(x => registeredContragentsGkhIds.Contains(x.Id))
                    .Where(x => x.Parent == null)
                    .Where(x => x.Inn != null && x.Inn != "")
                    .Select(x => x.Id);

                // Получение данных об обособленных подразделениях по следующим условиям:
                // 1) заполнено значение поля "Головная организация" в карточке контрагента;
                // 2) значения полей "ИНН" у дочерней и головной организации совпадают;
                var result = gkhContragentDomain.GetAll()
                    .Where(x => x.Parent != null && gkhParentContragentIds.Any(y => x.Parent.Id == y))
                    .Where(x => x.Inn != null && x.Inn != "")
                    .Where(x => x.Inn == x.Parent.Inn)
                    .ToList();

                return result;
            }
            finally
            {
                this.Container.Release(gkhContragentDomain);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="contragent">Сущность внешней системы</param>
        /// <param name="risSubsidiary">Ris сущность</param>
        protected override void UpdateRisEntity(Contragent contragent, RisSubsidiary risSubsidiary)
        {
            risSubsidiary.Parent = contragent.Parent != null
                ? this.risRegisteredContragentsByGkhId.Get(contragent.Parent.Id)
                : null;

            risSubsidiary.ExternalSystemEntityId = contragent.Id;
            risSubsidiary.ExternalSystemName = "gkh";
            risSubsidiary.FullName = contragent.Name;
            risSubsidiary.ShortName = contragent.ShortName;
            risSubsidiary.Ogrn = contragent.Ogrn;
            risSubsidiary.Inn = contragent.Inn;
            risSubsidiary.Kpp = contragent.Kpp;

            risSubsidiary.Okopf = contragent.OrganizationForm != null
                ? contragent.OrganizationForm.OkopfCode
                : string.Empty;

            risSubsidiary.Address = contragent.JuridicalAddress;

            risSubsidiary.FiasHouseGuid = contragent.FiasJuridicalAddress != null
                ? contragent.FiasJuridicalAddress.AddressGuid
                : string.Empty;

            risSubsidiary.ActivityEndDate = contragent.ActivityDateEnd;
            risSubsidiary.SourceName = "ГЖИ";
            // В ТФФ документе ничего не сказано про поле SourceDate
            // Поэтому аналитики предложили пока подставлять дату регистрации контрагента
            risSubsidiary.SourceDate = contragent.DateRegistration;
        }

        /// <summary>
        /// Выполнить обработку перед извлечением данных
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        protected override void BeforeExtractHandle(DynamicDictionary parameters)
        {
            var risContragentDomain = this.Container.ResolveDomain<RisContragent>();

            try
            {
                // Словарь id головных организаций зарегистрированных в ГИС
                this.risRegisteredContragentsByGkhId = risContragentDomain.GetAll()
                    .Where(x => x.OrgVersionGuid != null && x.OrgVersionGuid != ""
                             || x.OrgRootEntityGuid != null && x.OrgRootEntityGuid != "")
                    .ToDictionary(x => x.GkhId);
            }
            finally
            {
                this.Container.Release(risContragentDomain);
            }
        }
    }
}
