namespace Bars.Gkh.RegOperator.DomainService.RealityObjectAccount
{
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Сервис операций начислений на доме
    /// </summary>
    public interface IRealtyObjectRegopOperationService
    {
        /// <summary>
        /// Создать операции начислений для домов
        /// </summary>
        /// <param name="entity">Период</param>
        /// <param name="ro">Дом</param>
        /// <param name="indicator">Индикатор</param>
        void CreateRealtyObjectChargeOperations(ChargePeriod entity, RealityObject ro, IProgressIndicator indicator = null);

        /// <summary>
        /// Создать операции начислений для всех домов
        /// </summary>
        /// <param name="indicator">Индикатор</param>
        void CreateAllRealtyObjectChargeOperations(IProgressIndicator indicator = null);

        /// <summary>
        /// Создать операции для лс
        /// </summary>
        /// <param name="period">Период</param>
        /// <param name="ro">Дом</param>
        /// <param name="indicator">Индикатор</param>
        void CreatePersonalAccountSummaries(ChargePeriod period, RealityObject ro, IProgressIndicator indicator = null);

        /// <summary>
        /// Создать операции для всех лс
        /// </summary>
        /// <param name="period">Период</param>
        /// <param name="indicator">Индикатор</param>
        void CreateAllPersonalAccountSummaries(ChargePeriod period, IProgressIndicator indicator = null);
    }
}