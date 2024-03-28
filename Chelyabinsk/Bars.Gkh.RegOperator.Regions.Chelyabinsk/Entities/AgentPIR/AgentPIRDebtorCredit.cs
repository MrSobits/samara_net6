using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using System;

namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.Entities
{
    public class AgentPIRDebtorCredit : BaseEntity
    {
        /// <summary>
        /// Агент ПИР
        /// </summary>
        public virtual AgentPIRDebtor Debtor { get; set; }

        /// <summary>
        /// Зачтено
        /// </summary>
        public virtual decimal? Credit { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public virtual DateTime? Date { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public virtual string User { get; set; }

        /// <summary>
        /// Файл документа
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}
