namespace Bars.Gkh.RegOperator.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.Modules.FIAS;
    using B4.Modules.Reports;
    using B4.Utils;

    using Bars.Gkh.Utils;

    using Gkh.Domain;
    using Gkh.Entities;
    using Gkh.Enums;
    using Entities;
    using Enums;

    public class MkdRoomAbonentReportOld : BasePrintForm
    {
        private long _municipalityId;

        public IDomainService<Room> RoomService { get; set; }
        public IDomainService<RealityObject> RealityObjectService { get; set; }
        public IDomainService<BasePersonalAccount> AccountService { get; set; }
        public IDomainService<IndividualAccountOwner> IndividualAccountService { get; set; }
        public IDomainService<LegalAccountOwner> LegalAccountService { get; set; }
        public IDomainService<Fias> FiasService { get; set; }

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

        public MkdRoomAbonentReportOld() : base(new ReportTemplateBinary(Properties.Resources.MkdRoomAbonentReport))
        {
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var fiasDataByAoGuidDict = FiasService.GetAll()
                .Select(x => new
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
                    x => x.OrderByDescending(y => y.ActStatus)
                          .Select(y => new Tuple<string, string>(y.OffName, y.ShortName))
                          .First());

            var realtyObjects = RealityObjectService.GetAll()
                .Where(x => x.Municipality.Id == _municipalityId)
                .OrderBy(x => x.Municipality.Name)
                .ThenBy(x => x.Address)
                .Select(x => new
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
                .Select(x =>
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

            var legalOwners = LegalAccountService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Contragent.Inn,
                    x.Contragent.Kpp,
                    x.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, arg => arg.First());

            var individualOwners = IndividualAccountService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.BirthDate,
                    x.FirstName,
                    x.Surname,
                    x.SecondName
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, arg => arg.First());

            // Словарь детализации счетов. Ключ: Id квартиры
            var accoountsDict = AccountService.GetAll()
                .Select(x => new
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
                .ToDictionary(x => x.Key, x => x
                    .Select(y =>
                    {
                        var accountDetails = new AccountDetailsProxy
                        {
                            PersonalAccountNum = y.PersonalAccountNum,
                            AccOpenDate =
                                y.OpenDate != DateTime.MinValue ? y.OpenDate.ToString("dd.MM.yyyy") : string.Empty,
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
                        else if (y.OwnerType == PersonalAccountOwnerType.Individual &&
                                 individualOwners.ContainsKey(y.OwnerId))
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

            // Словарь квартир (продублированных по каждому собственнику, если их несколько по квартире). Ключ: Id дома 
            var roomDetailsByRoIdDict = RoomService.GetAll()
                .Where(x => x.RealityObject.Municipality.Id == _municipalityId)
                .Select(x => new
                {
                    x.Id,
                    x.RoomNum,
                    x.Area,
                    x.LivingArea,
                    x.Type,
                    x.OwnershipType,
                    roId = x.RealityObject.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(x => x.Key, x =>
                    {
                        var result = new List<RoomDetailsProxy>();

                        foreach (var roomDetail in x)
                        {
                            var accounts = accoountsDict.ContainsKey(roomDetail.Id)
                                ? accoountsDict[roomDetail.Id]
                                : new List<AccountDetailsProxy> { null };

                            accounts.ForEach(z => result.Add(new RoomDetailsProxy
                            {
                                RoomNum = roomDetail.RoomNum,
                                Area = roomDetail.Area,
                                LivingArea = roomDetail.LivingArea,
                                OwnershipType = roomDetail.OwnershipType,
                                Type = roomDetail.Type,
                                AccountDetails = z
                            }));
                        }

                        return result;
                    });

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            foreach (var realtyObject in realtyObjects)
            {
                // Если в доме нет квартир, выводим только данные по дому
                if (roomDetailsByRoIdDict.ContainsKey(realtyObject.Id))
                {
                    var roomDetailsByRo = roomDetailsByRoIdDict[realtyObject.Id];

                    foreach (var roomDetails in roomDetailsByRo)
                    {
                        FillRow(section, realtyObject, roomDetails);
                    }
                }
                else
                {
                    FillRow(section, realtyObject);
                }
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

        public override string Name
        {
            get { return "Отчет по адресам МКД и помещениям для импорта абонентов (старый)"; }
        }

        public override string Desciption
        {
            get { return "Отчет по адресам МКД и помещениям для импорта абонентов (старый)"; }
        }

        public override string GroupName
        {
            get { return "Региональный фонд"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.MkdRoomAbonentReport"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhRegOp.MkdRoomAbonentReport"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            _municipalityId = baseParams.Params.GetAsId("municipalityId");
        }
    }
}
