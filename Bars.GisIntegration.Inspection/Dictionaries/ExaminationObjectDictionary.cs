namespace Bars.GisIntegration.Inspection.Dictionaries
{
    using System.Collections.Generic;

    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Dictionaries.Impl;
    using Bars.GisIntegration.Inspection.Enums;

    using Castle.Windsor;

    /// <summary>
    /// НСИ "Предмет проверки" (реестровый номер 69)
    /// </summary>
    public class ExaminationObjectDictionary : BaseDictionary
    {
        /// <summary>
        /// Конструктор справочника
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        public ExaminationObjectDictionary(IWindsorContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Предмет проверки";

        /// <summary>
        /// Получить список сущностей внешней системы
        /// </summary>
        /// <returns>Список сущностей</returns>
        protected override List<ExternalEntityProxy> GetExternalEntities()
        {
            return ExternalEntityProxyList.FromEnum<ExaminationObject>();
        }
    }
}
