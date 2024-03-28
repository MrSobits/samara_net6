namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using B4;
    using B4.DataAccess;
    using Entities;

    using Castle.Windsor;

    public class ProtocolArticleLawService : IProtocolArticleLawService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddArticles(BaseParams baseParams)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var documentId = baseParams.Params.GetAs<long>("documentId");
                    var articleIds = baseParams.Params.GetAs<List<long>>("articleIds");

                    var serviceArticles = Container.Resolve<IDomainService<ProtocolArticleLaw>>();
                    var serviceProtocol = Container.Resolve<IDomainService<Protocol>>();
                    var serviceArticleLaw = Container.Resolve<IDomainService<ArticleLawGji>>();

                    // в этом списке будут id статей, которые уже связаны с этим предписанием
                    // (чтобы недобавлять несколько одинаковых документов в один и тотже протокол)
                    var listIds =
                        serviceArticles.GetAll()
                            .Where(x => x.Protocol.Id == documentId)
                            .Select(x => x.ArticleLaw.Id)
                            .Distinct()
                            .ToList();

                    var protocol = serviceProtocol.Load(documentId);

                    foreach (var id in articleIds)
                    {
                        // Если среди существующих статей уже есть такая статья, то пролетаем мимо
                        if (listIds.Contains(id))
                            continue;

                        // Если такой статьи еще нет, то добалвяем
                        var newObj = new ProtocolArticleLaw
                            {
                                Protocol = protocol,
                                ArticleLaw = serviceArticleLaw.Load(id)
                            };

                        serviceArticles.Save(newObj);
                    }

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return new BaseDataResult {Success = false, Message = e.Message};
                }
            }
        }
    }
}