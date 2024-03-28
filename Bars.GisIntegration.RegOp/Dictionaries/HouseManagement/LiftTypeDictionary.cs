namespace Bars.GisIntegration.RegOp.Dictionaries.HouseManagement
{
    using System.Collections.Generic;

    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Dictionaries.Impl;
    using Bars.Gkh.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Справочник "Тип лифта"
    /// </summary>
    public class LiftTypeDictionary : BaseDictionary
    {
        /// <summary>
        /// Конструктор справочника
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        public LiftTypeDictionary(IWindsorContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Тип лифта";

        /// <summary>
        /// Получить список сущностей внешней системы
        /// </summary>
        /// <returns>Список сущностей</returns>
        protected override List<ExternalEntityProxy> GetExternalEntities()
        {
            return new ExternalEntityProxyList()
            {
                new ExternalEntityProxy(0, "Не заполнено"),
                new ExternalEntityProxy(1, "Пассажирский"),
                new ExternalEntityProxy(2, "Грузовой"),
                new ExternalEntityProxy(3, "Грузо-пассажирский")
            };
        }
    }
}