using System;
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
using Bars.Gkh.RegOperator.Services.DataContracts;
using Ionic.Zip;
using Ionic.Zlib;

namespace Bars.Gkh.RegOperator.Services.Impl
{
    public partial class Service
    {
        public SyncRkcResult SyncChargePaymentRkc(SyncChargePaymentRkcRecord record)
        {
            var logImport = Container.Resolve<ILogImport>();

            var result = new SyncRkcResult
            {
                Code = "00",
                Description = "Успешно"
            };

            var cashPaymentCenterDomain = Container.ResolveDomain<CashPaymentCenter>();
            var regopServiceLogDomain = Container.ResolveDomain<RegopServiceLog>();

            string name = "Не удалось определить РКЦ";
            try
            {
                var date = record.RegDate.ToDateTime();
                var regnum = record.RegNumber;

                var cashPaymentCenter =
                    cashPaymentCenterDomain.GetAll()
                        .FirstOrDefault(x => x.Contragent.Inn == record.RkcInn);

                if (cashPaymentCenter != null)
                {
                    name = cashPaymentCenter.Contragent.Name;
                }

                if (record.Payments == null || !record.Payments.Any())
                {
                    logImport.Error("Ошибка", "Отсутствуют записи для загрузки");
                    result.Code = "05";
                    result.Description = "Отсутствуют записи для загрузки";
                }
                else if (regopServiceLogDomain.GetAll()
                    .Where(x => x.Status)
                    .Where(x => x.MethodType == RegopServiceMethodType.ImportChargePayment)
                    .Where(x => date <= DateTime.MinValue || x.FileDate == date)
                    .Any(x => x.FileNum == regnum))
                {
                    result.Description = "Переданный файл уже был загружен";
                    logImport.Error("Ошибка", "Переданный файл уже был загружен");
                }
                else if (record.FormatVersion != "1.1")
                {
                    logImport.Error("Ошибка", "Неверная версия формата");
                    result.Code = "01";
                    result.Description = "Неверная версия формата";
                }
                else
                {
                    if (cashPaymentCenter != null)
                    {
                        new SyncChargePaymentRkcHelper(Container)
                            .SyncChargePaymentRkc(
                                record,
                                cashPaymentCenter,
                                result,
                                logImport);
                    }
                    else
                    {
                        logImport.Error("Ошибка", "Не удалось определить РКЦ");
                        result.Code = "02";
                        result.Description = "РКЦ не зарегистрирован в системе";
                    }
                }
            }
            catch (Exception e)
            {
                logImport.Error("Ошибка", e.Message);
            }
            finally
            {
                Container.Release(cashPaymentCenterDomain);
            }

            logImport.PlacingResults();

            var logsZip = new ZipFile(Encoding.UTF8)
            {
                CompressionLevel = CompressionLevel.Level3,
                AlternateEncoding = Encoding.GetEncoding("cp866")
            };

            var buffer = logImport.GetFile().ReadAllBytes();

            logsZip.AddEntry(record.RegNumber + ".log.csv", buffer);

            using (var outputStream = new MemoryStream())
            {
                logsZip.Save(outputStream);

                var fileManager = Container.Resolve<IFileManager>();

                var logFileInfo = fileManager.SaveFile(outputStream, string.Format("{0}.log.zip", record.RegNumber));
                result.LogFileId = logFileInfo.Id;

                var fileDate = record.RegDate.ToDateTime();

                var log = new RegopServiceLog
                {
                    CashPayCenterName = name,
                    DateExecute = DateTime.Now,
                    FileNum = record.RegNumber,
                    FileDate = fileDate == DateTime.MinValue ? null : (DateTime?)fileDate,
                    MethodType = RegopServiceMethodType.ImportChargePayment,
                    Status = logImport.CountError == 0 && logImport.CountWarning == 0,
                    File = logFileInfo
                };

                regopServiceLogDomain.Save(log);
            }

            return result;
        }
    }
}