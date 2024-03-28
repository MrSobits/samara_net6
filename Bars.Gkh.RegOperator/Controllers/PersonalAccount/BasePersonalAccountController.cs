namespace Bars.Gkh.RegOperator.Controllers
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Modules.DataExport;
    using B4.Modules.FileStorage;
    using B4.Modules.Reports;
    using B4.Utils;

    using Bars.Gkh.Domain.TableLocker;
    using Bars.Gkh.Modules.RegOperator.DomainService;

    using Domain.Extensions;
    using Domain.PersonalAccountOperations;
    using Domain.PersonalAccountOperations.Impl;

    using DomainModelServices;

    using DomainService;
    using DomainService.PersonalAccount;

    using Entities;

    using Export.Reports;

    using Gkh.Domain;

    using Ionic.Zip;
    using Ionic.Zlib;

    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Entities.Views;
    using Bars.Gkh.Utils;

    using Castle.MicroKernel;

    using ViewModels;
    using ViewModels.PersonalAccount;
    using System.Collections.Generic;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Authentification;

    /// <summary>
    /// Контроллер Лицевых счетов
    /// </summary>
    internal partial class BasePersonalAccountController : FileStorageDataController<BasePersonalAccount>
    {
        #region Public Properties

        /// <summary>
        /// Сервис выгрузки документов на оплату
        /// </summary>
        public IPaymentDocumentService PaymentDocService { get; set; }

        /// <summary>
        /// Домен-сервис лицевых счетов
        /// </summary>
        public IDomainService<BasePersonalAccount> PersAccDomain { get; set; }

        /// <summary>
        /// Сервис лицевых счетов
        /// </summary>
        public IPersonalAccountService PersonalAccountService { get; set; }

        /// <summary>
        /// Сервис изменения лицевого счета
        /// </summary>
        public IPersonalAccountOperationService PersonalAccountOperationService { get; set; }

        /// <summary>
        /// Сервис лицевых счетов
        /// </summary>
        public IPersonalAccountReportService ReportService { get; set; }

        /// <summary>
        /// Сервис изменения параметров, влияющих на расчет лицевых счетов
        /// </summary>
        public IPersonalAccountChangeService ChangeService { get; set; }

        /// <summary>
        /// Репозиторий для <see cref="Room"/>
        /// </summary>
        public IRepository<Room> RoomRepo { get; set; }

        /// <summary>
        /// Домен-сервис для <see cref="EntityLogLight"/>
        /// </summary>
        public IDomainService<EntityLogLight> EntityLogLightDomain { get; set; }

        /// <summary>
        /// Менеджер пользователей
        /// </summary>
        public IGkhUserManager UserManager { get; set; }

        #endregion

        #region Public Methods and Operators

        #region IPersonalAccountOperation

        /// <summary>
        /// Выполнить операцию
        /// </summary>
        /// <param name="params">Параметры клиента</param>
        /// <returns>Результат запроса</returns>
        public ActionResult ExecuteOperation(BaseParams @params)
        {
            var code = @params.Params.GetAs<string>("operationCode", ignoreCase: true);
           
            var splitDate = @params.Params.GetAs<DateTime>("SplitDate", ignoreCase: true);
            if (code == "PersonalAccountSplitOperation")
            {
                var typeOwnershipNewLs = @params.Params.GetAs<List<BasePersonalAccount>>("records", ignoreCase: true);
                var typeOwnership = @params.Params.GetAs<RoomOwnershipType>("OwnershipType", ignoreCase: true);
                var valuetypeOwnershipNewLs = typeOwnershipNewLs[1].OwnershipTypeNewLs;
                var RoomId = @params.Params.GetAs<long>("RoomId");
                var ownTypeNew = @params.Params.GetAs<RoomOwnershipType>("ownershipTypeNew", ignoreCase: true);
                // если типы собственности разделяемых ЛС не равны ставим помещению смешанный тип
                if (typeOwnershipNewLs[0].Room != null && typeOwnership != valuetypeOwnershipNewLs)
                {
                    var roomId = typeOwnershipNewLs[0].Room.Id;
                    var room = RoomRepo.Get(roomId);
                    room.OwnershipType = RoomOwnershipType.Mixed;
                    RoomRepo.Update(room);

                    this.EntityLogLightDomain.Save(new EntityLogLight
                    {
                        EntityId = room.Id,
                        ClassName = "Room",
                        PropertyName = "OwnershipType",
                        PropertyValue = room.OwnershipType.ToString(),
                        DateApplied = DateTime.Now,
                        DateActualChange = splitDate,
                        ParameterName = "room_ownership_type",
                        User = this.UserManager.GetActiveUser().Return(x => x.Login)
                    });
                }
                else if (RoomId > 0 && ownTypeNew != 0)
                {
                    var room = RoomRepo.Get(RoomId);
                    room.OwnershipType = RoomOwnershipType.Mixed;
                    RoomRepo.Update(room);
                    this.EntityLogLightDomain.Save(new EntityLogLight
                    {
                        EntityId = room.Id,
                        ClassName = "Room",
                        PropertyName = "OwnershipType",
                        PropertyValue = room.OwnershipType.ToString(),
                        DateApplied = DateTime.Now,
                        DateActualChange = splitDate,
                        ParameterName = "room_ownership_type",
                        User = this.UserManager.GetActiveUser().Return(x => x.Login)
                    });
                }
            }

            ActionResult result;

            if (this.Container.Kernel.HasComponent(code))
            {
                if (code == typeof (PenaltyCancelOperation).Name.ToLower())
                {
                    var tableLocker = this.Container.Resolve<ITableLocker>();
                    try
                    {
                        if (tableLocker.CheckLocked<BasePersonalAccount>("INSERT"))
                        {
                            return this.JsFailure(TableLockedException.StandardMessage);
                        }
                    }
                    finally
                    {
                        this.Container.Release(tableLocker);
                    }
                }

                var operation = this.Container.Resolve<IPersonalAccountOperation>(code);
                var operResult = operation.Execute(@params);

                result = new JsonNetResult(operResult);
            }
            else
            {
                result = this.JsFailure("Не найден обработчик операции с кодом '{0}'".FormatUsing(code));
            }

            return result;
        }

        public ActionResult GetOperationDataForUI(BaseParams @params)
        {
            var code = @params.Params.GetAs<string>("operationCode", ignoreCase: true);

            ActionResult result;

            if (this.Container.Kernel.HasComponent(code))
            {
                var operation = this.Container.Resolve<IPersonalAccountOperation>(code);
                var operResult = operation.GetDataForUI(@params);

                result = new JsonNetResult(operResult);
            }
            else
            {
                result = this.JsFailure("Не найден обработчик операции с кодом '{0}'".FormatUsing(code));
            }

            return result;
        }

        #endregion

        /// <summary>
        /// Получение всех приходов за период
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        public ActionResult GetAccountChargeInfoInPeriod(BaseParams baseParams)
        {
            var result = this.PersonalAccountService.GetAccountChargeInfoInPeriod(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Получение информации по лицевым счетам для зачета средств за ранее выполненные работы
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult GetAccountsInfoForPerformedWorkDistribution(BaseParams baseParams)
		{
			var result = this.PersonalAccountOperationService.GetAccountsInfoForPerformedWorkDistribution(baseParams);
			return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
		}

		/// <summary>
		/// Отмена начислений
		/// </summary>
		/// <param name="baseParams">параметры клиента</param>
		/// <returns></returns>
		public ActionResult CancelCharges(BaseParams baseParams)
        {
            var cancelChargeOperation = this.Container.Resolve<IPersonalAccountOperation>("CancelChargeOperation");

            try
            {
                var result = cancelChargeOperation.Execute(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
            finally
            {
	            this.Container.Release(cancelChargeOperation);
            }
        }

        /// <summary>
        /// Создание лицевого счета
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        public ActionResult CreateAccounts(BaseParams baseParams)
        {
            return this.Resolve<IPersonalAccountCreateService>().CreateNewAccount(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Изменение статуса
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult ClosePersonalAccounts(BaseParams baseParams)
        {
            var result = this.PersonalAccountOperationService.ClosePersonalAccounts(baseParams);
            return result.Success ? this.JsSuccess() : new JsonNetResult(new { success = false, message = result.Message });
        }

        /// <summary>
        /// Повторное открытие лицевого счета
        /// </summary>
        /// <param name="baseParams">Параметры клиента</param>
        /// <returns>Результат повторного открытия счета</returns>
        public ActionResult ManuallyRecalc(BaseParams baseParams)
        {
            var manuallyRecalcOperation = this.Container.Resolve<IPersonalAccountOperation>(ManuallyRecalcOperation.Key);

            try
            {
                var result = manuallyRecalcOperation.Execute(baseParams);
                return new JsonNetResult(new { success = result.Success, message = result.Message });
            }
            finally
            {
	            this.Container.Release(manuallyRecalcOperation);
            }
        }

        /// <summary>
        /// Экспорт реестра лицевых счетов
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        public ActionResult Export(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            int size = 20000;
            var zipStream = new MemoryStream();

            var fileZip = new ZipFile(Encoding.GetEncoding("windows-1251"))
            {
                CompressionLevel = CompressionLevel.Level3,
                ProvisionalAlternateEncoding = Encoding.GetEncoding("windows-1251")
            };

            baseParams.Params["limit"] = size;
            var totalCount = 0;
            try
            {
                totalCount = this.PersAccDomain.GetAll().ToDto()
                                .Filter(loadParams, this.Container).Count();
            }
            catch(Exception e)
            {

            }
           

            // Вызывает метод выше несколько раз и грузит поток из него в excel фаил который потом попадает в поток zip
            for (int start = 0; start < totalCount; start += size)
            {
                baseParams.Params["start"] = start;
                Stream excel = this.DataList(baseParams);
                if (excel == null) break;
                fileZip.AddEntry(string.Format("export-{0}.xlsx", start), excel);
            }
            
            // сохраняем фаил потока в специальный тип zip архива 
            fileZip.Save(zipStream);
            var result = new ReportStreamResult(zipStream, "export.zip");
            return result;
        }

        /// <summary>
        /// Выгрузка информации по ЛС (отчет)
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        public ActionResult ExportPersonalAccounts(BaseParams baseParams)
        {
            if (!this.Container.Kernel.HasComponent(typeof(IPersonalAccountInfoExportService)))
            {
                return JsonNetResult.Failure("Для данного региона не найдена реализация \"Экспорт информации по лицевым счетам\"");
            }

            var service = this.Container.Resolve<IPersonalAccountInfoExportService>();
            try
            {
                var result = service.Export(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
            catch (Exception e)
            {
                return this.JsFailure(e.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Выгрузка в ЭБИР
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        public ActionResult ExportToEbir(BaseParams baseParams)
        {
            try
            {
                var type = baseParams.Params.GetAs("type", string.Empty);
                var periodId = baseParams.Params.GetAs<long>("periodId");

                var result = this.Resolve<IExportToEbirService>().Export(type, periodId);

                return this.JsSuccess(new {Id = result});
        }
            catch (Exception e)
            {
                return this.JsFailure(e.Message);
            }
        }

        /// <summary>
        /// Получение списка счетов по дому
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        public ActionResult GetAccountByRealtyObject(BaseParams baseParams)
        {
            var viewModel = this.Container.Resolve<IPersonalAccountCustomViewModel>();

            using (this.Container.Using(viewModel))
            {
                return viewModel.ListAccountByRealityObject(baseParams).ToJsonResult();
        }
        }

        /// <summary>
        /// Пункт меню документы на оплату из реестра лицевых счетов
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        public ActionResult GetPaymentDocuments(BaseParams baseParams)
        {
            return new JsonNetResult(this.PaymentDocService.GetPaymentDocuments(baseParams));
        }

        /// <summary>
        /// Проверить, имеетлся ли базовый слепок
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        public ActionResult CheckIsBaseSnapshots(BaseParams baseParams)
        {
            return new JsonNetResult(this.PaymentDocService.CheckIsBaseSnapshots(baseParams));
        }

        /// <summary>
        /// Выгрузка документа квитанции из карточки лицевого счета
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        public ActionResult ExportPaymentDocuments(BaseParams baseParams)
        {
            var persAccRepo = this.Container.ResolveRepository<BasePersonalAccount>();
            var chargePeriodDomain = this.Container.ResolveDomain<ChargePeriod>();
            var fileManager = this.Container.Resolve<IFileManager>();

            try
            {
                var accId = baseParams.Params.GetAs<long>("accountId");
                var periodId = baseParams.Params.GetAs<long>("periodId");
                var account = persAccRepo.GetAll().FirstOrDefault(x => x.Id == accId); //есть какая-то проблема с Get, возращает null; так работает
                var period = chargePeriodDomain.GetAll().FirstOrDefault(x => x.Id == periodId);

                // сохраняем слепки данных при печати квитанций (3 параметр true)
                var stream = this.PaymentDocService.GetPaymentDocument(account, period, false, true);
                var file = fileManager.SaveFile(stream, string.Format("{0}.pdf", account.PersonalAccountNum));

                return new JsonNetResult(new BaseDataResult(file.Id));
            }
            finally
            {
	            this.Container.Release(persAccRepo);
	            this.Container.Release(chargePeriodDomain);
	            this.Container.Release(fileManager);
            }
        }

        /// <summary>
        /// Получение номера лицевого счета по идентификатору
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        public ActionResult GetPersonalNumByAccount(BaseParams baseParams)
        {
            var result = (ListDataResult)this.PersonalAccountService.GetPersonalNumByAccount(baseParams);
            return new JsonListResult((IEnumerable)result.Data, result.TotalCount);
        }

        /// <summary>
        /// Получение тарифа по дому
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        public ActionResult GetTarifForRealtyObject(BaseParams baseParams)
        {
            var result = this.PersonalAccountService.GetTarifForRealtyObject(baseParams);
            return result.Success ? this.JsSuccess(result.Data) : this.JsFailure(result.Message, result);
        }

        /// <summary>
        /// Список операций
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        public ActionResult ListOperations(BaseParams baseParams)
        {
            return this.PersonalAccountService.ListOperations(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Получить информацию по оплатам за все периоды
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        public ActionResult ListPaymentsInfo(BaseParams baseParams)
        {
            return this.PersonalAccountService.ListPaymentsInfo(baseParams).ToJsonResult();
        }
        
        /// <summary>
        /// Отчет по лс
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        public ActionResult PersonalAccountReport(BaseParams baseParams)
        {
            return new JsonNetResult(this.ReportService.GetReport(baseParams));
        }

        /// <summary>
        /// Получение доли собственности по лицевомц счету
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        public ActionResult RoomAccounts(BaseParams baseParams)
        {
            return new JsonNetResult(this.PersonalAccountOperationService.RoomAccounts(baseParams));
        }

        /// <summary>
        /// Получение списка лицевых счетов по дому
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        public ActionResult PersonalAccountsByRo(BaseParams baseParams)
        {
            return new JsonNetResult(this.PersonalAccountService.PersonalAccountsByRo(baseParams));
        }

        /// <summary>
        /// Изменение баланса за период
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        public ActionResult SetBalance(BaseParams baseParams)
        {
            var result = this.ChangeService.ChangePeriodBalance(baseParams);
            return result.Success ? this.JsSuccess(result) : this.JsFailure(result.Message, result);
        }

        /// <summary>
        /// Изменение собственника лицевого счета
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        public ActionResult SetNewOwner(BaseParams baseParams)
        {
            
            var result = this.ChangeService.ChangeOwner(baseParams);
            return result.Success ? this.JsSuccess(result) : this.JsFailure(result.Message, result);
        }

        /// <summary>
        /// Ручное изменение пеней
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        public ActionResult SetPenalty(BaseParams baseParams)
        {
            var result = this.ChangeService.ChangePenalty(baseParams);
            return result.Success ? this.JsSuccess(result) : this.JsFailure(result.Message, result);
        }

        /// <summary>
        /// Изменение значения доли собственности
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        public ActionResult UpdateAreaShare(BaseParams baseParams)
        {
            var result = this.ChangeService.ChangeAreaShare(baseParams);
            return result.Success ? this.JsSuccess(result) : this.JsFailure(result.Message, result);
        }

        public ActionResult ExecuteAccountOperation(BaseParams baseParams)
        {
            var result = this.Resolve<IAccountOperationProvider>().Execute(baseParams);
            return result.Success ? this.JsSuccess(result.Data) : this.JsFailure(result.Message, result);
        }

        public ActionResult ListAccountOperations()
        {
            var authService = this.Container.ResolveAll<IAuthorizationService>().FirstOrDefault();
            var userIdentity = this.Resolve<IUserIdentity>();

            var result = this.Resolve<IAccountOperationProvider>().GetAllOperations()
                .Where(x => string.IsNullOrEmpty(x.PermissionKey) || authService.Grant(userIdentity, x.PermissionKey))
                .ToList();

            return new JsonListResult(result, result.Count);
        }

        public ActionResult ExportVtscp(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IExportVtscpService>();

            try
            {
                var result = service.Export(baseParams);
                return new JsonNetResult(result);
            }
            finally
            {
	            this.Container.Release(service);
            }
        }

        /// <summary>
        /// Выгрузка пеней
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        public ActionResult ExportPenalty(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IExportPenaltyService>();

            try
            {
                var result = service.Export(baseParams);
                return new JsonNetResult(result);
            }
            finally
            {
	            this.Container.Release(service);
            }
        }

        /// <summary>
        /// Выгрузка пеней в ексель
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        public ActionResult ExportPenaltyExcel(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IExportPenaltyService>();

            try
            {
                var result = service.ExportPenaltyExcel(baseParams);
                return new JsonNetResult(result);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Логи операций
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult GetOperationLog(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IPersonalAccountOperationLogService>();
            try
            {
                var result = service.GetOperationLog(baseParams);
                return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
	            this.Container.Release(service);
            }
        }

        /// <summary>
        /// Получение списка лицевых счетов для распределения
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        public ActionResult ListAccountsForDistribution(BaseParams baseParams)
        {
            var viewModel = this.Resolve<IPersonalAccountDistributionViewModel>();

            using (this.Container.Using(viewModel))
            {
                return viewModel.List(baseParams).ToJsonResult();
            }
        }

        /// <summary>
        /// Обновление кэша ЛС
        /// </summary>
        /// <param name="baseParams">Параметры клиента</param>
        /// <returns>Результат запроса</returns>
        public ActionResult UpdateCache(BaseParams baseParams)
        {
            var service = this.Resolve<IMassPersonalAccountDtoService>();

            using (this.Container.Using(service))
            {
                return service.MassCreatePersonalAccountDto(true).ToJsonResult();
            }         
        }

        /// <summary>
        /// Вернуть детализацию распределений
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public ActionResult ListDistributedPerformedWork(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IPerformedWorkDetailService>();

            using (this.Container.Using(service))
            {
                return service.ListDistributed(baseParams).ToJsonResult();
            }
        }

        /// <summary>
        /// Вернуть детализацию распределений
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public ActionResult ListAccountsByDate(BaseParams baseParams)
        {
            var repository = this.Container.Resolve<IViewAccOwnershipHistoryRepository>();

            using (this.Container.Using(repository))
            {
                var periodId = baseParams.Params.GetAsId("periodId");

                return repository.GetAllDto(periodId)
                    .ToListDataResult(baseParams.GetLoadParam())
                    .ToJsonResult();
            }
        }
        #endregion

        #region Private Methods
        // Метод формирует поток со строками для экспорта ЛС 
        private Stream DataList(BaseParams baseParams)
        {
            //возвращаем строку вместо enum для поля OwnerType
            if (!baseParams.Params.ContainsKey("returnOwnerTypeAsString"))
            {
                baseParams.Params.Add("returnOwnerTypeAsString", true);
            }

            //для нормальной печати номер лицевого счета ато пропадает операжающий ноль
            if (!baseParams.Params.ContainsKey("addAnApostrophe"))
            {
                baseParams.Params.Add("addAnApostrophe", true);
            }

            var Data = this.ViewModel.List(this.PersAccDomain, baseParams).Data;

            var generator = this.Container.Resolve<B4.Modules.Reports.IReportGenerator>("XlsIoGenerator");

            var report = this.Container.Resolve<IDataExportReport>("DataExportReport.PrintForm",
                new Arguments
                {
                    {"Data", Data},
                    {"BaseParams", baseParams}
                });

            var rp = new ReportParams();

            report.PrepareReport(rp);
            var template = report.GetTemplate();

            var result = new MemoryStream();

            generator.Open(template);
            generator.Generate(result, rp);
            result.Seek(0, SeekOrigin.Begin);
            return result;
        }

        /// <summary>
        /// Список Юр лиц
        /// </summary>
        /// <param name="baseParams">параметры </param>
        /// <returns></returns>
        public ActionResult ListJurialContragents(BaseParams baseParams)
        {
  
            return this.PersonalAccountService.ListJurialContragents(baseParams).ToJsonResult();
        }

        #endregion
    }
}