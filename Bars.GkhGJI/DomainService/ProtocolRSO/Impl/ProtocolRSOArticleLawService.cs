namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class ProtocolRSOArticleLawService : IProtocolRSOArticleLawService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddArticles(BaseParams baseParams)
        {
            var serviceArticles = Container.Resolve<IDomainService<ProtocolRSOArticleLaw>>();

            try
            {
                var documentId = baseParams.Params.GetAs("documentId", 0L);
                var articleIds = baseParams.Params.GetAs("articleIds", new long[] { });

                var currentIds = serviceArticles.GetAll()
                                    .Where(x => x.ProtocolRSO.Id == documentId)
                                    .Select(x => x.ArticleLaw.Id)
                                    .Distinct()
                                    .ToList();

                var listToSave = new List<ProtocolRSOArticleLaw>();

                foreach (var id in articleIds)
                {
                    // Если среди существующих статей уже есть такая статья, то пролетаем мимо
                    if (currentIds.Contains(id)) 
                        continue;

                    // Если такой статьи еще нет, то добалвяем
                    var newObj = new ProtocolRSOArticleLaw
                    {
                        ProtocolRSO = new ProtocolRSO { Id = documentId },
                                            ArticleLaw = new ArticleLawGji { Id = id }
                                        };

                    serviceArticles.Save(newObj);
                    
                }

                if (listToSave.Any())
                {
                    using (var tr = Container.Resolve<IDataTransaction>())
                    {
                        try
                        {
                            listToSave.ForEach(serviceArticles.Save);
                            tr.Commit();
                        }
                        catch (Exception exc)
                        {
                            tr.Rollback();
                            throw exc;
                        }
                    }
                }

                return new BaseDataResult();
            }
            finally
            {
                Container.Release(serviceArticles);
            }
        }
    }
}