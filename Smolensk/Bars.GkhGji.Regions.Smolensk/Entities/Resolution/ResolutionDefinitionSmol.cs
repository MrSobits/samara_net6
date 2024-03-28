namespace Bars.GkhGji.Regions.Smolensk.Entities.Resolution
{
    using GkhGji.Entities;

    public class ResolutionDefinitionSmol : ResolutionDefinition
    {
        /// <summary>
        /// Результат определения
        /// </summary>
        public virtual string DefinitionResult { get; set; }

        /// <summary>
        /// Установлено
        /// </summary>
        public virtual string DescriptionSet { get; set; }
    }
}
