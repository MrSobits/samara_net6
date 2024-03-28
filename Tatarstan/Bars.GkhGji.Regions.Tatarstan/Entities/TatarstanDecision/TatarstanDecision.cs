namespace Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanDecision
{
    using Bars.Gkh.Enums;
    using System;

    using Bars.B4.Modules.FIAS;

    /// <summary>
    /// "Решение" - документ, дублирующий определенный функционал документа "Распоряжение"
    /// </summary>
    public class TatarstanDecision : TatarstanDisposal
    {
        private YesNoNotSet _usingMeansRemoteInteraction;
        
        /// <summary>
        /// Учетный номер решения в ЕРКНМ
        /// </summary>
        public virtual string ErknmRegistrationNumber { get; set; }

        /// <summary>
        /// Дата присвоения учетного номера / идентификатора ЕРКНМ
        /// </summary>
        public virtual DateTime? ErknmRegistrationDate { get; set; }
        
        /// <summary>
        /// Место вынесения решения
        /// </summary>
        public virtual FiasAddress DecisionPlace { get; set; }

        /// <summary>
        /// Дата направления требования о предоставлении документов
        /// </summary>
        public virtual DateTime? SubmissionDate { get; set; }

        /// <summary>
        /// Дата получения документов во исполнение требования
        /// </summary>
        public virtual DateTime? ReceiptDate { get; set; }

        /// <summary>
        /// Использование средств дистанционного взаимодействия с контролируемым лицом
        /// </summary>
        public virtual YesNoNotSet UsingMeansRemoteInteraction
        {
            get
            {
                return this._usingMeansRemoteInteraction;
            }
            set
            {
                this._usingMeansRemoteInteraction = value == 0 ? YesNoNotSet.NotSet : value;
            }
        }

        /// <summary>
        /// Сведения об использовании средств дистанционного взаимодействия
        /// </summary>
        public virtual string InfoUsingMeansRemoteInteraction { get; set; }

        /// <summary>
        /// Гуид ЕРКНМ для организации
        /// </summary>
        public virtual string OrganizationErknmGuid { get; set; }
    }
}