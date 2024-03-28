namespace Bars.Gkh.Entities.Dicts
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Виды нормативных актов
    /// </summary>
    public class TypeNormativeAct : BaseEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Действует на
        /// </summary>
        public virtual ActionLevelNormativeAct ActionLevel { get; set; }
    }
}