namespace Bars.Gkh.RegOperator.DomainModelServices
{
    /// <summary>
    /// Интерфейс участника денежного трансфера
    /// </summary>
    public interface ITransferParty
    {
        /// <summary>
        /// Гуид, который запишется либо в SourceGuid, либо в TargetGuid объекта Transfer
        /// </summary>
        string TransferGuid { get; }
    }
}