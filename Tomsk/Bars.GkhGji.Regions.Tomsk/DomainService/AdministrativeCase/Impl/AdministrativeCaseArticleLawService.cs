namespace Bars.GkhGji.Regions.Tomsk.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    using Castle.Windsor;

    public class AdministrativeCaseArticleLawService : IAdministrativeCaseArticleLawService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<AdministrativeCaseArticleLaw> AdminCaseArticleLawDomain { get; set; }

        public IDataResult AddArticles(BaseParams baseParams)
        {
            try
            {
                var documentId = baseParams.Params.GetAs<long>("documentId");
                var articleIds = baseParams.Params.GetAs<long[]>("articleIds");

                if (articleIds != null && articleIds.Length > 0)
                {
                    // в этом списке будут id статей, которые уже связаны с этим предписанием
                    // (чтобы недобавлять несколько одинаковых документов в одно и тоже предписание)
                    var listIds = AdminCaseArticleLawDomain.GetAll()
                        .Where(x => x.AdministrativeCase.Id == documentId)
                        .Select(x => x.ArticleLaw.Id)
                        .Distinct()
                        .ToList();

                    foreach (var id in articleIds)
                    {
                        var newId = id.ToLong();

                        // Если среди существующих статей уже есть такая статья, то пролетаем мимо
                        if (listIds.Contains(newId))
                            continue;

                        if (newId > 0)
                        {
                            // Если такой статьи еще нет, то добалвяем
                            var newObj = new AdministrativeCaseArticleLaw
                            {
                                AdministrativeCase = new AdministrativeCase { Id = documentId },
                                ArticleLaw = new ArticleLawGji { Id = newId }
                            };

                            AdminCaseArticleLawDomain.Save(newObj);    
                        }
                    }
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult {Success = true, Message = e.Message};
            }
        }
    }
}