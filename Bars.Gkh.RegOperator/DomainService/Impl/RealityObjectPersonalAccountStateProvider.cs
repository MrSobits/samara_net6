namespace Bars.Gkh.RegOperator.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService.RegionalFormingOfCr;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;

    /// <summary>
    /// Сервис проставления статусов ЛС по состоянию дома
    /// </summary>
    public class RealityObjectPersonalAccountStateProvider : IRealityObjectPersonalAccountStateProvider
    {
        private bool initiated;

        private IList<CrFundFormationType> types;
        private bool useBlockedBuilding;
        private StatefulEntityInfo entityInfo;

        /// <summary>
        /// Домен-сервис <see cref="RealityObjectDecisionProtocol"/>
        /// </summary>
        public IDomainService<RealityObjectDecisionProtocol> RealityObjectDecisionProtocolDomain { get; set; }

        /// <summary>
        /// Репозиторий <see cref="State"/>
        /// </summary>
        public IRepository<State> StateRepository { get; set; }

        /// <summary>
        /// Репозиторий <see cref="BasePersonalAccount"/>
        /// </summary>
        public IRepository<BasePersonalAccount> BasePersonalAccountRepository { get; set; }

        /// <summary>
        /// Репозиторий <see cref="PersonalAccountChange"/>
        /// </summary>
        public IRepository<PersonalAccountChange> PersonalAccountChangeRepository { get; set; }

        /// <summary>
        /// Интерфейс для определения способа формирования фонда
        /// </summary>
        public ITypeOfFormingCrProvider FormingCrProvider { get; set; }

        /// <summary>
        /// Провайдер статусов
        /// </summary>
        public IStateProvider StateProvider { get; set; }

        /// <summary>
        /// Сервис работы со специальными счетами
        /// </summary>
        public ISpecialCalcAccountService SpecialCalcAccountService { get; set; }

        /// <summary>
        /// Получения конфига "Настройки версии ДПКР с МО"
        /// </summary>
        public IHouseTypesConfigService HouseTypesConfigService { get; set; }

        /// <summary>
        /// Интерфейс провайдера конфигураций
        /// </summary>
        public IConfigProvider ConfigProvider { get; set; }

        /// <summary>
        /// Расчетные периоды
        /// </summary>
        public IChargePeriodRepository PeriodRepository { get; set; }

        public IGkhUserManager GkhUserManager { get; set; }

        /// <summary>
        /// Проставить актуальные статусы ЛС по состоянию дома и единым настройкам
        /// </summary>
        /// <param name="realityObject">Жилой дом</param>
        public void SetPersonalAccountStateIfNeed(RealityObject realityObject)
        {
            var user = this.GkhUserManager.GetActiveUser()?.Login;

            this.InitVariablesIfNeed();

            if (!this.types.Any())
            {
                return;
            }

            // если ЛС дома должны рассчитываться
            var calculableRealityObject = this.IsRealityObjectCalculableBySettings(realityObject);
            bool activeByHouseState;

            // активность лицевого счёта по состоянию дома
            if (this.useBlockedBuilding)
            {
                activeByHouseState = realityObject.TypeHouse == TypeHouse.BlockedBuilding || realityObject.TypeHouse == TypeHouse.ManyApartments;
            }
            else
            {
                activeByHouseState = realityObject.TypeHouse == TypeHouse.ManyApartments;
            }

            var openState = this.StateRepository.GetAll().FirstOrDefault(x => x.Name == "Открыт" && x.TypeId == this.entityInfo.TypeId);
            var notActiveState = this.StateRepository.GetAll().FirstOrDefault(x => x.Name == "Не активен" && x.TypeId == this.entityInfo.TypeId);

            if (!realityObject.IsNotInvolvedCr && activeByHouseState && calculableRealityObject)
            {
                this.BasePersonalAccountRepository.GetAll()
                    .Where(x => x.Room.RealityObject.Id == realityObject.Id)
                    .Where(x => x.State.Id == notActiveState.Id) // меняем статус только у тех ЛС, которые неактивные (остальные не трогаем)
                    .ToList()
                    .ForEach(x =>
                    {
                        this.StateProvider.ChangeState(
                            x.Id, 
                            this.entityInfo.TypeId,
                            openState,
                            "На основании изменения данных по дому",
                            false);
                        this.BasePersonalAccountRepository.Update(x);

                        this.PersonalAccountChangeRepository.Save(new PersonalAccountChange
                        {
                            PersonalAccount = x,
                            ChangeType = PersonalAccountChangeType.Open,
                            Date = DateTime.Now,
                            Description = "Для ЛС установлен статус \"Открыт\"",
                            ActualFrom = DateTime.Now,
                            NewValue = "Статус ЛС = Открыт",
                            ChargePeriod = this.PeriodRepository.GetCurrentPeriod(),
                            Operator = user
                        });
                    });
            }
            else if ((!this.useBlockedBuilding && realityObject.TypeHouse == TypeHouse.BlockedBuilding && !realityObject.IsNotInvolvedCr) || realityObject.IsNotInvolvedCr || !calculableRealityObject)
            {
                // Для всех ЛС по этому дому делаем статус 'Не активен'
                this.BasePersonalAccountRepository.GetAll()
                    .Where(x => x.Room.RealityObject.Id == realityObject.Id)
                    .Where(x => x.State.Id == openState.Id) // меняем статус только у тех ЛС, которые открыты (закрытие не трогаем)
                    .AsEnumerable()
                    .ToList()
                    .ForEach(x =>
                    {
                        this.StateProvider.ChangeState(
                            x.Id,
                            this.entityInfo.TypeId,
                            notActiveState,
                            "На основании изменения данных по дому",
                            false);
                        this.BasePersonalAccountRepository.Update(x);

                        this.PersonalAccountChangeRepository.Save(new PersonalAccountChange
                        {
                            PersonalAccount = x,
                            ChangeType = PersonalAccountChangeType.NonActive,
                            Date = DateTime.Now,
                            Description = "Для ЛС установлен статус \"Не активен\"",

                                // Operator = userManager.GetActiveUser()?.Login,
                                ActualFrom = DateTime.Now,
                            NewValue = "Статус ЛС = Не активен",
                            Reason =
                                realityObject.IsNotInvolvedCr
                                    ? "Установление признака \"Не участвует в ДПКР\" в доме"
                                        : "Установление типа дома \"Блокированной застройки\" (тип дома не участвует в ДПКР)",
                                ChargePeriod = this.PeriodRepository.GetCurrentPeriod(),
                            Operator = user
                        });
                    });
            }
            else
            {
                // иначе находим актуальный протокол решения
                var actualProtocol = this.RealityObjectDecisionProtocolDomain.GetAll()
                    .Where(x => x.RealityObject.Id == realityObject.Id)
                    .Where(x => x.State.Code == "2")
                    .OrderByDescending(x => x.ProtocolDate)
                    .FirstOrDefault();

                if (actualProtocol != null)
                {
                    this.SpecialCalcAccountService.SetPersonalAccountStatesActiveIfAble(actualProtocol);
                    this.SpecialCalcAccountService.SetPersonalAccountStatesNonActiveIfNeeded(actualProtocol, false, true);
                }
            }
        }

        private void InitVariablesIfNeed()
        {
            if (this.initiated)
            {
                return;
            }

            ProxyHouseTypesConfig houseTypesConfig = null;

            if (this.HouseTypesConfigService != null)
            {
                houseTypesConfig = this.HouseTypesConfigService.GetHouseTypesConfig();
            }

            this.useBlockedBuilding = houseTypesConfig?.UseBlockedBuilding ?? false;

            if (this.types == null)
            {
                this.types = this.FormingCrProvider?.GetCrFundFormationTypesFromSettings() ?? new List<CrFundFormationType>();
            }

            this.entityInfo = this.StateProvider.GetStatefulEntityInfo(typeof(BasePersonalAccount));

            this.initiated = true;
        }

        private bool IsRealityObjectCalculableBySettings(RealityObject realityObject)
        {
            return realityObject.AccountFormationVariant.HasValue
                       ? this.types.Contains(realityObject.AccountFormationVariant.Value)
                       : this.types.Any(x => x == CrFundFormationType.Unknown || x == CrFundFormationType.NotSelected);
        }
    }
}