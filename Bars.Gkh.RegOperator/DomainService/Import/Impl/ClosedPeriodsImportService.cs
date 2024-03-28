namespace Bars.Gkh.RegOperator.DomainService.Import.Impl
{
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Castle.Windsor;
    using Ionic.Zip;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Microsoft.Extensions.Logging;
    using Bars.B4.Modules.FileStorage;
    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;
    using Bars.B4.Modules.Tasks.Common.Contracts;
    using Bars.B4.Modules.Tasks.Common.Entities;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Import;
    using Bars.Gkh.RegOperator.DomainService.CashPaymentCenter;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Import;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис для работы с импортами в закрытый период
    /// </summary>
    public class ClosedPeriodsImportService : IClosedPeriodsImportService
    {
        /// <summary>Предупреждение про ЛС при импорте в закрытый период</summary>
        public IDomainService<AccountWarningInClosedPeriodsImport> AccountWarningInClosedPeriodsImportDomain { get; set; }

        /// <summary>Лицевой счёт</summary>
        public IDomainService<BasePersonalAccount> BasePersonalAccountDomain { get; set; }

        /// <summary>РКЦ</summary>
        public IDomainService<CashPaymentCenter> CashPaymentCenterDomain { get; set; }

        /// <summary>Контейнер</summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>Журнал изменения сущности (в частности, лицевого счёта)</summary>
        public IDomainService<EntityLogLight> EntityLogLightDomain { get; set; }

        /// <summary>Методы для работы с файлами</summary>
        public IFileManager FileManager { get; set; }

        /// <summary>Описание файла</summary>
        public IDomainService<FileInfo> FileInfoDomain { get; set; }

        /// <summary>Журнал импортов</summary>
        public IDomainService<LogImport> LogImportDomain { get; set; }

        /// <summary>Шапака импорта в закрытый период</summary>
        public IDomainService<HeaderOfClosedPeriodsImport> HeaderOfClosedPeriodsImportDomain { get; set; }

        /// <summary>Менеджер пользователей</summary>
        public IGkhUserManager UserManager { get; set; }

        /// <summary>Задачи сервера вычислений</summary>
        public IDomainService<TaskEntry> TaskEntryDomain { get; set; }

        /// <summary>Логи в файл</summary>
        public ILogger LogManager { get; set; }

        /// <summary>
        /// Сопоставить лицевой счёт
        /// </summary>
        /// <remarks>
        /// Сопоставление может осуществляться в двух режимах:
        /// 1) автоматическом;
        /// 2) неавтоматическом (ручном);
        /// В случает автоматического сопоставления, идентификатор ЛС уже лежит в журнале предупреждений (AccountWarningInClosedPeriodsImport).
        /// При ручном сопоставлении - ЛС явно указывается через параметр compareToAccountId.
        /// </remarks>
        /// <param name="warningId">Идентификатор сущности "Журнал предупреждений по ЛС" (AccountWarningInClosedPeriodsImport)</param>
        /// <param name="compareToAccountId">Идентификатор лицевого счёта, на который делается неавтоматическое сопоставление</param>  
        /// <param name="isAuto">Это подтверждение автоматического сопоставления?</param>
        private void ComparePersonalAccount(long warningId, long? compareToAccountId = null, bool isAuto = false)
        {
            if (!isAuto && compareToAccountId == null)
            {
                throw new ValidationException("Требуется значение параметра compareToAccountId");
            }
            if (isAuto && compareToAccountId != null)
            {
                throw new ValidationException("Значение параметра compareToAccountId не требуется");
            }

            var currentWarning = this.AccountWarningInClosedPeriodsImportDomain.Get(warningId);
            var externalNumber = currentWarning.ExternalNumber; // Внеший идентификатор ЛС
            var externalRkcId = currentWarning.ExternalRkcId; // Внешний идентификатор РКЦ
            var task = currentWarning.Task; // Задача, которая разбирала импорт на сервере вычислений
            if (isAuto)
            {
                compareToAccountId = currentWarning.ComparingAccountId; // Вытащить ID ЛС из журанала, т.к. он не пришёл параметром compareToAccountId
            }

            // Период, за который делался импорт сохранён в шапке. Достать его (периода) дату начала.
            // С этой даты будет заменён внешний номер ЛС и РКЦ.
            var dateStart = this.HeaderOfClosedPeriodsImportDomain.GetAll()
                .Where(x => x.Task == task)
                .Select(x => x.Period.StartDate)
                .FirstOrDefault();

            // 1. Обновить внешний номер ЛС
            // Обновить сущность
            var account = this.BasePersonalAccountDomain.Get(compareToAccountId);
            account.PersAccNumExternalSystems = externalNumber;
            this.BasePersonalAccountDomain.Update(account);
            // Залогировать
            this.EntityLogLightDomain.Save(new EntityLogLight
            {
                ClassName = "BasePersonalAccount",
                EntityId = account.Id,
                PropertyName = "PersAccNumExternalSystems",
                PropertyValue = externalNumber,
                DateActualChange = dateStart,
                DateApplied = DateTime.UtcNow,
                ParameterName = "account_external_num",                
                Reason = "Сопоставление ЛС при импорте в закрытый период",
                User = this.UserManager.GetActiveUser()?.Login ?? "anonymous",
                ObjectCreateDate = DateTime.UtcNow,
                ObjectEditDate = DateTime.UtcNow                
            });

            // 2. Обновить внешний номер РКЦ  
            var cashPaymentCenterId = this.CashPaymentCenterDomain.GetAll().Where(x => x.Identifier == externalRkcId).Select(x => x.Id).FirstOrDefault();
            var cachPaymentCenterConnectionType = this.Container.GetGkhConfig<RegOperatorConfig>().GeneralConfig.CachPaymentCenterConnectionType;
            var serviceName = "CachPaymentCenterConnectionType.{0}".FormatUsing(cachPaymentCenterConnectionType.ToString("G"));
            var cashPaymentCenterAddObjectsService = this.Container.Resolve<ICashPaymentCenterObjectsService>(serviceName);
            using (this.Container.Using(cashPaymentCenterAddObjectsService))
            {
                cashPaymentCenterAddObjectsService.InsertObjects(
                    cashPaymentCenterId,
                    new[] { cachPaymentCenterConnectionType == CachPaymentCenterConnectionType.ByAccount ? account.Id : account.Room.RealityObject.Id },
                    dateStart,
                    null); // По бесконечность
            }

            // 3. Обновить статус записи
            currentWarning.IsProcessed = YesNo.Yes; // Признак, что запись обработана
            if (!isAuto)
            {
                currentWarning.ComparingAccountId = compareToAccountId; // Заполнит ЛС
                currentWarning.ComparingInfo = String.Format("({0}) {1} - {2}", // Заполнить информацию по сопоставлению: номер ЛС - адрес - ФИО
                        /* 0 */ account.PersonalAccountNum,
                        /* 1 */ account.Room.RealityObject.Address + ", кв. " +
                                    account.Room.RoomNum +
                                    (account.Room.ChamberNum != "" && account.Room.ChamberNum != null ? ", ком. " + account.Room.ChamberNum : string.Empty),
                        /* 2 */ (account.AccountOwner as IndividualAccountOwner).Name ?? (account.AccountOwner as LegalAccountOwner).Contragent.Name
                    );
                currentWarning.IsCanAutoCompared = YesNo.No; // Теперь уже не может автоматически сопоставляться (если вообще могла), т.к. сопоставлялась вручную
            }
            this.AccountWarningInClosedPeriodsImportDomain.Save(currentWarning);
        }

        /// <summary>
        /// Подтвердить атоматическое сопоставление лицевого счёта
        /// </summary>
        /// <param name="baseParams">Параметры от браузера. В частности, warnings - перечень идентификаторов записей журнала предупреждений.</param>
        /// <returns>Удалось ли подтвердить</returns>
        public ActionResult ConfirmAutoComparePersonalAccount(BaseParams baseParams)
        {
            // Идентификаторы записей из журнала предупреждений            
            var warningsSeparatedList = baseParams.Params.GetAs<string>("warnings");
            var warnings = warningsSeparatedList.ToLongArray();

            this.Container.InTransaction(() =>
            { 
                for (int i = 0; i < warnings.Length; i++)
                {
                    ComparePersonalAccount(warnings[i], isAuto: true);
                }
            });

            return JsonNetResult.Success;
        }

        /// <summary>
        /// Получить список логов
        /// </summary>
        /// <param name="baseParams">Параметры от браузера</param>
        /// <returns>Набор данных, пригодный для отображения в гриде</returns>
        public IDataResult LogsList(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            string importId = baseParams.Params["importId"].ToString(); // Тип импорта, например "Bars.Gkh.RegOperator.Imports.PaymentsToClosedPeriodsImport"

            var data = LogImportDomain.GetAll()
                .Where(x => x.ImportKey == importId)
                .Filter(loadParams, Container)
                .Select(x => new
                {
                    x.Id,
                    x.ObjectCreateDate,
                    x.FileName,
                    x.CountError,
                    x.CountWarning
                });

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        /// <summary>
        /// Сопоставить лицевой счёт вручную
        /// </summary>
        /// <param name="baseParams">Параметры от браузера. В частности warningId и compareToAccountId: какую запись журнала на какой ЛС сопоставить.</param>
        /// <returns>Удалось ли сопоставить</returns>
        public ActionResult ManualComparePersonalAccount(BaseParams baseParams)
        {
            var warningId = baseParams.Params.GetAsId("warningId");
            var compareToAccountId = baseParams.Params.GetAs<long>("compareToAccountId");

            ComparePersonalAccount(warningId, compareToAccountId);

            return JsonNetResult.Success;
        }

        /// <summary>
        /// Повторить импорт
        /// </summary>
        /// <param name="baseParams">Параметры от браузера</param>
        /// <returns>Удалось ли запустить</returns>
        public ActionResult ReImport(BaseParams baseParams)
        {
            // Каждый импорт имеет параметры. Такие как: тип и файл. 
            // Кроме того, импорт оплат в закрытый период имеет свои собственные параметры: период, и флаг обновления сальдо. А у импорта оплат ещё и РКЦ.
            // Для возможности повторного запуска, параметры сохраняютя в "шапку" (HeaderOfClosedPeriodsImport).

            // Параметры передаются через baseParams.

            // Соответственно, чтобы повторить импорт, необходимо:
            // 1) вытащить период, РКЦ и флаг обновления сальдо и т.д. из "шапки" (положить в baseParams);
            // 2) вытащить файл (положить в baseParams);
            // 3) и запустить задачу.

            try
            {
                // 1. Вытащить параметры из "шапки".
                // 1.1. Базовые.
                var id = baseParams.Params.GetAsId("Id"); 
                var logImort = this.LogImportDomain.Get(id);
                baseParams.Params["importId"] = logImort.ImportKey; // Тип импорта, например "Bars.Gkh.RegOperator.Imports.PaymentsToClosedPeriodsImport"                
                // 1.2. Специфические, под конкретный импорт.
                var headerService = this.Container.Resolve<IHeaderOfClosedPeriodsImportService>($"ImportKey: {logImort.ImportKey}");
                headerService.FillBaseParamsForReImport(baseParams, logImort.Task);
                this.Container.Release(headerService);

                // 2. Вытащить файл.
                var fileInfo = logImort.File;
                byte[] zippedData;
                using (var file = FileManager.GetFile(fileInfo))
                {
                    using (var tmpStream = new MemoryStream())
                    {
                        file.CopyTo(tmpStream);
                        zippedData = tmpStream.ToArray();
                    }
                }
                byte[] unzippedData;
                using (var zipFile = ZipFile.Read(new MemoryStream(zippedData)))
                {
                    var zipEntry = zipFile.FirstOrDefault();
                    using (var tmpStream = new MemoryStream())
                    {
                        zipEntry.Extract(tmpStream);
                        tmpStream.Seek(0, SeekOrigin.Begin);
                        unzippedData = tmpStream.ToArray();
                    }
                }
                baseParams.Files["importGkh"] = new FileData(fileInfo.Name, fileInfo.Extention, unzippedData);
            }
            catch (Exception exception)
            {
                var message = "Не удалось получить файл или параметры импорта";
                this.LogManager.LogError(exception, message);
                return JsonNetResult.Failure(message);
            }

            // 3. Запустить задачу.
            var importProvider = Container.Resolve<IGkhImportService>();
            using (Container.Using(importProvider))
            {
                ActionResult actionResult;
                try
                {
                    var result = importProvider.Import(baseParams);

                    if (result.Success)
                    {
                        actionResult = new JsonNetResult(new
                        {
                            success = result.Success,
                            title = string.Empty,
                            message = "Задачи успешно поставлены в очередь на выполнение"
                        });
                    }
                    else
                    {
                        actionResult = JsonNetResult.Failure(result.Message);
                    }
                }
                catch (NotImplementedException exception)
                {
                    actionResult = JsonNetResult.Failure(exception.Message);
                }
                return actionResult;
            }
        }

        /// <summary>
        /// Получить список задач по разбору импорта, выполняемых в текущий момент на сервере вычислений
        /// </summary>
        /// <param name="baseParams">Параметры от браузера</param>
        /// <returns>Набор данных, пригодный для отображения в гриде</returns>
        public IDataResult RunningTasksList(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            string importId = baseParams.Params["importId"].ToString(); // Тип импорта, например "Bars.Gkh.RegOperator.Imports.PaymentsToClosedPeriodsImport"

            var data = this.TaskEntryDomain.GetAll()
                .Where(x => x.ExecutorCode == importId)                
                .Where(x => x.Status == TaskStatus.InProgress ||
                        x.Status == TaskStatus.Suspended ||
                        x.Status == TaskStatus.Initial ||
                        x.Status == TaskStatus.Queued ||
                        x.Status == TaskStatus.Handling)
                .Filter(loadParams, Container)
                .Select(x => new
                {
                    x.ObjectCreateDate,
                    x.Percentage,
                    x.Status
                });

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
