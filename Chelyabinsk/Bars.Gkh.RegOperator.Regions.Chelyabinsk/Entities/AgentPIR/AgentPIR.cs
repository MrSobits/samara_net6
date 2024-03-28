using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.Gkh.Entities;
using System;

namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.Entities
{
    public class AgentPIR : BaseEntity
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }
        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime DateStart { get; set; }
        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime DateEnd { get; set; }
        /// <summary>
        /// Дата договора
        /// </summary>
        public virtual DateTime ContractDate { get; set; }
        /// <summary>
        /// Номер договора
        /// </summary>
        public virtual int ContractNumber { get; set; }
        /// <summary>
        /// Скан договора
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }
    }
}
