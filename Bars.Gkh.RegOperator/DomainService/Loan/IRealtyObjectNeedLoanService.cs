namespace Bars.Gkh.RegOperator.DomainService
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.Gkh.ConfigSections.RegOperator.Enums;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;

    /// <summary>
    /// Интерфейс получения потребности в займах
    /// </summary>
    public interface IRealtyObjectNeedLoanService
    {
        /// <summary>
        /// Источник получения потребности
        /// </summary>
        LoanFormationType LoanFormationType { get; }

        /// <summary>
        /// Получение домов, нуждающихся в займах
        /// </summary>
        IEnumerable<RealtyObjectNeedLoan> ListRealtyObjectNeedLoan(Municipality municipality, ProgramCr program);

        /// <summary>
        /// Получение домов, нуждающихся в займах
        /// </summary>
        IEnumerable<RealtyObjectNeedLoan> ListRealtyObjectNeedLoan(RealityObject[] robjects, ProgramCr program);
    }
}