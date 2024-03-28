namespace Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Impl
{
    using System;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;

    /// <summary>
    /// Установка и изменение сальдо - массовая
    /// </summary>
    public class MassSaldoChangeOperation : PersonalAccountOperationBase
    {
        /// <summary>
        /// Код регистрации
        /// </summary>
        public static string Key => nameof(MassSaldoChangeOperation);

        /// <summary>
        /// Код операции
        /// </summary>
        public override string Code => MassSaldoChangeOperation.Key;

        /// <summary>
        /// Наименование операции
        /// </summary>
        public override string Name => "Установка и изменение сальдо";

        /// <summary>
        /// Ключ прав доступа
        /// </summary>
        public override string PermissionKey => "GkhRegOp.PersonalAccount.Registry.Action.MassSaldoChangeOperation";

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Сервис массового изменения сальдо
        /// </summary>
        public IAccountSaldoChangeService AccountSaldoChangeService { get; set; }

        /// <summary>
        /// Репозиторий периода начислений
        /// </summary>
        public IChargePeriodRepository ChargePeriodRepository { get; set; }

        /// <summary>
        /// Метод выполнения операции
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public override IDataResult Execute(BaseParams baseParams)
        {
            try
            {
                return this.AccountSaldoChangeService.ProcessSaldoChange(baseParams);
            }
            catch (Exception exception)
            {
                return BaseDataResult.Error(exception.Message);
            }
           
        }

        /// <summary>
        /// Метод получения данных пользовательского интерфейса
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public override IDataResult GetDataForUI(BaseParams baseParams)
        {
            var accIds = baseParams.Params.GetAs<long[]>("accIds");
            if (accIds.IsEmpty())
            {
                return BaseDataResult.Error("Необходимо выбрать хотя бы один лицевой счет");
            }

            var data = this.AccountSaldoChangeService.GetPersonalAccounts<SaldoChangeData>(baseParams);

            return new BaseDataResult(data);
        }
    }
}