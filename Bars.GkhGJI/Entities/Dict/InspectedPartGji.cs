using Bars.Gkh.Entities;

namespace Bars.GkhGji.Entities
{
    /// <summary>
    /// Инспектируемая часть ГЖИ
    /// </summary>
    public class InspectedPartGji : BaseGkhEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }
    }
}