namespace Bars.Gkh.RegOperator.DomainModelServices
{
    using System.Collections.Generic;
    using B4;
    using Dto;
    using Entities;
    using GkhCr.Entities;

    /// <summary>
    /// Резерватор займов
    /// </summary>
    public interface ILoanReserver
    {
        /// <summary>
        /// Зарезервировать займ
        /// </summary>
        IDataResult ReserveLoan(BaseParams baseParams);

        /// <summary>
        /// Зарезервировать займ
        /// </summary>
        void ReserveLoan(IEnumerable<RealityObjectPaymentAccount> payaccs, IList<LoanTakenMoney> takenMoney, ProgramCr program, long documentNum);
    }
}