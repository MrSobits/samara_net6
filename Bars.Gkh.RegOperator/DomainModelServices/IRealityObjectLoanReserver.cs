namespace Bars.Gkh.RegOperator.DomainModelServices
{
    using Entities;

    /// <summary>
    /// Интерфейс для создания займов между домами
    /// </summary>
    public interface IRealityObjectLoanReserver
    {
        /// <summary>
        /// Проведение трансферов между заранее выбранными домами
        /// </summary>
        /// <param name="loan">Займ</param>
        void TakePreviouslyReservedLoan(RealityObjectLoan loan);
    }
}