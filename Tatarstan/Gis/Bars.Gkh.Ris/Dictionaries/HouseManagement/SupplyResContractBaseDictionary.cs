namespace Bars.Gkh.Ris.Dictionaries.HouseManagement
{
    using System.Collections.Generic;

    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Dictionaries.Impl;
    using Bars.Gkh.Modules.Gkh1468.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Справочник "Основание заключения договора ресурсоснабжения"
    /// </summary>
    public class SupplyResContractBaseDictionary : BaseDictionary
    {
        /// <summary>
        /// Конструктор справочника
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        public SupplyResContractBaseDictionary(IWindsorContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Основание заключения договора ресурсоснабжения";

        /// <summary>
        /// Получить список сущностей внешней системы
        /// </summary>
        /// <returns>Список сущностей</returns>
        protected override List<ExternalEntityProxy> GetExternalEntities()
        {
            return ExternalEntityProxyList.FromEnum<ResOrgReason>();
        }
    }
}
