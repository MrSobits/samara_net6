namespace Bars.Gkh.MetaValueConstructor.DomainModel
{
    /// <summary>
    /// Интерфейс данных с кодом и именем
    /// </summary>
    public interface IHasNameCode
    {
        /// <summary>
        /// Наименование
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Код
        /// </summary>
        string Code { get; }
    }

    /// <summary>
    /// Сущность, которая имеет идентификатор
    /// </summary>
    public interface IHasId
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        object Id { get; }
    }

    /// <summary>
    /// Интерфейс элемента, который имеет родителя
    /// </summary>
    /// <typeparam name="T">Хранимый объект</typeparam>
    public interface IHasParent<out T> where T : IHasId
    {
        /// <summary>
        /// Родитель
        /// </summary>
        T Parent { get; }
    }
}