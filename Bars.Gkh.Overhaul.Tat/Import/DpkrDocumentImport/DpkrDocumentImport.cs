namespace Bars.Gkh.Overhaul.Tat.Import.DpkrDocumentImport
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.Import.Impl;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.GkhExcel;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Сервис импорта домов в документ ДПКР
    /// </summary>
    public class DpkrDocumentImport : GkhImportBase
    {
        public static string Id = "DpkrDocumentImport";

        public override string Key => DpkrDocumentImport.Id;

        public override string CodeImport => DpkrDocumentImport.Id;

        public override string Name => "Импорт домов в документ ДПКР";

        public override string PossibleFileExtensions => "xls, xlsx";

        public override string PermissionName => "Ovrhl.DpkrDocument.dpkrDocumentImport.View";

        private Dictionary<long, string> addressesByDict;
        private List<string> dublicateFormatAddresses = new List<string>();
        private IEnumerable<long> includedDocumentRoIds;
        private IEnumerable<long> includedCurrentDocumentRoIds;
        private IEnumerable<long> excludedDocumentRoIds;
        private long[] listIncludedAdd = new long[] { };
        private long[] listExcludedAdd = new long[] { };

        public DpkrDocumentImport(ILogImportManager logImportManager, ILogImport logImport)
        {
            this.LogImportManager = logImportManager;
            this.LogImport = logImport;
            this.addressesByDict = new Dictionary<long, string>();
        }

        /// <summary>
        /// Парсер импорта домов в документ ДПКР
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public override ImportResult Import(BaseParams baseParams)
        {
            var dpkrDocumentId = baseParams.Params.GetAsId("dpkrDocumentId");
            var fileData = baseParams.Files["FileImport"];
            var message = string.Empty;

            this.InitLog(fileData.FileName);
            this.GetRealityObjectsList(dpkrDocumentId);

            var importData = this.GetImportData(fileData);

            this.CompareRealityObjectAddresses(importData, dpkrDocumentId);

            this.LogImportManager.Add(fileData, this.LogImport);
            this.LogImportManager.Save();

            message += this.LogImportManager.GetInfo();
            var status = this.LogImportManager.CountError > 0
                ? StatusImport.CompletedWithError
                : (this.LogImportManager.CountWarning > 0
                    ? StatusImport.CompletedWithWarning
                    : StatusImport.CompletedWithoutError);

            return new ImportResult(status, message, string.Empty, this.LogImportManager.LogFileId);
        }

        /// <summary>
        /// Форматирование адреса
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private string FormatAddress(string address) => address.Replace(" ", "").ToUpper();

        /// <summary>
        /// Получение домов для документов ДПКР
        /// </summary>
        /// <param name="baseParams"></param>
        private void GetRealityObjectsList(long dpkrDocumentId)
        {
            var dpkrDocumentRealityObjectDomain = this.Container.ResolveDomain<DpkrDocumentRealityObject>();
            using (this.Container.Using(dpkrDocumentRealityObjectDomain))
            {
                var dpkrRoList = dpkrDocumentRealityObjectDomain.GetAll()
                    .Select(x => new
                    {
                        DpkrId = x.DpkrDocument.Id,
                        x.RealityObject.Id,
                        x.IsIncluded
                    })
                    .ToList();

                this.includedDocumentRoIds = dpkrRoList
                    .Where(x => x.IsIncluded)
                    .Select(x => x.Id);

                this.includedCurrentDocumentRoIds = dpkrRoList
                    .Where(x => x.DpkrId == dpkrDocumentId && x.IsIncluded)
                    .Select(x => x.Id);

                this.excludedDocumentRoIds = dpkrRoList
                    .Where(x => !x.IsIncluded)
                    .Select(x => x.Id);
            }
        }

        /// <summary>
        /// Получение значений из файла Excel
        /// </summary>
        /// <param name="fileData">Файл Excel</param>
        /// <returns>Список строк из файла</returns>
        private List<string> GetImportData(FileData fileData)
        {
            var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");

            if (excel == null)
            {
                throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
            }

            var memoryStreamFile = new MemoryStream(fileData.Data);
            using (this.Container.Using(excel, memoryStreamFile))
            {
                if (fileData.Extention.ToLower() == "xlsx")
                {
                    excel.UseVersionXlsx();
                }

                excel.Open(memoryStreamFile);

                return excel.GetRows(0, 0)
                    .Where(x => x[0].Value != string.Empty)
                    .Select(x => x[0].Value)
                    .ToList();
            }
        }

        /// <summary>
        /// Сопоставление домов по адресам объектов жилищного фонда.
        /// </summary>
        /// <param name="importData"></param>
        /// <param name="dpkrDocumentId"></param>
        private void CompareRealityObjectAddresses(List<string> importData, long dpkrDocumentId)
        {
            var importDataDict = importData
                    .GroupBy(x => this.FormatAddress(x))
                    .ToDictionary(x => x.Key, y => y.Select(z => z));

            var realityObjectDomain = this.Container.Resolve<IDomainService<RealityObject>>();
            using (this.Container.Using(realityObjectDomain))
            {
                var appAddresses = realityObjectDomain.GetAll()
                    .Where(x => x.TypeHouse != TypeHouse.BlockedBuilding
                                && x.TypeHouse != TypeHouse.Individual
                                && x.ConditionHouse != ConditionHouse.Razed
                                && importDataDict.Keys.Contains(x.Address.Replace(" ", "").ToUpper()))
                    .Select(x => new
                    {
                        x.Id,
                        FormatAddress = x.Address.Replace(" ", "").ToUpper()
                    })
                    .ToList();

                this.dublicateFormatAddresses = appAddresses
                    .GroupBy(x => x.FormatAddress)
                    .Where(x => x.Select(y => y).Count() > 1)
                    .Select(x => x.Key)
                    .ToList();

                this.addressesByDict = appAddresses
                    .Where(x => !this.dublicateFormatAddresses.Contains(x.FormatAddress))
                    .ToDictionary(x => x.Id, y => y.FormatAddress);
            }

            var isAllMatched = this.addressesByDict.Keys.Count() == importDataDict.Keys.Count();

            if (isAllMatched)
            {
                this.AddHousesToTabs(dpkrDocumentId);
            }

            this.CreateLog(importData, importDataDict, listIncludedAdd, listExcludedAdd, isAllMatched);
        }

        /// <summary>
        /// Добавление домов на вкладки "Включенные" и "Исключенные"
        /// </summary>
        /// <param name="adressesByDict">Словарь "id дома - адрес"</param>
        /// <param name="dpkrDocumentId">Идентификатор документа ДПКР</param>
        private void AddHousesToTabs(long dpkrDocumentId)
        {
            // Добавление во вкладку "Включенные"
            this.listIncludedAdd = this.addressesByDict
                .Where(x => !this.includedDocumentRoIds.Contains(x.Key))
                .Select(x => x.Key)
                .ToArray();

            this.AddRealityObjects(listIncludedAdd, dpkrDocumentId, true);

            // Добавление текущего документа во вкладку "Исключенные"
            // Добавляем дома, которых НЕТ в загружаемом файле, но есть в других документах во вкладке "включенные дома."
            // и НЕТ во вкладке "Исключенные" всех документов, включая текущий.
            this.listExcludedAdd = this.includedDocumentRoIds
                .Except(this.includedCurrentDocumentRoIds)
                .Except(this.addressesByDict.Select(x => x.Key))
                .Except(this.excludedDocumentRoIds)
                .ToArray();

            this.AddRealityObjects(listExcludedAdd, dpkrDocumentId, false);
        }

        /// <summary>
        /// Создание лога импорта домов
        /// </summary>
        /// <param name="importData">Данные из файла</param>
        /// <param name="listIncludedAdd">Дома, включенные в ДПКР</param>
        /// <param name="listExcludedAdd">Дома, исключенные из ДПКР</param>
        /// /// <param name="isAllMatched">Признак сопоставления всех домов</param>
        private void CreateLog(List<string> importData, Dictionary<string, IEnumerable<string>> importDataDict,
            long[] listIncludedAdd, long[] listExcludedAdd, bool isAllMatched)
        {
            var matchedList = importData;
            var notMatchedList = new List<string>();

            if (!isAllMatched)
            {
                var dublicateAddresses = importDataDict
                    .Where(x => this.dublicateFormatAddresses.Contains(x.Key))
                    .SelectMany(x => x.Value)
                    .ToList();

                dublicateAddresses.ForEach(x => this.LogImport.Info("Дом сопоставлен с несколькими записями", x));

                matchedList = importDataDict
                    .Where(x => this.addressesByDict.Values.Contains(x.Key))
                    .SelectMany(x => x.Value)
                    .ToList();
                notMatchedList = importData.Except(matchedList).Except(dublicateAddresses).ToList();
            }

            // Лог включенных и исключенных в ДПКР домов
            Action AddLogIncludedHouses = () =>
            {
                var includedHouses = this.addressesByDict
                    .Where(x => listIncludedAdd.Contains(x.Key))
                    .Select(x => x.Value)
                    .ToList();

                if (includedHouses.Any())
                {
                    this.LogImport.Info("Включенные дома",
                                    "Во вкладку 'Включенные дома' добавлены следующие адреса:\n" +
                                    string.Join("\n", includedHouses));

                    this.LogImport.CountAddedRows += includedHouses.Count;
                }

                if (listExcludedAdd.Any())
                {
                    var realityObjectDomain = this.Container.Resolve<IDomainService<RealityObject>>();
                    using (this.Container.Using(realityObjectDomain))
                    {
                        var excludedHouses = realityObjectDomain.GetAll()
                            .Where(x => listExcludedAdd.Contains(x.Id))
                            .Select(x => x.Address)
                            .ToList();
                        
                            this.LogImport.Info("Исключенные дома",
                                            "Во вкладку 'Исключенные дома' добавлены следующие адреса:\n" +
                                            string.Join("\n", excludedHouses));

                            this.LogImport.CountAddedRows += excludedHouses.Count;
                    }
                }
            };

            // Лог сопоставленных домов
            Action AddMatchetLog = () => matchedList.ForEach(x => this.LogImport.Info("Дом сопоставлен", x));
            
            if (notMatchedList.Any())
            {
                notMatchedList
                    .ForEach(x =>
                    {
                        this.LogImport.Warn("Дом не сопоставлен", x);
                    });

                AddMatchetLog();
                AddLogIncludedHouses();
            }
            else
            {
                AddLogIncludedHouses();
                AddMatchetLog();
            }
        }

        /// <summary>
        /// Выполнение операции "Заполнение вкладок "Включенные дома", "Исключенные дома":
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="dpkrDocumentId"></param>
        /// <param name="included"></param>
        /// <returns></returns>
        private void AddRealityObjects(long[] ids, long dpkrDocumentId, bool included)
        {
            var dpkrDocumentRealityObjectDomain = this.Container.ResolveDomain<DpkrDocumentRealityObject>();

            try
            {
                var roToSave = ids
                    .Select(id => new DpkrDocumentRealityObject
                    {
                        DpkrDocument = new DpkrDocument { Id = dpkrDocumentId },
                        RealityObject = new RealityObject { Id = id },
                        IsIncluded = included,
                        IsExcluded = !included
                    });

                TransactionHelper.InsertInManyTransactions(this.Container, roToSave);
            }
            catch (Exception ex)
            {
                this.LogImport.IsImported = false;
                this.LogImport.Error($"Парсер exel", "Ошибка при заполнении вкладок 'Включенные дома', 'Исключенные дома'", ex.Message);
            }
            finally
            {
                this.Container.Release(dpkrDocumentRealityObjectDomain);
            }
        }
    }
}