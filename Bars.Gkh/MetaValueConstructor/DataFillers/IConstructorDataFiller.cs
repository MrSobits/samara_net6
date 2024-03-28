namespace Bars.Gkh.MetaValueConstructor.DataFillers
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.MetaValueConstructor.DomainModel;
    using Bars.Gkh.MetaValueConstructor.Enums;

    /// <summary>
    /// Интерфейс источника данных для конструктора
    /// </summary>
    public interface IConstructorDataFiller
    {
        /// <summary>
        /// Тип конструктора
        /// </summary>
        DataMetaObjectType Type { get; }

        /// <summary>
        /// Собрать кэш
        /// <remarks>
        /// Сделано для того, чтобы мы могли массового приготовить кэш
        /// </remarks>
        /// </summary>
        /// <param name="baseParams">  Параметры (можно пробрасывать в том числе <see cref="IQueryable{T}"/>) </param>
        void PrepareCache(BaseParams baseParams);

        /// <summary>
        /// Метод заполняет значение поля/атрибута из внешнего источника (системы)
        /// </summary>
        /// <param name="value">Объект</param>
        void SetValue(IDataValue value);
    }
}