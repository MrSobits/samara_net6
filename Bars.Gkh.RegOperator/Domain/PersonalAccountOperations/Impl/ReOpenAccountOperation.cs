namespace Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Impl
{
    using System;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Modules.FileStorage;
    using B4.Modules.Security;
    using B4.Modules.States;
    using B4.Utils;

    using Bars.Gkh.Domain.DatabaseMutex;

    using Castle.Windsor;
    using Decisions.Nso.Domain;
    using Decisions.Nso.Entities;
    using Entities;
    using Entities.PersonalAccount;
    using Enums;
    using Gkh.Domain;
    using Gkh.Domain.CollectionExtensions;
    using Gkh.Entities;
    using Gkh.Enums;
    using Gkh.Enums.Decisions;

    /// <summary>
    /// Повторное открытие ЛС после закрытия
    /// </summary>
    public class ReOpenAccountOperation : PersonalAccountOperationBase
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
        /// Провайдер сессий NHibernate
        /// </summary>
        public ISessionProvider SessionProvider;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container">Контейнер</param>
        /// <param name="personalAccountDomain">Домен-сервис лицевых счетов</param>
        /// <param name="stateDomainService">Домен-сервис состояний</param>
        /// <param name="sessionProvider">Провайдер сессий NHibernate</param>
        public ReOpenAccountOperation(
            IWindsorContainer container,
            IDomainService<BasePersonalAccount> personalAccountDomain,
            IDomainService<State> stateDomainService,
            ISessionProvider sessionProvider)
        {
            this.Container = container;
            this.PersonalAccountDomain = personalAccountDomain;
            this.StateDomain = stateDomainService;
            this.SessionProvider = sessionProvider;
        }

        /// <summary>
        /// Ключ
        /// </summary>
        public static string Key => "ReOpenAccountOperation";

        /// <summary>
        /// Код
        /// </summary>
        public override string Code => ReOpenAccountOperation.Key;

        /// <inheritdoc />
        public override string PermissionKey => "GkhRegOp.PersonalAccount.Registry.Action.ReOpenAccountOperation.View";

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Повторное открытие";

        /// <summary>
        /// Выполнение действия
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат</returns>
        public override IDataResult Execute(BaseParams baseParams)
        {
            IDataResult result = new BaseDataResult();

            try
            {
                this.Container.InTransaction(() =>
                    {
                        using (new DatabaseMutexContext("Re-opening_Account", "Повторное открытие лицевого счета"))
                        {
                            var accIds = baseParams.Params.GetAs<string>("accids").ToLongArray();
                            var personalAccounts = this.PersonalAccountDomain.GetAll().Where(x => accIds.Contains(x.Id)).ToList();

                            foreach (var personalAccount in personalAccounts)
                            {
                                result = this.OpenAccount(baseParams, personalAccount);

                                if (!result.Success)
                                {
                                    break;
                                }
                            }

                            if (!result.Success)
                            {
                                throw new ValidationException(result.Message);
                            }
                        }
                    });
                return result;
            }
            catch (DatabaseMutexException)
            {
                return BaseDataResult.Error("Повторное открытие лицевого счета уже запущено");
            }
        }

        /// <summary>
        /// Открытие Лицевой счет
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <param name="personalAccount">Лицевой счет</param>
        /// <returns></returns>
        private IDataResult OpenAccount(BaseParams baseParams, BasePersonalAccount personalAccount)
        {
            var persAccChange = this.Container.ResolveDomain<PersonalAccountChange>();
            var entityLogLightDomain = this.Container.ResolveDomain<EntityLogLight>();
            var fileManager = this.Container.Resolve<IFileManager>();
            var userRepo = this.Container.ResolveDomain<User>();
            var userIdentity = this.Container.Resolve<IUserIdentity>();
            var openBeforeClose = baseParams.Params.GetAs("openBeforeClose", false, true);

            try
            {
                if (personalAccount.State.Code != "2" && personalAccount.State.Code != "3")
                {
                    return new BaseDataResult(false, "Счет уже открыт!");
                }

                if (personalAccount.Room.RealityObject.ConditionHouse == ConditionHouse.Emergency
                    || personalAccount.Room.RealityObject.ConditionHouse == ConditionHouse.Razed)
                {
                    return new BaseDataResult(false,
                        "Состояние дома установлено {0}. Открытие счета не доступно".FormatUsing(
                            personalAccount.Room.RealityObject.ConditionHouse.GetEnumMeta().Display));
                }

                var isMergeAccount =
                    persAccChange.GetAll()
                        .Any(x => x.PersonalAccount.Id == personalAccount.Id && x.ChangeType == PersonalAccountChangeType.MergeAndClose);

                if (isMergeAccount)
                {
                    return new BaseDataResult(false, "Счет был закрыт в связи со слиянием. Открытие счета не доступно.");
                }

                var openDate = baseParams.Params.GetAs<DateTime>("openDate");
                var reason = baseParams.Params.GetAs<string>("Reason");

                var closeDate = persAccChange.GetAll()
                     .Where(x => x.PersonalAccount.Id == personalAccount.Id && x.ChangeType == PersonalAccountChangeType.Close)
                     .Select(x => x.ActualFrom ?? x.Date)
                     .OrderByDescending(x => x)
                     .FirstOrDefault();

                if (!openBeforeClose && openDate.Date < closeDate.Date)
                {
                    return new BaseDataResult(false, @"Дата повторного открытия счета не может быть раньше даты закрытия счета {0}.
                             Дата закрытия счета указана в карточке счета, в разделе 'История изменений'".FormatUsing(closeDate.ToShortDateString()));
                }

                var areaShare = entityLogLightDomain.GetAll()
                    .Where(x => x.EntityId == personalAccount.Id)
                    .Where(x => x.ParameterName == "area_share")
                    .Where(x => x.ClassName == "BasePersonalAccount")
                    .Where(x => (x.PropertyDescription ?? string.Empty) != "Изменение доли собственности в связи с закрытием ЛС")
                    .OrderByDescending(x => x.DateApplied)
                    .Select(x => x.PropertyValue)
                    .AsEnumerable()
                    .Select(x => x.ToDecimal())
                    .FirstOrDefault();

                var allAreaShare = this.PersonalAccountDomain.GetAll()
                    .Where(x => x.Room.Id == personalAccount.Room.Id)
                    .Where(x => x.Id != personalAccount.Id)
                    .SafeSum(x => x.AreaShare);

                if (areaShare + allAreaShare > 1)
                {
                    return new BaseDataResult(false, @"Доля собственности помещения по всем счетам превышает 1.
                                Доля собственности по текущему счету изменена не будет, и равна 0.
                                Проведите изменение доли собственности в карточке абонента.");
                }
                else
                {
                    personalAccount.AreaShare = areaShare;
                }

                personalAccount.State = this.GetPersonalAccountState(personalAccount.Room.RealityObject);
                personalAccount.SetCloseDate(DateTime.MinValue, false);
                personalAccount.OpenDate = openDate.Date;
                this.PersonalAccountDomain.Update(personalAccount);

                var documentInfo = baseParams.Files.ContainsKey("Document") ? fileManager.SaveFile(baseParams.Files["Document"]) : null;

                var login = userRepo.Get(userIdentity.UserId).Return(u => u.Login);

                if (personalAccount.AreaShare > 0)
                {
                    entityLogLightDomain.Save(new EntityLogLight
                    {
                        ClassName = "BasePersonalAccount",
                        EntityId = personalAccount.Id,
                        PropertyName = "AreaShare",
                        PropertyValue = areaShare.ToString(),
                        DateActualChange = openDate,
                        DateApplied = DateTime.UtcNow,
                        ParameterName = "area_share",
                        PropertyDescription = "Изменение доли собственности в связи с повторным открытием счета",
                        User = login.IsEmpty() ? "anonymous" : login
                    }); 
                }

                entityLogLightDomain.Save(new EntityLogLight
                {
                    ClassName = "BasePersonalAccount",
                    EntityId = personalAccount.Id,
                    PropertyDescription = "Открытие ЛС в связи с повторным открытием счета",
                    PropertyName = "OpenDate",
                    PropertyValue = personalAccount.OpenDate.ToString(),
                    DateActualChange = openDate,
                    DateApplied = DateTime.UtcNow,
                    Document = documentInfo,
                    Reason = reason,
                    ParameterName = "account_open_date",
                    User = login.IsEmpty() ? "anonymous" : login
                });      

                return new BaseDataResult(true);
            }
            finally
            {
                this.Container.Release(persAccChange);
                this.Container.Release(entityLogLightDomain);
                this.Container.Release(fileManager);
                this.Container.Release(userRepo);
                this.Container.Release(userIdentity);
            }
        }

        /// <summary>
        /// Получение статуса Лицевой счет
        /// </summary>
        /// <param name="realityObject">Жилой дом</param>
        /// <returns></returns>
        private State GetPersonalAccountState(RealityObject realityObject)
        {
            var realityObjectDecisionsService = this.Container.Resolve<IRealityObjectDecisionsService>();

            if (realityObject.IsNotInvolvedCr
                || realityObject.ConditionHouse == ConditionHouse.Emergency
                || realityObject.TypeHouse == TypeHouse.BlockedBuilding)
            {
                return this.GetNonActivePersonalAccountState();
            }

            var crFundDecision = realityObjectDecisionsService.GetActualDecision<CrFundFormationDecision>(realityObject, true);

            //если null, то на доме нет решений, а значит делаем ЛС Открытым
            CrFundFormationDecisionType? crTypeDecision = null;

            if (crFundDecision != null)
            {
                if (crFundDecision.Protocol.State.FinalState)
                {
                    crTypeDecision = crFundDecision.Return(x => x.Decision, CrFundFormationDecisionType.Unknown);
                }
                else
                {
                    crTypeDecision = CrFundFormationDecisionType.Unknown;
                }
            }
            else
            {
                var govDecision = realityObjectDecisionsService.GetCurrentGovDecision(realityObject);

                if (govDecision != null)
                {
                    //проверяем, что протокол находится в финальном статусе
                    crTypeDecision = govDecision.State.FinalState && govDecision.FundFormationByRegop
                        ? CrFundFormationDecisionType.RegOpAccount
                        : CrFundFormationDecisionType.Unknown;
                }
            }

            if (crTypeDecision.HasValue)
            {
                switch (crTypeDecision.Value)
                {
                    case CrFundFormationDecisionType.Unknown:
                        return this.GetNonActivePersonalAccountState();
                    case CrFundFormationDecisionType.RegOpAccount:
                        return this.GetOpenPersonalAccountState();
                    case CrFundFormationDecisionType.SpecialAccount:
                        var accOwnerDecision = realityObjectDecisionsService.GetActualDecision<AccountOwnerDecision>(realityObject);
                        //если нет и этого типа протоколов, то также делаем счёт Акивным
                        if (accOwnerDecision == null)
                        {
                            return this.GetOpenPersonalAccountState();
                        }

                        //также проверяем, что протокол решения в финальном статусе
                        if (accOwnerDecision.Protocol.State.FinalState)
                        {
                            switch (accOwnerDecision.DecisionType)
                            {
                                case AccountOwnerDecisionType.Custom:
                                    return this.GetNonActivePersonalAccountState();
                                case AccountOwnerDecisionType.RegOp:
                                    return this.GetOpenPersonalAccountState();
                            }
                        }
                        
                        return this.GetNonActivePersonalAccountState();

                        break;
                }
            }
            else
            {
                return this.GetOpenPersonalAccountState();
            }

            return null;
        }

        private State GetOpenPersonalAccountState()
        {
            return this.StateDomain.FirstOrDefault(x => x.TypeId == "gkh_regop_personal_account" && x.Code == "1");
        }

        private State GetNonActivePersonalAccountState()
        {
            return this.StateDomain.FirstOrDefault(x => x.TypeId == "gkh_regop_personal_account" && x.Code == "4");
        }
    }
}
