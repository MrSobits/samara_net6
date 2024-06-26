﻿namespace Bars.GisIntegration.Base.Entities.Inspection
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Связь между предписанием и вложением
    /// </summary>
    public class CancelPreceptAttachment : BaseRisEntity
    {
        /// <summary>
        /// Предписание
        /// </summary>
        public virtual Precept Precept { get; set; }

        /// <summary>
        /// Вложение
        /// </summary>
        public virtual Attachment Attachment { get; set; }
    }
}
