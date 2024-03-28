namespace Bars.GkhGji.Entities.Dict
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Основание для предостережения <see cref="WarningDocBasis"/>
    /// </summary>
    public class WarningBasis : BaseEntity
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