namespace Bars.Gkh.InspectorMobile.Api.Version1.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;

    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Dict;
    using Bars.Gkh.Services.ServiceContracts;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Utils.Attributes;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;

    using Castle.Core.Internal;
    using Castle.Windsor;

    using NHibernate.Linq;

    using FiasHouse = Bars.B4.Modules.FIAS.FiasHouse;

    /// <summary>
    /// API. Сервис для взаимодействия с НСИ
    /// </summary>
    public class DictService : IDictService
    {
        #region DependencyInjection
        private readonly IDomainService<Fias> fiasDomainService;
        private readonly IDomainService<FiasHouse> fiasHouseDomainService;
        private readonly IDomainService<Municipality> municipalityDomainService;
        private readonly IDomainService<State> stateDomain;
        private readonly IDomainService<StateTransfer> stateTransferDomain;

        private readonly IEnumerable<FiasLevelEnum> excludedAoLevelEnums = new[] { FiasLevelEnum.PlanningStruct, FiasLevelEnum.Extr };
        private readonly IStateProvider stateProvider;
        private readonly IGkhUserManager userManager;
        private readonly IWindsorContainer container;
        private readonly IConfigProvider configProvider;

        public DictService(
            IDomainService<Fias> fiasDomainService,
            IDomainService<FiasHouse> fiasHouseDomainService,
            IDomainService<Municipality> municipalityDomainService,
            IStateProvider stateProvider,
            IDomainService<StateTransfer> stateTransferDomain,
            IGkhUserManager userManager,
            IDomainService<State> stateDomain,
            IWindsorContainer container,
            IConfigProvider configProvider)
        {
            this.fiasDomainService = fiasDomainService;
            this.fiasHouseDomainService = fiasHouseDomainService;
            this.municipalityDomainService = municipalityDomainService;
            this.stateProvider = stateProvider;
            this.stateTransferDomain = stateTransferDomain;
            this.userManager = userManager;
            this.stateDomain = stateDomain;
            this.container = container;
            this.configProvider = configProvider;
        }
        #endregion

        /// <inheritdoc />
        public IEnumerable<Dict> GetList(DateTime? date)
        {
            var reflectionService = this.container.Resolve<IReflectionHelperService>();

            using (this.container.Using(reflectionService))
            {
                return Enum.GetValues(typeof(DictCode)).Cast<DictCode>().Select(code =>
                    {
                        List<DictEntry> dictEntries;
                        var type = code.GetAttribute<TypeAttribute>().Type;
                        var domain = this.container.ResolveDomain(type);

                        using (this.container.Using(domain))
                        {
                            var query = domain.GetAll();

                            switch (type.Name)
                            {
                                case nameof(NormativeDoc):
                                    dictEntries = query
                                        .Cast<NormativeDoc>()
                                        .WhereIf(date.HasValue, x => x.ObjectEditDate > date)
                                        .Where(x => x.Category == NormativeDocCategory.HousingSupervision &&
                                            !x.DateFrom.HasValue &&
                                            !x.DateTo.HasValue || DateTime.Now >= x.DateFrom && DateTime.Now <= x.DateTo)
                                        .Select(x => new DictEntry
                                        {
                                            Id = x.Id,
                                            Entry = x.Name
                                        })
                                        .ToList();

                                    break;

                                case nameof(Inspector):
                                    dictEntries = query
                                        .Cast<Inspector>()
                                        .WhereIf(date.HasValue, x => x.ObjectEditDate > date)
                                        .Where(x => x.IsActive)
                                        .Select(x => new DictEntry
                                        {
                                            Id = x.Id,
                                            Entry = x.Fio
                                        })
                                        .ToList();

                                    break;
                                case nameof(ProgramCr):
                                    dictEntries = query
                                        .Cast<ProgramCr>()
                                        .WhereIf(date.HasValue, x => x.ObjectEditDate > date)
                                        .Where(x => x.TypeVisibilityProgramCr == TypeVisibilityProgramCr.Full)
                                        .Select(x => new DictEntry()
                                        {
                                            Id = x.Id,
                                            Entry = x.Name
                                        })
                                        .ToList();
                                    break;
                                default:
                                    var propertyMapDict = new Dictionary<string, string>()
                                    {
                                        { "Id", "Id" },
                                        { "Name", "Entry" },
                                        { "ObjectEditDate", "EditDate" }
                                    };
                                    var selectNewExpression = reflectionService.GetGenericMethod<ExpressionExtension>(
                                        nameof(ExpressionExtension.GetSelectNewObjectExpression),
                                        type,
                                        typeof(DictEntry)
                                    ).Invoke(null, new object[] { propertyMapDict });
                                    var selectMethod = reflectionService.GetGenericMethod(
                                        typeof(Queryable),
                                        "Select",
                                        BindingFlags.Public | BindingFlags.Static,
                                        type,
                                        typeof(DictEntry)
                                    );

                                    dictEntries = selectMethod
                                        .Invoke(null, new[] { query, selectNewExpression })
                                        .CastAs<IQueryable<DictEntry>>()
                                        .WhereIf(date.HasValue, x => x.EditDate > date)
                                        .ToList();

                                    break;
                            }
                        }

                        return dictEntries.Any()
                            ? new Dict
                            {
                                Code = code,
                                DictEntry = dictEntries
                            }
                            : null;
                    })
                    .Where(x => x != null)
                    .ToList();
            }
        }

        /// <inheritdoc />
        public IEnumerable<GroupViolations> GroupViolationsList(DateTime? date)
        {
            var violationFeatureGjiDomain = this.container.ResolveDomain<ViolationFeatureGji>();
            var violationNormativeDocItemGjiDomain = this.container.ResolveDomain<ViolationNormativeDocItemGji>();

            using (this.container.Using(violationFeatureGjiDomain, violationNormativeDocItemGjiDomain))
            {
                return violationFeatureGjiDomain.GetAll()
                    .Join(violationNormativeDocItemGjiDomain.GetAll(),
                        x => x.ViolationGji.Id,
                        x => x.ViolationGji.Id,
                        (x, y) => new
                        {
                            FeatureViol = x.FeatureViolGji,
                            Viol = x.ViolationGji,
                            y.NormativeDocItem
                        })
                    .Where(x => x.FeatureViol.IsActual)
                    .Where(x => x.Viol.IsActual)
                    .WhereIf(date.HasValue, x => x.Viol.ObjectEditDate > date)
                    .Where(x => x.NormativeDocItem != null && x.NormativeDocItem.NormativeDoc != null)
                    .Where(x => x.NormativeDocItem.NormativeDoc.DateFrom <= DateTime.Now && x.NormativeDocItem.NormativeDoc.DateTo >= DateTime.Now)
                    .AsEnumerable()
                    .GroupBy(x => x.FeatureViol,
                        (x, y) => new GroupViolations
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Violations = y.Select(z => new Violation
                            {
                                Id = z.Viol.Id,
                                Name = z.Viol.Name
                            }).ToArray()
                        })
                    .ToList();
            }
        }

        /// <inheritdoc />
        public async Task<FiasResponse> Fias(long municipalityId, DateTime? lastDateUpdate)
        {
            if (municipalityId == default)
            {
                throw new ApiServiceException("Не передан идентификатор муниципального образования");
            }

            var result = new FiasResponse();

            if (lastDateUpdate.HasValue && !await this.IsNeedUpdate(lastDateUpdate.Value))
            {
                return result;
            }
            
            var config = this.configProvider.GetConfig().GetModuleConfig("Bars.B4.Modules.FIAS.AutoUpdater");
            var regionCode = config.GetAs("RegionCode", default(string), true);

            var tatarstanFias = await this.fiasDomainService.GetAll()
                .Where(x => x.AOLevel == FiasLevelEnum.Region 
                    && x.CodeRegion == regionCode
                    && x.ActStatus == FiasActualStatusEnum.Actual 
                    && (!x.EndDate.HasValue || x.EndDate >= DateTime.Today))
                .Select(this.FiasObjectSelector)
                .SingleAsync();

            var municipalityFiasGuid = this.municipalityDomainService.Get(municipalityId)?.FiasId;

            if (municipalityFiasGuid.IsNullOrEmpty())
            {
                throw new ApiServiceException("Не передан идентификатор муниципального образования");
            }

            var municipalityFias = await this.fiasDomainService.GetAll()
                .Where(x => municipalityFiasGuid == x.AOGuid && x.ActStatus == FiasActualStatusEnum.Actual && (!x.EndDate.HasValue || x.EndDate >= DateTime.Today))
                .Select(this.FiasObjectSelector)
                .ToListAsync();

            var mirrorGuidList = municipalityFias.Where(x => !string.IsNullOrEmpty(x.MirrorGuid)).Select(x => x.MirrorGuid).ToList();

            if (!mirrorGuidList.IsNullOrEmpty())
            {
                var mirrorFias = await this.fiasDomainService.GetAll()
                    .Where(x => mirrorGuidList.Contains(x.AOGuid) && x.ActStatus == FiasActualStatusEnum.Actual &&
                        (!x.EndDate.HasValue || x.EndDate >= DateTime.Today))
                    .Select(this.FiasObjectSelector)
                    .ToListAsync();

                municipalityFias.RemoveAll(x => !string.IsNullOrEmpty(x.MirrorGuid));
                municipalityFias.AddRange(mirrorFias);
            }

            municipalityFias.Add(tatarstanFias);

            result.Objects = (await this.GetChildrenFiasObjects(municipalityFias.Where(x => x.AoLevel != FiasLevelEnum.Region).Select(x => x.AoGuid)))
                .Concat(municipalityFias).ToArray();

            var streetGuids = result.Objects
                .Where(x => x.AoLevel == FiasLevelEnum.Street)
                .Select(y => y.AoGuid)
                .ToArray();

            if (lastDateUpdate.HasValue)
            {
                result.Objects = result.Objects.Where(x => x.UpdateDate > lastDateUpdate);
            }

            result.Houses = await this.fiasHouseDomainService.GetAll()
                .Where(x => streetGuids.Contains(x.AoGuid.ToString()) &&
                    x.ActualStatus == FiasActualStatusEnum.Actual && (!x.EndDate.HasValue || x.EndDate >= DateTime.Today))
                .WhereIf(lastDateUpdate.HasValue, x => x.UpdateDate > lastDateUpdate)
                .Select(x => new MobileFiasHouse
                {
                    AoGuid = x.AoGuid.ToString(),
                    HouseGuid = x.HouseGuid.ToString(),
                    HouseNum = x.HouseNum,
                })
                .ToListAsync();

            return result;
        }

        /// <summary>
        /// Проверить требуется ли обновление
        /// </summary>
        private async Task<bool> IsNeedUpdate(DateTime updateDateTime)
        {
            var needUpdate = await this.fiasDomainService.GetAll()
                .AnyAsync(x => x.UpdateDate > updateDateTime);

            if (needUpdate)
            {
                return needUpdate;
            }

            needUpdate = await this.fiasHouseDomainService.GetAll()
                .AnyAsync(x => x.UpdateDate > updateDateTime);

            return needUpdate;
        }

        /// <summary>
        /// Получить дочерние элементы для элемента ФИАС
        /// </summary>
        /// <param name="parentGuids">ГУИДы родительских элементов</param>
        /// <returns>Плоский список дочерних объектов всех уровней</returns>
        private async Task<IEnumerable<FiasObject>> GetChildrenFiasObjects(IEnumerable<string> parentGuids)
        {
            var result = await this.fiasDomainService.GetAll()
                .Where(x => parentGuids.Contains(x.ParentGuid) &&
                    !this.excludedAoLevelEnums.Contains(x.AOLevel) &&
                    x.ActStatus == FiasActualStatusEnum.Actual &&
                    (!x.EndDate.HasValue || x.EndDate >= DateTime.Today))
                .Select(this.FiasObjectSelector)
                .ToListAsync();

            if (!result.Any())
            {
                return Enumerable.Empty<FiasObject>();
            }

            var children = await this.GetChildrenFiasObjects(result.Select(x => x.AoGuid));
            if (children != null)
            {
                result = result.Concat(children).ToList();
            }

            return result;
        }

        /// <summary>
        /// Селектор объекта ФИАС
        /// </summary>
        private Expression<Func<Fias, FiasObject>> FiasObjectSelector =>
            x => new FiasObject
            {
                AoGuid = x.AOGuid,
                AoLevel = x.AOLevel,
                OffName = x.OffName,
                ParentGuid = x.ParentGuid,
                PlaceCode = x.CodePlace,
                ShortName = x.ShortName,
                StreetCode = x.CodeStreet,
                UpdateDate = x.UpdateDate,
                MirrorGuid = x.MirrorGuid
            };

        /// <inheritdoc />
        public async Task<IEnumerable<TransferDocStatus>> DocStatusListAsync()
        {
            var whiteListTypes = new[]
            {
                typeof(HeatSeasonDoc),
                typeof(DefectList),
                typeof(EstimateCalculation),
                typeof(ContractCr),
                typeof(BuildContract),
                typeof(PerformedWorkAct)
            };

            var typeIds = whiteListTypes.Select(x => this.stateProvider.GetStatefulEntityInfo(x).TypeId);

            var states = await this.stateDomain.GetAll()
                .Where(x => typeIds.Contains(x.TypeId))
                .ToListAsync();

            var availableStateTransfers = this.stateTransferDomain.GetAll()
                .Where(x => typeIds.Contains(x.CurrentState.TypeId))
                .Where(x => this.userManager.GetActiveOperatorRoles().Contains(x.Role))
                .GroupBy(x => x.CurrentState)
                .ToDictionary(x => x.Key);

            return states
                .Select(x => new TransferDocStatus
                {
                    Id = x.Id,
                    Name = x.Name,
                    Type = x.TypeId,
                    State = availableStateTransfers.Get(x)?
                        .Select(y => new DocStatus
                        {
                            Id = y.NewState.Id,
                            Name = y.NewState.Name,
                            Type = y.NewState.TypeId,
                        })
                });
        }
    }
}