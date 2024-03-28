namespace Bars.Gkh.Import
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Import;

    using Castle.Windsor;
    using Gkh.Import.Impl;

    using Microsoft.Extensions.Logging;

    public class LoadFundIds : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public virtual IWindsorContainer Container { get; set; }

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "FundImport"; }
        }

        public override string Name
        {
            get { return "Загрузка идентификаторов Фонда"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "csv"; }
        }

        public override string PermissionName
        {
            get { return "Import.LoadFundIdsImport.View"; }
        }

        public LoadFundIds(ILogImportManager logImportManager, ILogImport logImport)
        {
            this.LogImportManager = logImportManager;
            this.LogImport = logImport;
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            var servRealityObject = Container.Resolve<IDomainService<RealityObject>>();
            var message = string.Empty;

            var fileData = baseParams.Files["FileImport"];

            InitLog(fileData.FileName);

            var realtyObjects = servRealityObject.GetAll()
                .Where(x => x.FiasAddress.StreetCode != null)
                .Select(x => new { x.Id, x.FiasAddress.StreetCode, x.FiasAddress.House, x.FiasAddress.Housing })
                .AsEnumerable()
                .GroupBy(x => x.StreetCode)
                .ToDictionary(x => x.Key, x => x.ToList());

            var findRealtyObjects = new Dictionary<long, string>();

            using (var memoryStreamFile = new MemoryStream(fileData.Data))
            {
                memoryStreamFile.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(memoryStreamFile, Encoding.GetEncoding(1251));
                var headers = reader.ReadLine().ToStr().Split(';').Select(x => x.Trim('"')).ToArray();

                var indexFederalNum = -1;
                var indexCodeKladrStreet = -1;
                var indexHouse = -1;
                var indexHousing = -1;
                for (var i = 0; i < headers.Length; i++)
                {
                    var header = headers[i];
                    switch (header.ToUpper())
                    {
                        case "ID_MKD":
                            indexFederalNum = i;
                            break;
                        case "KLADR_STREET":
                            indexCodeKladrStreet = i;
                            break;
                        case "NUM":
                            indexHouse = i;
                            break;
                        case "KORPUS":
                            indexHousing = i;
                            break;
                    }
                }

                while (true)
                {
                    var data = reader.ReadLine().ToStr().Split(';').Select(x => x.Trim('"')).ToArray();
                    if (data.Length <= 1)
                    {
                        break;
                    }

                    var federalNum = data[indexFederalNum];
                    var codeKladrStreet = data[indexCodeKladrStreet];
                    var house = data[indexHouse];
                    var housing = data[indexHousing];

                    if (string.IsNullOrEmpty(codeKladrStreet) || codeKladrStreet.Length != 17)
                    {
                        LogImport.Warn(Name, string.Format("Не верный код КЛАДРа улицы {0}", codeKladrStreet));
                        continue;
                    }

                    if (string.IsNullOrEmpty(house))
                    {
                        LogImport.Warn(Name, string.Format("Не задан номер дома {0}", codeKladrStreet));
                        continue;
                    }

                    var codeKladr = codeKladrStreet.Substring(0, 15);

                    if (!realtyObjects.ContainsKey(codeKladr))
                    {
                        LogImport.Warn(Name, string.Format("Не найден объект {0}", codeKladrStreet));
                        continue;
                    }

                    var result = realtyObjects[codeKladr];
                    if (!string.IsNullOrEmpty(house) && !string.IsNullOrEmpty(housing))
                    {
                        result = result.Where(x => x.House == house.Trim() && x.Housing == housing.Trim()).ToList();
                    }
                    else if (!string.IsNullOrEmpty(house) && string.IsNullOrEmpty(housing))
                    {
                        result = result.Where(x => x.House == house.Trim() && string.IsNullOrEmpty(x.Housing)).ToList();
                    }
               
                    if (result.Count == 0)
                    {
                        LogImport.Warn(Name, string.Format("Не найден объект {0}", codeKladrStreet));
                        continue;
                    }

                    if (result.Count > 1)
                    {
                        LogImport.Warn(Name, string.Format("{0} - найдено {1} объектов. ", codeKladrStreet, result.Count));
                        continue;
                    }
                               
                    var home = result.First();

                    if (findRealtyObjects.ContainsKey(home.Id))
                    {
                        LogImport.Warn("Дублирующаяся запись. {0}.", federalNum);

                        // предыдущий тоже не грузим
                        findRealtyObjects[home.Id] = null;
                        continue;
                    }

                    findRealtyObjects.Add(home.Id, federalNum);
                }
            }

            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var findRealtyObject in findRealtyObjects.Where(x => x.Value != null))
                    {
                        var realityObj = servRealityObject.Load(findRealtyObject.Key);
                        realityObj.FederalNum = findRealtyObject.Value;
                        servRealityObject.Update(realityObj);
                        LogImport.Info("Обновлен объект с адресом {0}", realityObj.Address);
                    }
                }
                catch (Exception exc)
                {
                    try
                    {
                        LogImport.IsImported = false;
                        Container.Resolve<ILogger>().LogError(exc, "Импорт");
                        message = "Произошла неизвестная ошибка. Обратитесь к администратору";
                        LogImport.Error(Name, "Произошла неизвестная ошибка. Обратитесь к администратору.");

                        transaction.Rollback();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message, exc);
                    }
                }
            }

            LogImportManager.Add(fileData, LogImport);
            LogImportManager.Save();

            message += LogImportManager.GetInfo();
            var status = LogImportManager.CountError > 0 ? StatusImport.CompletedWithError : (LogImportManager.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
            return new ImportResult(status, message, string.Empty, LogImportManager.LogFileId);
        }

        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;
            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            var fileData = baseParams.Files["FileImport"];
            var extention = baseParams.Files["FileImport"].Extention;

            var fileExtentions = PossibleFileExtensions.Contains(",") ? PossibleFileExtensions.Split(',') : new[] { PossibleFileExtensions };
            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
                return false;
            }

            using (var memoryStreamFile = new MemoryStream(fileData.Data))
            {
                memoryStreamFile.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(memoryStreamFile, Encoding.GetEncoding(1251));
                var headers = reader.ReadLine().ToStr().Split(';').Select(x => x.Trim('"')).ToArray();
                if (headers.Length == 0)
                {
                    message = "Загаловок файла не соответствует формату";
                    return false;
                }

                var indexIdMkd = -1;
                var indexCodeKladrStreet = -1;
                var indexNumStreet = -1;
                var indexBlockStreet = -1;
                for (var i = 0; i < headers.Length; i++)
                {
                    var header = headers[i];
                    switch (header.ToUpper())
                    {
                        case "ID_MKD":
                            indexIdMkd = i;
                            break;
                        case "KLADR_STREET":
                            indexCodeKladrStreet = i;
                            break;
                        case "NUM":
                            indexNumStreet = i;
                            break;
                        case "KORPUS":
                            indexBlockStreet = i;
                            break;
                    }
                }

                if (indexIdMkd == -1)
                {
                    message = "Не найден столбец \"ID_MKD\"";
                    return false;
                }

                if (indexCodeKladrStreet == -1)
                {
                   message = "Не найден столбец \"KLADR_STREET\"";
                   return false;
                }

                if (indexNumStreet == -1)
                {
                    message = "Не найден заголовок \"NUM\"";
                    return false;
                }

                if (indexBlockStreet == -1)
                {
                    message = "Не найден заголовок \"KORPUS\"";
                    return false;
                }
            }

            return true;
        }

        public void InitLog(string fileName)
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
