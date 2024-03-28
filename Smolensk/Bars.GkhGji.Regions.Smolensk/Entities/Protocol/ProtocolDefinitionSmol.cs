namespace Bars.GkhGji.Regions.Smolensk.Entities.Protocol
{
    using System;

    using GkhGji.Entities;

    public class ProtocolDefinitionSmol : ProtocolDefinition
    {
        /// <summary>
        /// Результат определения
        /// </summary>
        public virtual string DefinitionResult { get; set; }

        /// <summary>
        /// Установлено
        /// </summary>
        public virtual string DescriptionSet { get; set; }

        /// <summary>
        /// Продлить до
        /// </summary>
        public virtual DateTime? ExtendUntil { get; set; }
    }
}
