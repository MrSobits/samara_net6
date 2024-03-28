namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Linq;
    using Castle.Windsor;

    using B4;
    using B4.Utils;

    using Bars.Gkh.Utils;

    using Enums;
    using Entities;
    using Gkh.Domain;
    using Gkh.Enums;
    using Gkh.Authentification;

	/// <summary>
	/// Сервис для работы с Акт без взаимодействия
	/// </summary>
    public class ActIsolatedService : IActIsolatedService
    {
		/// <summary>
		/// Контейнер
		/// </summary>
        public IWindsorContainer Container { get; set; }

		/// <summary>
		/// Домен сервис для Инспектор документа ГЖИ
		/// </summary>
        public IDocumentGjiInspectorService InspectorDomain { get; set; }

        /// <summary>
        /// Домен сервис для Дом акта без взаимодействия
        /// </summary>
        public IDomainService<ActIsolatedRealObj> ActIsolatedRoDomain { get; set; }

        /// <summary>
        /// Домен сервис для Акт без взаимодействия
        /// </summary>
        public IDomainService<ActIsolated> ActIsolatedDomain { get; set; }

        /// <summary>
        /// Получить информацию (для карточки Акта)
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public virtual IDataResult GetInfo(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");
                
            // получаем адрес дома и количество домов
            var realityObjAddress = string.Empty;
            var realityObjCount = 0;

            var actCheckRealityObjs = this.ActIsolatedRoDomain.GetAll().Where(x => x.ActIsolated.Id == documentId).ToList();
            if (actCheckRealityObjs.Count == 1)
            {
                var actCheckRealityObj = actCheckRealityObjs.FirstOrDefault();
                realityObjCount = 1;
                if (actCheckRealityObj != null)
                {
                    realityObjAddress = actCheckRealityObj.RealityObject?.FiasAddress != null 
                        ? actCheckRealityObj.RealityObject.FiasAddress.AddressName 
                        : string.Empty;
                }
            }

            // получаем основание проверки
            var typeBase = this.ActIsolatedDomain.GetAll()
                .Where(x => x.Id == documentId)
                .Select(x => x.Inspection.TypeBase)
                .FirstOrDefault();

            var inspectorNames = string.Empty;
            var inspectorIds = string.Empty;
            
            var dataInspectors = this.InspectorDomain.GetInspectorsByDocumentId(documentId)
                .Select(x => new
                {
                    InspectorId = x.Inspector.Id,
                    x.Inspector.Fio
                })
                .ToList();

            foreach (var item in dataInspectors)
            {
                if (!string.IsNullOrEmpty(inspectorNames))
                {
                    inspectorNames += ", ";
                }
                inspectorNames += item.Fio;

                if (!string.IsNullOrEmpty(inspectorIds))
                {
                    inspectorIds += ", ";
                }

                inspectorIds += item.InspectorId.ToString();
            }

            // получаем вид проверки из приказа (распоряжения)
            var parentDisposalKindCheck = this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                .Where(x => x.Children.Id == documentId && x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal)
                .Select(x => (x.Parent as Disposal).KindCheck)
                .FirstOrDefault();

            var typeCheck = parentDisposalKindCheck != null ? parentDisposalKindCheck.Code : 0;

	        return
		        new BaseDataResult(
			        new
			        {
				        inspectorNames,
				        inspectorIds,
				        typeBase,
				        realityObjAddress,
				        realityObjCount,
				        typeCheck
			        });
        }

		/// <summary>
		/// Получить список из вьюхи
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public virtual IDataResult ListView(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");

            var serviceActCheckRobject = this.Container.Resolve<IDomainService<ActIsolatedRealObj>>();
            var serviceDocumentChildren = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();

            var data = this.GetViewList()
                .WhereIf(dateStart > DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd > DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .WhereIf(realityObjectId > 0, y => serviceActCheckRobject.GetAll().Any(x => x.ActIsolated.Id == y.Id && x.RealityObject.Id == realityObjectId))
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.DocumentDate,
                    x.DocumentNumber,
                    x.DocumentNum,
                    MunicipalityNames = x.MunicipalityId != null ? x.MunicipalityNames : x.ContragentMuName,
                    MoSettlement = x.MoNames,
                    PlaceName = x.PlaceNames,
                    MunicipalityId = x.MunicipalityId ?? x.ContragentMuId,
                    x.ContragentName,
                    RealityObjectCount = serviceActCheckRobject.GetAll().Count(y => y.ActIsolated.Id == x.Id),
                    CountExecDoc = serviceDocumentChildren.GetAll()
                            .Count(y => y.Parent.Id == x.Id && (y.Children.TypeDocumentGji == TypeDocumentGji.Protocol || y.Children.TypeDocumentGji == TypeDocumentGji.Prescription)),
                    HasViolation = serviceActCheckRobject.GetAll().Any(y => y.ActIsolated.Id == x.Id && y.HaveViolation == YesNoNotSet.Yes),
                    x.InspectorNames,
                    x.InspectionId,
                    x.TypeBase,
                    x.TypeDocumentGji
                });

            return data.ToListDataResult(loadParam, this.Container);
        }

        /// <summary>
        /// Получить список из вьюхи
        /// </summary>
        /// <returns>Результат выполнения</returns>
        public IQueryable<ViewActIsolated> GetViewList()
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var inspectorIds = userManager.GetInspectorIds();
            var municipalityids = userManager.GetMunicipalityIds();

            var serviceDocumentInspector = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var serviceActCheckRobject = this.Container.Resolve<IDomainService<ActIsolatedRealObj>>();
            var serviceViewActIsolated = this.Container.Resolve<IDomainService<ViewActIsolated>>();

            try
            {
                return serviceViewActIsolated.GetAll()
               .WhereIf(inspectorIds.Count > 0, actCheck => serviceDocumentInspector.GetAll()
                   .Any(docInsp => docInsp.DocumentGji.Id == actCheck.Id && inspectorIds.Contains(docInsp.Inspector.Id)))
               .WhereIf(municipalityids.Count > 0, y => serviceActCheckRobject.GetAll()
                   .Any(x => x.ActIsolated.Id == y.Id && municipalityids.Contains(x.RealityObject.Municipality.Id)));
            }
            finally
            {
                this.Container.Release(serviceDocumentInspector);
                this.Container.Release(serviceActCheckRobject);
                this.Container.Release(serviceViewActIsolated);
            }
        }

        /// <inheritdoc />
        public IDataResult ListForStage(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var actIsolatedService = this.Container.Resolve<IDomainService<ActIsolated>>();
            var actIsolatedRealObjService = this.Container.Resolve<IDomainService<ActIsolatedRealObj>>();

            try
            {
                var stageId = baseParams.Params.GetAs("stageId", 0L);

                var dictRoAddress = actIsolatedRealObjService.GetAll()
                    .Where(x => x.ActIsolated.Stage.Id == stageId)
                    .Where(x => x.RealityObject != null)
                    .Select(x =>
                        new
                        {
                            ActId = x.ActIsolated.Id,
                            x.RealityObject.Address
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.ActId)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Address).FirstOrDefault());

                var data = actIsolatedService.GetAll()
                    .Where(x => x.Stage.Id == stageId)
                    .Select(x => new { x.Id, x.TypeDocumentGji, x.DocumentDate, x.DocumentNumber, x.State })
                    .AsEnumerable()
                    .Select(x =>
                        new
                        {
                            x.Id,
                            DocumentId = x.Id,
                            x.TypeDocumentGji,
                            x.DocumentDate,
                            x.DocumentNumber,
                            x.State,
                            Address = dictRoAddress.ContainsKey(x.Id) ? dictRoAddress[x.Id] : null
                        })
                    .AsQueryable()
                    .Filter(loadParam, this.Container)
                    .Order(loadParam)
                    .ToList();

                int totalCount = data.Count();

                return new ListDataResult(data, totalCount);
            }
            finally
            {
                this.Container.Release(actIsolatedService);
                this.Container.Release(actIsolatedRealObjService);
            }
        }
    }
}