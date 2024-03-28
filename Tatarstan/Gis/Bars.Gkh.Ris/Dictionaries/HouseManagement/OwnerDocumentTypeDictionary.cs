namespace Bars.Gkh.Ris.Dictionaries.HouseManagement
{
    using System.Collections.Generic;

    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Dictionaries.Impl;
    using Bars.Gkh.Modules.Gkh1468.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Справочник "Документ, удостоверяющий личность"
    /// </summary>
    public class OwnerDocumentTypeDictionary : BaseDictionary
    {
        /// <summary>
        /// Конструктор справочника
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        public OwnerDocumentTypeDictionary(IWindsorContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Документ, удостоверяющий личность";

        /// <summary>
        /// Получить список сущностей внешней системы
        /// </summary>
        /// <returns>Список сущностей</returns>
        protected override List<ExternalEntityProxy> GetExternalEntities()
        {
            return ExternalEntityProxyList.FromEnum<OwnerDocumentType>();
        }
    }
}
