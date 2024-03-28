namespace Bars.GkhGji.DomainService
{
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using Entities;

    using Castle.Windsor;

    public class PrescriptionArticleLawService : IPrescriptionArticleLawService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Добавление Статьи закона
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns>Ответ</returns>
        public IDataResult AddArticles(BaseParams baseParams)
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                var service = this.Container.Resolve<IDomainService<PrescriptionArticleLaw>>();
                var servicePrescr = this.Container.Resolve<IDomainService<Prescription>>();
                var serviceArticle = this.Container.Resolve<IDomainService<ArticleLawGji>>();

                try
                {
                    var documentId = baseParams.Params.GetAs<long>("documentId");
                    var articleIds = baseParams.Params.GetAs<long[]>("articleIds");

                    // в этом списке будут id статей, которые уже связаны с этим предписанием
                    // (чтобы недобавлять несколько одинаковых документов в одно и тоже предписание)
                    var listIds =
                        service.GetAll()
                            .Where(x => x.Prescription.Id == documentId)
                            .Where(x => articleIds.Contains(x.ArticleLaw.Id))
                            .Select(x => x.ArticleLaw.Id)
                            .Distinct()
                            .ToArray();

                    var prescription = servicePrescr.Load(documentId);

                    foreach (var id in articleIds)
                    {
                        // Если среди существующих статей уже есть такая статья, то пролетаем мимо
                        if (listIds.Contains(id))
                            continue;

                        var articleLaw = serviceArticle.Load(id);
                        // Если такой статьи еще нет, то добалвяем
                        var newObj = new PrescriptionArticleLaw
                            {
                                Prescription = prescription,
                                ArticleLaw = articleLaw,
                                Description = articleLaw.Description
                            };

                        service.Save(newObj);
                    }

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException e)
                {
                    transaction.Rollback();
                    return new BaseDataResult(new {Success = false, e.Message});
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    this.Container.Release(service);
                    this.Container.Release(servicePrescr);
                    this.Container.Release(serviceArticle);
                }
            }
        }
    }
}