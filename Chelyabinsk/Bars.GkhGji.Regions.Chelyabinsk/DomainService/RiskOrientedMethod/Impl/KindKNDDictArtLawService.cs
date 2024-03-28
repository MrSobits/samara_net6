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

    public class KindKNDDictArtLawService : IKindKNDDictArtLawService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<ArticleLawGji> ArticleLawGjiDomain { get; set; }

        public IDomainService<KindKNDDict> KindKNDDictDomain { get; set; }

        public IDomainService<KindKNDDictArtLaw> KindKNDDictArtLawDomain { get; set; }

        public IDataResult AddArticleLaw(BaseParams baseParams)
        {
            var parentId = baseParams.Params.ContainsKey("parentId") ? baseParams.Params["parentId"].ToLong() : 0;
            var artLawIds = baseParams.Params.ContainsKey("artLawIds") ? baseParams.Params["artLawIds"].ToString() : "";
            var listIds = new List<long>();
            var typeKND = KindKNDDictDomain.Get(parentId);

            if (parentId == null)
            {
                return new BaseDataResult(false, "Не удалось определить тип КНД по Id " + parentId.ToStr());
            }


            var gjiArtLawIds = artLawIds.Split(',').Select(id => id.ToLong()).ToList();

            listIds.AddRange(KindKNDDictArtLawDomain.GetAll()
                                .Where(x => x.KindKNDDict.Id == parentId)
                                .Select(x => x.ArticleLawGji.Id)
                                .Distinct()
                                .ToList());

            foreach (var newId in gjiArtLawIds)
            {

                // Если среди существующих документов уже есть такой документ то пролетаем мимо
                if (listIds.Contains(newId))
                    continue;

                // Если такого решения еще нет то добалвяем
                var newObj = new KindKNDDictArtLaw();
                newObj.ArticleLawGji = ArticleLawGjiDomain.Get(newId);
                newObj.KindKNDDict = typeKND;
                newObj.ObjectVersion = 1;
                newObj.ObjectCreateDate = DateTime.Now;
                newObj.ObjectEditDate = DateTime.Now;

                KindKNDDictArtLawDomain.Save(newObj);
            }

            return new BaseDataResult();
        }

        public IDataResult GetListArticleLaw(BaseParams baseParams, bool isPaging, out int totalCount)
        {
            totalCount = 0;
            var typeKNDDictId = baseParams.Params.GetAs<long>("typeKNDDictId");
            var loadParams = baseParams.GetLoadParam();
            var typeKNDDict = KindKNDDictDomain.Get(typeKNDDictId);
           
           var data = ArticleLawGjiDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name
                })
                .AsQueryable()
                .Filter(loadParams, Container);

            totalCount = data.Count();

            if (isPaging)
            {
                return new BaseDataResult(data.Order(loadParams).Paging(loadParams).ToList());
            }

            return new BaseDataResult(data.Order(loadParams).ToList());
                
         
            
        }




    }
}