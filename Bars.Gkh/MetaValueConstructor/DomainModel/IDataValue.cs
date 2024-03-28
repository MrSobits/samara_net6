namespace Bars.Gkh.MetaValueConstructor.DomainModel
{
    /// <summary>
    /// Интерфейс объекта-значения
    /// </summary>
    public interface IDataValue : IHasNameCode, IHasId
    {
        /// <summary>
        /// Описатель объекта
        /// </summary>
        DataMetaInfo MetaInfo { get; }

        /// <summary>
        /// Значение объекта
        /// </summary>
        object Value { get; set; }
    }
}