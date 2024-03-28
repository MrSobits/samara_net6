using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums.Import;
using Bars.Gkh.Helpers;
using Bars.Gkh.Import;
using Bars.Gkh.Import.Impl;
using Bars.GkhCr.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Bars.GkhCr.Import
{
    public class PerformedWorkImport : GkhImportBase
    {
        #region Properties

        public new ILogImport LogImport { get; set; }

        public new ILogImportManager LogImportManager { get; set; }

        public IDomainService<TypeWorkCr> TypeWorkCrDomain { get; set; }

        public IDomainService<BuildContractTypeWork> BuildContractTypeWorkDomain { get; set; }

        public IDomainService<BuildContract> BuildContractDomain { get; set; }

        public IDomainService<PerformedWorkAct> PerformedWorkActDomain { get; set; }

        public IDomainService<Contragent> ContragentDomain { get; set; }
        #endregion

        #region fields

        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public override string Key => Id;

        public override string CodeImport => "PerformedWorkImport";

        public override string Name => "Импорт выполненных работ";

        public override string PossibleFileExtensions => "xlsx";

        public override string PermissionName => "Import.PerformedWork.View";

        #endregion

        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;

            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            var fileData = baseParams.Files["FileImport"];
            var extention = fileData.Extention;

            var fileExtentions = PossibleFileExtensions.Contains(",") ? PossibleFileExtensions.Split(',') : new[] { PossibleFileExtensions };
            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
                return false;
            }

            return true;
        }

        protected override ImportResult Import(BaseParams baseParams,
            B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx,
            IProgressIndicator indicator,
            CancellationToken ct)
        {
            var file = baseParams.Files["FileImport"];
            LogImport.SetFileName(file.FileName);
            LogImport.ImportKey = CodeImport;

            //парсинг
            Indicate(0, "Парсинг файла");
            try
            {
                var records = Parce(file, ct);

                int i = 0;
                foreach (var record in records)
                {
                    Indicate(++i * 100 / records.Count, $"Обработка записи {i} из {records.Count}");
                    Process(record, LogImport, ct);
                }
            }
            catch (Exception e)
            {
                return new ImportResult(StatusImport.CompletedWithError, e.Message + '\n' + e.StackTrace);
            }

            LogImportManager.FileNameWithoutExtention = file.FileName;
            LogImportManager.Add(file, LogImport);
            LogImportManager.Save();

            var statusImport = LogImport.CountError > 0 ? StatusImport.CompletedWithError :
                        LogImport.CountWarning > 0 ? StatusImport.CompletedWithWarning :
                        StatusImport.CompletedWithoutError;

            return new ImportResult(statusImport, string.Format("Импортировано {0} записей", LogImport.CountAddedRows), string.Empty, LogImportManager.LogFileId);
        }

        private List<ImportRecord> Parce(FileData file, CancellationToken ct)
        {
            var result = new List<ImportRecord>();

            var importHelper = Container.Resolve<IImportHelper>();

            using (var xlsMemoryStream = new MemoryStream(file.Data))
            {
                //---открытие файла---
                importHelper.Excel.UseVersionXlsx();
                importHelper.Excel.Open(xlsMemoryStream);

                Indicate(0, "Поиск заголовка");
                var headerRow = importHelper.FindHeaderByText("Код конструктивного элемента (системы)", 5);

                //---заголовки---
                Indicate(0, "Сборка словаря заголовков");
                var headers = importHelper.GetComplexHeaders(3);

                //---данные---
                var count = importHelper.Excel.GetRowsCount(0, 0);

                for (int i = headerRow + 1; i < count; i++)
                {
                    ct.ThrowIfCancellationRequested();

                    if (String.IsNullOrWhiteSpace(importHelper.Read<string>(i, "Субъект Российской Федерации", true)))
                        continue;
                    if (importHelper.Read<string>(i, "Субъект Российской Федерации", true) == "1")
                        continue;
                    if (i > 10 && String.IsNullOrWhiteSpace(importHelper.Read<string>(i, "Субъект Российской Федерации", true)))
                    {
                        return result;
                    }

                    Indicate(i * 100 / count, $"Парсинг записи {i - headerRow} из {count - headerRow}");

                    result.Add(new ImportRecord()
                    {
                        N = i + 1,
                        HouseId = importHelper.Read<int>(i, "Код МКД", true),
                        WorkName = importHelper.Read<string>(i, "Вид работы (услуги)", true),
                        EndYear = importHelper.Read<ushort>(i, "Год завершения работы (услуги)", true),
                        SumByContract = importHelper.Read<decimal?>(i, "в соответствии с заключенными договорами", true),
                        SumPassed = importHelper.Read<decimal?>(i, "принято по актам", true),
                        Volume = importHelper.Read<decimal?>(i, "Объем", true),
                        CustomerName = importHelper.Read<string>(i, "Заказчик работы (услуги) по капитальному ремонту-Наименование", true),
                        CustomerInn = importHelper.Read<long?>(i, "Заказчик работы (услуги) по капитальному ремонту-ИНН", true),
                        ExecutorName = importHelper.Read<string>(i, "Исполнитель работы (услуги) по капитальному ремонту-Наименование", true),
                        ExecutorInn = importHelper.Read<long?>(i, "Исполнитель работы (услуги) по капитальному ремонту-ИНН", true),
                        ContractDate = importHelper.ReadDateTimeNullable(i, "Дата заключения договора подряда", true),
                        ContractPlannedEndDate = importHelper.ReadDateTimeNullable(i, "Плановая дата завершения работ (услуг) по договору подряда", true),
                        ActDate = importHelper.ReadDateTimeNullable(i, "Дата подписания акта приемки", true),
                    });
                }
            }

            return result;
        }

        private void Process(ImportRecord record, ILogImport logImport, CancellationToken ct)
        {
            var typework = FindTypeWork(record.HouseId, record.WorkName, record.EndYear);
            if (typework == null)
            {
                logImport.Error("Ошибка сопоставления typework", $"Не найден TypeWorkCr c домом {record.HouseId}, работой {record.WorkName} и годом {record.EndYear}, привязанную к КПКР");
                return;
            }

            var buildContracts = GetBuildContracts(typework);
            var acts = GetWorkAct(typework);

            if (record.Volume.HasValue)
            {
                if (!typework.Volume.HasValue || typework.Volume.Value != record.Volume.Value)
                {
                    typework.Volume = record.Volume.Value;
                    TypeWorkCrDomain.Update(typework);
                    logImport.CountChangedRows++;
                }
            }

            if (record.SumByContract.HasValue)
            {
                var sum = buildContracts.Select(x => x.Sum).Sum();
                if (sum.HasValue && sum.Value != record.SumByContract.Value)
                {
                    if (buildContracts.Length == 1)
                    {
                        buildContracts[0].Sum = sum.Value;
                        BuildContractDomain.Update(buildContracts[0]);
                        logImport.CountChangedRows++;
                    }
                    else
                        logImport.Warn("Ошибка суммы по договорам", $"Сумма {record.SumByContract.Value} не совпадает c суммой по договорам {sum.Value}, и их больше одного");
                }
            }

            if (record.SumPassed.HasValue)
            {
                var sum = acts.Select(x => x.Sum).Sum();
                if (sum.HasValue && sum.Value != record.SumPassed.Value)
                {
                    if (acts.Length == 1)
                    {
                        acts[0].Sum = sum.Value;
                        PerformedWorkActDomain.Update(acts[0]);
                        logImport.CountChangedRows++;
                    }
                    else
                        logImport.Warn("Ошибка суммы по актам выполенных работ", $"Сумма {record.SumByContract.Value} не совпадает c суммой по актам {sum.Value}, и их больше одного");
                }
            }

            if (record.CustomerInn.HasValue)
            {
                var customer = GetContragent(record.CustomerName, record.CustomerInn.Value.ToString(), logImport);

                if (!buildContracts.Where(x => x.Contragent.Id == customer.Id).Any())
                {
                    BuildContractDomain.Save(new BuildContract
                    {
                        Contragent = customer,
                        ObjectCr = typework.ObjectCr,
                        TypeContractBuild = Enums.TypeContractBuild.NotDefined,
                        DateStartWork = record.ContractDate
                    });

                    logImport.CountAddedRows++;
                }
            }

            if (record.ExecutorInn.HasValue)
            {
                var customer = GetContragent(record.CustomerName, record.CustomerInn.Value.ToString(), logImport);

                if (!buildContracts.Where(x => x.Contragent.Id == customer.Id).Any())
                {
                    BuildContractDomain.Save(new BuildContract
                    {
                        Contragent = customer,
                        ObjectCr = typework.ObjectCr,
                        TypeContractBuild = Enums.TypeContractBuild.NotDefined
                    });

                    logImport.CountAddedRows++;
                }
            }
        }

        private Contragent GetContragent(string name, string inn, ILogImport logImport)
        {
            var countragent = ContragentDomain.GetAll()
               .Where(x => x.Inn == inn)
               .FirstOrDefault();

            if (countragent == null)
            {
                logImport.Warn("Ошибка сопоставления контрагента", $"Не найден контрагент с ИНН {inn} - сопоставляем по имени");
                countragent = ContragentDomain.GetAll()
               .Where(x => x.Name == name)
               .FirstOrDefault();
            }

            if (countragent == null)
            {
                countragent = new Contragent
                {
                    ContragentState = Gkh.Enums.ContragentState.Active,
                    IsSite = false,
                    Name = name,
                    Inn = inn.ToString(),
                    TypeEntrepreneurship = Gkh.Enums.TypeEntrepreneurship.NotSet,
                    ActivityGroundsTermination = Gkh.Enums.GroundsTermination.NotSet
                };
                ContragentDomain.Save(countragent);

                logImport.Warn("Ошибка сопоставления контрагента", $"Не найден контрагент с ИНН {inn} и названием {name} - создали нового с id {countragent.Id}");
            }

            return countragent;
        }

        private BuildContract[] GetBuildContracts(TypeWorkCr typework)
        {
            return BuildContractTypeWorkDomain.GetAll()
                .Where(x => x.TypeWork.Id == typework.Id)
                .Where(x => x.BuildContract.State.FinalState)
                .Select(x => x.BuildContract)
                .ToArray();
        }

        private PerformedWorkAct[] GetWorkAct(TypeWorkCr typework)
        {
            return PerformedWorkActDomain.GetAll()
                .Where(x => x.TypeWorkCr.Id == typework.Id)
                .Where(x => x.State.FinalState)
                .ToArray();
        }

        private TypeWorkCr FindTypeWork(int houseId, string workName, ushort endYear)
        {
            return TypeWorkCrDomain.GetAll()
                .Where(x => x.ObjectCr.RealityObject.Id == houseId)
                .Where(x => x.Work.Name == workName)
                .Where(x => x.YearRepair == endYear)
                .Where(x => x.ObjectCr.ProgramCr != null) //я не знаю, откуда взялись ObjectCr без ProgramCr, но тем не менее, они есть в базе
                .FirstOrDefault();
        }

        private class ImportRecord
        {
            /// <summary>
            /// Номер строки
            /// </summary>
            internal int N;

            /// <summary>
            /// Код МКД
            /// </summary>
            internal int HouseId;

            /// <summary>
            /// Вид работы (услуги)
            /// </summary>
            internal string WorkName;

            /// <summary>
            /// Год завершения работы (услуги)
            /// </summary>
            internal ushort EndYear;

            /// <summary>
            /// в соответствии с заключенными договорами
            /// </summary>
            internal decimal? SumByContract;

            /// <summary>
            /// принято по актам
            /// </summary>
            internal decimal? SumPassed;

            /// <summary>
            /// Объем
            /// </summary>
            internal decimal? Volume;

            /// <summary>
            /// Заказчик работы (услуги) по капитальному ремонту-Наименование
            /// </summary>
            internal string CustomerName;

            /// <summary>
            /// Заказчик работы (услуги) по капитальному ремонту-ИНН
            /// </summary>
            internal long? CustomerInn;

            /// <summary>
            /// Исполнитель работы (услуги) по капитальному ремонту-Наименование
            /// </summary>
            internal string ExecutorName;

            /// <summary>
            /// Исполнитель работы (услуги) по капитальному ремонту-ИНН
            /// </summary>
            internal long? ExecutorInn;

            /// <summary>
            /// Дата заключения договора подряда
            /// </summary>
            internal DateTime? ContractPlannedEndDate;

            /// <summary>
            /// Плановая дата завершения работ (услуг) по договору подряда
            /// </summary>
            internal DateTime? ActDate;

            /// <summary>
            /// Дата подписания акта приемки
            /// </summary>
            internal DateTime? ContractDate;
        }
    }
}
