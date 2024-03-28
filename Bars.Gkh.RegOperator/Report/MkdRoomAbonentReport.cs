namespace Bars.Gkh.RegOperator.Report
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Properties;
    using Bars.Gkh.StimulReport;
    using Bars.Gkh.Utils;


    /// <summary>
    /// Отчет "Отчёт по помещениям_"
    /// </summary>
    public class MkdRoomAbonentReport : StimulReport, IGeneratedPrintForm
    {
        private long[] muIds;

        private readonly List<StimulRecord> records = new List<StimulRecord>();

        /// <summary>
        ///  Формат печатной формы
        /// </summary>
        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Excel2007; }
        }

        /// <summary>
        /// Домен сервис "Помещение"
        /// </summary>
        public IDomainService<Room> RoomService { get; set; }

        /// <summary>
        /// Домен сервис "Жилой дом"
        /// </summary>
        public IDomainService<RealityObject> RealityObjectService { get; set; }

        /// <summary>
        /// Домен сервис "Лицевой счет"
        /// </summary>
        public IDomainService<BasePersonalAccount> AccountService { get; set; }

        /// <summary>
        /// Домен сервис "Абонент - физ.лицо"
        /// </summary>
        public IDomainService<IndividualAccountOwner> IndividualAccountService { get; set; }

        /// <summary>
        /// Домен сервис "Абонент - юр.лицо"
        /// </summary>
        public IDomainService<LegalAccountOwner> LegalAccountService { get; set; }

        /// <summary>
        /// Домен сервис "ФИАС"
        /// </summary>
        public IDomainService<Fias> FiasService { get; set; }

        public Stream GetTemplate()
        {
            return new MemoryStream(Resources.MkdRoomAbonentReportStimul);
        }

        public void PrepareReport(ReportParams reportParams)
        {
            var realtyObjects = this.GetRealityObjects();

            var roomDetailsByRoIdDict = this.GetRoomDetails();

            //var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            foreach (var realtyObject in realtyObjects)
            {
                // Если в доме нет квартир, выводим только данные по дому
                if (roomDetailsByRoIdDict.ContainsKey(realtyObject.Id))
                {
                    var roomDetailsByRo = roomDetailsByRoIdDict[realtyObject.Id];

                    foreach (var roomDetails in roomDetailsByRo)
                    {
                        this.FillRow(realtyObject, roomDetails);
                    }

                    roomDetailsByRoIdDict.Remove(realtyObject.Id);
                }
                else
                {
                    this.FillRow(realtyObject);
                }
            }

            this.DataSources.Add(new MetaData
            {
                SourceName = "Записи",
                MetaType = nameof(StimulRecord),
                Data = this.records
            });
        }

        /// <summary>
        /// Название
        /// </summary>
        public string Name
        {
            get { return "Отчет по адресам МКД и помещениям для импорта абонентов"; }
        }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string Desciption
        {
            get { return "Отчет по адресам МКД и помещениям для импорта абонентов"; }
        }

        /// <summary>
        /// Имя группы
        /// </summary>
        public string GroupName
        {
            get { return "Региональный фонд"; }
        }

        /// <summary>
        /// Контролер
        /// </summary>
        public string ParamsController
        {
            get { return "B4.controller.report.MkdRoomAbonentReport"; }
        }

        /// <summary>
        /// Права ограничения
        /// </summary>
        public string RequiredPermission
        {
            get { return "Reports.GkhRegOp.MkdRoomAbonentReport"; }
        }

        /// <summary>
        /// Параметры
        /// </summary>
        /// <param name="baseParams">Базовые параметры </param>
        public void SetUserParams(BaseParams baseParams)
        {
            var municipalityId = baseParams.Params.GetAs<string>("municipalityId");
            if (municipalityId != null && !municipalityId.Contains("All"))
            {
                this.muIds = municipalityId.ToLongArray();
            }
        }

        private void FillRow(Section section, RealtyObjectProxy realtyObject, RoomDetailsProxy roomDetails = null)
        {
            section.ДобавитьСтроку();

            section["ID_DOMA"] = realtyObject.Id;
            section["MU"] = realtyObject.MunicipalityName;
            section["TYPE_CITY"] = realtyObject.PlaceShortName;
            section["CITY"] = realtyObject.PlaceName;
            section["TYPE_STREET"] = realtyObject.StreetShortName;
            section["STREET"] = realtyObject.StreetName;
            section["HOUSE_NUM"] = realtyObject.House;
            section["LITER"] = realtyObject.Letter;
            section["KORPUS"] = realtyObject.Housing;
            section["BUILDING"] = realtyObject.Building;

            if (roomDetails == null)
            {
                return;
            }

            section["FLAT_PLACE_NUM"] = roomDetails.RoomNum;
            section["TOTAL_AREA"] = roomDetails.Area;
            section["LIVE_AREA"] = roomDetails.LivingArea;
            section["FLAT_PLACE_TYPE"] = roomDetails.Type.GetEnumMeta().Display;
            section["PROPERTY_TYPE"] = roomDetails.OwnershipType.GetEnumMeta().Display;

            if (roomDetails.AccountDetails == null)
            {
                return;
            }

            var accDetails = roomDetails.AccountDetails;

            section["INN"] = accDetails.Inn;
            section["KPP"] = accDetails.Kpp;
            section["RENTER_NAME"] = accDetails.RenterName;
            section["BILL_TYPE"] = accDetails.BillType;

            section["SURNAME"] = accDetails.OwnerSurname;
            section["NAME"] = accDetails.OwnerName;
            section["LASTNAME"] = accDetails.OwnerSecondName;
            section["DATE"] = accDetails.OwnerBirthDate;

            section["LS_NUM"] = accDetails.PersonalAccountNum;
            section["LS_DATE"] = accDetails.AccOpenDate;
            section["SHARE"] = accDetails.AreaShare;
        }

        private void FillRow(RealtyObjectProxy realtyObject, RoomDetailsProxy roomDetails = null)
        {
            var rec = new StimulRecord();

            try
            {
                rec.ID_DOMA = realtyObject.Id.ToStr();
                rec.MU = realtyObject.MunicipalityName;
                rec.TYPE_CITY = realtyObject.PlaceShortName;
                rec.CITY = realtyObject.PlaceName;
                rec.TYPE_STREET = realtyObject.StreetShortName;
                rec.STREET = realtyObject.StreetName;
                rec.HOUSE_NUM = realtyObject.House;
                rec.LITER = realtyObject.Letter;
                rec.KORPUS = realtyObject.Housing;
                rec.BUILDING = realtyObject.Building;

                if (roomDetails == null)
                {
                    return;
                }

                rec.FLAT_PLACE_NUM = roomDetails.RoomNum;
                rec.TOTAL_AREA = roomDetails.Area.ToString();
                rec.LIVE_AREA = roomDetails.LivingArea.ToString();
                rec.FLAT_PLACE_TYPE = roomDetails.Type.GetEnumMeta().Display;
                rec.PROPERTY_TYPE = roomDetails.OwnershipType.GetEnumMeta().Display;

                if (roomDetails.AccountDetails == null)
                {
                    return;
                }

                var accDetails = roomDetails.AccountDetails;
                rec.INN = accDetails.Inn;
                rec.KPP = accDetails.Kpp;
                rec.RENTER_NAME = accDetails.RenterName;
                rec.BILL_TYPE = accDetails.BillType;

                rec.SURNAME = accDetails.OwnerSurname;
                rec.NAME = accDetails.OwnerName;
                rec.LASTNAME = accDetails.OwnerSecondName;
                rec.DATE = accDetails.OwnerBirthDate.ToDateTime();

                rec.LS_NUM = accDetails.PersonalAccountNum;
                rec.LS_DATE = accDetails.AccOpenDate.ToDateTime();
                rec.SHARE = accDetails.AreaShare.ToDecimal();
            }
            finally
            {
                this.records.Add(rec);
            }
        }

        private Dictionary<long, List<AccountDetailsProxy>> GetAccountDetails()
        {
            var legalOwners = this.LegalAccountService.GetAll()
                .Select(
                    x => new
                    {
                        x.Id,
                        x.Contragent.Inn,
                        x.Contragent.Kpp,
                        x.Name
                    })
                .AsEnumerable()
                .ToDictionary(x => x.Id);

            var individualOwners = this.IndividualAccountService.GetAll()
                .Select(
                    x => new
                    {
                        x.Id,
                        x.BirthDate,
                        x.FirstName,
                        x.Surname,
                        x.SecondName
                    })
                .AsEnumerable()
                .ToDictionary(x => x.Id);

            // Словарь детализации счетов. Ключ: Id квартиры
            var accountsDict = this.AccountService.GetAll()
                .Select(
                    x => new
                    {
                        x.Room.Id,
                        OwnerId = x.AccountOwner.Id,
                        x.AreaShare,
                        x.AccountOwner.Name,
                        x.PersonalAccountNum,
                        x.AccountOwner.OwnerType,
                        x.OpenDate
                    })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    x => x
                        .Select(
                            y =>
                            {
                                var accountDetails = new AccountDetailsProxy
                                {
                                    PersonalAccountNum = y.PersonalAccountNum,
                                    AccOpenDate = y.OpenDate != DateTime.MinValue
                                        ? y.OpenDate.ToString("dd.MM.yyyy")
                                        : string.Empty,
                                    AreaShare = y.AreaShare.ToStr()
                                };

                                if (y.OwnerType == PersonalAccountOwnerType.Legal && legalOwners.ContainsKey(y.OwnerId))
                                {
                                    var legalOwner = legalOwners[y.OwnerId];

                                    accountDetails.Inn = legalOwner.Inn;
                                    accountDetails.Kpp = legalOwner.Kpp;
                                    accountDetails.RenterName = legalOwner.Name;
                                    accountDetails.BillType = "Счет юр.лица";
                                }
                                else if (y.OwnerType == PersonalAccountOwnerType.Individual
                                    && individualOwners.ContainsKey(y.OwnerId))
                                {
                                    var indivOwner = individualOwners[y.OwnerId];

                                    accountDetails.OwnerSurname = indivOwner.Surname;
                                    accountDetails.OwnerName = indivOwner.FirstName;
                                    accountDetails.OwnerSecondName = indivOwner.SecondName;
                                    accountDetails.OwnerBirthDate = indivOwner.BirthDate.IsValid()
                                        ? indivOwner.BirthDate.Value.ToString("dd.MM.yyyy")
                                        : string.Empty;

                                    accountDetails.BillType = "Счет физ.лица";
                                }

                                return accountDetails;
                            })
                        .ToList());

            legalOwners.Clear();
            individualOwners.Clear();

            return accountsDict;
        }

        private IEnumerable<RealtyObjectProxy> GetRealityObjects()
        {
            var fiasDataByAoGuidDict = this.FiasService.GetAll()
                .Select(
                    x => new
                    {
                        x.AOGuid,
                        x.ShortName,
                        x.OffName,
                        x.ActStatus
                    })
                .AsEnumerable()
                .GroupBy(x => x.AOGuid)
                .ToDictionary(
                    x => x.Key,
                    x => x
                        .OrderByDescending(y => y.ActStatus)
                        .Select(y => new Tuple<string, string>(y.OffName, y.ShortName))
                        .First());

            var realtyObjects = this.RealityObjectService.GetAll()
                .WhereIf(this.muIds != null, x => this.muIds.Contains(x.Municipality.Id))
                .OrderBy(x => x.Municipality.Name)
                .ThenBy(x => x.Address)
                .Select(
                    x => new
                    {
                        x.Id,
                        MunicipalityName = x.Municipality.Name,
                        x.FiasAddress.House,
                        x.FiasAddress.Housing,
                        x.FiasAddress.Building,
                        x.FiasAddress.Letter,
                        x.FiasAddress.StreetGuidId,
                        x.FiasAddress.PlaceGuidId
                    })
                .AsEnumerable()
                .Select(
                    x =>
                    {
                        var realtyObject = new RealtyObjectProxy
                        {
                            Id = x.Id,
                            MunicipalityName = x.MunicipalityName,
                            House = x.House,
                            Housing = x.Housing,
                            Letter = x.Letter,
                            Building = x.Building
                        };

                        if (!string.IsNullOrWhiteSpace(x.StreetGuidId) && fiasDataByAoGuidDict.ContainsKey(x.StreetGuidId))
                        {
                            var data = fiasDataByAoGuidDict[x.StreetGuidId];

                            realtyObject.StreetName = data.Item1;
                            realtyObject.StreetShortName = data.Item2;
                        }

                        if (!string.IsNullOrWhiteSpace(x.PlaceGuidId) && fiasDataByAoGuidDict.ContainsKey(x.PlaceGuidId))
                        {
                            var data = fiasDataByAoGuidDict[x.PlaceGuidId];

                            realtyObject.PlaceName = data.Item1;
                            realtyObject.PlaceShortName = data.Item2;
                        }

                        return realtyObject;
                    })
                .ToArray();

            fiasDataByAoGuidDict.Clear();

            return realtyObjects;
        }

        private Dictionary<long, List<RoomDetailsProxy>> GetRoomDetails()
        {
            var accountsDict = this.GetAccountDetails();

            // Словарь квартир (продублированных по каждому собственнику, если их несколько по квартире). Ключ: Id дома 
            var roomDetailsByRoIdDict = this.RoomService.GetAll()
                .WhereIf(this.muIds != null, x => this.muIds.Contains(x.RealityObject.Municipality.Id))
                .Select(
                    x => new RoomDetailsProxy
                    {
                        RoomNum = x.RoomNum,
                        Id = x.Id,
                        Area = x.Area,
                        LivingArea = x.LivingArea,
                        Type = x.Type,
                        OwnershipType = x.OwnershipType,
                        RoId = x.RealityObject.Id
                    })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(
                    x => x.Key,
                    x =>
                    {
                        var result = new List<RoomDetailsProxy>();

                        foreach (var roomDetail in x)
                        {
                            var accounts = accountsDict.ContainsKey(roomDetail.Id)
                                ? accountsDict[roomDetail.Id]
                                : new List<AccountDetailsProxy> {null};

                            accounts.ForEach(
                                z =>
                                {
                                    var roomDetailCopy = new RoomDetailsProxy()
                                    {
                                        Id = roomDetail.Id,
                                        Area = roomDetail.Area,
                                        LivingArea = roomDetail.LivingArea,
                                        RoId = roomDetail.RoId,
                                        OwnershipType = roomDetail.OwnershipType,
                                        RoomNum = roomDetail.RoomNum,
                                        Type = roomDetail.Type,
                                        AccountDetails = z
                                    };
                                    result.Add(roomDetailCopy);
                                });
                        }

                        return result;
                    });

            return roomDetailsByRoIdDict;
        }

        private sealed class RealtyObjectProxy
        {
            public long Id;

            public string MunicipalityName;

            public string PlaceName;

            public string StreetName;

            public string House;

            public string Housing;

            public string Building;

            public string Letter;

            public string StreetShortName;

            public string PlaceShortName;
        }

        private sealed class RoomDetailsProxy
        {
            public long Id;

            public long RoId;

            public string RoomNum;

            public decimal Area;

            public decimal? LivingArea;

            public RoomType Type;

            public RoomOwnershipType OwnershipType;

            public AccountDetailsProxy AccountDetails;
        }

        private sealed class AccountDetailsProxy
        {
            public string Inn;

            public string Kpp;

            public string RenterName;

            public string BillType;

            public string OwnerSurname;

            public string OwnerName;

            public string OwnerSecondName;

            public string OwnerBirthDate;

            public string PersonalAccountNum;

            public string AccOpenDate;

            public string AreaShare;
        }

        private sealed class StimulRecord
        {
            public string ID_DOMA { get; set; }

            public string MU { get; set; }

            public string TYPE_CITY { get; set; }

            public string CITY { get; set; }

            public string STREET { get; set; }

            public string HOUSE_NUM { get; set; }

            public string LITER { get; set; }

            public string KORPUS { get; set; }

            public string BUILDING { get; set; }

            public string BILL_TYPE { get; set; }

            public string FLAT_PLACE_NUM { get; set; }

            public string TYPE_STREET { get; set; }

            public string TOTAL_AREA { get; set; }

            public string LIVE_AREA { get; set; }

            public string FLAT_PLACE_TYPE { get; set; }

            public string PROPERTY_TYPE { get; set; }

            public string SURNAME { get; set; }

            public string NAME { get; set; }

            public string LASTNAME { get; set; }

            public string INN { get; set; }

            public string KPP { get; set; }

            public string RENTER_NAME { get; set; }

            public decimal SHARE { get; set; }

            public DateTime DATE { get; set; }

            public string LS_NUM { get; set; }

            public DateTime LS_DATE { get; set; }
        }
    }
}