using Bars.B4.DataAccess;
using Bars.Gkh.Entities.Base;
using System;

namespace Bars.GkhGji.Regions.Tatarstan.Entities.Dict
{
    /// <summary>
    /// Обязательные требования.
    /// </summary>
    public class MandatoryReqs : BaseEntity, IUsedInTorIntegration
    {
        /// <summary>
        /// Наименование обязательного требования
        /// </summary>
        public virtual string MandratoryReqName { get; set; }

        /// <summary>
        /// Содержание обязательного требования
        /// </summary>
        public virtual string MandratoryReqContent { get; set; }

        /// <summary>
        /// Дата начала действия
        /// </summary>
        public virtual DateTime StartDateMandatory { get; set; }

        /// <summary>
        /// Дата окончания действия
        /// </summary>
        public virtual DateTime EndDateMandatory { get; set; }

        /// <summary>
        /// Идентификатор из ТОР КНД
        /// </summary>
        public virtual Guid? TorId { get; set; }
    }
}
