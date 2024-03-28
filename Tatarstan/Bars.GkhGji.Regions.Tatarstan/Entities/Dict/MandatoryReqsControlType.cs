using Bars.B4.DataAccess;

namespace Bars.GkhGji.Regions.Tatarstan.Entities.Dict
{
    using Bars.GkhGji.Entities.Dict;

    /// <summary>
    /// Данная таблица хранит всебе все обязательные требования к видам контроля
    /// </summary>
    public class MandatoryReqsControlType : BaseEntity
    {
        /// <summary>
        /// Обязательные требования
        /// </summary>
        public virtual MandatoryReqs MandatoryReqs { get; set; }

        /// <summary>
        /// Вид контроля
        /// </summary>
        public virtual ControlType ControlType { get; set; }

    }
}
