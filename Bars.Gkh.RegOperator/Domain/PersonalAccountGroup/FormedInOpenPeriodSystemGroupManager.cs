namespace Bars.Gkh.RegOperator.PersonalAccountGroup
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;

    /// <summary>
    /// Класс для работы с группой "Сформирован документ в открытом периоде"
    /// </summary>
    public class FormedInOpenPeriodSystemGroupManager : IGroupManager
    {
        private const string FormedInOpenPeriodPrefix = "Сформирован документ в открытом периоде";

        private readonly IWindsorContainer container;
        private readonly IChargePeriodRepository chargePeriodRepository;
        private readonly IPersonalAccountSystemGroupService systemGroupService;
        private readonly IDomainService<EntityLogLight> entLogLightService;
        private readonly IGkhUserManager userManager;

        /// <summary>
        /// .ctor
        /// </summary>
        public FormedInOpenPeriodSystemGroupManager(IWindsorContainer container, 
                                                    IChargePeriodRepository chargePeriodRepository, 
                                                    IPersonalAccountSystemGroupService systemGroupService, 
                                                    IDomainService<EntityLogLight> entLogLightService, 
                                                    IGkhUserManager userManager)
        {
            this.container = container;
            this.chargePeriodRepository = chargePeriodRepository;
            this.systemGroupService = systemGroupService;
            this.entLogLightService = entLogLightService;
            this.userManager = userManager;
        }

        /// <summary>
        /// Добавление в группу с проверкой периода
        /// </summary>
        /// <param name="period">Период</param>
        /// <param name="accountIds">Лс для добавления</param>
        public void AddToGroupWithCheckPeriod(IPeriod period, List<long> accountIds)
        {
            if (accountIds.Count > 0)
            {
                if (period.Id == this.chargePeriodRepository.GetCurrentPeriod().Id && !period.IsClosed)
                {
                    var result = this.systemGroupService.AddPersonalAccountsToSystemGroup(
                        accountIds,
                        this.GetCurrentSystemGroupNameFormedInOpenPeriod(period.Name),
                        true);

                    if (result.Success)
                    {
                        this.WriteHistoryAboutAddToSystemGroup(
                            accountIds.ToArray(),
                            this.GetCurrentSystemGroupNameFormedInOpenPeriod(period.Name),
                            period,
                            true);
                    }
                }
            }
        }

        /// <summary>
        /// Удаление ЛС из группы
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат выполнения</returns>
        public IDataResult RemoveFromGroup(BaseParams baseParams)
        {
            var accounts = baseParams.Params.GetAs<string>("accountsId").ToLongArray();
            if (accounts.IsNotNull() && accounts.Length > 0)
            {
                var systemGroupName = this.GetCurrentSystemGroupNameFormedInOpenPeriod();

                var result = this.systemGroupService.RemovePersonalAccountsFromSystemGroups(baseParams, systemGroupName);
                if (result.Success)
                {
                    this.WriteHistoryAboutAddToSystemGroup(accounts.ToArray(), this.GetCurrentSystemGroupNameFormedInOpenPeriod(), this.chargePeriodRepository.GetCurrentPeriod(), false);
                }

                return result;
            }

            return BaseDataResult.Error("Нужно выбрать лицевые счета");
        }

        /// <summary>
        /// Очистка группы
        /// </summary>
        public void RemoveAllFromGroup(IPeriod period)
        {
            var periodName = this.chargePeriodRepository.Get(period.Id).Name;
            var accounts = this.systemGroupService.RemoveAllPersonalAccountsFromSystemGroup(this.GetCurrentSystemGroupNameFormedInOpenPeriod(periodName));
            this.WriteHistoryAboutAddToSystemGroup(accounts.ToArray(), this.GetCurrentSystemGroupNameFormedInOpenPeriod(), period, false);
        }

        /// <summary>
        /// Возвращает количество ЛС в группе
        /// </summary>
        /// <returns>Количество ЛС в группе</returns>
        public IDataResult GetCountByGroup()
        {
            return this.systemGroupService.GetCountPersonalAccountsInSystemGroup(this.GetCurrentSystemGroupNameFormedInOpenPeriod());
        }

        /// <summary>
        /// Возвращает имя системной группы
        /// </summary>
        /// <returns></returns>
        private string GetCurrentSystemGroupNameFormedInOpenPeriod(string currentPeriodName = null)
        {
            if (currentPeriodName.IsNull())
            {
                var periodRepository = this.container.Resolve<IChargePeriodRepository>();
                currentPeriodName = periodRepository.GetCurrentPeriod().Name;
                this.container.Release(periodRepository);
            }

            return $"{FormedInOpenPeriodSystemGroupManager.FormedInOpenPeriodPrefix} ({currentPeriodName})";
        }


        /// <summary>
        /// Пишем лог в легковесное сущность о занесении или удалении ЛС из системной группы "Сформирован документ в текущем периоде"
        /// </summary>
        /// <param name="accountIds">Массив идентификаторов ЛС</param>
        /// <param name="groupName">Имя системной группы</param>
        /// <param name="period">Период</param>
        /// <param name="isAddToGroup">Добавление в группу или удаление из нее</param>
        private void WriteHistoryAboutAddToSystemGroup(long[] accountIds, string groupName, IPeriod period, bool isAddToGroup)
        {
            var userLogin = this.userManager.GetActiveUser().Return(y => y.Login);
            foreach (var t in accountIds)
            {
                this.entLogLightService.Save(
                    new EntityLogLight
                    {
                        EntityId = t,
                        ClassName = "BasePersonalAccount",
                        PropertyName = "PersonalAccount",
                        ParameterName = isAddToGroup
                            ? "Печать в открытом периоде"
                            : "Снятие признака печати в открытом периоде",
                        PropertyDescription = isAddToGroup
                            ? $"Автоматическое попадание ЛС в группу \"{groupName}\""
                            : $"Удаление ЛС из группы \"{groupName}\"",
                        PropertyValue = "",
                        DateActualChange = period.StartDate,
                        DateApplied = DateTime.Now,
                        User = userLogin,
                        Reason = "",
                        Document = null
                    });
            }
        }
    }
}
