namespace Bars.Gkh.SystemDataTransfer.Meta
{
    /// <summary>
    /// Поставщик описаний сущностей
    /// </summary>
    public interface ITransferEntityProvider
    {
        /// <summary>
        /// Заполнить контейнер
        /// </summary>
        /// <param name="container">Контейнер</param>
        void FillContainer(TransferEntityContainer container);
    }
}