namespace Bars.GkhGji.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class ProtocolMvdArticleLawService : IProtocolMvdArticleLawService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddArticles(BaseParams baseParams)
        {
            try
            {
                var documentId = baseParams.Params.ContainsKey("documentId") ? baseParams.Params["documentId"].ToLong() : 0;
                var articleIds = baseParams.Params.GetAs<long[]>("articleIds");

                if (articleIds != null && articleIds.Length > 0)
                {
                    // в этом списке будут id статей, которые уже связаны с этим предписанием
                    // (чтобы недобавлять несколько одинаковых документов в одно и тоже предписание)
                    var listIds = new List<long>();

                    var serviceArticles = Container.Resolve<IDomainService<ProtocolMvdArticleLaw>>();

                    listIds.AddRange(
                        serviceArticles.GetAll()
                                       .Where(x => x.ProtocolMvd.Id == documentId)
                                       .Select(x => x.ArticleLaw.Id)
                                       .Distinct()
                                       .ToList());

                    foreach (var id in articleIds)
                    {
                        var newId = id.ToLong();

                        // Если среди существующих статей уже есть такая статья, то пролетаем мимо
                        if (listIds.Contains(newId))
                            continue;

                        if (newId > 0)
                        {
                            // Если такой статьи еще нет, то добалвяем
                            var newObj = new ProtocolMvdArticleLaw
                            {
                                ProtocolMvd = new ProtocolMvd { Id = documentId },
                                ArticleLaw = new ArticleLawGji { Id = newId }
                            };

                            serviceArticles.Save(newObj);    
                        }
                    }
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = true, Message = e.Message };
            }
        }
    }
}