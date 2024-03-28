namespace Bars.Gkh.RegOperator.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Import;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Imports;
    using Bars.Gkh.RegOperator.Imports.ImportRkc;
    using Bars.Gkh.RegOperator.Services.DataContracts;

    using Ionic.Zip;
    using Ionic.Zlib;

    public partial class Service
    {
        public SyncRkcResult SyncPersAccRkc(ImportRkcRecord record)
        {
             var result = new SyncRkcResult
            {
                Code = "00",
                Description = "Успешно"
            };

            var accResult = new List<SyncRkcAccountResult>();

            var rkcImportService = Container.Resolve<IRkcImportService>();
            var cashPaymentCenterDomain = Container.ResolveDomain<CashPaymentCenter>();
            var regopServiceLogDomain = Container.ResolveDomain<RegopServiceLog>();

            var cashPaymentCenters = cashPaymentCenterDomain.GetAll()
                .Where(x => x.Contragent.Inn == record.RkcInn)
                .ToList();

            try
            {
                ILogImport logImport = null;
                var cashPayCenterName = "Не удалось определить РКЦ";
                if (record.FormatVersion != "1.1")
                {
                    logImport = Container.Resolve<ILogImport>();
                    logImport.Error("Ошибка", "Неверная версия формата");

                    result.Code = "01";
                    result.Description = "Не верная версия формата";
                }
                else
                {
                    if (cashPaymentCenters.Count > 1)
                    {
                        logImport = Container.Resolve<ILogImport>();
                        logImport.Error("Ошибка", "В системе заведены несколько РКЦ c таким ИНН");

                        result.Code = "04";
                        result.Description = "В системе заведены несколько РКЦ c таким ИНН";
                    }
                    else
                    {
                        var cashPaymentCenter = cashPaymentCenters.First();

                        if (cashPaymentCenter != null)
                        {
                            cashPayCenterName = cashPaymentCenter.Contragent.Name;
                            logImport = rkcImportService.Import(record);
                            accResult = rkcImportService.GetAccountResult();
                        }
                        else
                        {
                            logImport = Container.Resolve<ILogImport>();
                            logImport.Error("Ошибка", "Не удалось определить РКЦ");

                            result.Code = "02";
                            result.Description = "РКЦ не зарегистрирован в системе";
                        }
                    }
                }

                var logsZip = new ZipFile(Encoding.UTF8)
                {
                    CompressionLevel = CompressionLevel.Level3,
                    ProvisionalAlternateEncoding = Encoding.GetEncoding("cp866")
                };

                logImport.PlacingResults();

                var fileStream = logImport.GetFile();
                var buffer = new byte[fileStream.Length];

                fileStream.Seek(0, SeekOrigin.Begin);
                fileStream.Read(buffer, 0, buffer.Length);
                fileStream.Seek(0, SeekOrigin.Begin);

                logsZip.AddEntry(record.FileNumber + ".log.csv", buffer);

                using (var logFile = new MemoryStream())
                {
                    logsZip.Save(logFile);

                    var fileManager = Container.Resolve<IFileManager>();

                    var logFileInfo = fileManager.SaveFile(logFile, string.Format("{0}.log.zip", record.FileNumber));
                    result.LogFileId = logFileInfo.Id;

                    var fileDate = record.FileDate.ToDateTime();

                    var log = new RegopServiceLog
                    {
                        CashPayCenterName = cashPayCenterName,
                        DateExecute = DateTime.Now,
                        FileNum = record.FileNumber,
                        FileDate = fileDate == DateTime.MinValue ? null : (DateTime?) fileDate,
                        MethodType = RegopServiceMethodType.ImportPersAccRkc,
                        Status = logImport.CountError == 0 && logImport.CountWarning == 0,
                        File = logFileInfo
                    };

                    regopServiceLogDomain.Save(log);
                }

                result.AccountResult = accResult;

                return result;
            }
            finally
            {
                Container.Release(rkcImportService);
                Container.Release(cashPaymentCenterDomain);
                Container.Release(regopServiceLogDomain);
            }
        }
    }
}