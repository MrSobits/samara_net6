namespace Bars.Gkh.RegOperator.DomainModelServices
{
    /// <summary>
    /// Нехранимый участник денежного трансфера
    /// </summary>
    public class FakeTransferParty : ITransferParty
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="guid">Гуид</param>
        public FakeTransferParty(string guid)
        {
            this.TransferGuid = guid;
        }

        /// <inheritdoc />
        public string TransferGuid { get; }
    }
}