namespace Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Определение акта КНМ без взаимодействия с контролируемыми лицами
    /// </summary>
    public class ActActionIsolatedDefinition : BaseEntity
    {
        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string Number { get; set; }
        
        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? Date { get; set; }
        
        /// <summary>
        /// Тип определения
        /// </summary>
        public virtual ActActionIsolatedDefinitionType DefinitionType { get; set; }
        
        /// <summary>
        /// ДЛ, вынесшее определение
        /// </summary>
        public virtual Inspector Official { get; set; }
        
        /// <summary>
        /// Дата исполнения
        /// </summary>
        public virtual DateTime? ExecutionDate { get; set; }

        /// <summary>
        /// Дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Акт КНМ без взаимодействия с контролируемыми лицами
        /// </summary>
        public virtual ActActionIsolated Act { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }
    }
}