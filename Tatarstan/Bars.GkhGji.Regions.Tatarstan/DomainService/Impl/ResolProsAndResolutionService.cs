namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    using Castle.Windsor;

    public class ResolProsAndResolutionService : IResolProsAndResolutionService
    {
        public ResolProsAndResolutionService(IWindsorContainer container)
        {
            this.Container = container;
        }

        /// <summary>
        /// Container
        /// </summary>
        private IWindsorContainer Container { get; }

        /// <summary>
        /// Возвращает ResolPros с доп. полями "Должностное лицо", "Сумма штрафа" и "УИН" из DocumentGji
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <param name="resolProsService">Последовательность ResolPros</param>
        /// <param name="toExcel">Добавлять ли апостроф в начало строки поля "УИН"</param>
        /// <param name="useResolProsRoService">Дает возможность брать значение для поля "Муниципалитет"
        /// из ResolProsRealityObject при отсутствии в ResolPros</param>
        /// <returns></returns>
        public IList<ResolProsAndResolution> GetList(
            BaseParams baseParams,
            IDomainService<ResolPros> resolProsService,
            bool toExcel,
            bool useResolProsRoService)
        {
            var resolutionService = this.Container.Resolve<IDomainService<Resolution>>();
            var documentGjiService = this.Container.Resolve<IDomainService<DocumentGji>>();
            var resolProsRoService = this.Container.Resolve<IDomainService<ResolProsRealityObject>>();
            var resolutionDisputeDomain = this.Container.ResolveDomain<ResolutionDispute>();

            var userManager = this.Container.Resolve<IGkhUserManager>();

            var loadParam = baseParams.GetLoadParam();
            var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");
            var stageId = baseParams.Params.GetAs<long>("stageId");
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");

            try
            {
                var municipalityList = userManager.GetMunicipalityIds();
                var resolPros = resolProsService.GetAll()
                    .WhereIf(realityObjectId > 0,
                        y => resolProsRoService.GetAll()
                            .Where(x => x.RealityObject.Id == realityObjectId)
                            .Any(x => x.ResolPros.Id == y.Id))
                    .WhereIf(municipalityList.Count > 0,
                        y => resolProsRoService.GetAll()
                            .Where(x => municipalityList.Contains(x.RealityObject.Municipality.Id))
                            .Any(x => x.ResolPros.Id == y.Id))
                    .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                    .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                    .WhereIf(stageId > 0, x => x.Stage.Id == stageId);

                //словарь id пост.прокуратуры - наименование мо
                var resolProsMuDict = useResolProsRoService
                    ? resolProsRoService.GetAll()
                        .WhereIf(dateStart != DateTime.MinValue, x => x.ResolPros.DocumentDate >= dateStart)
                        .WhereIf(dateEnd != DateTime.MinValue, x => x.ResolPros.DocumentDate <= dateEnd)
                        .WhereIf(municipalityList.Count > 0, x => municipalityList.Contains(x.RealityObject.Municipality.Id))
                        .Select(x => new
                        {
                            x.ResolPros.Id,
                            x.RealityObject.Municipality.Name
                        })
                        .Where(x => x.Name != null)
                        .AsEnumerable()
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key, y => y.Select(x => x.Name).First())
                    : new Dictionary<long, string>();

                var resolutionsFromDocumentGji = documentGjiService.GetAll()
                    .Join(resolutionService.GetAll(),
                        f => f.Id,
                        s => s.Id,
                        (documentGji, resolution) => new
                        {
                            documentGji.DocumentDate,
                            resolution.Id,
                            InspectionId = documentGji.Stage.Inspection.Id,
                            Inspector = resolution.Official != null ? resolution.Official.Fio : "",
                            resolution.PenaltyAmount,
                            Uin = resolution.GisUin
                        })
                    // Исключаем постановления, которые были возвращены решением суда на новое рассмотрение
                    .Where(x => !resolutionDisputeDomain.GetAll().Any(y => y.Resolution.Id == x.Id && y.CourtVerdict.Code == "3"))
                    .Where(w => resolPros.Select(s => s.Inspection.Id).Contains(w.InspectionId)
                        && !resolPros.Select(s => s.Id).Contains(w.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.InspectionId)
                    .ToDictionary(d => d.Key, y => y.OrderByDescending(x => x.DocumentDate).FirstOrDefault());

                var data = resolPros
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        DocumentNumber = x.DocumentNumber ?? "",
                        x.DocumentDate,
                        Municipality = x.Municipality != null ? x.Municipality.Name : "",
                        Executant = x.Executant != null ? x.Executant.Name : "",
                        InspectionId = x.Inspection != null ? x.Inspection.Id : 0,
                        Contragent = x.Contragent != null ? x.Contragent.Name : "",
                        PhysicalPerson = x.PhysicalPerson ?? ""
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        resolutionsFromDocumentGji.TryGetValue(x.InspectionId, out var resolutionFromDocumentGji);

                        return new ResolProsAndResolution
                        {
                            Id = x.Id,
                            State = x.State,
                            DocumentNumber = x.DocumentNumber,
                            DocumentDate = x.DocumentDate,
                            Municipality = x.Municipality.Length == 0 ? resolProsMuDict.Get(x.Id) : x.Municipality,
                            Executant = x.Executant,
                            InspectionId = x.InspectionId,
                            Contragent = x.Contragent,
                            PhysicalPerson = x.PhysicalPerson,

                            Inspector = resolutionFromDocumentGji?.Inspector,
                            PenaltyAmount = resolutionFromDocumentGji?.PenaltyAmount,
                            Uin = toExcel ? $"'{resolutionFromDocumentGji?.Uin}" : resolutionFromDocumentGji?.Uin
                        };
                    })
                    .AsQueryable()
                    .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                    .Filter(loadParam, this.Container)
                    .Order(loadParam)
                    .ToList();

                return data;
            }
            finally
            {
                this.Container.Release(resolutionService);
                this.Container.Release(documentGjiService);
                this.Container.Release(resolProsRoService);
                this.Container.Release(resolutionDisputeDomain);
                this.Container.Release(userManager);
            }
        }
    }
}