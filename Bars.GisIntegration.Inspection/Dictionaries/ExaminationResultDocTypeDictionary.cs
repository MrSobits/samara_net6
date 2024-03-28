namespace Bars.GisIntegration.Inspection.Dictionaries
{
    using System.Collections.Generic;

    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Dictionaries.Impl;
    using Bars.GisIntegration.Inspection.Enums;

    using Castle.Windsor;

    /// <summary>
    /// НСИ "Вид документа по результатам проверки" (реестровый номер 64)
    /// </summary>
    public class ExaminationResultDocTypeDictionary : BaseDictionary
    {
        /// <summary>
        /// Конструктор справочника
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        public ExaminationResultDocTypeDictionary(IWindsorContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Вид документа по результатам проверки";

        /// <summary>
        /// Получить список сущностей внешней системы
        /// </summary>
        /// <returns>Список сущностей</returns>
        protected override List<ExternalEntityProxy> GetExternalEntities()
        {
            return ExternalEntityProxyList.FromEnum<ExaminationResultDocType>();
        }
    }
}
