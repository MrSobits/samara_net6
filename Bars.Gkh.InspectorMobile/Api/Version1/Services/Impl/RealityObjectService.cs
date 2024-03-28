namespace Bars.Gkh.InspectorMobile.Api.Version1.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.HeatingSeason;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.RealityObject;
    using Bars.Gkh.InspectorMobile.Extensions;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Bars.GkhGji.Entities;

    using NHibernate.Linq;

    using RealityObject = Bars.Gkh.InspectorMobile.Api.Version1.Models.RealityObject.RealityObject;

    /// <summary>
    /// API сервис для работы с <see cref="Bars.Gkh.Entities.RealityObject"/>
    /// </summary>
    public class RealityObjectService : IRealityObjectService
    {
        #region Services
        private readonly IRepository<Bars.Gkh.Entities.RealityObject> _realityObjectRepository;
        private readonly IDomainService<RealityObjectImage> _realityObjectImageDomain;
        private readonly IDomainService<ManOrgContractRealityObject> _roContractLinkDomain;
        private readonly IDomainService<RealityObjectStructuralElement> _roStructuralElementDomain;
        private readonly IDomainService<ObjectCr> _objectCrDomain;
        private readonly IDomainService<HeatSeason> _heatSeasonDomain;
        private readonly IDomainService<HeatSeasonDoc> _heatSeasonDocDomain;
        private readonly IDomainService<PropertyOwnerProtocols> _propertyOwnerProtocolsDomain;
        private readonly IDomainService<BasePropertyOwnerDecision> _propertyOwnerDecisionDomain;
        private readonly IDomainService<Fias> _fiasDomain;
        private readonly IGkhUserManager _userManager;
        private readonly IConfigProvider _configProvider;
        #endregion

        #region Constructor
        public RealityObjectService(IRepository<Bars.Gkh.Entities.RealityObject> realityObjectRepository,
            IDomainService<RealityObjectImage> realityObjectImageDomain,
            IDomainService<ManOrgContractRealityObject> roContractLinkDomain,
            IDomainService<RealityObjectStructuralElement> roStructuralElementDomain,
            IDomainService<ObjectCr> objectCrDomain,
            IDomainService<HeatSeason> heatSeasonDomain,
            IDomainService<HeatSeasonDoc> heatSeasonDocDomain,
            IDomainService<PropertyOwnerProtocols> propertyOwnerProtocolsDomain,
            IDomainService<BasePropertyOwnerDecision> propertyOwnerDecisionDomain,
            IDomainService<Fias> fiasDomain,
            IGkhUserManager userManager,
            IConfigProvider configProvider)
        {
            _realityObjectRepository = realityObjectRepository;
            _realityObjectImageDomain = realityObjectImageDomain;
            _roContractLinkDomain = roContractLinkDomain;
            _roStructuralElementDomain = roStructuralElementDomain;
            _objectCrDomain = objectCrDomain;
            _heatSeasonDomain = heatSeasonDomain;
            _heatSeasonDocDomain = heatSeasonDocDomain;
            _propertyOwnerProtocolsDomain = propertyOwnerProtocolsDomain;
            _propertyOwnerDecisionDomain = propertyOwnerDecisionDomain;
            _fiasDomain = fiasDomain;
            _userManager = userManager;
            _configProvider = configProvider;
        }
        #endregion
            
        /// <inheritdoc />
        public async Task<BaseRealityObject> GetAsync(long houseId) => (await this.GetEnumerationAsync(new[] { houseId })).FirstOrDefault();

        /// <inheritdoc />
        public async Task<IEnumerable<BaseRealityObject>> ListAsync(bool fullList, long[] houseIds) => (await this.GetEnumerationAsync(houseIds, fullList));

        /// <summary>
        /// Получить выборку домов
        /// </summary>
        private async Task<IEnumerable<BaseRealityObject>> GetEnumerationAsync(long[] houseIds, bool fullList = false)
        {
            Fias region = null;
            
            void InitRegionInfo()
            {
                var config = this._configProvider.GetConfig().GetModuleConfig("Bars.B4.Modules.FIAS.AutoUpdater");
                var regionCode = config.GetAs("RegionCode", default(string), true);
                
                region = this._fiasDomain.GetAll()
                    .Single(x => x.AOLevel == FiasLevelEnum.Region && x.CodeRegion == regionCode);
            }

            if (fullList)
            {
                var municipalityIds = this._userManager.GetMunicipalityIds();

                if (!municipalityIds.Any())
                    throw new ApiServiceException("У оператора не определено ни одного муниципального образования");

                InitRegionInfo();

                return await _realityObjectRepository.GetAll()
                    .Where(x => x.Municipality != null)
                    .Where(x => new[]
                        {
                            ConditionHouse.NotSelected,
                            ConditionHouse.Emergency,
                            ConditionHouse.Dilapidated,
                            ConditionHouse.Serviceable
                        }.Contains(x.ConditionHouse)
                    )
                    .Where(x => municipalityIds.Contains(x.Municipality.Id))
                    .Select(x => new BaseRealityObject
                    {
                        Id = x.Id,
                        HouseGuid = x.HouseGuid,
                        Address = x.Address,
                        Municipality = x.Municipality.Name,
                        Region = region.OffName
                    })
                    .ToListAsync();
            }

            if (houseIds == null || !houseIds.Any())
            {
                throw new ApiServiceException($"При значении параметра \"{nameof(fullList)}\"=false " +
                    $"должен быть передан хотя бы один идентификатор объекта жилищного фонда");
            }
            
            var imageDict = (await this._realityObjectImageDomain.GetAll()
                .Where(x => houseIds.Contains(x.RealityObject.Id))
                .Where(x => x.File != null)
                .Where(x => x.ImagesGroup == ImagesGroup.Avatar)
                .ToListAsync())
                .ToDictionary(x => x.RealityObject.Id, x => x.File.Id);

            var controlHomeDict = (await this._roContractLinkDomain.GetAll()
                .Where(x => x.ManOrgContract != null && x.RealityObject != null)
                .Where(x => houseIds.Contains(x.RealityObject.Id))
                .GroupBy(x => x.RealityObject.Id)
                .ToListAsync())
                .ToDictionary(x => x.Key,
                    x => x.Select(y => new ControlHome
                    {
                        ControlType = y.ManOrgContract.TypeContractManOrgRealObj.IsDefault() 
                            ? null 
                            : y.ManOrgContract.TypeContractManOrgRealObj.GetDisplayName(),
                        StartDate = y.ManOrgContract.StartDate,
                        EndDate = y.ManOrgContract.EndDate,
                        ContractType = y.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgJskTsj
                            ? "Передача управления"
                            : y.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag
                                ? "Оказание услуг"
                                : "Основной",
                        Document = y.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgJskTsj
                            || y.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgOwners
                                ? "Договор"
                                : y.ManOrgContract.DocumentName,
                        Organization = y.ManOrgContract?.ManagingOrganization != null
                        ? new Organization
                        {
                            Id = y.ManOrgContract.ManagingOrganization.Contragent.Id,
                            Name = y.ManOrgContract.ManagingOrganization.Contragent.Name,
                        }
                        : null
                    }).ToArray());

            var structElDict = (await this._roStructuralElementDomain.GetAll()
                .Where(x => x.RealityObject != null && x.StructuralElement != null)
                .Where(x => houseIds.Contains(x.RealityObject.Id))
                // Статус струк. элемента = "Актуальный" или "Дополнительный"
                .Where(x => new[] { "1", "3" }.Contains(x.State.Code))
                .GroupBy(x => x.RealityObject.Id)
                .ToListAsync())
                .ToDictionary(x => x.Key,
                    x => x.Select(y => new StructuralElements
                    {
                        StructuralElement = y.StructuralElement.Name,
                        Ooi = y.StructuralElement.Group?.CommonEstateObject?.Name,
                        Condition = y.Condition.IsDefault() ? null : y.Condition.GetDisplayName(),
                        YearInstallation = y.LastOverhaulYear,
                        SystemType = y.StructuralElement.Group?.CommonEstateObject?.IsEngineeringNetwork ?? false
                            ? y.SystemType.GetDisplayName()
                            : null,
                        Wear = y.Wearout,
                        Volume = y.Volume,
                        Measure = y.StructuralElement.UnitMeasure?.Name
                    }).ToArray());

            var overhaulProgramDict = (await this._objectCrDomain.GetAll()
                .Where(x => x.RealityObject != null && x.ProgramCr != null)
                .Where(x => x.ProgramCr.TypeVisibilityProgramCr == TypeVisibilityProgramCr.Full)
                .Where(x => houseIds.Contains(x.RealityObject.Id))
                .GroupBy(x => x.RealityObject.Id)
                .ToListAsync())
                .ToDictionary(x => x.Key,
                    x => x.Select(y => new OverhaulProgram
                    {
                        ObjectId = y.Id,
                        ProgramName = y.ProgramCr.Name,
                        ProgramPeriod = y.ProgramCr.Period?.Name
                    }).ToArray());

            var heatSeasonDict = (await this._heatSeasonDomain.GetAll()
                .Join(this._heatSeasonDocDomain.GetAll(),
                    obj => obj.Id,
                    doc => doc.HeatingSeason.Id,
                    (obj, doc) => new { obj, doc })
                .Where(x => houseIds.Contains(x.obj.RealityObject.Id))
                .GroupBy(x => x.obj.RealityObject.Id)
                .ToListAsync())
                .ToDictionary(x => x.Key, x => x.GroupBy(y => y.obj, (y, z) => new HeatingSeason
                    {
                        Id = y.Id,
                        HeatingSeasonPeriod = new HeatingSeasonPeriod
                        {
                            Id = y.Period.Id,
                            Name = y.Period.Name
                        },
                        Documents = z
                            .Where(c => c.doc != null)
                            .Select(c => new HeatingSeasonDocumentGet 
                            {
                                Id = c.doc.Id,
                                State = c.doc.State.GetStateInfo(),
                                DocumentType = c.doc.TypeDocument,
                                Number = c.doc.DocumentNumber,
                                Date = c.doc.DocumentDate,
                                Description = c.doc.Description,
                                FileId = c.doc.File?.Id
                            }).ToArray()
                    }).ToArray());

            var propertyOwnerDict = (await _propertyOwnerProtocolsDomain.GetAll()
                .Join(_propertyOwnerDecisionDomain.GetAll(),
                    protocol => protocol.Id,
                    decision => decision.PropertyOwnerProtocol.Id,
                    (protocol, decision) => new {protocol, decision})
                .Where(x => houseIds.Contains(x.protocol.RealityObject.Id))
                .Where(x => x.decision.PropertyOwnerDecisionType == PropertyOwnerDecisionType.SelectMethodForming)
                .Where(x => x.protocol.TypeProtocol != PropertyOwnerProtocolType.SelectManOrg)
                .GroupBy(x => x.protocol.RealityObject.Id)
                .ToListAsync())
                .ToDictionary(x => x.Key,
                    x =>
                    {
                        var lastProtocolDateObject = x.OrderByDescending(y => y.protocol.DocumentDate).First();

                        return new
                        {
                            lastProtocolDateObject.decision.MethodFormFund,
                            lastProtocolDateObject.protocol.DocumentDate
                        };
                    });

            InitRegionInfo();

            return (await this._realityObjectRepository.GetAll()
                .Where(x => houseIds.Contains(x.Id))
                .ToListAsync())
                .Select(x =>
                {
                    var propertyOwner = propertyOwnerDict.Get(x.Id);

                    return new RealityObject
                    {
                        Id = x.Id,
                        HouseGuid = x.HouseGuid,
                        Address = x.Address,
                        Region = region.OffName,
                        Municipality = x.Municipality.Name,
                        Type = x.TypeHouse.GetDisplayName(),
                        Condition = x.ConditionHouse.GetDisplayName(),
                        BuildYear = x.BuildYear,
                        Square = x.AreaMkd,
                        SquarePrivate = x.AreaOwned,
                        SquareMunicipal = x.AreaMunicipalOwned,
                        SquareRoom = x.AreaLiving,
                        SquarePremises = x.AreaNotLivingPremises,
                        Entrance = x.NumberEntrances,
                        Room = x.NumberApartments,
                        Premises = x.NumberNonResidentialPremises,
                        Reside = x.NumberLiving,
                        TypeOwnerShip = x.TypeOwnership?.Name,
                        FloorMin = x.Floors,
                        FloorMax = x.MaximumFloors,
                        RoofMaterial = x.RoofingMaterial?.Name,
                        WallMaterial = x.WallMaterial?.Name,
                        RoofType = x.TypeRoof.GetDisplayName(),
                        Heating = x.HeatingSystem.GetDisplayName(),
                        RepairYear = x.DateLastOverhaul?.Year,
                        Photo = imageDict.Get(x.Id, (long?)null),
                        FormWay = propertyOwner?.MethodFormFund?.GetDisplayName(),
                        ProtocolDate = propertyOwner?.DocumentDate,
                        StructuralElements = structElDict.Get(x.Id),
                        ControlHome = controlHomeDict.Get(x.Id),
                        OverhaulPrograms = overhaulProgramDict.Get(x.Id),
                        HeatingSeason = heatSeasonDict.Get(x.Id)
                    };
                });
        }
    }
}