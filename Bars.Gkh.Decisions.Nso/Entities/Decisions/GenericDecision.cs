namespace Bars.Gkh.Decisions.Nso.Entities
{
    using System;
    
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;

    /// <summary>
    ///     Решение, принятое по протоколу
    /// </summary>
    public class GenericDecision : BaseImportableEntity
    {
        /// <summary>
        ///     Код решения
        /// </summary>
        public virtual string DecisionCode { get; set; }

        /// <summary>
        ///     Является актуальным
        /// </summary>
        public virtual bool IsActual { get; set; }

        /// <summary>
        ///     Протокол
        /// </summary>
        public virtual RealityObjectDecisionProtocol Protocol { get; set; }

        /// <summary>
        ///     Дата ввода в действие
        /// </summary>
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        ///     Храним значение решения
        /// </summary>
        public virtual string JsonObject { get; set; }

        /// <summary>
        ///     Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}