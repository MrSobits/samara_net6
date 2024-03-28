namespace Bars.GkhGji.Entities.Dict
{
    using Bars.Gkh.Entities;

    public class InspectionReason : BaseGkhEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

    }
}
