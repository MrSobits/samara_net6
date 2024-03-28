namespace Bars.GisIntegration.Inspection.Dictionaries
{
    using System.Collections.Generic;
    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Dictionaries.Impl;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    /// <summary>
    /// НСИ "Причина отмены документа" (предписания)
    /// </summary>
    public class PrescriptionCloseReasonDictionary : BaseDictionary
    {
        /// <summary>
        /// Конструктор справочника
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        public PrescriptionCloseReasonDictionary(IWindsorContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Причина отмены документа";

        /// <summary>
        /// Получить список сущностей внешней системы
        /// </summary>
        /// <returns>Список сущностей</returns>
        protected override List<ExternalEntityProxy> GetExternalEntities()
        {
            return ExternalEntityProxyList.FromEnum<PrescriptionCloseReason>();
        }
    }
}
