namespace Bars.Gkh.Ris.Dictionaries.Nsi
{
    using System.Collections.Generic;

    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Dictionaries.Impl;
    using Bars.Gkh.Gis.Enum.ManOrg;

    using Castle.Windsor;

    /// <summary>
    /// Справочник "Назначение работы"
    /// </summary>
    public class ServiceWorkPurposeDictionary : BaseDictionary
    {
        /// <summary>
        /// Конструктор справочника
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        public ServiceWorkPurposeDictionary(IWindsorContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Назначение работы";

        /// <summary>
        /// Получить список сущностей внешней системы
        /// </summary>
        /// <returns>Список сущностей</returns>
        protected override List<ExternalEntityProxy> GetExternalEntities()
        {
            return ExternalEntityProxyList.FromEnum<ServiceWorkPurpose>();
        }
    }
}
