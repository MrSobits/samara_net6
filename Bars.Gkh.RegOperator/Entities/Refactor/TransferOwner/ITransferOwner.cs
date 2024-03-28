namespace Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner
{
    using Bars.B4.DataModels;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    /// <summary>
    /// Владелец трансфера
    /// </summary>
    public interface ITransferOwner : IHaveId
    {
        /// <summary>
        /// Тип владельца трансфера
        /// </summary>
        WalletOwnerType TransferOwnerType { get; }

        /// <summary>
        /// Метод создания трансфера
        /// </summary>
        /// <param name="sourceGuid">Источник</param>
        /// <param name="targetGuid">Целевой гуид</param>
        /// <param name="moneyStream">Поток средств</param>
        /// <returns>Трансфер</returns>
        Transfer CreateTransfer(string sourceGuid, string targetGuid, MoneyStream moneyStream);

        /// <summary>
        /// Метод возвращает описание владельца трансфера
        /// </summary>
        /// <returns>Описание</returns>
        string GetDescription();
    }
}