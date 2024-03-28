namespace Bars.GkhGji.Regions.Smolensk.Entities
{
    using System;
    using Enums;
    using GkhGji.Entities;

    /// <summary>
    /// Решение
    /// </summary>
    public class PrescriptionCancelSmol : PrescriptionCancel
    {
        /// <summary>
        /// Номер ходатайства
        /// </summary>
        public virtual string SmolPetitionNum { get; set; }

        /// <summary>
        /// Дата ходатайства
        /// </summary>
        public virtual DateTime? SmolPetitionDate { get; set; }

        /// <summary>
        /// Установлено
        /// </summary>
        public virtual string SmolDescriptionSet { get; set; }

        /// <summary>
        /// Результат решения
        /// </summary>
        public virtual string SmolCancelResult { get; set; }

        /// <summary>
        /// Тип решения
        /// </summary>
        public virtual TypePrescriptionCancel SmolTypeCancel { get; set; }
    }
}