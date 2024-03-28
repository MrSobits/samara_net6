namespace Bars.Gkh.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Services.DataContracts;
    using Bars.Gkh.Services.DataContracts.GetHousesInfoByEditDate;
    using Bars.B4;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities.RealityObj;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.Gkh.Utils;
    using Bars.B4.DataAccess;
    using NHibernate.Transform;

    public partial class Service
    {

        private readonly Dictionary<string, string> gasTypesDictED = new Dictionary<string, string>
        {
            {"1", "Центральное" },
            {"2", "Нецентральное" },
            {"3", "Отсутствует" }
        };

        private readonly Dictionary<string, string> firefightingTypesDictED = new Dictionary<string, string>
        {
            {"0", "Не заполнено" },
            {"1", "Отсутствует" },
            {"2", "Автоматическая" },
            {"3", "Пожарные гидранты" }
        };

        private readonly Dictionary<string, string> ventilationTypeDictED = new Dictionary<string, string>
        {
            {"1", "Приточная вентиляция" },
            {"2", "Вытяжная вентиляция" },
            {"3", "Приточно-вытяжная вентиляция" },
            {"4", "Отсутствует"}
        };

        public GetHousesInfoByEditDateResponse GetHousesInfoByEditDate(string date)
        {
            DateTime dateDt = Convert.ToDateTime(date);
            var ids = RealObjDomainService.GetAll().Where(x => x.ObjectCreateDate>= dateDt).Select(x => x.Id).ToArray();

            var housesInfo = this.GetHouseInfoByEditDate(ids);

            var result = housesInfo.Length == 0 ? Result.DataNotFound : Result.NoErrors;

            return new GetHousesInfoByEditDateResponse { HouseInfoByEditDate = housesInfo, Result = result };
        }

        private HouseInfoByEditDate[] GetHouseInfoByEditDate(long[] ids)
        {
            var ownersCount = this.OwnersService?.GetOwnersCount(ids);
            var perfomedWorkActIntegration = this.PerfomedWorkActIntegrationService.GetPerfomedWorkActProxies(ids);

            var regOpName = this.RegOperatorDomainService.GetAll().Select(x => x.Contragent.Name).FirstOrDefault();

            IRealObjOverhaulDataService realObjOverhaulDataService = null;

            var roIdBuildingFeatureList = this.RealityObjectBuildingFeatureDomain
                .GetAll()
                .Where(y => ids.Contains(y.RealityObject.Id))
                .Where(y => y.BuildingFeature.Code == "1")
                .GroupBy(y => y.RealityObject.Id)
                .Select(y => y.Key)
                .ToList();

            if (this.Container.Kernel.HasComponent(typeof(IRealObjOverhaulDataService)))
            {
                realObjOverhaulDataService = this.Container.Resolve<IRealObjOverhaulDataService>();
            }

            IRealityObjectRealEstateTypeService realObjRealEstTypeService = null;
            if (this.Container.Kernel.HasComponent(typeof(IRealityObjectRealEstateTypeService)))
            {
                realObjRealEstTypeService = this.Container.Resolve<IRealityObjectRealEstateTypeService>();
            }

            var ci = CultureInfo.InvariantCulture.Clone() as CultureInfo;
            NumberFormatInfo numberformat = null;
            if (ci != null)
            {
                ci.NumberFormat.NumberDecimalSeparator = ".";
                numberformat = ci.NumberFormat;
            }



            try
            {
                string roids = "";
                foreach (var str in ids)
                {
                    roids += str + ",";
                }
                roids = roids.Substring(0, roids.Length - 1);

                //собираем словарь сальдо
                var saldoDict = new Dictionary<long, decimal>();

                var session = this.Container.Resolve<ISessionProvider>().OpenStatelessSession();

                var sql = $@"select ro_id, debt_total - credit_total as saldo from regop_ro_payment_account where ro_id in ({roids})";

                var SaldoResponceList = session.CreateSQLQuery(sql)
                    .SetResultTransformer(Transformers.AliasToBean<SaldoResponce>())
                    .List<SaldoResponce>();

                foreach (var str in SaldoResponceList)
                {
                    if (!saldoDict.ContainsKey(str.ro_id))
                    {
                        saldoDict.Add(str.ro_id, Decimal.Round(str.saldo, 2));
                    }
                }

                Container.Release(session);

                var houses = this.RealObjDomainService
                .GetAll()
                .Where(x => ids.Contains(x.Id))
                .AsEnumerable()
                .Select(x =>
                    {
                        var isAutoRealEstateType = false;
                        var roTypes = string.Empty;

                        if (realObjRealEstTypeService != null)
                        {
                            var result = realObjRealEstTypeService.GetAutoRealEstateType(x);
                            isAutoRealEstateType = result.Success;
                            roTypes = result.Data.ToStr();
                        }

                        return new HouseInfoByEditDate
                        {
                            Id = x.Id,
                            MUnion = x.Municipality.Name,
                            Settlement = x.MoSettlement != null ? x.MoSettlement.Name : string.Empty,
                            BuildYear = x.BuildYear ?? 0,
                            ExplYear = x.DateCommissioning?.Year ?? 0,
                            FlatNumber = x.NumberApartments.ToInt(),
                            Floor = x.MaximumFloors.ToInt(),
                            LiveArea = x.AreaLiving?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                            LiveUnliveArea = x.AreaLivingNotLivingMkd?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                            OfWearPers = x.PhysicalWear?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                            PeopleLivingArea = x.AreaLivingOwned?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                            TotalArea = x.AreaMkd?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                            RoofMaterial = x.RoofingMaterial?.Name,
                            RoofType = x.TypeRoof.GetEnumMeta().Display,
                            WallMaterial = x.WallMaterial?.Name ?? string.Empty,
                            Residents = x.NumberLiving.ToInt(),
                            Address = x.Address,
                            AddressFias = x.FiasAddress.AddressName,
                            NumberLiving = x.NumberLiving ?? 0,
                            MinFloor = x.Floors?.ToString() ?? string.Empty,
                            PrivatRoom = x.HasPrivatizedFlats.HasValue && x.HasPrivatizedFlats.Value ? "Да" : "Нет",
                            Data1PrivatRoom = x.PrivatizationDateFirstApartment?.ToShortDateString() ?? string.Empty,
                            NeedKapRem = x.NecessaryConductCr.GetEnumMeta().Display,
                            LastKapRemDate = x.DateLastOverhaul?.ToShortDateString() ?? string.Empty,
                            ProjectTip = x.TypeProject?.Name ?? string.Empty,
                            CadastrNumber = x.CadastreNumber,
                            CadastralHouseNumber = x.CadastralHouseNumber,
                            StaircaseNum = x.NumberEntrances?.ToString() ?? string.Empty,
                            LiftNum = x.NumberLifts?.ToString() ?? string.Empty,
                            HeatingSystem = x.HeatingSystem.GetEnumMeta().Display,
                            FloorHeight = x.FloorHeight?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                            AreaCleaning = x.AreaCleaning?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                            PublicArea = x.AreaCommonUsage?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                            AreaNonResidental = x.AreaNotLivingPremises.ToDecimal(),
                            EntranceCount = x.NumberEntrances.ToInt(),
                            FormingOverhaulFund = x.AccountFormationVariant.ToInt(),
                            HouseType = x.TypeHouse.GetDisplayName(),
                            BuiltYear = x.BuildYear ?? 0,
                            PeopleNotLivingArea = x.AreaNotLivingPremises ?? 0,
                            GeneralArea = (x.AreaNotLivingFunctional ?? 0).RoundDecimal(2),
                            FlatsCount = (x.NumberApartments ?? 0) + (x.NumberNonResidentialPremises ?? 0),
                            NotLivingQuartersCount = x.NumberNonResidentialPremises ?? 0,
                            FloorCountMax = x.MaximumFloors ?? 0,
                            FloorCountMin = x.Floors ?? 0,
                            HouseState = x.ConditionHouse.GetDisplayName(),
                            OverhaulProgramShip = !x.IsNotInvolvedCr,
                            DateOfInclusionInProgram = (realObjOverhaulDataService?.GetPublishDateByRo(x)).ToDateString(),
                            DateOfExclusion = x.UnpublishDate.ToDateString(),
                            TechnicalSurveyDate = x.LatestTechnicalMonitoring.ToDateString(),
                            MunicipalArea = x.AreaMunicipalOwned?.RoundDecimal(2).ToStr(),
                            PublicalArea = x.AreaGovernmentOwned?.RoundDecimal(2).ToStr(),
                            SpecialMark = roIdBuildingFeatureList.Contains(x.Id) ? "YES" : "NO",
                            HouseClassification = isAutoRealEstateType ? roTypes : x.RealEstateType?.Name,
                            DemolitionDate = x.DateDemolition.ToDateString(),
                            Monument = x.IsCulturalHeritage,
                            CRSaldo = saldoDict.ContainsKey(x.Id) ? saldoDict[x.Id] : 0
                        };
                    })
                    .ToArray();

                return houses;
            }
            finally
            {
                this.Container.Release(realObjOverhaulDataService);

            }
        }
    }
}