using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.Gkh.Enums;
using System;

namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.Entities
{
    /// <summary>
    /// Документ агент ПИР
    /// </summary>
    public class AgentPIRDocument : BaseEntity
    {
        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime DocumentDate { get; set; }

        /// <summary>
        /// Сумма основного долга
        /// </summary>
        public virtual decimal? DebtSum { get; set; }

        /// <summary>
        /// Сумма долга по пени
        /// </summary>
        public virtual decimal? PeniSum { get; set; }

        /// <summary>
        /// Сумма пени
        /// </summary>
        public virtual decimal? Duty { get; set; }

        /// <summary>
        /// Копия документа
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
        public virtual AgentPIRDocumentType DocumentType { get; set; }

        /// <summary>
        /// Должник агент ПИР
        /// </summary>
        public virtual AgentPIRDebtor AgentPIRDebtor { get; set; }

        /// <summary>
        /// Сумма погашения
        /// </summary>
        public virtual decimal? Repaid { get; set; }

        /// <summary>
        /// Погашено
        /// </summary>
        public virtual bool YesNo { get; set; }
    }
}
