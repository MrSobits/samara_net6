namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Castle.Windsor;

    using B4;
    using B4.Utils;

    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.Entities.Dict;

    using Enums;
    using Entities;
    using Gkh.Domain;
    using Bars.GkhGji.Entities.Dict;
    using Gkh.Authentification;

    using Newtonsoft.Json;

    /// <summary>
	/// Сервис для работы с Акт проверки
	/// </summary>
    public class ActCheckService : IActCheckService
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
		/// Домен сервис для Дом акта проверки
		/// </summary>
        public IDomainService<ActCheckRealityObject> ActCheckRoDomain { get; set; }

		/// <summary>
		/// Домен сервис для Нарушение акта проверки
		/// </summary>
		public IDomainService<ActCheckViolation> ActCheckViolDomain { get; set; }

		/// <summary>
		/// Домен сервис для Акт проверки
		/// </summary>
		public IDomainService<ActCheck> ActCheckDomain { get; set; }

		/// <summary>
		/// Получить информацию
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        public virtual IDataResult GetInfo(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");
                
            // получаем адрес дома и количество домов
            var realityObjAddress = string.Empty;
            var realityObjCount = 0;
	        var personsWhoHaveViolated = string.Empty;
			var officialsGuiltyActions = string.Empty; 

            var actCheckRealityObjs = this.ActCheckRoDomain.GetAll().Where(x => x.ActCheck.Id == documentId).ToList();
            if (actCheckRealityObjs.Count == 1)
            {
                var actCheckRealityObj = actCheckRealityObjs.FirstOrDefault();
                realityObjCount = 1;
                if (actCheckRealityObj != null)
                {
                    realityObjAddress = actCheckRealityObj.RealityObject != null && actCheckRealityObj.RealityObject.FiasAddress != null ? actCheckRealityObj.RealityObject.FiasAddress.AddressName : string.Empty;
	                personsWhoHaveViolated = actCheckRealityObj.PersonsWhoHaveViolated;
					officialsGuiltyActions = actCheckRealityObj.OfficialsGuiltyActions;
                }
            }

            // поулчаем основание проверки
            var typeBase = this.ActCheckDomain.GetAll().Where(x => x.Id == documentId).Select(x => x.Inspection.TypeBase).FirstOrDefault();

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

            // Получаем Признак выявлены или не выявлены нарушения?
            var isExistViolation = this.ActCheckViolDomain.GetAll().Any(x => x.Document.Id == documentId);


            // получаем вид проверки из приказа (распоряжения)
            var parentDisposalKindCheck = this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                .Where(x => x.Children.Id == documentId && x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal)
                .Select(x => (x.Parent as Disposal).KindCheck)
                .FirstOrDefault();

            var typeCheck = parentDisposalKindCheck != null ? parentDisposalKindCheck.Code : 0;
            
            var result = new ActCheckInfo
            {
	            InspectorNames = inspectorNames,
	            InspectorIds = inspectorIds,
	            TypeBase = typeBase,
	            RealityObjAddress = realityObjAddress,
	            RealityObjCount = realityObjCount,
	            IsExistViolation = isExistViolation,
	            TypeCheck = typeCheck,
	            PersonsWhoHaveViolated = personsWhoHaveViolated,
	            OfficialsGuiltyActions = officialsGuiltyActions
            };

	        return new BaseDataResult(result);
        }
		
		public IDataResult AddActCheckControlMeasures(BaseParams baseParams)
        {
            try
            {
                var documentId = baseParams.Params.ContainsKey("documentId") ? baseParams.Params["documentId"].ToLong() : 0;
                var controlMeasuresIds =
                    baseParams.Params.ContainsKey("controlMeasuresIds")
                        ? baseParams.Params["controlMeasuresIds"].ToString()
                        : string.Empty;

                var disposal = ActCheckDomain.Get(documentId);

                if (!string.IsNullOrEmpty(controlMeasuresIds))
                {
                    // список уже добавленных мероприятий по контролю
                    var listTypes =
                        this.ActCheckControlMeasuresDomain
                            .GetAll()
                            .Where(x => x.ActCheck.Id == documentId)
                            .Select(x => x.ControlActivity.Id)
                            .ToList();

                    foreach (var controlMeasureIds in controlMeasuresIds.Split(','))
                    {
                        var newId = controlMeasureIds.ToLong();

                        if (!listTypes.Contains(newId))
                        {
                            string controlActivityName = ControlActivityDomain.Get(newId).Name;
                            var newObj = new ActCheckControlMeasures
                            {
                                ActCheck = new GkhGji.Entities.ActCheck { Id = documentId },
                                ControlActivity = new ControlActivity { Id = newId },
                                Description = controlActivityName
                            };

                            this.ActCheckControlMeasuresDomain.Save(newObj);
                        }
                    }
                }
                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }
		
        public IDomainService<ActCheckControlMeasures> ActCheckControlMeasuresDomain { get; set; }
        public IDomainService<ControlActivity> ControlActivityDomain { get; set; }

		/// <summary>
		/// Получить список из вьюхи
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public virtual IDataResult ListView(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var isExport = baseParams.Params.GetAs<bool>("isExport");
            var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");

            var query = this.GetViewList()
		        .WhereIf(dateStart > DateTime.MinValue, x => x.DocumentDate >= dateStart)
		        .WhereIf(dateEnd > DateTime.MinValue, x => x.DocumentDate <= dateEnd)
		        .WhereIf(realityObjectId > 0, x => x.RealityObjectIds.Contains("/" + realityObjectId + "/"))
		        .Select(x => new
		        {
			        x.Id,
			        x.State,
			        x.DocumentDate,
			        x.DocumentNumber,
			        x.DocumentNum,
			        MunicipalityNames = x.MunicipalityId != null ? x.MunicipalityNames : x.ContragentMuName,
			        MunicipalityId = x.MunicipalityId ?? x.ContragentMuId,
			        PersonInspectionAddress = x.RealityObjectAddresses.Replace(";", ","),
			        x.ContragentName,
			        x.RealityObjectCount,
			        x.CountExecDoc,
			        x.HasViolation,
			        x.InspectorNames,
			        x.InspectionId,
			        x.TypeBase,
			        x.TypeDocumentGji
		        })
		        .Filter(loadParam, this.Container);

	        var totalCount = query.Count();

	        var orderField = loadParam.Order.FirstOrDefault(x => x.Name == "State");

	        query = orderField != null
                ? orderField.Asc
                    ? query.OrderBy(x => x.State.Code)
                    : query.OrderByDescending(x => x.State.Code)
                : query.Order(loadParam);

	        return new ListDataResult((!isExport ? query.Paging(loadParam) : query).ToList(), totalCount);
	    }

        /// <summary>
        /// В данном методе идет получение списка актов по Этапу проверки (в этапе может быть больше 1 акта)
        /// </summary>
        public IDataResult ListForStage(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var actCheckService = this.Container.Resolve<IDomainService<ActCheck>>();
            var actCheckViolService = this.Container.Resolve<IDomainService<ActCheckRealityObject>>();

            try
            {
                var stageId = baseParams.Params.GetAs("stageId", 0L);

                var dictRoAddress =
                    actCheckViolService.GetAll()
                                       .Where(x => x.ActCheck.Stage.Id == stageId)
                                       .Where(x => x.RealityObject != null)
                                       .Select(
                                           x =>
                                           new
                                               {
                                                   actId = x.ActCheck.Id,
                                                   address = x.RealityObject.Address
                                               })
                                       .AsEnumerable()
                                       .GroupBy(x => x.actId)
                                       .ToDictionary(x => x.Key, y => y.Select(x => x.address).FirstOrDefault());


                var data =
                    actCheckService.GetAll()
                                   .Where(x => x.Stage.Id == stageId)
                                   .Select(x => new { x.Id, x.TypeDocumentGji, x.DocumentDate, x.DocumentNumber, x.State })
                                   .AsEnumerable()
                                   .Select(
                                       x =>
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
	            this.Container.Release(actCheckService);
	            this.Container.Release(actCheckViolService);
            }
        }

		/// <summary>
		/// Получить список из вьюхи
		/// </summary>
		/// <returns>Результат выполнения</returns>
        public IQueryable<ViewActCheck> GetViewList()
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var inspectorIds = userManager.GetInspectorIds();
            var municipalityids = userManager.GetMunicipalityIds();

            var serviceDocumentInspector = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var serviceActCheckRobject = this.Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var serviceViewActCheck = this.Container.Resolve<IDomainService<ViewActCheck>>();

            try
            {
                return serviceViewActCheck.GetAll()
               .WhereIf(inspectorIds.Count > 0, actCheck => serviceDocumentInspector.GetAll()
                   .Any(docInsp => docInsp.DocumentGji.Id == actCheck.Id && inspectorIds.Contains(docInsp.Inspector.Id)))
               .WhereIf(municipalityids.Count > 0, y => serviceActCheckRobject.GetAll()
                   .Any(x => x.ActCheck.Id == y.Id && municipalityids.Contains(x.RealityObject.Municipality.Id)));
            }
            finally
            {
	            this.Container.Release(serviceDocumentInspector);
	            this.Container.Release(serviceActCheckRobject);
	            this.Container.Release(serviceViewActCheck);
            }
        }

		/// <inheritdoc />
		public ActionResult Export(BaseParams baseParams)
		{
			var export = this.Container.Resolve<IDataExportService>(this.GetDataExportRegistrationName());

			using (this.Container.Using(export))
			{
				return export?.ExportData(baseParams);
			}
		}

		/// <summary>
		/// Получить наименование, под которым зарегистрирован сервис
		/// </summary>
		/// <returns></returns>
		protected virtual string GetDataExportRegistrationName() => "ActCheckDataExport";
    }

	public class ActCheckInfo
	{
		[JsonProperty("inspectorNames")]
		public string InspectorNames { get; set; }

		[JsonProperty("inspectorIds")]
		public string InspectorIds { get; set; }

		[JsonProperty("typeBase")]
		public TypeBase TypeBase { get; set; }

		[JsonProperty("realityObjAddress")]
		public string RealityObjAddress { get; set; }

		[JsonProperty("realityObjCount")]
		public int RealityObjCount { get; set; }

		[JsonProperty("isExistViolation")]
		public bool IsExistViolation { get; set; }
        
		[JsonProperty("typeCheck")]
		public TypeCheck TypeCheck { get; set; }

		[JsonProperty("personsWhoHaveViolated")]
		public string PersonsWhoHaveViolated { get; set; }

		[JsonProperty("officialsGuiltyActions")]
		public string OfficialsGuiltyActions { get; set; }

		[JsonProperty("controlTypeId")]
		public long ControlTypeId { get; set; }
	}
}