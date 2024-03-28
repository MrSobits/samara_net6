namespace Bars.Gkh.RegOperator.DomainService
{
    using System.IO;
    using Microsoft.AspNetCore.Mvc;
    using B4;

    /// <summary>
    /// Интерфейс для работы с займами
    /// </summary>
    public interface ILoanService
    {
        /// <summary>
        /// Сформировать займ по данным, сформированным на клиентской стороне
        /// </summary>
        IDataResult TakeLoanManually(BaseParams baseParams);

        /// <summary>
        /// Сформировать займ автоматически
        /// </summary>
        IDataResult TakeLoanAutomatically(BaseParams baseParams);

        /// <summary>
        /// Скачать распоряжение
        /// </summary>
        Stream DownloadDisposal(BaseParams baseParams);
    }
}