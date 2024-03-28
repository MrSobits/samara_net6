namespace Bars.GkhGji.Regions.Tatarstan.Entities.Dict
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Типовой вопрос проверочного листа.
    /// </summary>
    public class ControlListTypicalQuestion : BaseEntity
    {
        /// <summary>
        /// Вопрос проверочного листа.
        /// </summary>
        public virtual string Question { get; set; }

        /// <summary>
        /// НПА.
        /// </summary>
        public virtual NormativeDoc NormativeDoc { get; set; }

        /// <summary>
        /// Обязательное требование.
        /// </summary>
        public virtual MandatoryReqs MandatoryRequirement { get; set; }
    }
}
