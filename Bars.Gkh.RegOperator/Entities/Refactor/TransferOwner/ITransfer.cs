namespace Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner
{
    /// <summary>
    /// Трансфер с владельцем
    /// </summary>
    /// <typeparam name="TOwner">Владелец</typeparam>
    public interface ITransfer<out TOwner> where TOwner : ITransferOwner
    {
        /// <summary>
        /// Владелец
        /// </summary>
        TOwner Owner { get; }
    }
}