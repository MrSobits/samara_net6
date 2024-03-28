namespace Bars.Gkh.Entities.Dicts
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Интерфейс спрвочника
    /// </summary>
    public interface IGkhDict
    {
        /// <summary>
        /// Код
        /// </summary>
        string Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        string Name { get; set; }
    }


    /// <summary>
    /// Базовый класс справочника
    /// </summary>
    public abstract class BaseGkhDict : BaseEntity, IGkhDict
    {
        /// <inheritdoc />
        public virtual string Code { get; set; }

        /// <inheritdoc />
        public virtual string Name { get; set; }
    }
}