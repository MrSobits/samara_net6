namespace Bars.GisIntegration.Inspection.Dictionaries
{
    using System.Collections.Generic;

    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Dictionaries.Impl;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Справочник "Форма проведения проверки (Основания проверок = "Плановые проверки юридических лиц", "Проверки по требованию прокуратуры", "Проверки по поручению руководителей органов государственного контроля")"
    /// НСИ "Форма проведения проверки" (реестровый номер 71)
    /// </summary>
    public class ExaminationJurPersonFormDictionary : BaseDictionary
    {
        /// <summary>
        /// Конструктор справочника
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        public ExaminationJurPersonFormDictionary(IWindsorContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Форма проведения проверки (Основания проверок = \"Плановые проверки юридических лиц\", \"Проверки по требованию прокуратуры\", \"Проверки по поручению руководителей органов государственного контроля\")";

        /// <summary>
        /// Получить список сущностей внешней системы
        /// </summary>
        /// <returns>Список сущностей</returns>
        protected override List<ExternalEntityProxy> GetExternalEntities()
        {
            return ExternalEntityProxyList.FromEnum<TypeFormInspection>();
        }
    }
}
