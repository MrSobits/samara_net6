namespace Bars.Gkh.SystemDataTransfer.Meta
{
    /// <summary>
    /// Сущность, которая хранит дополнительный ключ
    /// </summary>
    public interface IComplexImportEntity
    {
        /// <summary>
        /// Внешний ключ
        /// </summary>
        object ComplexKey { get; set; }
    }
}