namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService
{
    using Entities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
  

    using Castle.Windsor;
    using System.Text;

    public class CourtPracticeOperationsService : ICourtPracticeOperationsService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<CourtPractice> CourtPracticeDomain { get; set; }

        public IDomainService<CourtPracticeRealityObject> CourtPracticeRealityObjectDomain { get; set; }

        public IDomainService<KindKNDDictArtLaw> KindKNDDictArtLawDomain { get; set; }

        public IDomainService<ResolutionDecision> ResolutionDecisionService { get; set; }
        public IDomainService<AppealCitsDecision> AppealCitsDecisionService { get; set; }
        public IDomainService<AppealCitsDefinition> AppealCitsDefinitionService { get; set; }
        public IDomainService<ResolutionDefinition> ResolutionDefinitionService { get; set; }

        public IDataResult GetInfo(BaseParams baseParams)
        {
            var serviceCourtPracticePrescription = Container.Resolve<IDomainService<CourtPracticePrescription>>();
            try
            {
                var courtpracticeId = baseParams.Params.ContainsKey("courtpracticeId") ? baseParams.Params["courtpracticeId"].ToLong() : 0;

                var docNames = new StringBuilder();
                var docIds = new StringBuilder();


                // Пробегаемся по инспекторам и формируем итоговую строку наименований и строку идентификаторов


                var dataDocs = serviceCourtPracticePrescription.GetAll()
                    .Where(x => x.CourtPractice.Id == courtpracticeId)
                    .Select(x => new
                    {
                        x.DocumentGji.Id,
                        DocumentNumber = x.DocumentGji.DocumentDate.HasValue ? x.DocumentGji.DocumentNumber + " от " + x.DocumentGji.DocumentDate.Value.ToString("dd.MM.yyyy") : x.DocumentGji.DocumentNumber
                    })
                    .ToArray();


                foreach (var item in dataDocs)
                {
                    if (!string.IsNullOrEmpty(item.DocumentNumber))
                    {
                        if (docNames.Length > 0)
                            docNames.Append(", ");

                        docNames.Append(item.DocumentNumber);
                    }

                    if (item.Id > 0)
                    {
                        if (docIds.Length > 0)
                            docIds.Append(", ");

                        docIds.Append(item.Id);
                    }
                }

                return new BaseDataResult(new
                {
                    docNames = docNames.ToString(),
                    docIds = docIds.ToString()
                });
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
            finally
            {
                this.Container.Release(serviceCourtPracticePrescription);
            }
        }

        public IDataResult AddDocs(BaseParams baseParams)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var courtpracticeId = baseParams.Params.ContainsKey("courtpracticeId") ? baseParams.Params["courtpracticeId"].ToLong() : 0;
                    var docIds = baseParams.Params.GetAs<long[]>("objectIds") ?? new long[0];

                    var serviceDoc = Container.Resolve<IDomainService<DocumentGji>>();
                    var servicecpDoc = Container.Resolve<IDomainService<CourtPracticePrescription>>();

                    var dictDocs =
                        servicecpDoc.GetAll()
                            .Where(x => x.CourtPractice.Id == courtpracticeId)
                            .AsEnumerable()
                            .GroupBy(x => x.DocumentGji.Id)
                            .ToDictionary(x => x.Key, y => y.Select(x => x.Id).FirstOrDefault());

                    var cpr = CourtPracticeDomain.Get(courtpracticeId);


                    // По переданным id инспекторов если их нет в списке существующих, то добавляем
                    foreach (var id in docIds)
                    {
                        if (dictDocs.ContainsKey(id))
                        {
                            // Если с таким id уже есть в списке то удалем его из списка и просто пролетаем дальше
                            // без добавления в БД
                            dictDocs.Remove(id);
                            continue;
                        }

                        var newObj = new CourtPracticePrescription
                        {
                            CourtPractice = cpr,
                            DocumentGji = serviceDoc.Load(id)
                        };

                        servicecpDoc.Save(newObj);
                    }

                    foreach (var keyValue in dictDocs)
                    {
                        servicecpDoc.Delete(keyValue.Value);
                    }

                    Container.Release(servicecpDoc);
                    Container.Release(serviceDoc);

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException e)
                {
                    transaction.Rollback();
                    return new BaseDataResult { Success = false, Message = e.Message };
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public IDataResult ListDocs(BaseParams baseParams)
        {
            var courtpracticeId = baseParams.Params.ContainsKey("courtpracticeId") ? baseParams.Params["courtpracticeId"].ToLong() : 0;
            var service = Container.Resolve<IDomainService<CourtPracticePrescription>>();

            var data = service.GetAll()
                .Where(x => x.CourtPractice.Id == courtpracticeId)
                .Select(x => new
                {
                    x.DocumentGji.Id,
                    DocumentNumber = x.DocumentGji.DocumentDate.HasValue ? x.DocumentGji.DocumentNumber + " от " + x.DocumentGji.DocumentDate.Value.ToString("dd.MM.yyyy") : x.DocumentGji.DocumentNumber
                });

            int totalCount = data.Count();
            var result = data.ToArray();

            Container.Release(service);

            return new ListDataResult(result, totalCount);
        }

        public IDataResult AddCourtPracticeRealityObjects(BaseParams baseParams)
        {
            var courtpracticeId = baseParams.Params.ContainsKey("courtpracticeId") ? baseParams.Params["courtpracticeId"].ToLong() : 0;
            var objectIds = baseParams.Params.ContainsKey("objectIds") ? baseParams.Params["objectIds"].ToString() : "";
            var listIds = new List<long>();

            if (courtpracticeId == null)
            {
                return new BaseDataResult(false, "Не удалось определить сущность по Id " + courtpracticeId.ToStr());
            }

            var courtPractice = CourtPracticeDomain.Get(courtpracticeId);


            var roIds = objectIds.Split(',').Select(id => id.ToLong()).ToList();

            listIds.AddRange(CourtPracticeRealityObjectDomain.GetAll()
                                .Where(x => x.CourtPractice.Id == courtpracticeId)
                                .Select(x => x.RealityObject.Id)
                                .Distinct()
                                .ToList());

            foreach (var newId in roIds)
            {

                // Если среди существующих документов уже есть такой документ то пролетаем мимо
                if (listIds.Contains(newId))
                    continue;

                // Если такого решения еще нет то добалвяем
                var newObj = new CourtPracticeRealityObject();
                newObj.RealityObject = new RealityObject { Id = newId };
                newObj.CourtPractice = courtPractice;
                newObj.ObjectVersion = 1;
                newObj.ObjectCreateDate = DateTime.Now;
                newObj.ObjectEditDate = DateTime.Now;

                CourtPracticeRealityObjectDomain.Save(newObj);
            }

            return new BaseDataResult();
        }

        public IDataResult ListDocsForSelect(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var docsServ = this.Container.Resolve<IDomainService<DocumentGji>>();

            var data = docsServ.GetAll()
                .Where(x => x.ObjectCreateDate >= DateTime.Now.AddMonths(-24))
                .Where(x => x.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Prescription || x.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Resolution)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNumber,
                    x.TypeDocumentGji,
                    x.DocumentNum,
                    x.DocumentDate
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        public IDataResult GetListDecision(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var data = ResolutionDecisionService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.DocumentName,
                    x.Apellant,
                    x.AppealNumber,
                    x.AppealDate,
                    x.TypeDecisionAnswer
                })
                .Filter(loadParams, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }

        public IDataResult GetListAppealDecision(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var data = AppealCitsDecisionService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.DocumentName,
                    x.Apellant,
                    x.AppealNumber,
                    x.AppealDate,
                    x.TypeDecisionAnswer
                })
                .Filter(loadParams, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }

        public IDataResult GetListAppealCitsDefinition(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var data = AppealCitsDefinitionService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNum,
                    x.DocumentDate,
                    x.TypeDefinition
                })
                .Filter(loadParams, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }

        public IDataResult GetListResolutionDefinition(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var data = ResolutionDefinitionService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNum,
                    x.DocumentDate,
                    x.TypeDefinition
                })
                .Filter(loadParams, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }



    }
}