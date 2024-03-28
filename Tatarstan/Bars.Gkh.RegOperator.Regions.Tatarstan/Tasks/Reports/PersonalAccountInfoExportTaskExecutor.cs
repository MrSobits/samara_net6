namespace Bars.Gkh.RegOperator.Tasks.Reports
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Microsoft.Extensions.Logging;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Modules.Tasks.Common.Utils;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.Entities;
    using Bars.GkhRf.Entities;
    using Castle.Windsor;
    using Ionic.Zip;
    using Ionic.Zlib;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading;

    using Microsoft.Extensions.Logging;

    using ExecutionContext = B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Исполнитель задачи выгрузки информации по ЛС
    /// </summary>
    public class PersonalAccountInfoExportTaskExecutor : ITaskExecutor
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode => PersonalAccountInfoExportTaskExecutor.Id;

        /// <summary>
        /// Метод для выполнения задачи
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <param name="ctx">Контекст выполнения</param>
        /// <param name="indicator">Индикатор прогресса</param>
        /// <param name="ct">Обработчик отмены</param>
        /// <returns>Результат выполнения</returns>
        public IDataResult Execute(BaseParams baseParams, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            // Получение домен-сервисов для наших сущностей
            var chargeDomain = this.Container.ResolveDomain<ChargePeriod>();
            var municipalityDomain = this.Container.ResolveDomain<Municipality>();
            var transAllDomain = this.Container.ResolveDomain<TransferObject>();
            var contractRfObjectDomain = this.Container.ResolveDomain<ContractRfObject>();
            var fiasCacheDomain = this.Container.ResolveDomain<Fias>();
            // Ситуация по ЛС на период
            var summaryDomain = this.Container.ResolveDomain<PersonalAccountPeriodSummary>();
            var realityDomain = this.Container.ResolveDomain<RealityObject>();
            var transHireDomain = this.Container.ResolveDomain<TransferHire>();
            var fileManager = Container.Resolve<IFileManager>();
            try
            {
                indicator?.Indicate(null, 0, "Идет предварительная обработка...");

                var zipStream = new MemoryStream();
                var fileZip = new ZipFile(Encoding.GetEncoding("utf-8"))
                {
                    CompressionLevel = CompressionLevel.Level3,
                    AlternateEncoding = Encoding.GetEncoding("utf-8")
                };

                var periodId = baseParams.Params.GetAs<long>("chargePeriodId");
                var municipalityIds = baseParams.Params.GetAs<long[]>("municipalityIds") ?? new long[0];
                var municipalityName = baseParams.Params.GetAs<string>("municipalityName");
                if (municipalityIds.Length > 1)
                    municipalityName = string.Format("{0} районов", municipalityIds.Length);

                var period = chargeDomain.Get(periodId);

                // Муниципальные образования (если не одно)
                var municipality = municipalityDomain.GetAll()
                    .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Id));

                var transAll = transAllDomain.GetAll()
                    .Where(x => municipality.Any(m => m.Id == x.RealityObject.Municipality.Id))
                    .Where(x => (x.TransferRecord.TransferDate >= period.StartDate || period.StartDate == null) && (x.TransferRecord.TransferDate <= period.EndDate || period.EndDate == null))
                    .Where(x => x.TransferRecord.State.Id == 22)
                    .Where(x => x.TransferredSum != null)
                    .GroupBy(x => new { RealityObjectId = x.RealityObject.Id, id = x.TransferRecord.Id })
                    .Select(b => new
                    {
                        id = b.Key.id,
                        RealityObjectId = b.Key.RealityObjectId,
                        TransferSum = b.Sum(x => x.TransferredSum.Value)
                    })
                    .ToList();

                // Трансферы по домам
                var transRO = transAll.GroupBy(x => x.RealityObjectId)
                .Select(b => new
                {
                    RealityObjectId = b.Key,
                    TransferSum = b.Sum(x => x.TransferSum)
                })
                .ToList()
                .ToDictionary(x => x.RealityObjectId, y => y);

                var documentDate = new DateTime(2014, 1, 1);

                // Контрагенты
                var contractRfObject = contractRfObjectDomain.GetAll()
                .Where(x => x.ContractRf.DocumentDate >= documentDate)
                .OrderByDescending(x => x.ContractRf.DocumentDate)
                .ThenBy(x => x.ContractRf.ManagingOrganization.Contragent.ContragentState)
                .Select(x => new
                {
                    RealityObjectId = x.RealityObject.Id,
                    ContragentName = x.ContractRf.ManagingOrganization.Contragent.Name.Replace("\"", ""),
                    ContragentID = x.ContractRf.ManagingOrganization.Contragent.Id
                })
                .ToList();

                // Дома которые нужно обрабатывать, только id
                var realityIDs = summaryDomain.GetAll()
                    .Where(x => x.Period.Id == periodId)
                    .Where(x => municipality.Any(m => m.Id == x.PersonalAccount.Room.RealityObject.Municipality.Id))
                    .Select(x => new
                    {
                        id = x.PersonalAccount.Room.RealityObject.Id
                    })
                    .Distinct()
                    .ToList();

                // Все дома по району
                var reality = realityDomain.GetAll()
                    .Where(x => x.ConditionHouse == ConditionHouse.Serviceable)
                    .Where(x => municipality.Any(m => m.Id == x.Municipality.Id))
                    .Select(x => new
                    {
                        ObjectID = x.Id,
                        x.FiasAddress.HouseGuid,
                        MunicipalityName = x.Municipality.Name,
                        MoSettlementName = x.MoSettlement.Name,
                        City = x.FiasAddress.PlaceName,
                        Street = x.FiasAddress.StreetName,
                        House = x.FiasAddress.House,
                        Liter = x.FiasAddress.Letter,
                        Housing = x.FiasAddress.Housing,
                        Building = x.FiasAddress.Building,
                    })
                    .AsEnumerable()
                    .Where(x => realityIDs.Any(i => i.id == x.ObjectID))
                    .Select(x => new RealityObjectItem()
                    {
                        ObjectID = x.ObjectID,
                        Contragent = contractRfObject
                            .Where(c => c.RealityObjectId == x.ObjectID)
                            .Select(c => new ContragentItem()
                            {
                                ID = c.ContragentID,
                                Name = c.ContragentName
                            })
                            .FirstOrDefault(),
                        FiasID = x.HouseGuid,
                        MunicipalityName = x.MunicipalityName,
                        MoSettlementName = x.MoSettlementName,
                        City = x.City,
                        Street = x.Street,
                        House = x.House,
                        Liter = x.Liter,
                        Housing = x.Housing,
                        Building = x.Building,
                        ContractSent = transRO.Count > 0 && !transRO.Get(x.ObjectID).IsNull() ? transRO.Get(x.ObjectID).TransferSum : 0.0m,
                        Month = period.StartDate.ToString("d"),
                        Year = period.StartDate.ToString("yyyy"),
                    })
                    .OrderBy(x => x.ObjectID)
                    .ToList();

                // Счетчик домов
                int realityIndex = 0;

                // Перебор домов
                foreach (var ro in reality)
                {
                    var transIds = transAll
                    .Where(x => x.RealityObjectId == ro.ObjectID)
                    .Select(x => new
                    {
                        id = x.id
                    })
                    .Distinct()
                    .ToDictionary(x => x.id, y => y);

                    // Перечисления по найму, это нужно для Татарстана
                    var transHire = transHireDomain.GetAll()
                        .Where(x => x.Account.Room.RealityObject.Id == ro.ObjectID)
                        .Select(x => new
                        {
                            RecId = x.TransferRecord.Id,
                            AccId = x.Account.Id,
                            TransferedSum = x.TransferredSum
                        })
                        .ToList()
                        .Where(x => transIds.Any(i => i.Key == x.RecId));

                    var summaries = summaryDomain.GetAll()
                        .Where(x => x.PersonalAccount.Room.RealityObject.Id == ro.ObjectID)
                        .Where(x => x.Period.Id == periodId)
                        .Select(x => new PersonalAccountItem()
                        {
                            PersonalAcc = x.PersonalAccount.PersonalAccountNum,
                            RoomNum = x.PersonalAccount.Room.RoomNum,
                            ChargedSum = x.ChargeTariff,
                            ChargedPeni = x.Penalty,
                            Square = x.PersonalAccount.Room.Area * x.PersonalAccount.AreaShare,
                            Paid = x.TariffPayment + x.PenaltyPayment,
                            PaidPeni = x.PenaltyPayment,
                            OpeningBalance = x.SaldoIn,
                            OutgoingBalance = x.SaldoOut,
                            Payable = x.SaldoIn + x.ChargeTariff - (x.TariffPayment + x.PenaltyPayment),
                            Tariff = x.PersonalAccount.Tariff == 0 ? 5 : x.PersonalAccount.Tariff,
                            ServiceType = (int)x.PersonalAccount.ServiceType,
                            RecalculationSumm = x.RecalcByBaseTariff,
                            NonLiving = x.PersonalAccount.Room.Type == RoomType.Living ? false : true,
                            AccId = x.PersonalAccount.Id,
                            ObjectID = x.PersonalAccount.Room.RealityObject.Id,
                            OwnerShip = (int)x.PersonalAccount.Room.OwnershipType
                        })
                        .AsEnumerable()
                        .OrderBy(x => x.AccId)
                        .ToList();

                    // Подсчитать перечисления
                    var ContractSent = transHire
                        .Where(x => transIds.Any(i => i.Key == x.RecId))
                        .Where(x => summaries.First().AccId == x.AccId)
                        .Select(x => new
                        {
                            TransferedSum = x.TransferedSum
                        }).
                        Sum(x => x.TransferedSum) +
                        ro.ContractSent;

                    // Внести изменения по перечислению
                    ro.ContractSent = ContractSent;

                    // Добавить в список ЛС
                    ro.Accounts = new List<PersonalAccountItem>();
                    ro.Accounts.AddRange(summaries);

                    // Индикация, сколько выполнено
                    if (indicator != null)
                    {
                        realityIndex++;
                        var percent = realityIndex * 100m / reality.Count;
                        indicator.Indicate(null, (uint)percent, string.Format("Обработано {0} из {1} домов", realityIndex, reality.Count));
                    }
                }

                // Сформировать окончательную выборку
                var reportOutput = reality.Select(x => new RealityObjectProxy()
                {
                    ObjectID = x.ObjectID.ToString(),
                    ContragentName = x.Contragent.IsNull() ? "" : x.Contragent.Name,
                    ContragentID = x.Contragent.IsNull() ? "" : x.Contragent.ID.ToString(),
                    FiasID = x.FiasID.HasValue ? x.FiasID.ToString() : "",
                    MunicipalityName = x.MunicipalityName,
                    MoSettlementName = x.MoSettlementName.IsNull() ? "" : x.MoSettlementName,
                    City = x.City,
                    Street = x.Street,
                    House = x.House,
                    Liter = x.Liter.IsNull() ? "" : x.Liter,
                    Housing = x.Housing.IsNull() ? "" : x.Housing,
                    Building = x.Building.IsNull() ? "" : x.Building,
                    ContractSent = x.ContractSent.ToString("0.00", CultureInfo.InvariantCulture),
                    Month = x.Month,
                    Year = x.Year,
                    Accounts = x.Accounts
                        .AsEnumerable()
                        .Select(p => new PersonalAccountProxy()
                        {
                            PersonalAcc = p.PersonalAcc,
                            RoomNum = p.RoomNum,
                            ChargedSum = p.ChargedSum.ToString("0.00", CultureInfo.InvariantCulture),
                            ChargedPeni = p.ChargedPeni.ToString("0.00", CultureInfo.InvariantCulture),
                            Square = p.Square.ToString("0.00", CultureInfo.InvariantCulture),
                            Paid = p.Paid.ToString("0.00", CultureInfo.InvariantCulture),
                            PaidPeni = p.PaidPeni.ToString("0.00", CultureInfo.InvariantCulture),
                            OpeningBalance = p.OpeningBalance.ToString("0.00", CultureInfo.InvariantCulture),
                            OutgoingBalance = p.OutgoingBalance.ToString("0.00", CultureInfo.InvariantCulture),
                            Payable = p.Payable.ToString("0.00", CultureInfo.InvariantCulture),
                            Tariff = p.Tariff.ToString(),
                            ServiceType = p.ServiceType.ToString(),
                            RecalculationSumm = p.RecalculationSumm.ToString("0.00", CultureInfo.InvariantCulture),
                            NonLiving = p.NonLiving.ToString().ToLower(),
                            OwnerShip = ((OwnershipType)(Enum.IsDefined(typeof(OwnershipType), p.OwnerShip) ? p.OwnerShip : 0)
                                ).GetAttribute<DisplayAttribute>().Value
                        }).ToList()
                });

                // Сериализовать в json-строку
                var mStream = JsonConvert.SerializeObject(reportOutput, Formatting.Indented);

                // Добавить точку входа для Zip-файла
                if (mStream != null)
                {
                    fileZip.AddEntry("export.txt", mStream, Encoding.UTF8);
                }

                // Сохранить файл потока в специальный тип zip архива 
                fileZip.Save(zipStream);

                using (Container.Using(fileManager))
                {
                    var file = fileManager.SaveFile(zipStream, string.Format("Информация по ЛС ({0}).zip", municipalityName));
                    return new BaseDataResult(file.Id);
                }
            }
            catch (Exception e)
            {
                return BaseDataResult.Error(" message: {0} \r\n stacktrace: {1}".FormatUsing(e.Message, e.StackTrace));
            }
            finally
            {
                this.Container.Release(chargeDomain);
                this.Container.Release(municipalityDomain);
                this.Container.Release(transAllDomain);
                this.Container.Release(contractRfObjectDomain);
                this.Container.Release(fiasCacheDomain);
                this.Container.Release(summaryDomain);
                this.Container.Release(realityDomain);
                this.Container.Release(transHireDomain);
                this.Container.Release(fileManager);
            }
        }

        /// <summary>
        /// Информация по ЛС (для предварительной выборки)
        /// </summary>
        private class PersonalAccountItem
        {
            public string PersonalAcc { get; set; }
            public string RoomNum { get; set; }
            public decimal ChargedSum { get; set; }
            public decimal ChargedPeni { get; set; }
            public decimal Square { get; set; }
            public decimal Paid { get; set; }
            public decimal PaidPeni { get; set; }
            public decimal OpeningBalance { get; set; }
            public decimal OutgoingBalance { get; set; }
            public decimal Payable { get; set; }
            public decimal Tariff { get; set; }
            public int ServiceType { get; set; }
            public decimal RecalculationSumm { get; set; }
            public bool NonLiving { get; set; }
            public long AccId { get; set; }
            public long ObjectID { get; set; }
            public int OwnerShip { get; set; }
        }

        /// <summary>
        /// Контрагент (для предварительной выборки)
        /// </summary>
        private class ContragentItem
        {
            public long ID { get; set; }
            public string Name { get; set; }
        }

        /// <summary>
        /// Информация по дому (для предварительной выборки)
        /// </summary>
        private class RealityObjectItem
        {
            public long ObjectID { get; set; }
            public ContragentItem Contragent { get; set; }
            public Guid? FiasID { get; set; }
            public string MunicipalityName { get; set; }
            public string MoSettlementName { get; set; }
            public string City { get; set; }
            public string Street { get; set; }
            public string House { get; set; }
            public string Liter { get; set; }
            public string Housing { get; set; }
            public string Building { get; set; }
            public decimal ContractSent { get; set; }
            public string Month { get; set; }
            public string Year { get; set; }
            public List<PersonalAccountItem> Accounts { get; set; }
        }

        /// <summary>
        /// Информация по ЛС (для выгрузки в json)
        /// </summary>
        [JsonObject]
        private class PersonalAccountProxy
        {
            [JsonProperty("personal_acc")]
            public string PersonalAcc { get; set; }
            [JsonProperty("room_num")]
            public string RoomNum { get; set; }
            [JsonProperty("charged_sum")]
            public string ChargedSum { get; set; }
            [JsonProperty("charged_peni")]
            public string ChargedPeni { get; set; }
            [JsonProperty("square")]
            public string Square { get; set; }
            [JsonProperty("paid")]
            public string Paid { get; set; }
            [JsonProperty("paid_peni")]
            public string PaidPeni { get; set; }
            [JsonProperty("opening_balance")]
            public string OpeningBalance { get; set; }
            [JsonProperty("outgoing_balance")]
            public string OutgoingBalance { get; set; }
            [JsonProperty("payable")]
            public string Payable { get; set; }
            [JsonProperty("tariff")]
            public string Tariff { get; set; }
            [JsonProperty("service")]
            public string ServiceType { get; set; }
            [JsonProperty("recalculation_summ")]
            public string RecalculationSumm { get; set; }
            [JsonProperty("NonLiving")]
            public string NonLiving { get; set; }
            [JsonProperty("ownership")]
            public string OwnerShip { get; set; }
        }

        /// <summary>
        /// Информация по дому (для выгрузки в json)
        /// </summary>
        [JsonObject]
        private class RealityObjectProxy
        {
            [JsonProperty("object_ID")]
            public string ObjectID { get; set; }
            [JsonProperty("uk")]
            public string ContragentName { get; set; }
            [JsonProperty("uk_ID")]
            public string ContragentID { get; set; }
            [JsonProperty("FiasID")]
            public string FiasID { get; set; }
            [JsonProperty("mr")]
            public string MunicipalityName { get; set; }
            [JsonProperty("mu")]
            public string MoSettlementName { get; set; }
            [JsonProperty("city")]
            public string City { get; set; }
            [JsonProperty("street")]
            public string Street { get; set; }
            [JsonProperty("house")]
            public string House { get; set; }
            [JsonProperty("liter")]
            public string Liter { get; set; }
            [JsonProperty("housing")]
            public string Housing { get; set; }
            [JsonProperty("building")]
            public string Building { get; set; }
            [JsonProperty("contract_sent")]
            public string ContractSent { get; set; }
            [JsonProperty("month")]
            public string Month { get; set; }
            [JsonProperty("year")]
            public string Year { get; set; }
            [JsonProperty("Personal")]
            public List<PersonalAccountProxy> Accounts { get; set; }
        }

        /// <summary>
        /// Тип собственности помещения (используется только для экспорта)
        /// </summary>
        private enum OwnershipType
        {
            /// <summary>
            /// Частная
            /// </summary>
            [Display("individual")]
            Private = 10,

            /// <summary>
            /// Муниципальная
            /// </summary>
            [Display("mo")]
            Municipal = 30,

            /// <summary>
            /// Федеральная
            /// </summary>
            [Display("fed ")]
            Federal = 80,

            /// <summary>
            /// Областная
            /// </summary>
            [Display("rt")]
            Regional = 90,

            /// <summary>
            /// Другие
            /// </summary>
            [Display("")]
            Other = 0
        }
    }
}
