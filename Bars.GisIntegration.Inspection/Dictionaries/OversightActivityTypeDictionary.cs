namespace Bars.GisIntegration.Inspection.Dictionaries
{
    using System.Collections.Generic;

    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Dictionaries.Impl;
    using Bars.GisIntegration.Inspection.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Справочник "Вид осуществления контрольной деятельности" НСИ 65
    /// </summary>
    public class OversightActivityTypeDictionary : BaseDictionary
    {
        /// <summary>
        /// Конструктор справочника
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        public OversightActivityTypeDictionary(IWindsorContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Вид осуществления контрольной деятельности";

        /// <summary>
        /// Получить список сущностей внешней системы
        /// </summary>
        /// <returns>Список сущностей</returns>
        protected override List<ExternalEntityProxy> GetExternalEntities()
        {
            return ExternalEntityProxyList.FromEnum<OversightActivityType>();
        }
    }
}
