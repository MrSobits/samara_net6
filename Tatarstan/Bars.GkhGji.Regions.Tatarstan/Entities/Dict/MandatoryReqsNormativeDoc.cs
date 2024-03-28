using Bars.B4.DataAccess;
using Bars.Gkh.Entities.Dicts;

namespace Bars.GkhGji.Regions.Tatarstan.Entities.Dict
{
    public class MandatoryReqsNormativeDoc : BaseEntity
    {
        /// <summary>
        /// Обязательные требования
        /// </summary>
        public virtual MandatoryReqs MandatoryReqs { get; set; }

        /// <summary>
        /// Нормативно-правовой акт
        /// </summary>
        public virtual NormativeDoc Npa { get; set; }

    }
}
