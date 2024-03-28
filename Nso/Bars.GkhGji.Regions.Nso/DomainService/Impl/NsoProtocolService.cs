namespace Bars.GkhGji.Regions.Nso.DomainService.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
	using Bars.B4.DataAccess;
	using Bars.B4.Utils;
	using Bars.Gkh.Domain;
	using Bars.GkhGji.DomainService;
	using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Utils;
    using Bars.GkhGji.Regions.Nso.Entities;

	/// <summary>
    /// Сервис работы с протоколами
    /// </summary>
	public class NsoProtocolService : ProtocolService, INsoProtocolService
    {
        /// <summary>
        /// Получить информацию по протоколу
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        /// <returns>Информация по протоколу</returns>
        public override IDataResult GetInfo(long? documentId)
        {
            var baseResult = base.GetInfo(documentId);
            if (!baseResult.Success)
            {
                return baseResult;
            }

            var info = (ProtocolGetInfoProxy)baseResult.Data;
            var reqService = this.Container.ResolveDomain<NsoProtocolSurveySubjectRequirement>();
            var dirService = this.Container.ResolveDomain<ProtocolActivityDirection>();
            var childrenService = this.Container.ResolveDomain<DocumentGjiChildren>();
            var violService = this.Container.ResolveDomain<ProtocolViolation>();
            try
            {
                var reqs =
                    reqService.GetAll()
                              .Where(x => x.Protocol.Id == documentId)
                              .Select(x => x.Requirement)
                              .Select(x => new { x.Id, x.Name })
                              .ToArray();
                var dirs =
                    dirService.GetAll()
                              .Where(x => x.Protocol.Id == documentId)
                              .Select(x => x.ActivityDirection)
                              .Select(x => new { x.Id, x.Name })
                              .ToArray();
                var disposal =
                    Utils.GetParentDocumentByType(
                        childrenService,
                        new Protocol { Id = documentId.ToLong() },
                        TypeDocumentGji.Disposal) as Disposal;
                var viols =
                    violService.GetAll()
                               .Where(x => x.Document.Id == documentId)
                               .Select(x => x.InspectionViolation.RealityObject)
                               .ToArray();
                return
                    new BaseDataResult(
                        new NsoProtocolGetInfoProxy
                            {
                                InspectorNames = info.InspectorNames,
                                BaseName = info.BaseName,
                                InspectorIds = info.InspectorIds,
                                requirementIds = string.Join(", ", reqs.Select(x => x.Id)),
                                requirementNames = string.Join(", ", reqs.Select(x => x.Name)),
                                directionIds = string.Join(", ", dirs.Select(x => x.Id)),
                                directionNames = string.Join(", ", dirs.Select(x => x.Name)),
                                documentGjiCheck =
                                    disposal.Return(
                                        x => x.TypeDisposal == TypeDisposalGji.DocumentGji),
                                hasRealityObjects = viols.Length > 0 && viols.Any(x => x != null)
                            });
            }
            finally
            {
                this.Container.Release(reqService);
                this.Container.Release(childrenService);
                this.Container.Release(dirService);
                this.Container.Release(violService);
            }
        }

        /// <summary>
        /// Добавить требования
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult AddRequirements(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");
            var reqIds =
                baseParams.Params.GetAs<string>("requirementIds").Return(x => x.Split(',').Select(y => y.ToLong()));

            var domain = this.Container.ResolveDomain<NsoProtocolSurveySubjectRequirement>();
            try
            {
                var reqs =
                    domain.GetAll()
                          .Where(x => x.Protocol.Id == documentId)
                          .Select(x => new { x.Id, ProtocolId = x.Protocol.Id, RequirementId = x.Requirement.Id })
                          .ToArray();

                this.Container.InTransaction(
                    () =>
                        {
                            foreach (var req in reqs.Where(x => !reqIds.Contains(x.RequirementId)))
                            {
                                domain.Delete(req.Id);
                            }

                            foreach (var reqId in reqIds)
                            {
                                if (reqs.Any(x => x.RequirementId == reqId))
                                {
                                    continue;
                                }

                                domain.Save(
                                    new NsoProtocolSurveySubjectRequirement
                                        {
                                            Protocol =
                                                new NsoProtocol { Id = documentId },
                                            Requirement =
                                                new SurveySubjectRequirement
                                                    {
                                                        Id
                                                            =
                                                            reqId
                                                    }
                                        });
                            }
                        });

                return new BaseDataResult();
            }
            finally
            {
                this.Container.Release(domain);
            }
        }

        /// <summary>
        /// Добавить направления деятельности
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult AddDirections(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");
            var dirIds =
                baseParams.Params.GetAs<string>("directionIds").Return(x => x.Split(',').Select(y => y.ToLong()));

            var domain = this.Container.ResolveDomain<ProtocolActivityDirection>();
            try
            {
                var reqs =
                    domain.GetAll()
                          .Where(x => x.Protocol.Id == documentId)
                          .Select(x => new { x.Id, ProtocolId = x.Protocol.Id, DirectionId = x.ActivityDirection.Id })
                          .ToArray();

                this.Container.InTransaction(
                    () =>
                        {
                            foreach (var dir in reqs.Where(x => !dirIds.Contains(x.DirectionId)))
                            {
                                domain.Delete(dir.Id);
                            }

                            foreach (var dirId in dirIds)
                            {
                                if (reqs.Any(x => x.DirectionId == dirId))
                                {
                                    continue;
                                }

                                domain.Save(
                                    new ProtocolActivityDirection
                                        {
                                            Protocol = new NsoProtocol { Id = documentId },
                                            ActivityDirection =
                                                new ActivityDirection { Id = dirId }
                                        });
                            }
                        });

                return new BaseDataResult();
            }
            finally
            {
                this.Container.Release(domain);
            }
        }

        /// <summary>
        /// Вернуть список текущих направлений деятельности по протоколу
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Список направлений</returns>
	    public IDataResult ListDirections(BaseParams baseParams)
	    {
            var domain = this.Container.ResolveDomain<ProtocolActivityDirection>();

	        try
	        {
	            var documentId = baseParams.Params.GetAsId("documentId");

	            var data = domain.GetAll()
	                .Where(x => x.Protocol.Id == documentId)
                    .Select(x => x.ActivityDirection);

	            var count = data.Count();
	            var result = data.ToArray();

                return new ListDataResult(result, count);

	        }
	        catch (Exception e)
	        {
                return new BaseDataResult(false, e.Message);
	        }
	        finally
	        {
                this.Container.Release(domain);
	        }
        }
    }

    /// <summary>
    /// Класс-прокси для получения информации по протоколу
    /// </summary>
    public class NsoProtocolGetInfoProxy : ProtocolGetInfoProxy
    {
        /// <summary>
        /// Идентификаторы требований
        /// </summary>
        public string requirementIds { get; set; }

        /// <summary>
        /// Наименования требований
        /// </summary>
        public string requirementNames { get; set; }

        /// <summary>
        /// Идентификаторы направления деятельности
        /// </summary>
        public string directionIds { get; set; }

        /// <summary>
        /// Наименования направлений деятельности
        /// </summary>
        public string directionNames { get; set; }

        /// <summary>
        /// На проверку документа ГЖИ
        /// </summary>
        public bool documentGjiCheck { get; set; }

        /// <summary>
        /// Имеются нарушения по домам
        /// </summary>
        public bool hasRealityObjects { get; set; }
    }
}