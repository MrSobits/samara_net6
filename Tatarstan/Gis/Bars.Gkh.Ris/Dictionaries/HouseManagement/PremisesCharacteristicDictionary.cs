﻿namespace Bars.Gkh.Ris.Dictionaries.HouseManagement
{
    using System.Collections.Generic;

    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Dictionaries.Impl;
    using Bars.Gkh.Ris.Enums.HouseManagement;

    using Castle.Windsor;

    /// <summary>
    /// Справочник "Характеристика помещения"
    /// </summary>
    public class PremisesCharacteristicDictionary : BaseDictionary
    {
        /// <summary>
        /// Конструктор справочника
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        public PremisesCharacteristicDictionary(IWindsorContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Характеристика помещения";

        /// <summary>
        /// Получить список сущностей внешней системы
        /// </summary>
        /// <returns>Список сущностей</returns>
        protected override List<ExternalEntityProxy> GetExternalEntities()
        {
            return ExternalEntityProxyList.FromEnum<PremisesCharacteristic>();
        }
    }
}
