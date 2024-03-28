namespace Bars.Gkh.FormatDataExport.Domain.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;

    using Castle.Windsor;


    public class FormatDataExportFilterService : IFormatDataExportFilterService
    {
        public IWindsorContainer Container { get; set; }
        public IChargePeriodRepository ChargePeriodRepository { get; set; }

        private int BulkSize { get; set; }
        private IList<long> ContragentIdList { get; set; }
        private IList<long> MunicipalityIdList { get; set; }

        /// <inheritdoc />
        public bool UseIncremental { get; private set; }

        /// <inheritdoc />
        public DateTime StartEditDate { get; private set; }

        /// <inheritdoc />
        public DateTime EndEditDate { get; private set; }

        /// <inheritdoc />
        public long PeriodId { get; private set; }

        /// <inheritdoc />
        public long MainContragentId { get; private set; }

        /// <inheritdoc />
        public FormatDataExportProviderType Provider { get; private set; }

        /// <inheritdoc />
        public DateTime ExportDate { get; private set; }

        /// <inheritdoc />
        public LoadParam InspectionFilter { get; private set; }

        /// <inheritdoc />
        public LoadParam PersAccFilter { get; private set; }

        /// <inheritdoc />
        public LoadParam ProgramVersionFilter { get; private set; }

        /// <inheritdoc />
        public LoadParam ObjectCrFilter { get; private set; }

        /// <inheritdoc />
        public LoadParam RealityObjectFilter { get; private set; }

        /// <inheritdoc />
        public LoadParam DuUstavFilter { get; private set; }
        
        /// <inheritdoc />
        public ReadOnlyCollection<long> MunicipalityIds { get; private set; } = new ReadOnlyCollection<long>(new List<long>());

        /// <inheritdoc />
        public ReadOnlyCollection<long> ContragentIds { get; private set; } = new ReadOnlyCollection<long>(new List<long>());

        /// <inheritdoc />
        public ReadOnlyCollection<long> InspectionIds { get; private set; } = new ReadOnlyCollection<long>(new List<long>());

        /// <inheritdoc />
        public ReadOnlyCollection<long> PersAccIds { get; private set; } = new ReadOnlyCollection<long>(new List<long>());

        /// <inheritdoc />
        public ReadOnlyCollection<long> ProgramVersionIds { get; private set; } = new ReadOnlyCollection<long>(new List<long>());

        /// <inheritdoc />
        public ReadOnlyCollection<long> ObjectCrIds { get; private set; } = new ReadOnlyCollection<long>(new List<long>());

        /// <inheritdoc />
        public ReadOnlyCollection<long> RealityObjectIds { get; private set; } = new ReadOnlyCollection<long>(new List<long>());

        /// <inheritdoc />
        public ReadOnlyCollection<long> DuUstavIds { get; private set; } = new ReadOnlyCollection<long>(new List<long>());

        /// <inheritdoc />
        public bool HasContragentFilter => this.ContragentIds.Any();

        /// <inheritdoc />
        public bool HasMunicipalityFilter => this.MunicipalityIds.Any();

        /// <inheritdoc />
        public bool HasPersAccFilter => this.PersAccIds.Any() || this.PersAccFilter.ComplexFilter != null || this.PersAccFilter.DataFilter != null;

        /// <inheritdoc />
        public void Init(FormatDataExportProviderType provider, DynamicDictionary filterParams, int bulkSize)
        {
            this.UseIncremental = filterParams.GetAs("UseIncremental", false);
            this.StartEditDate = filterParams.GetAs("StartEditDate", DateTime.MinValue);
            this.EndEditDate = filterParams.GetAs("EndEditDate", DateTime.Today.AddDays(1));

            this.Provider = provider;
            this.BulkSize = bulkSize;
            var persAccIdList = filterParams.GetAs("PersAccList", new List<long>());
            var programVersionIdList = filterParams.GetAs("ProgramVersionList", new List<long>());
            var objectCrIdList = filterParams.GetAs("ObjectCrList", new List<long>());
            var programVersionMunicipalityIdList = filterParams.GetAs("ProgramVersionMunicipalityList", new List<long>());
            var programCrIdList = filterParams.GetAs("ProgramCrList", new List<long>());
            var objectCrMunicipalityIdList = filterParams.GetAs("ObjectCrMunicipalityList", new List<long>());
            var inspectionIdList = filterParams.GetAs("InspectionList", new List<long>());
            var duUstavIdList = filterParams.GetAs("DuUstavList", new List<long>());
            var realityObjectIdList = filterParams.GetAs("RealityObjectList", new List<long>());
            this.MainContragentId = filterParams.GetAsId("MainContragent");
            this.ContragentIdList = filterParams.GetAs("ContragentList", new List<long>());

            this.MunicipalityIdList = filterParams.GetAs("MunicipalityList", new List<long>());

            if (filterParams.ContainsKey("InspectionFilter"))
            {
                this.InspectionFilter = filterParams.GetLoadParam("InspectionFilter");
                this.InspectionFilter.Filter.Add("StartDate", filterParams.GetAs<DateTime?>("DisposalStartDate"));
                this.InspectionFilter.Filter.Add("EndDate", filterParams.GetAs<DateTime?>("DisposalEndDate"));
                this.InspectionFilter.Filter.Add("AuditType", filterParams.GetAs<int>("AuditType"));
            }

            if (filterParams.ContainsKey("PersAccFilter"))
            {
                this.PersAccFilter = filterParams.GetLoadParam("PersAccFilter");
            }
            
            if (filterParams.ContainsKey("ProgramVersionFilter"))
            {
                this.ProgramVersionFilter = filterParams.GetLoadParam("ProgramVersionFilter");
                this.ProgramVersionFilter.Filter.Add("municipalityId", programVersionMunicipalityIdList);
            }

            if (filterParams.ContainsKey("ObjectCrFilter"))
            {
                this.ObjectCrFilter = filterParams.GetLoadParam("ObjectCrFilter");
                this.ObjectCrFilter.Filter.Add("programCrIdList", programCrIdList);
                this.ObjectCrFilter.Filter.Add("municipalityIdList", objectCrMunicipalityIdList);
            }

            if (filterParams.ContainsKey("RealityObjectFilter"))
            {
                this.RealityObjectFilter = filterParams.GetLoadParam("RealityObjectFilter");
                this.RealityObjectFilter.Filter.Add("realityObjectList", realityObjectIdList);
            }

            if (filterParams.ContainsKey("DuUstavFilter"))
            {
                this.DuUstavFilter = filterParams.GetLoadParam("DuUstavFilter");
                this.DuUstavFilter.Filter.Add("duUstavIdList", duUstavIdList);
            }

            this.PeriodId = filterParams.GetAs<long>("ChargePeriod");
            this.ExportDate = DateTime.Today;
            
            if (this.ChargePeriodRepository != null)
            {
                this.ExportDate = this.PeriodId == 0
                    ? this.ChargePeriodRepository.GetCurrentPeriod().EndDate ?? DateTime.Today
                    : this.ChargePeriodRepository.Get(this.PeriodId).EndDate ?? DateTime.Today;
            }

            this.PersAccIds = new ReadOnlyCollection<long>(persAccIdList);
            this.ProgramVersionIds = new ReadOnlyCollection<long>(programVersionIdList);
            this.ObjectCrIds = new ReadOnlyCollection<long>(objectCrIdList);
            this.MunicipalityIds = new ReadOnlyCollection<long>(this.MunicipalityIdList);
            this.InspectionIds = new ReadOnlyCollection<long>(inspectionIdList);
            this.DuUstavIds = new ReadOnlyCollection<long>(duUstavIdList);
            this.RealityObjectIds = new ReadOnlyCollection<long>(realityObjectIdList);

            switch (provider)
            {
                case FormatDataExportProviderType.Uo:
                    this.InitUo();
                    return;

                case FormatDataExportProviderType.Ogv:
                    this.InitOgv();
                    return;

                case FormatDataExportProviderType.Oms:
                    this.InitOms();
                    return;

                case FormatDataExportProviderType.Rc:
                    this.InitRc();
                    return;

                case FormatDataExportProviderType.Rso:
                    this.InitRso();
                    return;


                case FormatDataExportProviderType.Gji:
                case FormatDataExportProviderType.Omjk:
                    this.InitGji();
                    return;

                case FormatDataExportProviderType.RegOpCr:
                case FormatDataExportProviderType.OgvEnergo:
                case FormatDataExportProviderType.RegOpWaste:
                    break;
            }

            this.ContragentIds = new ReadOnlyCollection<long>(this.ContragentIdList);
        }

        /// <inheritdoc />
        public IQueryable<Contragent> FilterByContragent(IQueryable<Contragent> query)
        {
            return this.WhereContains(query, x => x, this.ContragentIds);
        }

        /// <inheritdoc />
        public IQueryable<TEntity> FilterByContragent<TEntity>(IQueryable<TEntity> query, Expression<Func<TEntity, Contragent>> contragentSelector)
        {
            return this.WhereContains(query, contragentSelector, this.ContragentIds);
        }

        /// <inheritdoc />
        public IEnumerable<TEntity> FilterByContragent<TEntity>(IEnumerable<TEntity> data, Expression<Func<TEntity, Contragent>> contragentSelector)
        {
            return this.FilterByContragent(data.AsQueryable(), contragentSelector);
        }

        /// <inheritdoc />
        public IQueryable<TEntity> FilterByMunicipality<TEntity>(IQueryable<TEntity> query, Expression<Func<TEntity, Municipality>> municipalitySelector)
        {
            return this.WhereContains(query, municipalitySelector, this.MunicipalityIds);
        }

        /// <inheritdoc />
        public IEnumerable<TEntity> FilterByMunicipality<TEntity>(IEnumerable<TEntity> data, Expression<Func<TEntity, Municipality>> municipalitySelector)
        {
            return this.WhereContains(data.AsQueryable(), municipalitySelector, this.MunicipalityIds);
        }

        /// <inheritdoc />
        public IQueryable<TEntity> GetFiltredQuery<TEntity>()
            where TEntity : BaseEntity
        {
            if (this.Container.HasComponent<IFormatDataExportRepository<TEntity>>())
            {
                var service = this.Container.Resolve<IFormatDataExportRepository<TEntity>>();
                using (this.Container.Using(service))
                {
                    return service.GetQuery(this);
                }
            }
            else
            {
                throw new ArgumentException($"Не найдена реализация IFormatDataExportRepository<{typeof(TEntity).Name}>", typeof(TEntity).Name);
            }
        }

        private IQueryable<TEntity> WhereContains<TEntity, TProperty>(
            IQueryable<TEntity> query,
            Expression<Func<TEntity, TProperty>> propertySelector,
            ReadOnlyCollection<long> collection)
        {
            if (collection.IsEmpty())
            {
                return query;
            }

            var offset = 0;
            var count = collection.Count;
            var entity = propertySelector.Parameters[0];
            var id = typeof(Contragent).GetProperty("Id");
            var idSelector = Expression.Property(propertySelector.Body, id);

            var expressionParts = new List<Expression>();

            while (offset < count)
            {
                var partSize = count > this.BulkSize
                    ? this.BulkSize
                    : count - offset;

                var partData = collection.Skip(offset).Take(partSize).ToList();

                expressionParts.Add(Expression.Call(
                    Expression.Constant(partData),
                    partData.GetType().GetMethod("Contains"),
                    idSelector));

                offset += partSize;
            }

            if (expressionParts.Count > 0)
            {
                var wholeExpression = expressionParts.First();
                foreach (var expression in expressionParts.Skip(1))
                {
                    wholeExpression = Expression.MakeBinary(ExpressionType.OrElse, wholeExpression, expression);
                }

                var predicate = Expression.Lambda<Func<TEntity, bool>>(wholeExpression, entity);

                query = query.Where(predicate);
            }
            else
            {
                query = query.Where(x => false);
            }

            return query;
        }

        private void InitUo()
        {
            var manOrgRepository = this.Container.ResolveRepository<ManagingOrganization>();

            using (this.Container.Using(manOrgRepository))
            {
                var query = manOrgRepository.GetAll();
                if (this.MunicipalityIdList.IsNotEmpty())
                {
                    query = query.WhereContainsBulked(x => x.Contragent.Municipality.Id, this.MunicipalityIdList, 5000);
                }
                query = this.ContragentIdList.IsNotEmpty()
                    ? query.WhereContainsBulked(x => x.Contragent.Id, this.ContragentIdList, 5000)
                    : query.Where(x => x.Contragent.Id == this.MainContragentId);

                var contragentIds = query
                    .Select(x => x.Contragent.Id)
                    .ToList();

                this.ContragentIds = new ReadOnlyCollection<long>(contragentIds);

                this.CheckContragents("Не удалось определить контрагентов УО");
            }
        }

        private void InitOms()
        {
            var omsRepository = this.Container.ResolveRepository<LocalGovernment>();
            var omsMoRepository = this.Container.ResolveRepository<LocalGovernmentMunicipality>();
            var realityObjectRepository = this.Container.ResolveRepository<RealityObject>();

            using (this.Container.Using(omsRepository, omsMoRepository, realityObjectRepository))
            {
                var contragentIds = new List<long>();

                if (this.ContragentIdList.IsNotEmpty())
                {
                    contragentIds = omsRepository.GetAll()
                        .WhereContainsBulked(x => x.Contragent.Id, this.ContragentIdList, 5000)
                        .Select(x => x.Contragent.Id)
                        .ToList();

                } else if (this.MunicipalityIdList.IsNotEmpty())
                {
                    contragentIds = omsMoRepository.GetAll()
                        .WhereNotNull(x => x.LocalGovernment)
                        .WhereContainsBulked(x => x.Municipality.Id, this.MunicipalityIdList, 5000)
                        .Select(x => x.LocalGovernment.Contragent.Id)
                        .ToList();
                }
                else
                {
                    contragentIds = omsRepository.GetAll()
                        .Where(x => x.Contragent.Id == this.MainContragentId)
                        .Select(x => x.Contragent.Id)
                        .ToList();
                }

                var uniqueContragentIds = contragentIds.DistinctValues().ToList();

                this.ContragentIds = new ReadOnlyCollection<long>(uniqueContragentIds);

                this.CheckContragents("Не удалось определить контрагентов ОМС");
            }
        }

        private void InitOgv()
        {
            var ogvRepository = this.Container.ResolveRepository<PoliticAuthority>();
            var ogvMoRepository = this.Container.ResolveRepository<PoliticAuthorityMunicipality>();
            var realityObjectRepository = this.Container.ResolveRepository<RealityObject>();

            using (this.Container.Using(ogvRepository, ogvMoRepository, realityObjectRepository))
            {
                var contragentIds = new List<long>();

                if (this.ContragentIdList.IsNotEmpty())
                {
                    contragentIds = ogvRepository.GetAll()
                        .WhereContainsBulked(x => x.Contragent.Id, this.ContragentIdList, 5000)
                        .Select(x => x.Contragent.Id)
                        .ToList();

                }
                else if (this.MunicipalityIdList.IsNotEmpty())
                {
                    contragentIds = ogvMoRepository.GetAll()
                        .WhereNotNull(x => x.PoliticAuthority)
                        .WhereContainsBulked(x => x.Municipality.Id, this.MunicipalityIdList, 5000)
                        .Select(x => x.PoliticAuthority.Contragent.Id)
                        .ToList();
                }
                else
                {
                    contragentIds = ogvRepository.GetAll()
                        .Where(x => x.Contragent.Id == this.MainContragentId)
                        .Select(x => x.Contragent.Id)
                        .ToList();
                }

                var uniqueContragentIds = contragentIds.DistinctValues().ToList();

                this.ContragentIds = new ReadOnlyCollection<long>(uniqueContragentIds);

                this.CheckContragents("Не удалось определить контрагентов ОГВ");
            }
        }

        private void InitRso()
        {
            var publicServiceOrgRepository = this.Container.ResolveRepository<PublicServiceOrg>();
            var publicServiceOrgContractRealObjRepository = this.Container.ResolveRepository<PublicServiceOrgContractRealObj>();

            using (this.Container.Using(publicServiceOrgRepository, publicServiceOrgContractRealObjRepository))
            {
                var query = publicServiceOrgRepository.GetAll();
                if (this.MunicipalityIdList.IsNotEmpty())
                {
                    query = query.WhereContainsBulked(x => x.Contragent.Municipality.Id, this.MunicipalityIdList, 5000);
                }
                query = this.ContragentIdList.IsNotEmpty()
                    ? query.WhereContainsBulked(x => x.Contragent.Id, this.ContragentIdList, 5000)
                    : query.Where(x => x.Contragent.Id == this.MainContragentId);

                var contragentIds = query
                    .Select(x => x.Contragent.Id)
                    .ToList();

                this.ContragentIds = new ReadOnlyCollection<long>(contragentIds);

                this.CheckContragents("Не удалось определить контрагентов РСО");
            }
        }

        private void InitRc()
        {
            var cashPaymentCenterRepository = this.Container.ResolveRepository<CashPaymentCenter>();
            var cashPaymentCenterRealObjRepository = this.Container.ResolveRepository<CashPaymentCenterRealObj>();

            using (this.Container.Using(cashPaymentCenterRepository, cashPaymentCenterRealObjRepository))
            {
                var query = cashPaymentCenterRepository.GetAll();
                if (this.MunicipalityIdList.IsNotEmpty())
                {
                    query = query.WhereContainsBulked(x => x.Contragent.Municipality.Id, this.MunicipalityIdList, 5000);
                }
                query = this.ContragentIdList.IsNotEmpty()
                    ? query.WhereContainsBulked(x => x.Contragent.Id, this.ContragentIdList, 5000)
                    : query.Where(x => x.Contragent.Id == this.MainContragentId);

                var contragentIds = query
                    .Select(x => x.Contragent.Id)
                    .ToList();

                this.ContragentIds = new ReadOnlyCollection<long>(contragentIds);

                this.CheckContragents("Не удалось определить контрагентов РКЦ");
            }
        }

        private void InitGji()
        {
        }

        private void CheckContragents(string errorMessage)
        {
            if (this.ContragentIds.IsEmpty())
            {
                throw new Exception(errorMessage);
            }
        }
    }
}