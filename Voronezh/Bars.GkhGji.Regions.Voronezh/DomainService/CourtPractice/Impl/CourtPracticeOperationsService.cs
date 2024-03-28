namespace Bars.GkhGji.Regions.Voronezh.DomainService
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

    public class CourtPracticeOperationsService : ICourtPracticeOperationsService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<CourtPractice> CourtPracticeDomain { get; set; }

        public IDomainService<CourtPracticeRealityObject> CourtPracticeRealityObjectDomain { get; set; }

        public IDomainService<KindKNDDictArtLaw> KindKNDDictArtLawDomain { get; set; }

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
                newObj.RealityObject = new RealityObject { Id = newId};
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

        public object GetDocInfo(BaseParams baseParams)
        {
                      
            var docId = baseParams.Params.GetAs("docId", 0L); // это идентификатор Disposal, энтити наследуется от DocumentGji
            var typeEntity = baseParams.Params.GetAs<string>("typeEntity", "");
            if (typeEntity == "MKDLicRequest")
            {
                long courtId = 0;
                var courtP = CourtPracticeDomain.GetAll()
                .Where(x => x.MKDLicRequest != null && x.MKDLicRequest.Id == docId).FirstOrDefault();
                if (courtP != null)
                {
                    courtId = courtP.Id;
                }
                var data = new
                {
                    courtId
                };

                return data;
            }
            else
            {
                long courtId = 0;
                var courtP = CourtPracticeDomain.GetAll()
                .Where(x => x.DocumentGji != null && x.DocumentGji.Id == docId).FirstOrDefault();
                if (courtP != null)
                {
                    courtId = courtP.Id;
                }

                var data = new
                {
                    courtId
                };

                return data;
            }
        }

    }
}