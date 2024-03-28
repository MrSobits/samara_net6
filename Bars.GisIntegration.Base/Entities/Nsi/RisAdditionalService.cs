namespace Bars.GisIntegration.Base.Entities.Nsi
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Запись справочника «Дополнительные услуги»
    /// </summary>
    public class RisAdditionalService : BaseRisEntity
    {
        /// <summary>
        /// Наименование вида дополнительной услуги
        /// </summary>
        public virtual string AdditionalServiceTypeName { get; set; }

        /// <summary>
        /// Код ОКЕИ
        /// </summary>
        public virtual string Okei { get; set; }

        /// <summary>
        /// Другая единица измерения
        /// </summary>
        public virtual string StringDimensionUnit { get; set; }
    }
}
