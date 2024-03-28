namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Views;
	using Bars.GkhGji;

    using Castle.Windsor;

    public class Protocol197Service: IProtocol197Service
    {
        public IWindsorContainer Container { get; set; }
		public IDomainService<DocumentGjiInspector> ServiceInspectorDomain { get; set; }
		public IDomainService<DocumentGjiChildren> ServiceChildrenDomain { get; set; }
		public IDomainService<Protocol197SurveySubjectRequirement> Protocol197SurveySubjectRequirementDomain { get; set; }
		public IDomainService<Protocol197ActivityDirection> Protocol197ActivityDirectionDomain { get; set; }

        public virtual IDataResult ListView(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            /*
             * В качестве фильтров приходят следующие параметры
             * dateStart - Необходимо получить документы больше даты начала
             * dateEnd - Необходимо получить документы меньше даты окончания
             * realityObjectId - Необходимо получить документы по дому
            */

            var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");

            var data = this.GetViewList()
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .WhereIf(realityObjectId > 0, x => x.RealityObjectIds.Contains("/" + realityObjectId.ToString() + "/"))
                .Select(x => new
                    {
                        x.Id,
                        x.State,
                        x.ContragentName,
                        MunicipalityNames = x.MunicipalityId != null ? x.MunicipalityNames : x.ContragentMuName,
                        MoSettlement = x.MoNames,
                        PlaceName = x.PlaceNames,
                        MunicipalityId = x.MunicipalityId ?? x.ContragentMuId,
                        x.TypeExecutant,
                        x.CountViolation,
                        x.InspectorNames,
                        x.DocumentDate,
                        x.DocumentNumber,
                        x.DocumentNum,
                        x.InspectionId,
                        x.TypeBase,
                        x.TypeDocumentGji,
						x.ArticleLaw,
						x.ControlType,
						HasResolution = GetResolution(x.Id, x.DocumentDate)
				})
                .Filter(loadParam, this.Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

		private bool GetResolution(long protocolId, DateTime? dateToCourt)
		{
			if (!dateToCourt.HasValue)
			{
				return true;
			}
			if (dateToCourt.Value.AddMonths(2) > DateTime.Now)
			{
				return true;
			}
			var childrenId = this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
			 .Where(x => x.Parent.Id == protocolId && x.Children.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Resolution)
			.Select(x => x.Children.Id).FirstOrDefault();


			return childrenId > 0;

		}

		public virtual IQueryable<ViewProtocol197> GetViewList()
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var serviceDocumentInspector = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var serviceViewProtocol = this.Container.Resolve<IDomainService<ViewProtocol197>>();

            try
            {
                var inspectorList = userManager.GetInspectorIds();
                var municipalityList = userManager.GetMunicipalityIds();
                inspectorList.Clear();//убираем проверку на инспектора
                return serviceViewProtocol.GetAll()
                                .WhereIf(inspectorList.Count > 0, y => serviceDocumentInspector.GetAll()
                                    .Any(x => y.Id == x.DocumentGji.Id
                                        && inspectorList.Contains(x.Inspector.Id)))
                                .WhereIf(municipalityList.Count > 0, x => x.MunicipalityId.HasValue && municipalityList.Contains(x.MunicipalityId.Value));
            }
            finally 
            {
                this.Container.Release(userManager);
                this.Container.Release(serviceDocumentInspector);
                this.Container.Release(serviceViewProtocol);
            }
        }

		public virtual IDataResult GetInfo(long? documentId)
		{
			try
			{
				var inspectorNames = string.Empty;
				var inspectorIds = string.Empty;

				// Сначала пробегаемся по инспекторам и формируем итоговую строку наименований и строку идентификаторов
				var inspectors =
					this.ServiceInspectorDomain.GetAll()
						.Where(x => x.DocumentGji.Id == documentId)
						.Select(x => new {x.Inspector.Id, x.Inspector.Fio})
						.ToList();

				foreach (var item in inspectors)
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

					inspectorIds += item.Id;
				}

				var reqs = this.Protocol197SurveySubjectRequirementDomain.GetAll()
					.Where(x => x.Protocol197.Id == documentId)
					.Select(x => x.Requirement)
					.Select(x => new {x.Id, x.Name})
					.ToArray();

				var dirs = this.Protocol197ActivityDirectionDomain.GetAll()
					.Where(x => x.Protocol197.Id == documentId)
					.Select(x => x.ActivityDirection)
					.Select(x => new {x.Id, x.Name})
					.ToArray();

				return
					new BaseDataResult(
						new Protocol197GetInfoProxy
						{
							inspectorNames = inspectorNames,
							inspectorIds = inspectorIds,
							requirementIds = string.Join(", ", reqs.Select(x => x.Id)),
							requirementNames = string.Join(", ", reqs.Select(x => x.Name)),
							directionIds = string.Join(", ", dirs.Select(x => x.Id)),
							directionNames = string.Join(", ", dirs.Select(x => x.Name))
						});
			}
			catch (ValidationException e)
			{
				return new BaseDataResult {Success = false, Message = e.Message};
			}
		}

		public IDataResult AddRequirements(BaseParams baseParams)
		{
			var documentId = baseParams.Params.GetAsId("documentId");
			var reqIds = baseParams.Params.GetAs<string>("requirementIds").Return(x => x.Split(',').Select(y => y.ToLong()));

			var reqs = this.Protocol197SurveySubjectRequirementDomain.GetAll()
				.Where(x => x.Protocol197.Id == documentId)
				.Select(x => new {x.Id, ProtocolId = x.Protocol197.Id, RequirementId = x.Requirement.Id})
				.ToArray();

			this.Container.InTransaction(() =>
			{
				foreach (var req in reqs.Where(x => !reqIds.Contains(x.RequirementId)))
				{
					this.Protocol197SurveySubjectRequirementDomain.Delete(req.Id);
				}

				foreach (var reqId in reqIds)
				{
					if (reqs.Any(x => x.RequirementId == reqId))
					{
						continue;
					}

					this.Protocol197SurveySubjectRequirementDomain.Save(
						new Protocol197SurveySubjectRequirement
						{
							Protocol197 = new Protocol197 {Id = documentId},
							Requirement = new SurveySubjectRequirement {Id = reqId}
						});
				}
			});

			return new BaseDataResult();
		}

		public IDataResult AddDirections(BaseParams baseParams)
		{
			var documentId = baseParams.Params.GetAsId("documentId");
			var dirIds = baseParams.Params.GetAs<string>("directionIds").Return(x => x.Split(',').Select(y => y.ToLong()));

			var reqs = this.Protocol197ActivityDirectionDomain.GetAll()
				.Where(x => x.Protocol197.Id == documentId)
				.Select(x => new {x.Id, ProtocolId = x.Protocol197.Id, DirectionId = x.ActivityDirection.Id})
				.ToArray();

			this.Container.InTransaction(() =>
			{
				foreach (var dir in reqs.Where(x => !dirIds.Contains(x.DirectionId)))
				{
					this.Protocol197ActivityDirectionDomain.Delete(dir.Id);
				}

				foreach (var dirId in dirIds)
				{
					if (reqs.Any(x => x.DirectionId == dirId))
					{
						continue;
					}

					this.Protocol197ActivityDirectionDomain.Save(
						new Protocol197ActivityDirection
						{
							Protocol197 = new Protocol197 {Id = documentId},
							ActivityDirection = new ActivityDirection {Id = dirId}
						});
				}
			});

			return new BaseDataResult();
		}

		public class Protocol197GetInfoProxy
		{
			public string inspectorNames { get; set; }
			public string inspectorIds { get; set; }
			public string requirementIds { get; set; }
			public string requirementNames { get; set; }
			public string directionIds { get; set; }
			public string directionNames { get; set; }
		}
    }
}