namespace Bars.Gkh.RegOperator.DomainModelServices.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.DatabaseMutex;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccount;
    using Bars.Gkh.RegOperator.DomainModelServices.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Owner;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Extenstions;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using DomainService.PersonalAccount;

    public class PersonalAccountChangeService : IPersonalAccountChangeService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Менеджер пользователей
        /// </summary>
        public IGkhUserManager UserManager { get; set; }

        /// <summary>
        /// Менеджер файлов
        /// </summary>        
        public IFileManager FileManager { get; set; }

        /// <summary>
        /// Провайдер статусов
        /// </summary>
        public IStateProvider StateProvider { get; set; }

        /// <summary>
        /// Интерфейс сервиса абонентов
        /// </summary>
        public IPersonalAccountOwnerService PersonalAccountOwnerService { get; set; }

        /// <summary>
        /// Сервис массового изменения сальдо
        /// </summary>
        public IAccountSaldoChangeService AccountSaldoChangeService { get; set; }

        /// <summary>
        /// Интерфес сервиса для массовой работы с BasePersonalAccountDto
        /// </summary>
        public IMassPersonalAccountDtoService MassPersonalAccountDtoService { get; set; }

        /// <summary>
        /// Сервис для валидации площади помещения
        /// </summary>
        public IRoomAreaShareSpecification RoomAreaShareSpecification { get; set; }

        /// <summary>
        /// Репозиторий для Периода начислений
        /// </summary>
        public IChargePeriodRepository PeriodRepository { get; set; }

        /// <summary>
        /// Фабрика для создания ЛС
        /// </summary>
        public IPersonalAccountFactory PersonalAccountFactory { get; set; }

        /// <summary>
        /// Домен-сервис для <see cref="BasePersonalAccount"/>
        /// </summary>
        public IDomainService<BasePersonalAccount> AccountDomain { get; set; }

        /// <summary>
        /// Домен-сервис для <see cref="PersonalAccountPeriodSummary"/>
        /// </summary>
        public IDomainService<PersonalAccountPeriodSummary> PersonalAccountPeriodSummaryDomain { get; set; }

        /// <summary>
        /// Домен-сервис для <see cref="PersonalAccountOwner"/>
        /// </summary>
        public IDomainService<PersonalAccountOwner> AccountOwnerDomain { get; set; }

        /// <summary>
        /// Домен-сервис для <see cref="PersonalAccountChange"/>
        /// </summary>
        public IDomainService<PersonalAccountChange> ChangeDomain { get; set; }

        /// <summary>
        /// Домен-сервис для <see cref="PersonalAccountOwnerInformation"/>
        /// </summary>
        public IDomainService<PersonalAccountOwnerInformation> OwnerInfoDomain { get; set; }

        /// <summary>
        /// Домен-сервис для <see cref="Wallet"/>
        /// </summary>
        public IDomainService<Wallet> WalletDomain { get; set; }


        /// <summary>
        /// Домен-сервис для <see cref="Room"/>
        /// </summary>
        public IDomainService<Room> RoomDomain { get; set; }


        /// <summary>
        /// Домен-сервис для <see cref="EntityLogLight"/>
        /// </summary>
        public IDomainService<EntityLogLight> EntityLogLightDomain { get; set; }

        /// <summary>
        /// Домен-сервис для <see cref="FileInfo"/>
        /// </summary>
        public IDomainService<FileInfo> FileInfoDomain { get; set; }

        /// <summary>
        /// Домен-сервис для <see cref="Transfer"/>
        /// </summary>
        public IDomainService<Transfer> TransferDomain { get; set; }

        private static readonly object syncRoot = new object();

        public IDataResult ChangeAreaShare(BaseParams baseParams)
        {
            var areaShare = baseParams.Params.GetAs<string>("value");
            var dateActual = baseParams.Params.GetAs<DateTime>("factDate");

            decimal value;
            if (!decimal.TryParse(areaShare, out value))
            {
                return new BaseDataResult(false, "Неверное число");
            }

            var id = baseParams.Params.GetAsId("entityId");

            try
            {
                var account = this.AccountDomain.Get(id);
                
                var file = baseParams.Files.FirstOrDefault();
                FileInfo fileInfo = null;
                if (file.Value != null)
                {
                    fileInfo = this.FileManager.SaveFile(file.Value);
                }

                this.ChangeAreaShare(account, value, dateActual, fileInfo);

                this.AccountDomain.Update(account);
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }

            return new BaseDataResult(true, "Success!");
        }

        /// <summary>
        /// Смена абонента.
        /// </summary>
        /// <param name="baseParams">The base params.</param>
        /// <returns>The <see cref="IDataResult"/>.</returns>
        public IDataResult ChangeOwner(BaseParams baseParams)
        {
            var hasOwnerInfo = false;
            var hasNewAccount = false;

            if (!Monitor.TryEnter(PersonalAccountChangeService.syncRoot))
            {
                return BaseDataResult.Error("\"Смена абонента\" уже запущена");
            }

            try
            {
                this.Container.InTransaction(
                    () =>
                        {
                            var accountId = baseParams.Params.GetAsId("AccountId");
                            var newOwnerId = baseParams.Params.GetAsId("NewOwner");
                            var account = this.AccountDomain.GetAll().FirstOrDefault(x => x.Id == accountId);
                            var newOwner = this.AccountOwnerDomain.GetAll().FirstOrDefault(x => x.Id == newOwnerId);
                            var owners = new List<PersonalAccountOwner> { newOwner, account.AccountOwner };
                            var changeInfo = PersonalAccountChangeInfo.FromParams(baseParams);
                            var room = account != null ? RoomDomain.GetAll().Where(x => x.Id == account.Room.Id).FirstOrDefault() : null;
                            var newOwnershipType = baseParams.Params.GetAs<RoomOwnershipType>("NewOwnershipType");
                            room.OwnershipType = newOwnershipType;
                            RoomDomain.Update(room);

                            this.EntityLogLightDomain.Save(new EntityLogLight
                            {
                                EntityId = room.Id,
                                ClassName = "Room",
                                PropertyName = "OwnershipType",
                                PropertyValue = room.OwnershipType.ToString(),
                                DateApplied = DateTime.UtcNow,
                                DateActualChange = changeInfo.DateActual,
                                ParameterName = "room_ownership_type",
                                User = this.UserManager.GetActiveUser().Return(x => x.Login)
                            });

                hasOwnerInfo = this.OwnerInfoDomain.GetAll().Any(x => x.BasePersonalAccount.Id == accountId);

                            var governmentTypes = new[]
                            {
                                RoomOwnershipType.Municipal,
                                RoomOwnershipType.Federal,
                                RoomOwnershipType.Goverenment,
                                RoomOwnershipType.Regional
                            };

                            if (changeInfo.NewLS)//  (account.AccountOwner.OwnerType == PersonalAccountOwnerType.Legal && governmentTypes.Contains(account.Room.OwnershipType)))
                            {
                                account.AreaShare = 0m;
                                this.StateProvider.SetDefaultState(account);
                                this.AccountDomain.Update(account);
                                this.EntityLogLightDomain.Save(new EntityLogLight
                                {
                                    ClassName = "BasePersonalAccount",
                                    EntityId = account.Id,
                                    PropertyDescription = $"Изменение доли собственности в связи со сменой абонента с \"{account.AccountOwner.Return(x => x.Name)}\" на \"{newOwner.Return(x => x.Name)}\"",
                                    PropertyName = "AreaShare",
                                    PropertyValue = account.AreaShare.ToStr(),
                                    DateActualChange = changeInfo.DateActual,
                                    DateApplied = DateTime.UtcNow,
                                    Document = changeInfo.Document,
                                    Reason = changeInfo.Reason,
                                    ParameterName = "area_share",
                                    User = this.UserManager.GetActiveUser().Return(x => x.Login)
                                });
                                
                                var newAccount = this.PersonalAccountFactory.CreateNewAccount(
                                    newOwner,
                                    account.Room,
                                    changeInfo.DateActual,
                                    1m);

                                newAccount.Area = account.Area;
                                newAccount.LivingArea = account.LivingArea;
                                newAccount.PersAccNumExternalSystems = newAccount.PersonalAccountNum;

                                hasNewAccount = true;
                                this.MassPersonalAccountDtoService.AddPersonalAccount(newAccount);

                                newAccount.GetWallets().ForEach(x => this.WalletDomain.Save(x));
                                this.AccountDomain.Save(newAccount);

                                this.SaveHistory(
                                    account,
                                    PersonalAccountChangeType.OwnerChange,
                                    $"Создался новый лицевой счет {newAccount.PersonalAccountNum} в связи со сменой абонента с \"{account.AccountOwner.Return(x => x.Name)}\" на \"{newOwner.Return(x => x.Name)}\"",
                                    newOwner.Return(x => x.Id).ToStr(),
                                    changeInfo.Document,
                                    changeInfo.Reason,
                                    changeInfo.DateActual);

                                this.SaveHistory(
                                    newAccount,
                                    PersonalAccountChangeType.OwnerChange,
                                    $"Смена абонента ЛС с \"{account.AccountOwner.Return(x => x.Name)}\" на \"{newOwner.Return(x => x.Name)}\"",
                                    newOwner.Return(x => x.Id).ToStr(),
                                    changeInfo.Document,
                                    changeInfo.Reason,
                                    changeInfo.DateActual);
                            }
                            else
                            {
                                this.ChangeOwner(account, newOwner, changeInfo);
                                this.AccountDomain.Update(account);
                            }
                            
                            foreach (var owner in owners)
                            {
                                if (this.PersonalAccountOwnerService.OnUpdateOwner(owner))
                                {
                                    this.AccountOwnerDomain.Update(owner);
                                }
                            }
                        });
            }
            catch (Exception e)
            {
                hasNewAccount = false;
                return new BaseDataResult(false, e.Message);
            }
            finally
            {
                if (hasNewAccount)
                {
                    this.MassPersonalAccountDtoService.ApplyChanges();
                }
                
                Monitor.Exit(PersonalAccountChangeService.syncRoot);
            }

            return new BaseDataResult(hasOwnerInfo);
        }

        public IDataResult ChangeDateOpen(BaseParams baseParams)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Изменение баланса за период
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат операции</returns>
        public IDataResult ChangePeriodBalance(BaseParams baseParams)
        {
            var accountId = baseParams.Params.GetAsId("AccountId");
            var newValue = baseParams.Params.GetAs<decimal>("NewValue");
            var reason = baseParams.Params.GetAs<string>("Reason");
            var document = baseParams.Files.Get("Document");

            IDataResult result = new BaseDataResult();

            try
            {
                var account = this.AccountDomain.GetAll().FirstOrDefault(x => x.Id == accountId);

                if (account == null)
                {
                    return new BaseDataResult(false, "Не удалось получить лицевой счет");
                }

                this.Container.InTransaction(() =>
                {
                    using (new DatabaseMutexContext($"Update_Balance_{account.Id}", "Установка и изменение сальдо"))
                    {
                        result = this.AccountSaldoChangeService.ProcessSaldoChange(account, newValue, document, reason);
                    }
                });
            }
            catch (DatabaseMutexException)
            {
                return BaseDataResult.Error("Установка и изменение сальдо по данному абоненту уже запущен");
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(false, e.Message);
            }
            catch (InvalidOperationException e)
            {
                return new BaseDataResult(false, e.Message);
            }

            return result;
        }

        /// <summary>
        /// Метод ручного изменения пени 
        /// </summary>
        public IDataResult ChangePenalty(BaseParams baseParams)
        {
            var accountId = baseParams.Params.GetAs<long>("AccountId", ignoreCase: true);
            var newPenalty = baseParams.Params.GetAs<decimal>("NewPenalty", ignoreCase: true);
            var debetPenalty = baseParams.Params.GetAs<decimal>("DebtPenalty", ignoreCase: true);
            var reason = baseParams.Params.GetAs<string>("Reason", ignoreCase: true);
            var doc = baseParams.Files.Get("Document");
            var period = this.PeriodRepository.GetCurrentPeriod();
            var banRecalcManager = this.Container.Resolve<IPersonalAccountBanRecalcManager>();

            try
            {
                var acc = this.AccountDomain.Get(accountId);

                if (acc == null)
                {
                    return new BaseDataResult(false, "Не удалось найти лицевой счет!");
                }

                this.Container.InTransaction(() =>
                    {
                        using (new DatabaseMutexContext($"Update_Penalty_{acc.Id}", "Отмена начислений пени"))
                        {
                            // Сохраняем пришедший документ основание изменения пени
                            FileInfo fileInfo = null;
                            if (doc != null)
                            {
                                fileInfo = this.FileManager.SaveFile(doc);
                                this.FileInfoDomain.Save(fileInfo);
                            }

                            // Производим изменение пеней на счете
                            var transfer = acc.ChangePenalty(newPenalty, debetPenalty, reason, fileInfo, period);

                            // Сохраняем изменения на счете
                            this.AccountDomain.Update(acc);

                            var summary = acc.GetOpenedPeriodSummary();
                            this.PersonalAccountPeriodSummaryDomain.Update(summary);

                            // сохраняем трансфер
                            if (transfer != null)
                            {
                                this.TransferDomain.Save(transfer);

                                this.SaveHistory(acc,
                                    PersonalAccountChangeType.PenaltyChange,
                                    $"Операция изменения пени на \"{newPenalty}\" " + $"согласно Документа-основания \"{doc.Return(x => x.FileName)}\" "
                                    + $"по причине \"{reason}\"",
                                    Convert.ToString(newPenalty),
                                    fileInfo,
                                    reason);
                            }

                            banRecalcManager.CreateBanRecalc(
                                acc,
                                period.StartDate,
                                period.GetEndDate(),
                                BanRecalcType.Penalty,
                                fileInfo,
                                "Установка и изменение пени");
                            banRecalcManager.SaveBanRecalcs();
                        }
                    });
            }
            catch (DatabaseMutexException)
            {
                return BaseDataResult.Error("Отмена начислений пени по данному абоненту уже запущен");
            }
            catch (Exception e)
            {
                return new BaseDataResult
                {
                    Success = false,
                    Message = "Произошла ошибка при изменении пени: " + e.Message
                };
            }

            return new BaseDataResult(true, "Изменение пени прошло успешно.");
        }

        public void ChangeAreaShare(BasePersonalAccount account, decimal newValue, DateTime dateActual, FileInfo fileInfo)
        {
            ArgumentChecker.NotNull(account, nameof(account));

            var decimals = this.Container.GetGkhConfig<RegOperatorConfig>().GeneralConfig.AreaShareConfig.DecimalsAreaShare;

            newValue = newValue.RegopRoundDecimal(decimals);

            account.SetAreaShare(this.RoomAreaShareSpecification, newValue, dateActual, fileInfo);
        }

        /// <summary>
        /// Смена абонента.
        /// </summary>
        /// <param name="account">Лицевой счет.</param>
        /// <param name="newOwner">Новый собственник ЛС.</param>
        /// <param name="changeInfo">Информация для истории изменений.</param>
        public void ChangeOwner(BasePersonalAccount account, PersonalAccountOwner newOwner, PersonalAccountChangeInfo changeInfo)
        {
            ArgumentChecker.NotNull(account, nameof(account));

            account.SetOwner(newOwner, changeInfo);
        }

        public void ChangeDateOpen(BasePersonalAccount account, DateTime newDateOpen, DateTime dateActual)
        {
            ArgumentChecker.NotNull(account, nameof(account));

            account.SetDateOpen(newDateOpen);
        }

        private void SaveHistory(BasePersonalAccount account, PersonalAccountChangeType type, string description,
            string newValue, FileInfo document, string reason, DateTime? actualFrom = null)
        {
            this.ChangeDomain.Save(new PersonalAccountChange
            {
                PersonalAccount = account,
                ChangeType = type,
                Date = DateTime.UtcNow,
                Description = description,
                Operator = this.UserManager.GetActiveUser().Return(x => x.Login),
                ActualFrom = actualFrom,
                NewValue = newValue,
                Document = document,
                Reason = reason,
                ChargePeriod = this.PeriodRepository.GetCurrentPeriod()
            });
        }
    }
}