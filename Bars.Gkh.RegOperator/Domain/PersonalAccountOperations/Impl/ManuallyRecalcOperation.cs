namespace Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Impl
{
    using System;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Modules.States;

    using Bars.Gkh.Domain.DatabaseMutex;

    using Castle.Windsor;
    using DomainEvent.Events.PersonalAccount;
    using DomainModelServices;
    using Entities;
    using Entities.PersonalAccount;
    using Enums;
    using Gkh.Domain;

    /// <summary>
    /// Ручной перерасчет
    /// </summary>
    public class ManuallyRecalcOperation : PersonalAccountOperationBase
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        protected readonly IWindsorContainer Container;

        /// <summary>
        /// Домен-сервис лицевых счетов
        /// </summary>
        protected readonly IDomainService<BasePersonalAccount> PersonalAccountDomain;

        /// <summary>
        /// Домен-сервис состояний
        /// </summary>
        protected readonly IDomainService<State> StateDomain;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container">Контейнер</param>
        /// <param name="personalAccountDomain">Домен-сервис лицевых счетов</param>
        /// <param name="stateDomainService">Домен-сервис состояний</param>
        public ManuallyRecalcOperation(
            IWindsorContainer container,
            IDomainService<BasePersonalAccount> personalAccountDomain,
            IDomainService<State> stateDomainService)
        {
            this.Container = container;
            this.PersonalAccountDomain = personalAccountDomain;
            this.StateDomain = stateDomainService;
        }

        /// <summary>
        /// Ключ
        /// </summary>
        public static string Key
        {
            get { return "ManuallyRecalcOperation"; }
        }

        /// <summary>
        /// Код
        /// </summary>
        public override string Code
        {
            get { return ManuallyRecalcOperation.Key; }
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name
        {
            get { return "Ручной перерасчет"; }
        }

        /// <summary>
        /// Права доступа
        /// </summary>
        public override string PermissionKey
        {
            get { return "GkhRegOp.PersonalAccount.Registry.Action.ManuallyRecalc"; }
        }

        /// <summary>
        /// Выполнение действия
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат</returns>
        public override IDataResult Execute(BaseParams baseParams)
        {
            var accIds = baseParams.Params.GetAs<string>("accIds").ToLongArray();
            var recalcDateStart = baseParams.Params.GetAs<DateTime?>("recalcDateStart");

            var persAccChangeDomain = this.Container.ResolveDomain<PersonalAccountChange>();
            var historyFactory = this.Container.Resolve<IPersonalAccountHistoryCreator>();
            try
            {
                this.Container.InTransaction(() =>
                    {
                        using (new DatabaseMutexContext("Manually_Recalc", "Ручной перерасчет"))
                        {
                            var changeInfo = PersonalAccountChangeInfo.FromParams(baseParams);

                            var changes = accIds.Select(
                                accId => historyFactory.CreateChange(
                                    new BasePersonalAccount {Id = accId},
                                    PersonalAccountChangeType.ManuallyRecalc,
                                    string.Empty,
                                    string.Empty,
                                    string.Empty,
                                    recalcDateStart,
                                    changeInfo.Document,
                                    changeInfo.Reason)).ToList();

                            TransactionHelper.InsertInManyTransactions(this.Container, changes);
                        }
                    });

            }
            catch (DatabaseMutexException)
            {
                return BaseDataResult.Error("Ручной перерасчет уже запущен");
            }
            finally
            {
                this.Container.Release(persAccChangeDomain);
                this.Container.Release(historyFactory);
            }

            return new BaseDataResult();
        }
    }
}
