namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Решение об отмене в предписании ГЖИ
    /// </summary>
    public class PrescriptionCancel : BaseGkhEntity
    {
        /// <summary>
        /// Предписание
        /// </summary>
        public virtual Prescription Prescription { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Дата отмены
        /// </summary>
        public virtual DateTime? DateCancel { get; set; }

        /// <summary>
        /// Дл, вынесшее решение
        /// </summary>
        public virtual Inspector IssuedCancel { get; set; }

        /// <summary>
        /// Отменено судом
        /// </summary>
        public virtual YesNoNotSet IsCourt { get; set; }

        /// <summary>
        /// Причина отмены
        /// </summary>
        public virtual string Reason { get; set; }

        /// <summary>
        /// Тип решения
        /// </summary>
        public virtual TypePrescriptionCancel TypeCancel { get; set; }

        /// <summary>
        /// Орган, вынесший решение
        /// </summary>
        public virtual DecisionMakingAuthorityGji DecisionMakingAuthority { get; set; }

        /// <summary>
        /// Дата вступления в силу решения суда
        /// </summary>
        public virtual DateTime? DateDecisionCourt { get; set; }

        /// <summary>
        /// Номер ходатайства
        /// </summary>
        public virtual string PetitionNumber { get; set; }

        /// <summary>
        /// Дата ходатайства
        /// </summary>
        public virtual DateTime? PetitionDate { get; set; }

        /// <summary>
        /// Установлено
        /// </summary>
        public virtual string DescriptionSet { get; set; }

        /// <summary>
        /// Продлено
        /// </summary>
        public virtual TypeProlongation Prolongation { get; set; }

        /// <summary>
        /// Продлить до
        /// </summary>
        public virtual DateTime? DateProlongation { get; set; }
    }
}