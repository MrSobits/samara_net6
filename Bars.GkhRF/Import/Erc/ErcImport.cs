namespace Bars.GkhRf.Import
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.GkhRf.Entities;
    using Bars.GkhRf.Enums;
    using Bars.GkhRf.Import.Erc;

    using Castle.Windsor;
    using Gkh.Import.Impl;
    using Ionic.Zip;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Импорт оплат КР из Erc
    /// </summary>
    public class ErcImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// Коды услуг
        /// </summary>
        private readonly string[] codes = new[] { "267", "268", "269", "2", "21", "22", "23", "259" };

        private readonly Dictionary<string, string> streetDict = new Dictionary<string, string>();

        private int year;

        private int month;

        /// <summary>
        /// Признак перезаписи
        /// </summary>
        private bool resave;

        public virtual IWindsorContainer Container { get; set; }

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "Erc"; }
        }

        public override string Name
        {
            get { return "Импорт из ЕРЦ"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "zip,txt"; }
        }

        public override string PermissionName
        {
            get { return "GkhRf.Payment.Import"; }
        }

        public ErcImport(ILogImportManager logImportManager, ILogImport logImport)
        {
            this.LogImportManager = logImportManager;
            this.LogImport = logImport;
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            var message = string.Empty;

            var fileData = baseParams.Files["FileImport"];
            this.resave = baseParams.Params["Resave"].ToStr() == "on";

            InitLog(fileData.FileName);

            try
            {
                foreach (var bytes in this.PrepareFiles(fileData))
                {
                    using (var streamReader = new StreamReader(new MemoryStream(bytes), Encoding.GetEncoding(1251)))
                    {
                        foreach (var dataDict in this.PrepareData(streamReader))
                        {
                            using (var transaction = Container.Resolve<IDataTransaction>())
                            {
                                try
                                {
                                    this.ImportItem(ref message, dataDict.Key, dataDict.Value);
                                    transaction.Commit();
                                }
                                catch (Exception exc)
                                {
                                    LogImport.Error(this.Name, exc.Message);
                                    transaction.Rollback();
                                }
                            }
                        }
                    }
                }

                if (LogImport.CountImportedRows == 0)
                {
                    LogImport.Warn(Name, "Не удалось обнаружить записи для импорта");
                    message = !string.IsNullOrEmpty(message)
                        ? message + ". Не удалось обнаружить записи для импорта"
                        : "Не удалось обнаружить записи для импорта";
                }
            }
            catch (Exception exc)
            {
                try
                {
                    LogImport.IsImported = false;
                    Container.Resolve<ILogger>().LogError(exc, "Импорт из ЕРЦ");
                    message = "Произошла неизвестная ошибка. Обратитесь к администратору";
                    LogImport.Error(Name, "Произошла неизвестная ошибка. Обратитесь к администратору.");
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, exc);
                }
            }

            LogImportManager.Add(fileData, LogImport);
            var endMsg = LogImportManager.GetInfo();
            message = !string.IsNullOrEmpty(message) ? message + ". " + endMsg : endMsg;
            LogImportManager.Save();

            var status = LogImport.CountError > 0
                ? StatusImport.CompletedWithError
                : (LogImport.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
            return new ImportResult(status, message, string.Empty, LogImportManager.LogFileId);
        }

        public override bool Validate(BaseParams baseParams, out string message)
        {
            try
            {
                message = null;
                if (!baseParams.Files.ContainsKey("FileImport"))
                {
                    message = "Не выбран файл для импорта";
                    return false;
                }

                var extention = baseParams.Files["FileImport"].Extention;

                var fileExtentions = PossibleFileExtensions.Contains(",") ? PossibleFileExtensions.Split(',') : new[] { PossibleFileExtensions };
                if (fileExtentions.All(x => x != extention))
                {
                    message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
                    return false;
                }

               /* if (lineParts[8].Trim() != "1.04")
                {
                    var msg = string.Format("Неизвестная версия формата выгрузки: {0}", lineParts[8].Trim());
                    logImport.Error(Name, msg);
                }*/

                return true;
            }
            catch (Exception exp)
            {
                Container.Resolve<ILogger>().LogError(exp, "Валидация файла импорта");
                message = "Произошла неизвестная ошибка при проверки формата файла";
                return false;
            }
        }

        /// <summary>
        /// Сериализуем данные для дальнейшей обработки
        /// </summary>
        /// <param name="sr">StreamReader</param>
        /// <returns>Словарь(инн|кпп, данные)</returns>
        protected Dictionary<string, Dictionary<int, DataErc>> PrepareData(StreamReader sr)
        {
            var dataDict = new Dictionary<string, Dictionary<int, DataErc>>();

            string line;
            string key = string.Empty;

            var lineNum = 0;
            while ((line = sr.ReadLine()) != null && !string.IsNullOrEmpty(line))
            {
                var lineParts = line.Split('|');
                var lineCode = lineParts[0];

                lineNum++;

                switch (lineCode)
                {
                    case "1":
                        {
                            key = string.Format("{0}|{1}", lineParts[4], lineParts[5]);
                            if (!dataDict.ContainsKey(key))
                            {
                                dataDict.Add(key, new Dictionary<int, DataErc>());
                            }
                        }

                        break;
                    case "2":
                          var date = lineParts[1].ToDateTime();
                          month = date.Month;
                          year = date.Year;
                        break;

                    case "4":
                        if (!this.streetDict.ContainsKey(lineParts[1]))
                        {
                            this.streetDict.Add(lineParts[1], lineParts[2]);
                        }

                        break;

                    case "5":
                        {
                            if (string.IsNullOrEmpty(key))
                            {
                                continue;
                            }

                            var codeErc = lineParts[1];
                            var typePayment = lineParts[6];
                            var keyRec = (codeErc + typePayment + month).GetHashCode();
                            if (dataDict[key].ContainsKey(keyRec))
                            {
                                // На случай, если в 5 секции будет несколько записей с одинаковым типом
                                var rec = dataDict[key][keyRec];
                                rec.PaidPopulation += lineParts[10].ToDecimal().To<decimal?>();
                                rec.OutgoingBalance += lineParts[11].ToDecimal().To<decimal?>();
                                rec.IncomeBalance += lineParts[7].ToDecimal().To<decimal?>();
                                rec.ChargePopulation += lineParts[8].ToDecimal().To<decimal?>();
                                rec.Recalculation += lineParts[9].ToDecimal().To<decimal?>();
                                rec.TotalArea += lineParts[12].ToDecimal().To<decimal?>();
                            }
                            else
                            {
                                dataDict[key].Add(
                                    keyRec,
                                    new DataErc
                                        {
                                            Year = year,
                                            Month = month,
                                            LineNum = lineNum.ToStr(),
                                            CodeErc = lineParts[1],
                                            CodeStreet = lineParts[3],
                                            HouseNum = lineParts[4],
                                            IncomeBalance = lineParts[7].ToDecimal().To<decimal?>(),
                                            OutgoingBalance = lineParts[11].ToDecimal().To<decimal?>(),
                                            Recalculation = lineParts[9].ToDecimal().To<decimal?>(),
                                            ChargePopulation = lineParts[8].ToDecimal().To<decimal?>(),
                                            PaidPopulation = lineParts[10].ToDecimal().To<decimal?>(),
                                            TotalArea = lineParts[12].ToDecimal().To<decimal?>(),
                                            TypePayment = lineParts[6],
                                        });
                            }
                        }

                        break;
                }
            }

            return dataDict;
        }

        /// <summary>
        /// Подготавливаем файлы
        /// </summary>
        /// <param name="fileData">файл</param>
        /// <returns>список файлов(потоком)</returns>
        protected List<byte[]> PrepareFiles(FileData fileData)
        {
            var result = new List<byte[]>();

            if (fileData.Extention == "txt")
            {
                result.Add(fileData.Data);
            }

            if (fileData.Extention == "zip")
            {
                using (var zipFile = ZipFile.Read(new MemoryStream(fileData.Data)))
                {
                    var txtZipEntryList = zipFile.Where(x => x.FileName.EndsWith(".txt")).ToArray();
                    if (txtZipEntryList.Any())
                    {
                        using (var ms = new MemoryStream())
                        {
                            foreach (var txtZipEntry in txtZipEntryList)
                            {
                                txtZipEntry.Extract(ms);
                                result.Add(ms.GetBuffer());
                                ms.Seek(0, SeekOrigin.Begin);
                            }
                        }
                    }
                    else
                    {
                        LogImport.IsImported = false;
                        LogImport.Error(Name, "Не удалось обнаружить txt файлов для импорта");
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Импорт файла
        /// </summary>
        /// <param name="message"></param>
        /// <param name="key"> inn + kpp</param>
        /// <param name="dataList"> данные</param>
        protected void ImportItem(ref string message, string key, Dictionary<int, DataErc> dataList)
        {
            var realityObjService = this.Container.Resolve<IRepository<RealityObject>>();
            var paymentService = this.Container.Resolve<IDomainService<Payment>>();
            var paymentItemService = this.Container.Resolve<IRepository<PaymentItem>>();
            var managingOrgService = this.Container.Resolve<IDomainService<ManagingOrganization>>();

            var listPaymentItemForSave = new List<PaymentItem>();
            var chargeDate = new DateTime(this.year, this.month, 1);
            var inn = key.Split('|')[0];
            var kpp = key.Split('|')[1];
            var codesErc = dataList.Select(x => x.Value.CodeErc).AsEnumerable().Distinct().ToArray();

            var realityObjsDict = GetRealityObjects(realityObjService, codesErc);

            // Получаем упр. организацию
            var managingOrg = managingOrgService.GetAll().FirstOrDefault(x => x.Contragent.Inn == inn && x.Contragent.Kpp == kpp);
            if (managingOrg == null)
            {
                var msg = string.Format("Не найдена управляющая организация (ИНН {0} и КПП {1})", inn, kpp);
                LogImport.Warn(Name, msg);
                message += msg;
                managingOrg = GetManOrg(chargeDate, realityObjsDict);
                if (managingOrg == null) return;
            }

            // сгрупирован по объекту недвижимости
            var paymentsDict = GetPayments(paymentService, realityObjsDict.Values.Select(x => x.Id).ToArray());

            var paymentsIds = paymentsDict.Values.Select(x => x.Id).Distinct().ToArray();

            var paymentItemDict = paymentItemService.GetAll().Where(x => paymentsIds.Contains(x.Payment.Id)).GroupBy(x => x.Payment.Id).ToDictionary(x => x.Key, y => y.ToList());

            foreach (var data in dataList.Values)
            {
                var codeErc = data.CodeErc;
                var codeStreet = data.CodeStreet;
                var houseNum = data.HouseNum;

                var realityObj = realityObjsDict.ContainsKey(codeErc) ? realityObjsDict[codeErc] : null;
                if (realityObj == null)
                {
                    LogImport.Warn(Name, string.Format("Не удалось найти жилой дом по коду ЕРЦ: {0}. Адрес: {1} д. {2}", codeErc, this.streetDict.ContainsKey(codeStreet) ? this.streetDict[codeStreet] : string.Empty, houseNum));
                    continue;
                }

                // Получаем оплату по дому, если нет то создаем
                var payment = paymentsDict.ContainsKey(realityObj.Id)  ? paymentsDict[realityObj.Id] : null;
                if (payment == null)
                {
                    payment = new Payment { Id = 0, RealityObject = realityObj };

                    paymentService.Save(payment);

                    paymentsDict.Add(realityObj.Id, payment);

                    LogImport.CountAddedRows++;
                    LogImport.Info(Name, string.Format("Добавлен объект {0} с кодом ЕРЦ {1} за {2}", realityObj.FiasAddress.AddressName, realityObj.CodeErc, chargeDate.ToString("MM.yyyy", CultureInfo.InvariantCulture)));
                }

                if (!codes.Contains(data.TypePayment))
                {
                    continue;
                }

                var typePayment = TypePayment.Cr;
                switch (data.TypePayment)
                {
                    case "268":
                        typePayment = TypePayment.HireRegFund;
                        break;
                    case "269":
                        typePayment = TypePayment.Cr185;
                        break;
                    case "2":
                        typePayment = TypePayment.BuildingCurrentRepair;
                        break;
                    case "21":
                        typePayment = TypePayment.SanitaryEngineeringRepair;
                        break;
                    case "22":
                        typePayment = TypePayment.HeatingRepair;
                        break;
                    case "23":
                        typePayment = TypePayment.ElectricalRepair;
                        break;
                    case "259":
                        typePayment = TypePayment.BuildingRepair;
                        break;
                }

                // Пробуем получить оплату по импортируемым данным
                PaymentItem paymentItem = null;
                var paymentParam = false;

                if (paymentItemDict.ContainsKey(payment.Id))
                {
                    paymentItem = paymentItemDict[payment.Id].FirstOrDefault(x => x.ChargeDate.HasValue
                        && x.ChargeDate.Value.Year == this.year
                        && x.ChargeDate.Value.Month == month
                        && x.TypePayment == typePayment);

                    paymentParam = paymentItemDict[payment.Id]
                        .Any(x => x.ChargeDate.HasValue && x.ChargeDate.Value > chargeDate);
                }

                // Если нет оплаты по заданным год месяц и нет оплат с датами большими чем у нас то создаем новую
                if (paymentItem == null && !paymentParam)
                {
                    paymentItem = new PaymentItem
                    {
                        Id = 0,
                        Payment = payment,
                        ManagingOrganization = managingOrg,
                        ChargeDate = chargeDate,
                        IncomeBalance = data.IncomeBalance,
                        OutgoingBalance = data.OutgoingBalance,
                        Recalculation = data.Recalculation,
                        ChargePopulation = data.ChargePopulation,
                        PaidPopulation = data.PaidPopulation,
                        TypePayment = typePayment
                    };

                    listPaymentItemForSave.Add(paymentItem);

                    LogImport.CountAddedRows++;
                    LogImport.Info(Name, string.Format("Данные по объекту {0} с кодом ЕРЦ {1} за {2} загружены", realityObj.FiasAddress.AddressName, realityObj.CodeErc, chargeDate.ToString("MM.yyyy", CultureInfo.InvariantCulture)));
                }
                else if (paymentItem != null && !paymentParam && this.resave) 
                {
                    // если есть такая оплата и нет оплат с датами большими чем у нас и разрешена перезапись то обновляем
                    paymentItem.ManagingOrganization = managingOrg;
                    paymentItem.ChargeDate = chargeDate;
                    paymentItem.IncomeBalance = data.IncomeBalance;
                    paymentItem.OutgoingBalance = data.OutgoingBalance;
                    paymentItem.Recalculation = data.Recalculation;
                    paymentItem.ChargePopulation = data.ChargePopulation;
                    paymentItem.PaidPopulation = data.PaidPopulation;
                    paymentItem.TypePayment = typePayment;

                    listPaymentItemForSave.Add(paymentItem);
                    LogImport.CountChangedRows++;
                    LogImport.Info(Name, string.Format("Данные по объекту {0} с кодом ЕРЦ {1} за {2} изменены", realityObj.FiasAddress.AddressName, realityObj.CodeErc, chargeDate.ToString("MM.yyyy", CultureInfo.InvariantCulture)));
                }
                else
                {
                    var msg =
                        string.Format(
                            "Не возможно добавить запись по дому {2} из строки {0}, т.к. имеются записи за {1} месяц или более новые",
                            data.LineNum,
                            chargeDate.ToString("MM.yyyy", CultureInfo.InvariantCulture),
                            payment.RealityObject.FiasAddress.AddressName);

                    LogImport.Warn(Name, msg);
                }
            }

            InTransaction(listPaymentItemForSave, paymentItemService);
        }

        private ManagingOrganization GetManOrg(DateTime chargeDate, Dictionary<string, RealityObject> realityObjects)
        {
            var realityObject = realityObjects.Values.FirstOrDefault();
            if (realityObject == null) return null;

            return Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                  .Where(x =>
             (x.ManOrgContract.StartDate.HasValue && x.ManOrgContract.EndDate.HasValue
             && x.ManOrgContract.StartDate.Value <= chargeDate && x.ManOrgContract.EndDate.Value >= chargeDate)
             ||
             (x.ManOrgContract.StartDate.HasValue && !x.ManOrgContract.EndDate.HasValue && x.ManOrgContract.StartDate.Value <= chargeDate)
             ||
             (!x.ManOrgContract.StartDate.HasValue && x.ManOrgContract.EndDate.HasValue && x.ManOrgContract.EndDate.Value >= chargeDate))
             .Where(x => x.RealityObject.Id == realityObject.Id)
             .Select(x => x.ManOrgContract.ManagingOrganization)
             .FirstOrDefault();
        }

        private Dictionary<long, Payment> GetPayments(IDomainService<Payment> paymentService, long[] realtyObjIds)
        {
            var result = new List<Payment>();

            for (var i = 0; i < realtyObjIds.Length; i += 1000)
            {
                var takeCount = realtyObjIds.Length - i < 1000 ? realtyObjIds.Length - i : 1000;
                var tempList = realtyObjIds.Skip(i).Take(takeCount).ToArray();

                result.AddRange(paymentService.GetAll().Where(x => tempList.Contains(x.RealityObject.Id)).ToList());
            }

            return result.GroupBy(x => x.RealityObject.Id).ToDictionary(x => x.Key, x => x.First());
        }

        private Dictionary<string, RealityObject> GetRealityObjects(IRepository<RealityObject> servRealityObject, string[] codesErc)
        {
            var result = new List<RealityObject>();

            for (var i = 0; i < codesErc.Length; i += 1000)
            {
                var takeCount = codesErc.Length - i < 1000 ? codesErc.Length - i : 1000;
                var tempList = codesErc.Skip(i).Take(takeCount).ToArray();

                result.AddRange(servRealityObject.GetAll().Where(x => tempList.Contains(x.CodeErc)).ToList());
            }

            return result.GroupBy(x => x.CodeErc).ToDictionary(x => x.Key, x => x.First());
        }

        private void InTransaction(IEnumerable<IEntity> list, IRepository repos)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var entity in list)
                    {
                        if (entity.Id.ToLong() > 0)
                        {
                            repos.Update(entity);
                        }
                        else
                        {
                            repos.Save(entity);
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(
                            string.Format(
                                "Произошла не известная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                e.Message,
                                e.StackTrace),
                            exc);
                    }

                    throw;
                }
            }
        }

        private void InitLog(string fileName)
        {
            if (this.LogImportManager == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImportManager");
            }

            this.LogImportManager.FileNameWithoutExtention = fileName;
            this.LogImportManager.UploadDate = DateTime.Now;

            if (this.LogImport == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImport");
            }

            this.LogImport.SetFileName(fileName);
            this.LogImport.ImportKey = Key;
        }
    }
}
