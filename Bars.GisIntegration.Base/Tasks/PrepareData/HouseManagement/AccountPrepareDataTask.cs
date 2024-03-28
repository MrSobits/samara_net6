namespace Bars.GisIntegration.Base.Tasks.PrepareData.HouseManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.HouseManagementAsync;
    using Bars.GisIntegration.Base.Tasks.PrepareData;
    using Bars.Gkh.Quartz.Scheduler.Log;
    using Castle.Core.Internal;

    /// <summary>
    /// Задача подготовки данных по ЛС
    /// </summary>
    public class AccountPrepareDataTask : BasePrepareDataTask<importAccountRequest>
    {
        private List<RisAccount> accounts;

        /// <summary>
        /// Размер блока предаваемых данных (максимальное количество записей)
        /// </summary>
        private const int Portion = 100;

        /// <summary>
        /// Собрать данные
        /// </summary>
        /// <param name="parameters">Параметры экспорта</param>
        protected override void ExtractData(DynamicDictionary parameters)
        {
            var extractor = this.Container.Resolve<BaseSlimDataExtractor<RisAccount>>("AccountDataExtractor");

            try
            {
                this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Начато извлечение данных по ЛС"));
                this.accounts = this.RunExtractor(extractor, parameters);
                this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Завершено извлечение данных по ЛС"));
            }
            finally
            {
                this.Container.Release(extractor);
            }
        }

        /// <summary>
        /// Валидация данных
        /// </summary>
        /// <returns>Результат валидации</returns>
        protected override List<ValidateObjectResult> ValidateData()
        {
            return this.accounts
                .Select(this.CheckItem)
                .Where(checkResult => checkResult.State != ObjectValidateState.Success)
                .ToList();
        }

        private ValidateObjectResult CheckItem(RisAccount account)
        {
            var messages = new StringBuilder();
            var guidRegex = new Regex("([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}");

            if (account.RisAccountType == default(RisAccountType))
            {
                messages.Append("AccountType ");
            }
            if (account.BeginDate == null)
            {
                messages.Append("CreationDate ");
            }
            if (account.LivingPersonsNumber != null && account.LivingPersonsNumber > byte.MaxValue)
            {
                messages.Append("LivingPersonsNumber ");
            }
            if (account.TotalSquare != null && (account.TotalSquare < 0 || account.TotalSquare > 100000000))
            {
                messages.Append("TotalSquare ");
            }
            if (account.ResidentialSquare != null && (account.ResidentialSquare < 0 || account.ResidentialSquare > 100000000))
            {
                messages.Append("ResidentialSquare ");
            }
            if (account.HeatedArea != null && (account.HeatedArea < 0 || account.HeatedArea > 100000000))
            {
                messages.Append("HeatedArea ");
            }
            if (account.Closed)
            {
                if (account.CloseDate == null)
                {
                    messages.Append("CloseDate ");
                }
                if (account.CloseReasonCode.IsNullOrEmpty())
                {
                    messages.Append("CloseReasonCode ");
                }
                if (account.CloseReasonGuid.IsNullOrEmpty())
                {
                    messages.Append("CloseReasonGuid ");
                }
            }
            if (account.LivingPremiseGuid.IsNullOrEmpty() && account.HouseFiasGuid.IsNullOrEmpty() && account.LivingRoomGuid.IsNullOrEmpty())
            {
                messages.Append("Accommodation ");
            }
            if (!account.LivingPremiseGuid.IsNullOrEmpty() && !guidRegex.IsMatch(account.LivingPremiseGuid))
            {
                messages.Append("PremisesGuid ");
            }
            if (!account.HouseFiasGuid.IsNullOrEmpty() && !guidRegex.IsMatch(account.HouseFiasGuid))
            {
                messages.Append("HouseFiasGuid ");
            }
            if (!account.LivingRoomGuid.IsNullOrEmpty() && !guidRegex.IsMatch(account.LivingRoomGuid))
            {
                messages.Append("LivingRoomGuid ");
            }

            if (account.OwnerInd == null && account.OwnerOrg == null)
            {
                messages.Append("PayerInfo ");
            }
            else if (account.OwnerInd != null)
            {
                if (account.OwnerInd.Surname.IsNullOrEmpty())
                {
                    messages.Append("Surname ");
                }
                if (account.OwnerInd.FirstName.IsNullOrEmpty())
                {
                    messages.Append("FirstName ");
                }

                if (!account.OwnerInd.IdTypeCode.IsNullOrEmpty())
                {
                    if (account.OwnerInd.IdTypeCode.IsNullOrEmpty())
                    {
                        messages.Append("IdTypeCode ");
                    }
                    if (account.OwnerInd.IdTypeGuid.IsNullOrEmpty())
                    {
                        messages.Append("IdTypeGuid ");
                    }
                    if (account.OwnerInd.IdNumber.IsNullOrEmpty())
                    {
                        messages.Append("Number ");
                    }
                    if (account.OwnerInd.IdIssueDate == null)
                    {
                        messages.Append("IdIssueDate ");
                    }
                }
                else if (account.OwnerInd.Snils.IsEmpty())
                {
                    messages.Append("SNILS ");
                }
            }

            if (account.AccountNumber.IsEmpty())
            {
                messages.Append("AccountNumber ");
            }

            return new ValidateObjectResult
            {
                Id = account.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "ЛС"
            };
        }

        /// <summary>
        /// Сформировать объекты запросов к асинхронному сервису ГИС
        /// </summary>
        /// <returns>Словарь Объект запроса - Словарь Транспортных идентификаторов: Тип обектов - Словарь: Транспортный идентификатор - Идентификатор объекта</returns>
        protected override Dictionary<importAccountRequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<importAccountRequest, Dictionary<Type, Dictionary<string, long>>>();

            foreach (var iterationList in this.GetPortions())
            {
                var transportGuidDictionary = new Dictionary<Type, Dictionary<string, long>>();
                var request = this.GetRequestObject(iterationList, transportGuidDictionary);

                result.Add(request, transportGuidDictionary);
            }

            return result;
        }


        /// <summary>
        /// Получает список порций объектов ГИС для формирования объектов для запроса.
        /// </summary>
        /// <returns>Список порций объектов ГИС</returns>
        private List<IEnumerable<RisAccount>> GetPortions()
        {
            var result = new List<IEnumerable<RisAccount>>();

            if (this.accounts.Count > 0)
            {
                var startIndex = 0;
                do
                {
                    result.Add(this.accounts.Skip(startIndex).Take(AccountPrepareDataTask.Portion));
                    startIndex += AccountPrepareDataTask.Portion;
                }
                while (startIndex < this.accounts.Count);
            }

            return result;
        }

        /// <summary>
        /// Получить объект запроса.
        /// </summary>
        /// <param name="listForImport">Список объектов для импорта</param>
        /// <param name="transportGuidDictionary">Список объектов для импорта</param>
        /// <returns>Объект запроса</returns>
        private importAccountRequest GetRequestObject(IEnumerable<RisAccount> listForImport, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var accountsList = new List<importAccountRequestAccount>();

            var accountTransportGuidDictionary = new Dictionary<string, long>();

            foreach (var account in listForImport)
            {
                var listItem = this.GetImportAccountRequestContract(account);
                accountsList.Add(listItem);

                accountTransportGuidDictionary.Add(listItem.TransportGUID, account.Id);
            }

            transportGuidDictionary.Add(typeof(RisAccount), accountTransportGuidDictionary);

            return new importAccountRequest { Id = Guid.NewGuid().ToString(), Account = accountsList.ToArray() };
        }

        /// <summary>
        /// Создать объект importAccountRequestAccount по RisAccount
        /// </summary>
        /// <param name="account">Объект типа RisAccount</param>
        /// <returns>Объект типа importAccountRequestAccount</returns>
        private importAccountRequestAccount GetImportAccountRequestContract(RisAccount account)
        {
            object payerInfoItem = null;

            if (account.OwnerInd != null)
            {
                // Собираем документ
                object document;
                if (!account.OwnerInd.Snils.IsNullOrEmpty())
                {
                    document = account.OwnerInd.Snils;
                }
                else
                {
                    document = new ID
                    {
                        Type = new nsiRef
                        {
                            Code = account.OwnerInd.IdTypeCode,
                            GUID = account.OwnerInd.IdTypeGuid
                        },
                        IssueDate = account.OwnerInd.IdIssueDate ?? default(DateTime),
                        Number = account.OwnerInd.IdNumber,
                        Series = account.OwnerInd.IdSeries
                    };
                }

                // Собираем плательщика
                payerInfoItem = new AccountIndType
                {
                    Surname = account.OwnerInd.Surname,
                    FirstName = account.OwnerInd.FirstName,
                    Patronymic = account.OwnerInd.Patronymic,
                    Sex = account.OwnerInd.Sex == RisGender.F ? AccountIndTypeSex.F : AccountIndTypeSex.M,
                    SexSpecified = true,
                    DateOfBirth = account.OwnerInd.DateOfBirth ?? default(DateTime),
                    DateOfBirthSpecified = account.OwnerInd.DateOfBirth.HasValue,
                    Item = document
                };
            }
            else if (account.OwnerOrg != null)
            {
                payerInfoItem = new RegOrgVersionType
                {
                    orgVersionGUID = account.OwnerOrg.OrgVersionGuid
                };
            }

            //Собираем помещение
            var premises = new List<AccountTypeAccommodation>();
            if (!account.HouseFiasGuid.IsNullOrEmpty() && account.LivingRoomGuid.IsNullOrEmpty() && account.LivingPremiseGuid.IsNullOrEmpty())
            {
                premises.Add(new AccountTypeAccommodation
                {
                    Item = account.HouseFiasGuid,
                    ItemElementName = ItemChoiceType5.FIASHouseGuid
                });
            }
            if (!account.LivingPremiseGuid.IsNullOrEmpty() && account.LivingRoomGuid.IsNullOrEmpty())
            {
                premises.Add(new AccountTypeAccommodation
                {
                    Item = account.LivingPremiseGuid,
                    ItemElementName = ItemChoiceType5.PremisesGUID
                });
            }
            if (!account.LivingRoomGuid.IsNullOrEmpty())
            {
                premises.Add(new AccountTypeAccommodation
                {
                    Item = account.LivingRoomGuid,
                    ItemElementName = ItemChoiceType5.LivingRoomGUID
                });
            }

            //Собираем объект
            var importAccountRequest = new importAccountRequestAccount
            {
                TransportGUID = Guid.NewGuid().ToString(),
                CreationDate = account.BeginDate ?? default(DateTime),
                CreationDateSpecified = account.BeginDate.HasValue,
                LivingPersonsNumber = (sbyte)(account.LivingPersonsNumber ?? default(int)),
                LivingPersonsNumberSpecified = false, //account.LivingPersonsNumber.HasValue,  в xml - не передавать!!!
                TotalSquare = account.TotalSquare?.RoundDecimal(2) ?? default(decimal),
                TotalSquareSpecified = account.TotalSquare.HasValue,
                ResidentialSquare = account.ResidentialSquare?.RoundDecimal(2) ?? default(decimal),
                ResidentialSquareSpecified = account.ResidentialSquare.HasValue,
                HeatedArea = account.HeatedArea?.RoundDecimal(2) ?? default(decimal),
                HeatedAreaSpecified = false, //account.HeatedArea.HasValue,   в xml - не передавать!!!
                PayerInfo = new AccountTypePayerInfo
                {
                    IsRenterSpecified = account.IsRenter.HasValue,
                    Item = payerInfoItem
                },
                //      Item1 = account.AccountNumber,
                Accommodation = premises.ToArray(),
                Item = true,
                ItemElementName = ItemChoiceType4.isCRAccount,
                AccountNumber = account.AccountNumber
            };

            if (account.IsRenter.HasValue && account.IsRenter == true)
            {
                importAccountRequest.PayerInfo.IsRenter = true;
            }

            if (account.Operation == RisEntityOperation.Update)
            {
                importAccountRequest.AccountGUID = account.Guid;
            }

            //if (account.Closed)   в xml - не передавать!!!
            //{
            //    importAccountRequest.Closed = new ClosedAccountAttributesType
            //    {
            //        CloseDate = account.CloseDate ?? default(DateTime),

            //        CloseReason = new nsiRef
            //        {
            //            Code = account.CloseReasonCode,
            //            GUID = account.CloseReasonGuid
            //        }
            //    };
            //}

            return importAccountRequest;
        }
    }
}
