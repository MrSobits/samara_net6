using Bars.Gkh.Entities;
using System;
using Bars.GkhGji.Enums;

namespace Bars.GkhGji.Entities
{
    public class AppealCitsInfo : BaseGkhEntity
    {
        /// <summary>
        /// Номер обращения
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Дата обращения
        /// </summary>
        public virtual DateTime AppealDate { get; set; }

        /// <summary>
        /// Дата операции
        /// </summary>
        public virtual DateTime OperationDate { get; set; }

        /// <summary>
        /// Корреспондент
        /// </summary>
        public virtual string Correspondent { get; set; }

        /// <summary>
        /// Тип операции
        /// </summary>
        public virtual AppealOperationType OperationType { get; set; }

        /// <summary>
        /// Оператор
        /// </summary>
        public virtual string Operator { get; set; }
    }
}
