namespace Bars.Gkh.RegOperator.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Tasks.Common.Utils;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.CodedReports;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Services.DataContracts;
    using Bars.Gkh.RegOperator.Services.DataContracts.Accounts;
    using Bars.Gkh.Services.DataContracts;

    using Castle.Core.Internal;

    /// <summary>
    /// Сервис по работе с лиц. счетом
    /// </summary>
    public partial class Service
    {
        private PersonalAccountMobileDTO PersAcToDto(BasePersonalAccount account)
        {
            return account != null
                ? new PersonalAccountMobileDTO
                {
                    Id = account.Id,
                    PersonalAccountNum = account.PersonalAccountNum,
                    Address = account.Room.RealityObject.Address + ", кв. " + account.Room.RoomNum,
                    Area = account.Area,
                    Share = account.AreaShare,
                    Tariff = account.Tariff
                }
                : new PersonalAccountMobileDTO();
        }
        
         /// <summary>
        /// Получение номер л/с
        /// </summary>
        /// <param name="exNum">Номер Л/С во внешних системах</param>
        /// <param name="address">Адрес помещения</param>
        /// <param name="lastName">Фамилия</param>
        /// <param name="firstName">Имя</param>
        /// <param name="middleName">Отчество</param>
        /// <returns>Номер л/с</returns>
        public PersonalAccountMobileDTO GetPersonalAccountNumMobileQR(string exNum, string address, string lastName, string firstName, string middleName)
        {
            var service = this.Container.Resolve<IDomainService<BasePersonalAccount>>();

            List<BasePersonalAccount> accounts = new List<BasePersonalAccount>();
            try
            {
                if (!exNum.IsNullOrEmpty())
                {

                    //проверяем номер ЛС в системе
                    accounts.AddRange(
                        service.GetAll()
                            .Where(x => x.PersonalAccountNum.Equals(exNum)));



                    if (accounts.Count != 0)
                    {
                        var account = accounts.FirstOrDefault();

                        return this.PersAcToDto(account);
                    }
                    //проверяем полное совпадение
                    accounts.AddRange(
                 service
                     .GetAll()
                     .Where(x => x.PersAccNumExternalSystems.Equals(exNum)
                     ).FilterByFio(lastName, firstName, middleName));
                    
                    //если не нашли ищем контэйнс
                    if (accounts.Count == 0)
                    {
                        accounts.AddRange(service
                                .GetAll()
                                .Where(x =>
                                    x.PersAccNumExternalSystems != string.Empty
                                    && x.PersAccNumExternalSystems != null
                                    && exNum.Contains(x.PersAccNumExternalSystems))
                                .FilterByFio(lastName, firstName, middleName)
                                .OrderByDescending(x => x.PersAccNumExternalSystems.Length))
                            ;
                    }
                    else
                    {
                        var account = accounts.FirstOrDefault();

                        return this.PersAcToDto(account);
                    }


                    //проверяем контэйнс наоборот 
                    if (accounts.Count == 0)
                    {
                        accounts.AddRange(service
                            .GetAll()
                            .Where(x => x.PersAccNumExternalSystems.Contains(exNum)));
                    }
                    else
                    {
                        var account = accounts.FirstOrDefault();

                        return this.PersAcToDto(account);
                    }
                }


                //по адресу и ФИ(О)
                if (accounts.Count == 0)
                {
                    if (string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(firstName))
                        return new PersonalAccountMobileDTO();

                    var roomNums = Regex.Matches(address, @"д\. ?\w, ?.*");
                    var roomNum = string.Empty;
                    if (roomNums.Count > 0)
                    {
                        roomNum = Regex.Replace(roomNums[0].Value, @"д\. ?\w, *кв\. *", "");
                    }

                    var houseAddr = Regex.Replace(address, @",? *кв\.?.*", "");

                    var accountList = service.GetAll()
                        .Where(x => x.Room.RealityObject.Address.ToLower()
                            .Replace(" ", "")
                            .Replace(".", "")
                            .Replace(",", "")
                            .Contains(houseAddr.ToLower()
                                .Replace(" ", "")
                                .Replace(".", "")
                                .Replace(",", "")
                            ))
                        .FilterByFio(lastName, firstName, middleName)
                        .OrderByDescending(x => x.Room.RealityObject.Address.Length).ToList();
                    accounts.AddRange(accountList.Where(x => x.Room.RoomNum.Trim() == roomNum.Trim()).ToList());


                    var account = accounts.FirstOrDefault();
                    //добавляем запись в лог
                    if (account == null || account.Id == 0)
                    {
                        try
                        {
                            AddLogRecord(exNum, address, lastName, firstName, middleName);
                        }
                        catch
                        {
                            
                        }
                    }

                    return this.PersAcToDto(account);
                }
            }
            finally
            {
                this.Container.Release(service);
            }

            return new PersonalAccountMobileDTO();
        }
         
           private void AddLogRecord(string exNum, string address, string lastName, string firstName, string middleName)
        {
            if (exNum.Contains("00110"))
            {
                exNum = exNum.Replace("00110","");
            }
            var service = this.Container.Resolve<IDomainService<BasePersonalAccount>>();
            var comparsionService = this.Container.Resolve<IDomainService<MobileAppAccountComparsion>>();
            try
            {
                var systemAcc = service.GetAll()
                    .Where(x => x.PersonalAccountNum == exNum || exNum == x.PersAccNumExternalSystems)
                    .Select(x=> new BasePersonalAccount
                    {
                        PersAccNumExternalSystems = x.PersAccNumExternalSystems,
                        PersonalAccountNum = x.PersonalAccountNum,
                        AccountOwner = x.AccountOwner
                    })
                    .FirstOrDefault();
                if (systemAcc != null)
                {
                    comparsionService.Save(new MobileAppAccountComparsion
                    {
                        DecisionType = Enums.MobileAccountComparsionDecision.NotSet,
                        ExternalAccountNumber = systemAcc.PersAccNumExternalSystems,
                        IsViewed = false,
                        IsWorkOut = false,
                        MobileAccountNumber = exNum,
                        ObjectCreateDate = DateTime.Now,
                        MobileAccountOwnerFIO = $"{lastName} {firstName} {middleName}",
                        ObjectEditDate = DateTime.Now,
                        ObjectVersion = 1,
                        OperatinDate = DateTime.Now,
                        PersonalAccountNumber = systemAcc.PersonalAccountNum,
                        PersonalAccountOwnerFIO = systemAcc.AccountOwner.Name
                    });

                }
            }
            catch (Exception e)
            {
                
            }
            finally
            {
                this.Container.Release(service);
                this.Container.Release(comparsionService);
            }
        }

        
        
        
         /// <summary>
        /// Получение списка должников по дому
        /// </summary>    
        /// <returns>Номер и адрес л/с</returns>
        public GetListDebtorsResponse GetListDebtorsResponse(string houseId)
        {
            var mkdId = houseId.ToLong();
            if (mkdId == 0)
            {
                var result = new Result { Code = "404", Message = "Не указан ИД дома" };
                return new GetListDebtorsResponse { Debtors = new DebtorProxy[0], Result = result };
            }
            else
            {
                var service = this.Container.Resolve<IDomainService<BasePersonalAccount>>();
                var periodDomain = this.Container.Resolve<IDomainService<ChargePeriod>>();
                var summaryDomain = this.Container.Resolve<IDomainService<PersonalAccountPeriodSummary>>();
                try
                {
                    var currentPeriod = periodDomain
                        .GetAll().FirstOrDefault(x => x.IsClosed == false);
                    var persAccList = summaryDomain.GetAll()
                        .Where(x => x.Period == currentPeriod)
                        .Where(x => x.PersonalAccount.Room.RealityObject.Id == mkdId && x.PersonalAccount.Room.RealityObject.ExportedToPortal)

                        //    .Where(x => x.SaldoOut > 1000)
                        .Select(x => new
                        {
                            x.PersonalAccount.Id,
                            x.PersonalAccount.PersonalAccountNum,
                            Address = x.PersonalAccount.Room.RealityObject.Address + ", кв." + x.PersonalAccount.Room.RoomNum
                        }).Distinct().ToList();
                    List<DebtorProxy> debtors = new List<DebtorProxy>();
                    foreach (var row in persAccList)
                    {
                        DebtorProxy dp = new DebtorProxy
                        {
                            Id = row.Id,
                            Address = row.Address,
                            NumLs = row.PersonalAccountNum
                        };
                        debtors.Add(dp);
                    }

                    var result = new Result { Code = "0", Message = "" };
                    return new GetListDebtorsResponse { Debtors = debtors.ToArray(), Result = result };
                }
                finally
                {
                    this.Container.Release(service);
                    this.Container.Release(periodDomain);
                    this.Container.Release(summaryDomain);
                }
            }

            return null;
        }
         
        /// <summary>
        /// Получение баланса лиц. счета
        /// </summary>
        /// <param name="numLs">Номер л/с</param>
        /// <param name="month">Месяц</param>
        /// <param name="year">Год</param>
        /// <returns>Информация по л/c за период</returns>
        public AccountPeriodSummary GetLastPeriodAccountBalance(string numLs)
        {
            var periodDomain = this.Container.Resolve<IDomainService<ChargePeriod>>();
            var summaryDomain = this.Container.Resolve<IDomainService<PersonalAccountPeriodSummary>>();
            var accountDomain = this.Container.Resolve<IDomainService<BasePersonalAccount>>();

            using (this.Container.Using(periodDomain, summaryDomain))
            {
                var account = accountDomain.GetAll().FirstOrDefault(x => x.PersonalAccountNum == numLs);
                if (account == null)
                {
                    return new AccountPeriodSummary
                    {
                        ResponseCode = 1,
                        Message = "Счета с указанным номером не существует."
                    };
                }


                // указываем середину месяца, чтобы получить нужный период
                var period = periodDomain.GetAll().Where(x => x.IsClosed == false).FirstOrDefault();
                if (period == null)
                {
                    return new AccountPeriodSummary
                    {
                        ResponseCode = 5,
                        Message = "Нет подходящих периодов."
                    };
                }

                var summary =
                    summaryDomain.GetAll().FirstOrDefault(x => x.Period.Id == period.Id && x.PersonalAccount.Id == account.Id);

                if (summary == null)
                {
                    return new AccountPeriodSummary
                    {
                        ResponseCode = 6,
                        Message = "Информация о начислении по счету отстутвует."
                    };
                }

                if (account.CloseDate != DateTime.MinValue)
                {
                    return new AccountPeriodSummary
                    {
                        ResponseCode = 2,
                        Message = "Счет не активен."
                    };
                }

                return this.Container.Resolve<IAccountPeriodSummaryService>().GetSummary(account, period, summary);
            }
        }
        
        private string GetAccessTokenMegafon(string accnum)
        {
            var token = $"{DateTime.Now.ToString("dd.MM.yyyy")}_MGF_RB443";
            var hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(accnum + token));
            var str = Convert.ToBase64String(hash);
            return Convert.ToBase64String(hash);
        }
        
        private bool ValidateTokenMegafon(string check_token, string accnum)
        {
            return this.GetAccessTokenMegafon(accnum) == check_token;
        }
        
         public PersonalAccountDebtInfoResponse GetDebtInfo(string account, string token)
        {
            if (!ValidateTokenMegafon(token, account))
            {
                return new PersonalAccountDebtInfoResponse
                {
                    PersonalAccountDebtInfo = null,
                    RequestResult = RequestResult.IncorrectToken
                };
            }

            var accountDomain = Container.ResolveDomain<BasePersonalAccount>();
            var periodDomain = Container.ResolveDomain<ChargePeriod>();
            var SummaryDomain = Container.ResolveDomain<PersonalAccountPeriodSummary>();
            var transferDomain = Container.ResolveDomain<PersonalAccountPaymentTransfer>();

            var lastPeriod = periodDomain.GetAll().FirstOrDefault(x => x.IsClosing);
            if (lastPeriod != null)
            {
                return new PersonalAccountDebtInfoResponse
                {
                    PersonalAccountDebtInfo = null,
                    RequestResult = new RequestResult
                    {
                        Code = "10",
                        Message = "Идет закрытие расчетного периода, данные недоступны"
                    }
                };
            }

            if (string.IsNullOrEmpty(account))
            {
                return new PersonalAccountDebtInfoResponse
                {
                    PersonalAccountDebtInfo = null,
                    RequestResult = RequestResult.DataNotFound
                };
            }

            try
            {
                var persAcc = accountDomain.GetAll()
                    .Where(x => x.PersonalAccountNum == account)
                    .FirstOrDefault();

                if (persAcc != null)
                {
                    var persAccSummary = SummaryDomain.GetAll()
                        .Where(x => x.PersonalAccount.Id == persAcc.Id && x.Period.IsClosed == false).FirstOrDefault();
                    if (persAccSummary == null)
                    {
                        return new PersonalAccountDebtInfoResponse
                        {
                            PersonalAccountDebtInfo = null,
                            RequestResult = RequestResult.DataNotFound
                        };
                    }

                    PersonalAccountDebtInfo di = new PersonalAccountDebtInfo
                    {
                        AccountNumber = account,
                        CloseDate = persAcc.CloseDate,
                        OpenDate = persAcc.OpenDate,
                        FeeDebtTotal = persAccSummary.BaseTariffDebt - persAccSummary.TariffPayment,
                        TotalPenaltyDebt = persAccSummary.PenaltyDebt - persAccSummary.PenaltyPayment,
                        TotalDebt = persAccSummary.BaseTariffDebt + persAccSummary.PenaltyDebt - persAccSummary.TariffPayment - persAccSummary.PenaltyPayment,
                        LastPaymentDate = null,
                        LastPaymentSum = 0
                    };

                    var persAccPayment = transferDomain.GetAll()
                       .Where(x => x.Owner.Id == persAcc.Id && !x.Operation.IsCancelled)
                    .Where(
                        x => x.Reason == "Оплата по базовому тарифу" ||
                             x.Reason == "Оплата пени")
                   .OrderByDescending(x => x.PaymentDate).FirstOrDefault();
                    if (persAccPayment != null)
                    {
                        di.LastPaymentSum = persAccPayment.Amount;
                        di.LastPaymentDate = persAccPayment.PaymentDate;
                    }
                    return new PersonalAccountDebtInfoResponse
                    {
                        PersonalAccountDebtInfo = di,
                        RequestResult = RequestResult.NoErrors
                    };
                }
                else
                {
                    return new PersonalAccountDebtInfoResponse
                    {
                        PersonalAccountDebtInfo = null,
                        RequestResult = RequestResult.DataNotFound
                    };
                }

            }
            catch (Exception e)
            {
                return new PersonalAccountDebtInfoResponse
                {
                    PersonalAccountDebtInfo = null,
                    RequestResult = new RequestResult
                    {
                        Code = "10",
                        Message = e.Message
                    }
                };
            }
            finally
            {
                Container.Release(accountDomain);
                Container.Release(periodDomain);
                Container.Release(SummaryDomain);
                Container.Release(transferDomain);
            }
        }
        
        
        /// <summary>
        /// Получение баланса лиц. счета по всем периодам
        /// </summary>
        /// <param name="lsId">Айди л/с</param>
        /// <returns>Информация по л/c за периоды</returns>
        public AccountPeriodSummary[] GetAllPeriodsAccountBalance(long lsId)
        {
            var periodDomain = this.Container.Resolve<IDomainService<ChargePeriod>>();
            var summaryDomain = this.Container.Resolve<IDomainService<PersonalAccountPeriodSummary>>();


            using (this.Container.Using(periodDomain, summaryDomain))
            {
                var summaries = summaryDomain.GetAll().Where(x => x.PersonalAccount.Id == lsId);
                var result = new List<AccountPeriodSummary>();
                summaries.ForEach(x =>
                {
                    result.Add(new AccountPeriodSummary
                    {
                        Id = x.Id,
                        PeriodId = x.Period.Id,
                        BeginDebt = x.SaldoIn,
                        EndDebt = x.SaldoOut,
                        ChargedSum = x.ChargeTariff,
                        ChargedBase = x.BaseTariffChange,
                        ChargedSolution = x.DecisionTariffChange,
                        PenaltyCharged = x.Penalty,
                        PaidPenalty = x.PenaltyPayment,
                        PaidSum = x.TariffPayment,
                        PaidSoiution = x.TariffDecisionPayment
                    });
                });

                return result.ToArray();
            }
        }
        
        /// <summary>
        /// Проверка валидности лиц. счета
        /// </summary>
        /// <param name="numLs">Номер л/с</param>
        /// <param name="lastName">Фамилия</param>
        /// <param name="flatNum">Номер помещения</param>
        /// <param name="ownerType">Тип абонента</param>
        /// <returns>Валидность л/с</returns>
        public bool IsAccountValid(string numLs, string lastName, string flatNum, string ownerType = "0")
        {
            var accountDomain = this.Container.Resolve<IDomainService<BasePersonalAccount>>();
            var individualOwnerDomain = this.Container.Resolve<IDomainService<IndividualAccountOwner>>();
            var legalOwnerDomain = this.Container.Resolve<IDomainService<LegalAccountOwner>>();

            using (this.Container.Using(accountDomain, individualOwnerDomain, legalOwnerDomain))
            {                
                var accountList = accountDomain
                    .GetAll()
                    .Where(x => x.PersonalAccountNum == numLs || x.PersAccNumExternalSystems == numLs)
                    .ToList();

                if (ownerType == "1")
                {
                    var ownerDict = legalOwnerDomain.GetAll()
                        .WhereContains(x => x.Id, accountList.Select(y => y.AccountOwner.Id))
                        .ToDictionary(x => x.Id);

                    foreach (var account in accountList)
                    {
                        var owner = ownerDict.Get(account.AccountOwner.Id);
                        if (owner == null || account.Room.RoomNum != flatNum)
                        {
                            continue;
                        }
                        return true;
                    }
                }
                else
                {
                    var ownerDict = individualOwnerDomain.GetAll()
                        .WhereContains(x => x.Id, accountList.Select(y => y.AccountOwner.Id))
                        .ToDictionary(x => x.Id);

                    foreach (var account in accountList)
                    {
                        var owner = ownerDict.Get(account.AccountOwner.Id);
                        if (owner == null || 
                            !(account.Room.RoomNum == flatNum && (string.IsNullOrEmpty(lastName) || string.Equals(owner.Surname, lastName, StringComparison.CurrentCultureIgnoreCase))))
                        {
                            continue;
                        }
                        return true;                       
                    }
                }              
                return false;
            }
        }

        /// <summary>
        /// Получение баланса лиц. счета
        /// </summary>
        /// <param name="numLs">Номер л/с</param>
        /// <param name="month">Месяц</param>
        /// <param name="year">Год</param>
        /// <returns>Информация по л/c за период</returns>
        public AccountPeriodSummary GetAccountBalance(string numLs, string month, string year)
        {
            var intMonth = month.ToInt();
            var intYear = year.ToInt();

            if (intMonth < 1 || intMonth > 12 || intYear < 1)
            {
                return new AccountPeriodSummary
                {
                    ResponseCode = 3,
                    Message = "Неверно указан период."
                };
            }

            var periodDomain = this.Container.Resolve<IDomainService<ChargePeriod>>();
            var summaryDomain = this.Container.Resolve<IDomainService<PersonalAccountPeriodSummary>>();
            var accountDomain = this.Container.Resolve<IDomainService<BasePersonalAccount>>();

            using (this.Container.Using(periodDomain, summaryDomain))
            {
                var account = accountDomain.GetAll().FirstOrDefault(x => x.PersonalAccountNum == numLs);
                if (account == null)
                {
                    return new AccountPeriodSummary
                    {
                        ResponseCode = 1,
                        Message = "Счета с указанным номером не существует."
                    };
                }
                var date = new DateTime(intYear, intMonth, 15); // так как начало и конец периода совпадает с началом и концом месяца, 

                // указываем середину месяца, чтобы получить нужный период
                var period = periodDomain.GetAll().Where(x => x.StartDate < date).OrderByDescending(x => x.StartDate).FirstOrDefault();
                if (period == null)
                {
                    return new AccountPeriodSummary
                    {
                        ResponseCode = 5,
                        Message = "Нет подходящих периодов."
                    };
                }

                var summary =
                    summaryDomain.GetAll().FirstOrDefault(x => x.Period.Id == period.Id && x.PersonalAccount.Id == account.Id);

                if (summary == null)
                {
                    return new AccountPeriodSummary
                    {
                        ResponseCode = 6,
                        Message = "Информация о начислении по счету отстутвует."
                    };
                }

                if (account.CloseDate != DateTime.MinValue)
                {
                    return new AccountPeriodSummary
                    {
                        ResponseCode = 2,
                        Message = "Счет не активен."
                    };
                }

                return this.Container.Resolve<IAccountPeriodSummaryService>().GetSummary(account, period, summary);
            }
        }

        /// <summary>
        /// Получение номер л/с
        /// </summary>
        /// <param name="exNum">Номер Л/С во внешних системах</param>
        /// <param name="mId">Муниципальное образование</param>
        /// <returns>Номер л/с</returns>
        public string GetPersonalAccountNum(string exNum, string mId)
        {
            var service = this.Container.Resolve<IDomainService<BasePersonalAccount>>();
            string accountNumber;
            try
            {
                accountNumber =
                    service.GetAll()
                        .Where(x => x.PersAccNumExternalSystems.Equals(exNum) && x.Room.RealityObject.Municipality.Id == mId.ToLong())
                        .Select(x => x.PersonalAccountNum)
                        .FirstOrDefault();
            }
            finally
            {
                this.Container.Release(service);
            }

            return accountNumber;
        }

        /// <summary>
        /// Получение баланса лиц. счета
        /// </summary>
        /// <param name="numLs">Номер л/с</param>
        /// <returns>Информация по л/c за период</returns>
        public GetReportLsResponse GetReportLsResponse(string numLs)
        {
            var accountDomain = this.Container.Resolve<IDomainService<BasePersonalAccount>>();
            var account = accountDomain.GetAll().Where(x => x.State.StartState).FirstOrDefault(x => x.PersonalAccountNum == numLs);
            string reportId = "PersonalAccountReport";
            var codedReport = this.Container.Resolve<IPersonalAccountCodedReport>(reportId);
            try
            {
                if (account == null)
                {
                    return new GetReportLsResponse { Base64File = "", Result = new Result { Code = "404", Message = "Не найден лицевой счет" } };
                }
                codedReport.AccountId = account.Id;
                codedReport.GenerateFromService();
                Stream st = codedReport.ReportFileStream;
                st.Seek(0, SeekOrigin.Begin);
                var bytes = st.ToBytes();

                return new GetReportLsResponse { Base64File = Convert.ToBase64String(bytes), FileName = codedReport.OutputFileName, Result = new Result { Code = "0" } };
            }
            finally
            {
                Container.Release(accountDomain);
                Container.Release(codedReport);
            }
        }
    }
}