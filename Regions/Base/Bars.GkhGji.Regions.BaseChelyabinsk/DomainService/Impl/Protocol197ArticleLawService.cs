namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;

    using Castle.Windsor;

    public class Protocol197ArticleLawService : IProtocol197ArticleLawService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddArticles(BaseParams baseParams)
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var documentId = baseParams.Params.GetAs<long>("documentId");
                    var articleIds = baseParams.Params.GetAs<List<long>>("articleIds");

                    var serviceArticles = this.Container.Resolve<IDomainService<Protocol197ArticleLaw>>();
                    var serviceProtocol = this.Container.Resolve<IDomainService<Protocol197>>();
                    var serviceArticleLaw = this.Container.Resolve<IDomainService<ArticleLawGji>>();

                    // в этом списке будут id статей, которые уже связаны с этим предписанием
                    // (чтобы недобавлять несколько одинаковых документов в один и тотже протокол)
                    var listIds =
                        serviceArticles.GetAll()
                            .Where(x => x.Protocol197.Id == documentId)
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
                        var newObj = new Protocol197ArticleLaw
                            {
                                Protocol197 = protocol,
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